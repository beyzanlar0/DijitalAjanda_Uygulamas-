using Microsoft.Data.SqlClient;
 // E�er User s�n�f�n ayr� dosyadaysa ve Models klas�r�ndeyse


namespace MauiApp1;

public partial class SearchPage : ContentPage
    
{
    private SearchPageViewModel viewModel;
    public SearchPage()
	{
		InitializeComponent();
        viewModel = new SearchPageViewModel();
    }

    private async void OnSearchButtonClicked_1(object sender, EventArgs e)
    {
        var searchTerm = searchEntry.Text;

        if(string.IsNullOrEmpty(searchTerm))
    {
            await DisplayAlert("Hata", "Arama terimi girin.", "Tamam");
            return;
        }
      
        var searchResults = await SearchUsersAsync(searchTerm);

        if (searchResults != null && searchResults.Count > 0)
        {
            searchResultsList.ItemsSource = searchResults;
        }
        else
        {
            await DisplayAlert("Sonu� Bulunamad�", "Arad���n�z kullan�c� bulunamad�.", "Tamam");
        }
    }

    private async Task<List<User>> SearchUsersAsync(string searchTerm)
    {
        List<User> users = new List<User>();

        
        string connectionString = "Data Source=.;Initial Catalog=DijitalAjandaDB;Integrated Security=true;TrustServerCertificate=true;";
        string query = "SELECT UserId, UserName FROM Users WHERE Username LIKE @searchTerm";

        using (var connection = new SqlConnection(connectionString))
        {
            await connection.OpenAsync();

            var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@searchTerm", "%" + searchTerm + "%");

            var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                users.Add(new User
                {
                    UserId = reader.GetInt32(0),
                    UserName = reader.GetString(1)
                });
            }
        }

        return users;
    }
    private async void OnAddFriendButtonClicked(object sender, EventArgs e)
    {
        string fromUser = "currentUsername"; 
        string toUser = (sender as Button).CommandParameter.ToString();

        bool isSuccess = await viewModel.SendFriendRequestAsync(fromUser, toUser);

        if (isSuccess)
        {
            await DisplayAlert("Ba�ar�l�", "Arkada� eklendi", "Tamam");
        }
        else
        {
            await DisplayAlert("Hata", "Arkada� eklenemedi.", "Tamam");
        }
    }
    private User currentUser = new User
    {
        
    };

    private User selectedUser = new User
    {
      
    };

   private async void OnUserTapped(object sender, EventArgs e)
{
    var tappedUser = (sender as Frame).BindingContext as User;

    if (tappedUser != null)
    {
        await Navigation.PushAsync(new Chat(currentUser, selectedUser)); // SORUN BURADA
    }
}

    public async void OnChatButtonClicked_2(object sender, EventArgs e)
    {
        string username = searchEntry.Text?.Trim(); // searchEntry do�ru isim
        if (string.IsNullOrWhiteSpace(username))
        {
            await DisplayAlert("Uyar�", "L�tfen bir kullan�c� ad� girin.", "Tamam");
            return;
        }

        User selectedUser = await GetUserByUsernameAsync(username);

        if (selectedUser != null)
        {
            await Navigation.PushAsync(new Chat(currentUser, selectedUser));
        }
        else
        {
            await DisplayAlert("Hata", "Kullan�c� bulunamad�.", "Tamam");
        }
        Console.WriteLine("Current User: " + currentUser?.Email);
        Console.WriteLine("Selected User: " + selectedUser?.Email);

    }

    // Kullan�c�y� MSSQL'den getir
    private async Task<User> GetUserByUsernameAsync(string username)
    {
        string connectionString = "Data Source=.;Initial Catalog=DijitalAjandaDB;Integrated Security=true;TrustServerCertificate=true;";
        string query = "SELECT UserId, UserName, Email FROM Users WHERE UserName = @uname\r\n";

        using (var conn = new SqlConnection(connectionString))
        {
            await conn.OpenAsync();

            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@uname", username);

            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new User
                {
                    UserId = reader.GetInt32(0),
                    UserName = reader.GetString(1),
                    Email = reader.GetString(2) // ? email alan�n� da ekle!
                };
            }
        }

        return null;

    }
        

}
