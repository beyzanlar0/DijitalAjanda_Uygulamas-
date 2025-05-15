namespace MauiApp1;

public partial class ProfilDuzenle : ContentPage
{
    public ProfilDuzenle()
    {
        InitializeComponent();
        LoadUserProfile();
    }
    // Kullan�c� bilgilerini y�kleme
    private void LoadUserProfile()
    {
        // Kullan�c� bilgilerini g�ncelleme formuna y�kl�yoruz
        nameEntry.Text = Session.CurrentUser.NameSurname;
        userNameEntry.Text = Session.CurrentUser.UserName;
        emailEntry.Text = Session.CurrentUser.Email;
        phoneEntry.Text = Session.CurrentUser.MobileNumber;
    }


    // Profil g�ncelleme butonuna t�klan�ld���nda �al��acak metot
    private async void OnUpdateProfileClicked(object sender, EventArgs e)
    {
        

        // Kullan�c� taraf�ndan girilen bilgileri al�yoruz
        var updatedUser = new User
        {
            UserId = Session.CurrentUser.UserId,  // UserId'yi de�i�tirmiyoruz, mevcut kullan�c�y� g�ncelleyece�iz
            NameSurname = nameEntry.Text,
            UserName = userNameEntry.Text,
            Email = emailEntry.Text,
            MobileNumber = phoneEntry.Text,
           

        };
        Session.UpdateSessionUserInfo(updatedUser);

        // Veritaban�nda g�ncelleme i�lemi
        bool isUpdated = await Session.SaveUserToDatabaseAsync(updatedUser);

        if (isUpdated)
        {
            // G�ncelleme ba�ar�l�
            await DisplayAlert("Ba�ar�", "Profil bilgileri ba�ar�yla g�ncellendi.", "Tamam");
            // Profil sayfas�na d�n�yoruz
            await Navigation.PopAsync();
        }
        else
        {
            // G�ncelleme ba�ar�s�z
            await DisplayAlert("Hata", "Profil g�ncellenemedi. L�tfen tekrar deneyin.", "Tamam");
        }


    }

}
    
