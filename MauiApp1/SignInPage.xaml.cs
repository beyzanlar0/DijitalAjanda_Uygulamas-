
namespace MauiApp1
{
    public partial class SignInPage : ContentPage
    {

        AuthService authService;
        public SignInPage()
        {
            InitializeComponent();
           
        }
        private async void OnLoginClicked(object sender, EventArgs e)
        {
            if (authService == null) authService = new AuthService();
            var Email = mail.Text;
            var Password = password.Text;


            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
            {
                await DisplayAlert("Hata", "Kullanıcı adı ve şifre boş olamaz.", "Tamam");
                return;
            }
            if (authService.dogrula(Email, Password))
            {
                Session.CurrentUserEmail = Email;
                await DisplayAlert("BAŞARILI", "Giriş doğrulaması başarılı", "Tamam");
                await Shell.Current.GoToAsync("//HomePage");
            }
            else await DisplayAlert("BAŞARISIZ", "Giriş doğrulaması başarısız", "Tamam");
        }

        private async void TapGestureRecognizer_Tapped_For_SignUp(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//SignUp");
        }


    }

}
