using Microsoft.Data.SqlClient;

namespace MauiApp1;

public partial class FriendRequestsPage : ContentPage
{
    public FriendRequestsPage()
    {
        InitializeComponent();
        LoadFriendRequests();
    }

    public async Task<bool> SendFriendRequestAsync(string fromUser, string toUser)
    {
        try
        {
            string connectionString = "Data Source=.;Initial Catalog=DijitalAjandaDB;Integrated Security=True;TrustServerCertificate=True";

            string query = "INSERT INTO FriendRequests (FromUser, ToUser, Status, RequestDate) VALUES (@FromUser, @ToUser, @Status, @RequestDate)";

            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FromUser", fromUser);
                    command.Parameters.AddWithValue("@ToUser", toUser); // Burada 'toUser' parametresi doðru olmalý
                    command.Parameters.AddWithValue("@Status", "Pending");
                    command.Parameters.AddWithValue("@RequestDate", DateTime.Now);

                    int rowsAffected = await command.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return false;
        }
    }

    private async void LoadFriendRequests()
    {

        try
        {
            Console.WriteLine($"CurrentUserEmail: {Session.CurrentUserEmail}");
            var friendRequests = await GetFriendRequestsAsync(Session.CurrentUserEmail);
            Console.WriteLine($"Friend requests count: {friendRequests.Count}");
            FriendRequestsListView.ItemsSource = friendRequests;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Hata: {ex.Message}");
        }
    }

    // Veritabanýndan arkadaþlýk isteklerini çeken method
    private async Task<List<FriendRequest>> GetFriendRequestsAsync(string toUser)
    {
        var requests = new List<FriendRequest>();

        string connectionString = "Data Source=.;Initial Catalog=DijitalAjandaDB;Integrated Security=True;TrustServerCertificate=True";

        string query = "SELECT Id, FromUser, ToUser, Status, RequestDate, IsAccepted FROM FriendRequests WHERE ToUser = @ToUser AND Status = 'Pending'";
        using (var connection = new SqlConnection(connectionString))
        {
            await connection.OpenAsync();
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@ToUser", toUser);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (reader.HasRows)
                    {
                        while (await reader.ReadAsync())
                        {
                            var request = new FriendRequest
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                FromUser = reader["FromUser"].ToString(),
                                ToUser = reader["ToUser"].ToString(),
                                Status = reader["Status"].ToString(),
                                RequestDate = Convert.ToDateTime(reader["RequestDate"]),
                                IsAccepted = Convert.ToBoolean(reader["IsAccepted"])
                            };
                            requests.Add(request);
                        }
                    }
                    else
                    {
                        Console.WriteLine("No data found.");
                    }
                }
            }
        }
        return requests;
    }



    private async void OnAcceptButtonClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var fromUser = button.CommandParameter.ToString();

        bool result = await UpdateFriendRequestStatusAsync(fromUser, Session.CurrentUserEmail, "Accepted");
        if (result)
        {
            await DisplayAlert("Baþarýlý", $"{fromUser} ile arkadaþlýk isteði kabul edildi.", "Tamam");
            LoadFriendRequests();
        }
        else
        {
            await DisplayAlert("Hata", "Arkadaþlýk isteði kabul edilemedi.", "Tamam");
        }
    }

    private async void OnRejectButtonClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var fromUser = button.CommandParameter.ToString();

        bool result = await UpdateFriendRequestStatusAsync(fromUser, Session.CurrentUserEmail, "Rejected");
        if (result)
        {
            await DisplayAlert("Baþarýlý", $"{fromUser} ile arkadaþlýk isteði reddedildi.", "Tamam");
            LoadFriendRequests(); // Listeyi güncelle
        }
        else
        {
            await DisplayAlert("Hata", "Arkadaþlýk isteði reddedilemedi.", "Tamam");
        }
    }


    private async Task<bool> UpdateFriendRequestStatusAsync(string fromUser, string toUser, string status)
    {
        try
        {
            string connectionString = "Data Source=.;Initial Catalog=DijitalAjandaDB;Integrated Security=True;TrustServerCertificate=True";

            string query = "UPDATE FriendRequests SET Status = @Status WHERE FromUser = @FromUser AND ToUser = @ToUser";

            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Status", status);
                    command.Parameters.AddWithValue("@FromUser", fromUser);
                    command.Parameters.AddWithValue("@ToUser", toUser);

                    int rowsAffected = await command.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return false;
        }
    }


    private async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem != null)
        {
            var selectedRequest = (FriendRequest)e.SelectedItem;

            await DisplayAlert("Seçilen Ýstek", $"Kullanýcý: {selectedRequest.FromUser}", "Tamam");


        }
    }
}



