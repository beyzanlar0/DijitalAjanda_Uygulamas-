namespace MauiApp1;

public partial class ProfilDuzenle : ContentPage
{
    public ProfilDuzenle()
    {
        InitializeComponent();
        LoadUserProfile();
    }
    // Kullanýcý bilgilerini yükleme
    private void LoadUserProfile()
    {
        // Kullanýcý bilgilerini güncelleme formuna yüklüyoruz
        nameEntry.Text = Session.CurrentUser.NameSurname;
        userNameEntry.Text = Session.CurrentUser.UserName;
        emailEntry.Text = Session.CurrentUser.Email;
        phoneEntry.Text = Session.CurrentUser.MobileNumber;
    }


    // Profil güncelleme butonuna týklanýldýðýnda çalýþacak metot
    private async void OnUpdateProfileClicked(object sender, EventArgs e)
    {
        

        // Kullanýcý tarafýndan girilen bilgileri alýyoruz
        var updatedUser = new User
        {
            UserId = Session.CurrentUser.UserId,  // UserId'yi deðiþtirmiyoruz, mevcut kullanýcýyý güncelleyeceðiz
            NameSurname = nameEntry.Text,
            UserName = userNameEntry.Text,
            Email = emailEntry.Text,
            MobileNumber = phoneEntry.Text,
           

        };
        Session.UpdateSessionUserInfo(updatedUser);

        // Veritabanýnda güncelleme iþlemi
        bool isUpdated = await Session.SaveUserToDatabaseAsync(updatedUser);

        if (isUpdated)
        {
            // Güncelleme baþarýlý
            await DisplayAlert("Baþarý", "Profil bilgileri baþarýyla güncellendi.", "Tamam");
            // Profil sayfasýna dönüyoruz
            await Navigation.PopAsync();
        }
        else
        {
            // Güncelleme baþarýsýz
            await DisplayAlert("Hata", "Profil güncellenemedi. Lütfen tekrar deneyin.", "Tamam");
        }


    }

}
    
