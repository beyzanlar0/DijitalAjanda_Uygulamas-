using System.Collections.ObjectModel;
using Microsoft.Maui.Controls;
using System;
using System.Threading.Tasks;
using System.Formats.Tar;
using System.Data;
using Microsoft.Data.SqlClient;
using ClosedXML.Excel;



namespace MauiApp1
{
    public partial class HomePage : ContentPage
    {
        private string connectionString = "Data Source=.;Initial Catalog=DijitalAjandaDB;Integrated Security=True;TrustServerCertificate=True";
        private ObservableCollection<Task> Tasks { get; set; } 
        private StackLayout _currentTaskContainer;
        private int activeUserId = 1;
        public HomePage()
        {
            InitializeComponent();
            Tasks = new ObservableCollection<Task>();
            BindingContext = this;
            _ = GorevleriYukle(activeUserId);
          
        }


        private void OnTarihiKaydetClicked(object sender, EventArgs e)
        {
            DateTime secilenTarih = TarihSecici.Date;

            // Tarihi bir label'a yazdýrma
            SecilenTarihLabel.Text = $"Seçilen Tarih: {secilenTarih.ToString("dd.MM.yyyy")}";

            // Burada veritabanýna da gönderebilirsin örneðin:
            // SaveDateToDatabase(secilenTarih);

            // Veya baþka iþlemler yapabilirsin
        }


      

        //private void OnHaftalikKaydetClicked_1(object sender, EventArgs e)
        //{
        //    DateTime secilenTarih = HaftalikTarih.Date; // Haftalýk tarih picker'ý

        //    // Haftanýn baþlangýcý (Pazartesi)
        //    DateTime haftaninBaslangici = secilenTarih.AddDays(-(int)secilenTarih.DayOfWeek + 1); // Pazartesi
        //    DateTime haftaninBitisi = haftaninBaslangici.AddDays(6); // Haftanýn sonu (Pazar)

        //    // Haftalýk tarihleri bir label'a yazdýrma
        //    HaftalikLabel.Text = $"Seçilen Haftanýn Baþlangýcý: {haftaninBaslangici.ToString("dd.MM.yyyy")} - Haftanýn Sonu: {haftaninBitisi.ToString("dd.MM.yyyy")}";

        //}

        //private void OnAylikKaydetClicked(object sender, EventArgs e)
        //{

        //    DateTime secilenTarih = AyPicker.Date; // Aylýk tarih picker'ý

        //    // Aylýk baþlangýç tarihi (ay baþý)
        //    DateTime aylikBaslangic = new DateTime(secilenTarih.Year, secilenTarih.Month, 1);
        //    // Aylýk bitiþ tarihi (ay sonu)
        //    DateTime aylikBitis = aylikBaslangic.AddMonths(1).AddDays(-1);

        //    // Aylýk tarihleri bir label'a yazdýrma
        //    AyPicker.Text = $"Seçilen Ay: {secilenTarih.ToString("MMMM yyyy")} - Baþlangýç: {aylikBaslangic.ToString("dd.MM.yyyy")} - Bitiþ: {aylikBitis.ToString("dd.MM.yyyy")}";

        //    // Burada veritabanýna da gönderebilirsin
        //    // SaveMonthToDatabase(aylikBaslangic, aylikBitis);

        //    // Veya baþka iþlemler yapabilirsin

        //}

        //private void OnYillikKaydetClicked(object sender, EventArgs e)
        //{

        //    DateTime secilenTarih = YillikTarih.Date; // Yýllýk tarih picker'ý

        //    // Yýlýn baþlangýç tarihi
        //    DateTime yilBaslangici = new DateTime(secilenTarih.Year, 1, 1);
        //    // Yýlýn bitiþ tarihi
        //    DateTime yilBitisi = new DateTime(secilenTarih.Year, 12, 31);

        //    // Yýllýk tarihleri bir label'a yazdýrma
        //    YillikLabel.Text = $"Seçilen Yýl: {secilenTarih.Year} - Baþlangýç: {yilBaslangici.ToString("dd.MM.yyyy")} - Bitiþ: {yilBitisi.ToString("dd.MM.yyyy")}";

        //    // Burada veritabanýna da gönderebilirsin
        //    // SaveYearToDatabase(yilBaslangici, yilBitisi);

        //    // Veya baþka iþlemler yapabilirsin

        //}
        private async Task<bool> GorevleriYukle(int userId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    await conn.OpenAsync();
                    string query = "SELECT * FROM Tasks WHERE UserId = @UserId";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserId", userId);

                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                string gunluk = reader["GunlukGorev"]?.ToString();
                                string haftalik = reader["HaftalikHedefler"]?.ToString();
                                string aylik = reader["AylikHedefler"]?.ToString();
                                string yillik = reader["YillikHedefler"]?.ToString();
                                bool tamamlandiMi = Convert.ToBoolean(reader["TamamlandiMi"]);
                                if (!string.IsNullOrWhiteSpace(gunluk))
                                    AddTaskToUI(TaskListContainer, gunluk, tamamlandiMi);

                                if (!string.IsNullOrWhiteSpace(haftalik))
                                    AddTaskToUI(TaskListContainer1, haftalik, tamamlandiMi);

                                if (!string.IsNullOrWhiteSpace(aylik))
                                    AddTaskToUI(TaskListContainer2, aylik, tamamlandiMi);

                                if (!string.IsNullOrWhiteSpace(yillik))
                                    AddTaskToUI(TaskListContainer3, yillik, tamamlandiMi);
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Hata", "Görevler yüklenirken bir hata oluþtu:\n" + ex.Message, "Tamam");
                return false;
            }
        }

        private async void AddTaskToUI(StackLayout container, string text, bool isCompleted)
        {
            StackLayout taskItem = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            CheckBox checkBox = new CheckBox
            {
                IsChecked = isCompleted
            };

            Label taskLabel = new Label
            {
                Text = text,
                TextColor = Colors.Black,
                VerticalOptions = LayoutOptions.Center,
                TextDecorations = isCompleted ? TextDecorations.Strikethrough : TextDecorations.None
            };

            Button deleteButton = new Button
            {
                Text = "Sil",
                BackgroundColor = Colors.Purple,
                TextColor = Colors.White,
                FontSize = 12,
                Padding = new Thickness(5),
                HorizontalOptions = LayoutOptions.EndAndExpand
            };

            checkBox.CheckedChanged += async (s, args) =>
            {
                taskLabel.TextDecorations = checkBox.IsChecked ? TextDecorations.Strikethrough : TextDecorations.None;

                // Güncelleme iþlemi (veritabanýna iþaretli bilgisini kaydet)
                await GorevDurumuGuncelle(taskLabel.Text, checkBox.IsChecked, activeUserId);

                // Tüm günlük görevler tamamlandý mý kontrol et
                if (container == TaskListContainer)
                    KontrolEtVeHedefeUlasildiEkle(TaskListContainer, GunlukHedefLabel);
            };



            // Silme butonuna týklanýnca görev UI'dan silinir ve veritabanýndan silinir
            deleteButton.Clicked += async (s, args) =>
            {
                string taskMetni = taskLabel.Text; // Silinen görev metnini alýyoruz

                // Görevi UI'dan kaldýr
                container.Children.Remove(taskItem);

                // Görevi veritabanýndan sil
                await GorevVeritabanindanSil(taskMetni, activeUserId); // Aktif kullanýcý ID'si
            };

            taskItem.Children.Add(checkBox);
            taskItem.Children.Add(taskLabel);
            taskItem.Children.Add(deleteButton);

            container.Children.Add(taskItem);
        }

        private async Task<bool> GorevVeritabanindanSil(string gorevMetni, int userId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    await conn.OpenAsync();

                    string query = @"DELETE FROM Tasks 
                                     WHERE (GunlukGorev = @Gorev OR HaftalikHedefler = @Gorev 
                                            OR AylikHedefler = @Gorev OR YillikHedefler = @Gorev)
                                           AND UserId = @UserId";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Gorev", gorevMetni);
                        cmd.Parameters.AddWithValue("@UserId", userId);

                        await cmd.ExecuteNonQueryAsync(); // Veritabanýndan silme iþlemi
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Hata", "Görev silinirken hata oluþtu:\n" + ex.Message, "Tamam");
                return false;
            }
        }
        private void OnAddTaskClicked(object sender, EventArgs e)
        {

            Button button = sender as Button;
            if (button != null)
            {
                if (button == BtnAddTask1) _currentTaskContainer = TaskListContainer;
                else if (button == BtnAddTask2) _currentTaskContainer = TaskListContainer1;
                else if (button == BtnAddTask3) _currentTaskContainer = TaskListContainer2;
                else if (button == BtnAddTask4) _currentTaskContainer = TaskListContainer3;

                TaskPopup.IsVisible = true;
            }
        }
        private async Task<bool> GorevVeritabaniKaydet(string gunluk, string haftalik, string aylik, string yillik, bool tamamlandiMi, int userId)
        {

            try
            {

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    await conn.OpenAsync();  // Asenkron baðlantýyý açýyoruz

                    string query = @"INSERT INTO Tasks 
                            (GunlukGorev, HaftalikHedefler, AylikHedefler, YillikHedefler, TamamlandiMi, UserId)
                            VALUES (@Gunluk, @Haftalik, @Aylik, @Yillik, @TamamlandiMi, @UserId)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Gunluk", gunluk);
                        cmd.Parameters.AddWithValue("@Haftalik", haftalik);
                        cmd.Parameters.AddWithValue("@Aylik", aylik);
                        cmd.Parameters.AddWithValue("@Yillik", yillik);
                        cmd.Parameters.AddWithValue("@TamamlandiMi", tamamlandiMi);
                        cmd.Parameters.AddWithValue("@UserId", userId);  // Burada aktifKullaniciId geçmelisiniz

                        int affectedRows = await cmd.ExecuteNonQueryAsync();
                        return affectedRows > 0; // Kayýt baþarýlýysa true döner
                    }
                    }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Hata", "Veritabanýna kaydedilirken bir hata oluþtu:\n" + ex.Message, "Tamam");
                return false;
            }
        }

        
        

        private void Button_Clicked_4(object sender, EventArgs e)
        {
            _currentTaskContainer = TaskListContainer1;
            TaskPopup.IsVisible = true;
        }

        private void Button_Clicked_5(object sender, EventArgs e)
        {
            _currentTaskContainer = TaskListContainer2;
            TaskPopup.IsVisible = true;
        }

        private void Button_Clicked_6(object sender, EventArgs e)
        {
            _currentTaskContainer = TaskListContainer3;
            TaskPopup.IsVisible = true;
        }


        private async void OnTaskAdded(object sender, EventArgs e)
        {

            if (_currentTaskContainer != null && !string.IsNullOrEmpty(TaskEntry.Text))
            {
                StackLayout taskItem = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    HorizontalOptions = LayoutOptions.FillAndExpand
                };

                CheckBox checkBox = new CheckBox();


                Label taskLabel = new Label
                {
                    Text = TaskEntry.Text,
                    TextColor = Colors.Black,
                    VerticalOptions = LayoutOptions.Center
                };

                Button deleteButton = new Button
                {
                    Text = "Sil",
                    BackgroundColor = Colors.Purple,
                    TextColor = Colors.White,
                    FontSize = 12,
                    Padding = new Thickness(5),
                    HorizontalOptions = LayoutOptions.EndAndExpand
                };



               checkBox.CheckedChanged += async (s, args) =>
    {
        taskLabel.TextDecorations = checkBox.IsChecked ? TextDecorations.Strikethrough : TextDecorations.None;

        // Görev durumunu veritabanýnda güncelle
        await GorevDurumuGuncelle(taskLabel.Text, checkBox.IsChecked, activeUserId);

        // Eðer tüm görevler tamamlandýysa hedef mesajýný göster
        KontrolEtVeHedefeUlasildiEkle(TaskListContainer, GunlukHedefLabel);
    };


                deleteButton.Clicked += (s, args) => _currentTaskContainer.Children.Remove(taskItem);

                taskItem.Children.Add(checkBox);
                taskItem.Children.Add(taskLabel);
                taskItem.Children.Add(deleteButton);

                _currentTaskContainer.Children.Add(taskItem);

                try
                {
                    string hedefTipi = _currentTaskContainer == TaskListContainer ? "gunluk"
                                      : _currentTaskContainer == TaskListContainer1 ? "haftalik"
                                      : _currentTaskContainer == TaskListContainer2 ? "aylik"
                                      : "yillik";

                    string gunluk = hedefTipi == "gunluk" ? TaskEntry.Text : "";
                    string haftalik = hedefTipi == "haftalik" ? TaskEntry.Text : "";
                    string aylik = hedefTipi == "aylik" ? TaskEntry.Text : "";
                    string yillik = hedefTipi == "yillik" ? TaskEntry.Text : "";

                    
                    // Asenkron metodu doðru þekilde çaðýrýyoruz
                    await GorevVeritabaniKaydet(gunluk, haftalik, aylik, yillik, false, 1);
                }
                catch (Exception ex)
                {
                    // Hata durumunda kullanýcýya bildirim gösteriyoruz
                    await DisplayAlert("Hata", "Veritabanýna kaydedilirken bir hata oluþtu:\n" + ex.Message, "Tamam");
                }

                TaskEntry.Text = "";
                TaskPopup.IsVisible = false;
            }
        }
        private void KontrolEtVeHedefeUlasildiEkle(StackLayout taskListContainer, Label hedefLabel)
        {
            bool hepsiSecili = true;

            foreach (var item in TaskListContainer.Children)
            {
                if (item is StackLayout taskItem)
                {
                    var checkBox = taskItem.Children[0] as CheckBox;
                    if (checkBox != null && !checkBox.IsChecked)
                    {
                        hepsiSecili = false;
                        break;
                    }
                }
            }

            // Zaten eklenmiþ mi kontrol et
            var mevcutHedefLabel = TaskListContainer.Children.FirstOrDefault(c => c is Label label && label.Text == "Hedefe ulaþýldý");
            if (hepsiSecili)
            {
                if (mevcutHedefLabel == null)
                {
                    TaskListContainer.Children.Add(new Label
                    {
                        Text = "Hedefe ulaþýldý",
                        TextColor = Colors.Green,
                        FontAttributes = FontAttributes.Bold,
                        Margin = new Thickness(5)
                    });
                }
            }
            else
            {
                if (mevcutHedefLabel != null)
                {
                    TaskListContainer.Children.Remove(mevcutHedefLabel);
                }
            }
        }
        private async Task<bool> GorevDurumuGuncelle(string gorevMetni, bool tamamlandiMi, int userId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    await conn.OpenAsync();

                    string query = @"UPDATE Tasks 
                             SET TamamlandiMi = @TamamlandiMi
                             WHERE (GunlukGorev = @Gorev OR HaftalikHedefler = @Gorev 
                                    OR AylikHedefler = @Gorev OR YillikHedefler = @Gorev)
                                   AND UserId = @UserId";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@TamamlandiMi", tamamlandiMi);
                        cmd.Parameters.AddWithValue("@Gorev", gorevMetni);
                        cmd.Parameters.AddWithValue("@UserId", userId);

                        await cmd.ExecuteNonQueryAsync();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Hata", "Görev durumu güncellenirken bir hata oluþtu:\n" + ex.Message, "Tamam");
                return false;
            }
        }

        private void OnCancel(object sender, EventArgs e)
        {
            TaskPopup.IsVisible = false;

        }


        private async void OnSearchButtonClicked(object sender, EventArgs e)
        {
            // Arama ekranýna geçiþ yapýyoruz
            await Navigation.PushAsync(new SearchPage());
        }

        private async void OnFriendRequestsButtonClicked(object sender, EventArgs e)
        {
            // FriendRequestsPage sayfasýna yönlendir
            await Navigation.PushAsync(new FriendRequestsPage());
        }
        private async void OnProfileButtonClicked (object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ProfilePage());
        }

        private void btnExceleAktar_Clicked(object sender, EventArgs e)
        {
            exceleAktar();
        }
        void exceleAktar()
        {
            using (var workbook=new XLWorkbook())
            {
                var worksheet = workbook.AddWorksheet("sayfa1");
                List<Task> gorevler = null;
                using (var con=new DataBase())
                {
                    gorevler = con.Tasks.ToList();
                }
                if (gorevler!=null&&gorevler.Count>0)
                {
                    int satir = 1;
                    worksheet.Cell(satir, 1).Value ="Id";
                    worksheet.Cell(satir, 2).Value = " GunlukGorev";
                    worksheet.Cell(satir, 3).Value =  "HaftalikHedefler";
                    worksheet.Cell(satir, 4).Value =  "AylikHedefler";
                    worksheet.Cell(satir, 5).Value = "YillikHedefler";
                    worksheet.Cell(satir, 6).Value = "TamamlandiMi";
                    satir ++;
                    foreach (Task gorev in gorevler)
                    {
                        worksheet.Cell(satir, 1).Value = gorev.Id;
                        worksheet.Cell(satir, 2).Value = gorev.GunlukGorev;
                        worksheet.Cell(satir, 3).Value = gorev.HaftalikHedefler;
                        worksheet.Cell(satir, 4).Value = gorev.AylikHedefler;
                        worksheet.Cell(satir, 5).Value = gorev.YillikHedefler;
                        worksheet.Cell(satir, 6).Value = gorev.TamamlandiMi;
                        satir++;
                    }

                    
                    var dosyaYolu = Path.Combine(FileSystem.CacheDirectory, "myabc.xlsx");
                    workbook.SaveAs(dosyaYolu);
                    dosyaPaylas(dosyaYolu);
                }
                
            }
        }
        public async void dosyaPaylas(string filePath)
        {
            try
            {
                var file = new FileInfo(filePath);
                if (file.Exists)
                {
                    if (filePath.EndsWith(".xlsx"))
                    {
                        await Share.RequestAsync(new ShareFileRequest
                        {
                            Title = "Excel File",
                            File = new ShareFile(filePath)
                        });
                    }
                    if (filePath.EndsWith(".mid"))
                    {
                        await Share.RequestAsync(new ShareFileRequest
                        {
                            Title = "MIDI File",
                            File = new ShareFile(filePath)
                        });
                    }

                }
                else
                {
                    await DisplayAlert("Hata", "Dosya bulunamadý.", "Tamam");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Hata", "Dosya bulunamadý.", "Tamam");
            }
        }
    }
}


