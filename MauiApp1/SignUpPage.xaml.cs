namespace MauiApp1;

public partial class SignUpPage : ContentPage
{
	public SignUpPage()
	{
		InitializeComponent();
	}

    private async void TapGestureRecognizer_Tapped_For_SignIn(object sender, TappedEventArgs e)
    {
		await Shell.Current.GoToAsync("//SignIn");
    }
    private async void OnSignUpClicked(object sender, EventArgs e)
    {
        string namesurname = entryNameSurname.Text; // entryNameSurname
        string username = entryUsername.Text; // entryUsername
        string email = entryEmail.Text; // entryEmail
        string phone = entryPhoneNumber.Text; // entryPhoneNumber
        string password = entryPassword.Text;

        if (string.IsNullOrWhiteSpace(namesurname) || string.IsNullOrWhiteSpace(username) ||
            string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(phone) || string.IsNullOrWhiteSpace(password))
        {
            await DisplayAlert("Hata", "Lütfen tüm alanlarý doldurun!", "Tamam");
            return;
        }

        AuthService authService = new AuthService();
        bool isRegistered = authService.OnRegisterClicked(namesurname, username, email, phone, password);

        if (isRegistered)
        {
            await DisplayAlert("Baþarýlý", "Kayýt baþarýlý! Giriþ sayfasýna yönlendiriliyorsunuz.", "Tamam");
            await Shell.Current.GoToAsync("//SignIn");
        }
        else
        {
            await DisplayAlert("Hata", "Bu e-posta adresi zaten kayýtlý!", "Tamam");
        }
    }

    //public bool OnRegisterClicked(string namesurname, string username, string email, string phone, string password)
    //{
    //    if (_context.Users.Any(u => u.Email == email))
    //    {
    //        return false; // Kullanýcý zaten var
    //    }

    //    var newUser = new User
    //    {
    //        NameSurname = namesurname,
    //        UserName = username,
    //        Email = email,
    //        MobileNumber = phone,
    //        PasswordHash = BCrypt.Net.BCrypt.HashPassword(password) // Þifreyi hashle
    //    };

    //    _context.Users.Add(newUser);
    //    _context.SaveChanges();
    //    return true;
    //}





}