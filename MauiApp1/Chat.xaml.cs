using Firebase.Database;
using MauiApp1;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;


namespace MauiApp1
{
    public partial class Chat : ContentPage
    {
        private User currentUser;
        private User selectedUser;
        private ObservableCollection<Messages> MessageList = new();
        string connectionString = @"Data Source=.;Initial Catalog=DijitalAjandaDB;Integrated Security=true;TrustServerCertificate=true";

        public Chat()
        {
            InitializeComponent();
        }
        public Chat(User currentUser, User selectedUser)
        {
            InitializeComponent();
            if (currentUser == null || selectedUser == null)
            {
                DisplayAlert("Hata", "Kullanýcý bilgileri eksik.", "Tamam");
                return;
            }

            this.currentUser = currentUser;
            this.selectedUser = selectedUser;

            // E-posta bilgileri eksikse göster
            if (string.IsNullOrEmpty(currentUser.Email) || string.IsNullOrEmpty(selectedUser.Email))
            {
                DisplayAlert("Bilgi", "E-posta bilgileri eksik ama mesajlaþma devam edebilir.", "Tamam");
            }

            MessagesList.ItemsSource = MessageList;
            LoadMessages();

        }
        private async void LoadMessages()
        {
            try
            {
                MessageList.Clear();

                using SqlConnection conn = new(connectionString);
                await conn.OpenAsync();

                string query = @"
                SELECT * FROM Messages
                WHERE 
                    (FromUserId = @CurrentUserId AND ToUserId = @SelectedUserId)
                    OR
                    (FromUserId = @SelectedUserId AND ToUserId = @CurrentUserId)
                ORDER BY SentTime";

                using SqlCommand cmd = new(query, conn);
                cmd.Parameters.AddWithValue("@CurrentUserId", currentUser.UserId);
                cmd.Parameters.AddWithValue("@SelectedUserId", selectedUser.UserId);

                using SqlDataReader reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    MessageList.Add(new Messages
                    {
                        FromUserId = (int)reader["FromUserId"],
                        ToUserId = (int)reader["ToUserId"],
                        FromEmail = reader["FromEmail"].ToString(),
                        ToEmail = reader["ToEmail"].ToString(),
                        MessageText = reader["MessageText"].ToString(),
                        SentTime = Convert.ToDateTime(reader["SentTime"])
                    });
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Hata", ex.Message, "Tamam");
            }
        }
        private async void OnSendClicked(object sender, EventArgs e)
        {
            // ...

            try
            {
                using SqlConnection conn = new(connectionString);
                await conn.OpenAsync();

                // FromEmail ve ToEmail parametrelerini isteðe baðlý hale getirin
                string query = @"INSERT INTO Messages 
                       (FromUserId, ToUserId, MessageText, SentTime";

                // E-posta deðerleri varsa ekle
                if (!string.IsNullOrEmpty(currentUser.Email))
                    query += ", FromEmail";
                if (!string.IsNullOrEmpty(selectedUser.Email))
                    query += ", ToEmail";

                query += ") VALUES (@FromUserId, @ToUserId, @MessageText, @SentTime";

                // E-posta deðerleri varsa ekle
                if (!string.IsNullOrEmpty(currentUser.Email))
                    query += ", @FromEmail";
                if (!string.IsNullOrEmpty(selectedUser.Email))
                    query += ", @ToEmail";

                query += ")";

                using SqlCommand cmd = new(query, conn);
                cmd.Parameters.AddWithValue("@FromUserId", currentUser.UserId);
                cmd.Parameters.AddWithValue("@ToUserId", selectedUser.UserId);
                cmd.Parameters.AddWithValue("@MessageText", MessageEntry.Text.Trim());
                cmd.Parameters.AddWithValue("@SentTime", DateTime.Now);

                // E-posta deðerleri varsa ekle
                if (!string.IsNullOrEmpty(currentUser.Email))
                    cmd.Parameters.AddWithValue("@FromEmail", currentUser.Email);
                if (!string.IsNullOrEmpty(selectedUser.Email))
                    cmd.Parameters.AddWithValue("@ToEmail", selectedUser.Email);

                await cmd.ExecuteNonQueryAsync();

                MessageEntry.Text = "";
                LoadMessages();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Hata", ex.Message, "Tamam");
            }
        }
    }
    
}
