using System.Xml;
using Microsoft.Data.SqlClient;
namespace MauiApp1;

public partial class ProfilePage : ContentPage
{
	public ProfilePage()
	{
		InitializeComponent();
        
        LoadUserProfile();

   
    }
    private async void LoadUserProfile()
    {
        var userId = 1; // Örneðin, giriþ yapan kullanýcýnýn ID'si (örnek olarak 1 yazýldý, bunu dinamik yapabilirsiniz)

        var user = await GetUserFromDatabaseAsync(userId);

        if (user != null)
        {
            // Veritabanýndan alýnan bilgileri Session'a kaydediyoruz
            Session.UpdateSessionUserInfo(user);

            // UI'da gösteriyoruz
            NameSurnameLabel.Text = user.NameSurname ?? "Bilinmeyen Kullanýcý";
            UserNameLabel.Text = user.UserName ?? "Bilinmeyen Kullanýcý Adý";
            EmailLabel.Text = user.Email ?? "-";
            PhoneLabel.Text = user.MobileNumber ?? "-";
        }
        else
        {
            // Veritabanýnda kullanýcý bulunamadýysa "Bilgiler mevcut deðil" mesajý göster
            NameSurnameLabel.Text = "Ad Soyad: Bilgiler mevcut deðil";
            UserNameLabel.Text = "Kullanýcý Adý: Bilgiler mevcut deðil";
            EmailLabel.Text = "E-Posta: Bilgiler mevcut deðil";
            PhoneLabel.Text = "Telefon: Bilgiler mevcut deðil";
        }
    }

    private async Task<User> GetUserFromDatabaseAsync(int userId)
    {
        string connectionString ="Data Source=.;Initial Catalog=DijitalAjandaDB;Integrated Security=True;TrustServerCertificate=True";


    var user = new User();

        using (var connection = new SqlConnection(connectionString))
        {
            await connection.OpenAsync();

            var query = "SELECT UserId, NameSurname, UserName, MobileNumber, Email FROM Users WHERE UserId = @UserId";
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@UserId", userId);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        user.UserId = reader.GetInt32(0);
                        user.NameSurname = reader.GetString(1);
                        user.UserName = reader.GetString(2);
                        user.MobileNumber = reader.GetString(3);
                        user.Email = reader.GetString(4);
                    }
                }
            }
        }
        return user;
    }
    private async void OnEditProfileClicked(object sender, EventArgs e)
    {

        await Navigation.PushAsync(new ProfilDuzenle());
    }

    private async void OnUpdateProfileClicked(object sender, EventArgs e)
    {

    }

    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new SignInPage());

    }

}