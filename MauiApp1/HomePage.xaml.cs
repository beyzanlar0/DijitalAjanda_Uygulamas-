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

            // Tarihi bir label'a yazd�rma
            SecilenTarihLabel.Text = $"Se�ilen Tarih: {secilenTarih.ToString("dd.MM.yyyy")}";

            // Burada veritaban�na da g�nderebilirsin �rne�in:
            // SaveDateToDatabase(secilenTarih);

            // Veya ba�ka i�lemler yapabilirsin
        }


      

        //private void OnHaftalikKaydetClicked_1(object sender, EventArgs e)
        //{
        //    DateTime secilenTarih = HaftalikTarih.Date; // Haftal�k tarih picker'�

        //    // Haftan�n ba�lang�c� (Pazartesi)
        //    DateTime haftaninBaslangici = secilenTarih.AddDays(-(int)secilenTarih.DayOfWeek + 1); // Pazartesi
        //    DateTime haftaninBitisi = haftaninBaslangici.AddDays(6); // Haftan�n sonu (Pazar)

        //    // Haftal�k tarihleri bir label'a yazd�rma
        //    HaftalikLabel.Text = $"Se�ilen Haftan�n Ba�lang�c�: {haftaninBaslangici.ToString("dd.MM.yyyy")} - Haftan�n Sonu: {haftaninBitisi.ToString("dd.MM.yyyy")}";

        //}

        //private void OnAylikKaydetClicked(object sender, EventArgs e)
        //{

        //    DateTime secilenTarih = AyPicker.Date; // Ayl�k tarih picker'�

        //    // Ayl�k ba�lang�� tarihi (ay ba��)
        //    DateTime aylikBaslangic = new DateTime(secilenTarih.Year, secilenTarih.Month, 1);
        //    // Ayl�k biti� tarihi (ay sonu)
        //    DateTime aylikBitis = aylikBaslangic.AddMonths(1).AddDays(-1);

        //    // Ayl�k tarihleri bir label'a yazd�rma
        //    AyPicker.Text = $"Se�ilen Ay: {secilenTarih.ToString("MMMM yyyy")} - Ba�lang��: {aylikBaslangic.ToString("dd.MM.yyyy")} - Biti�: {aylikBitis.ToString("dd.MM.yyyy")}";

        //    // Burada veritaban�na da g�nderebilirsin
        //    // SaveMonthToDatabase(aylikBaslangic, aylikBitis);

        //    // Veya ba�ka i�lemler yapabilirsin

        //}

        //private void OnYillikKaydetClicked(object sender, EventArgs e)
        //{

        //    DateTime secilenTarih = YillikTarih.Date; // Y�ll�k tarih picker'�

        //    // Y�l�n ba�lang�� tarihi
        //    DateTime yilBaslangici = new DateTime(secilenTarih.Year, 1, 1);
        //    // Y�l�n biti� tarihi
        //    DateTime yilBitisi = new DateTime(secilenTarih.Year, 12, 31);

        //    // Y�ll�k tarihleri bir label'a yazd�rma
        //    YillikLabel.Text = $"Se�ilen Y�l: {secilenTarih.Year} - Ba�lang��: {yilBaslangici.ToString("dd.MM.yyyy")} - Biti�: {yilBitisi.ToString("dd.MM.yyyy")}";

        //    // Burada veritaban�na da g�nderebilirsin
        //    // SaveYearToDatabase(yilBaslangici, yilBitisi);

        //    // Veya ba�ka i�lemler yapabilirsin

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
                await DisplayAlert("Hata", "G�revler y�klenirken bir hata olu�tu:\n" + ex.Message, "Tamam");
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

                // G�ncelleme i�lemi (veritaban�na i�aretli bilgisini kaydet)
                await GorevDurumuGuncelle(taskLabel.Text, checkBox.IsChecked, activeUserId);

                // T�m g�nl�k g�revler tamamland� m� kontrol et
                if (container == TaskListContainer)
                    KontrolEtVeHedefeUlasildiEkle(TaskListContainer, GunlukHedefLabel);
            };



            // Silme butonuna t�klan�nca g�rev UI'dan silinir ve veritaban�ndan silinir
            deleteButton.Clicked += async (s, args) =>
            {
                string taskMetni = taskLabel.Text; // Silinen g�rev metnini al�yoruz

                // G�revi UI'dan kald�r
                container.Children.Remove(taskItem);

                // G�revi veritaban�ndan sil
                await GorevVeritabanindanSil(taskMetni, activeUserId); // Aktif kullan�c� ID'si
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

                        await cmd.ExecuteNonQueryAsync(); // Veritaban�ndan silme i�lemi
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Hata", "G�rev silinirken hata olu�tu:\n" + ex.Message, "Tamam");
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
                    await conn.OpenAsync();  // Asenkron ba�lant�y� a��yoruz

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
                        cmd.Parameters.AddWithValue("@UserId", userId);  // Burada aktifKullaniciId ge�melisiniz

                        int affectedRows = await cmd.ExecuteNonQueryAsync();
                        return affectedRows > 0; // Kay�t ba�ar�l�ysa true d�ner
                    }
                    }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Hata", "Veritaban�na kaydedilirken bir hata olu�tu:\n" + ex.Message, "Tamam");
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

        // G�rev durumunu veritaban�nda g�ncelle
        await GorevDurumuGuncelle(taskLabel.Text, checkBox.IsChecked, activeUserId);

        // E�er t�m g�revler tamamland�ysa hedef mesaj�n� g�ster
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

                    
                    // Asenkron metodu do�ru �ekilde �a��r�yoruz
                    await GorevVeritabaniKaydet(gunluk, haftalik, aylik, yillik, false, 1);
                }
                catch (Exception ex)
                {
                    // Hata durumunda kullan�c�ya bildirim g�steriyoruz
                    await DisplayAlert("Hata", "Veritaban�na kaydedilirken bir hata olu�tu:\n" + ex.Message, "Tamam");
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

            // Zaten eklenmi� mi kontrol et
            var mevcutHedefLabel = TaskListContainer.Children.FirstOrDefault(c => c is Label label && label.Text == "Hedefe ula��ld�");
            if (hepsiSecili)
            {
                if (mevcutHedefLabel == null)
                {
                    TaskListContainer.Children.Add(new Label
                    {
                        Text = "Hedefe ula��ld�",
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
                await DisplayAlert("Hata", "G�rev durumu g�ncellenirken bir hata olu�tu:\n" + ex.Message, "Tamam");
                return false;
            }
        }

        private void OnCancel(object sender, EventArgs e)
        {
            TaskPopup.IsVisible = false;

        }


        private async void OnSearchButtonClicked(object sender, EventArgs e)
        {
            // Arama ekran�na ge�i� yap�yoruz
            await Navigation.PushAsync(new SearchPage());
        }

        private async void OnFriendRequestsButtonClicked(object sender, EventArgs e)
        {
            // FriendRequestsPage sayfas�na y�nlendir
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
                    await DisplayAlert("Hata", "Dosya bulunamad�.", "Tamam");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Hata", "Dosya bulunamad�.", "Tamam");
            }
        }
    }
}


