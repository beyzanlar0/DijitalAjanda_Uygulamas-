using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using System.Security.Cryptography;


namespace MauiApp1
{
    public static class Session
    {
        public static int CurrentUserId { get; set; }
        public static string CurrentUserEmail { get; set; }
        public static string CurrentUserName { get; set; }
        public static User CurrentUser { get; set; }
        

        private static string connectionString = "Data Source=.;Initial Catalog=DijitalAjandaDB;Integrated Security=True;TrustServerCertificate=True";


     
        public static async Task<bool> SaveUserToDatabaseAsync(User user)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {

                    await connection.OpenAsync();  // Veritabanına bağlanıyoruz

                    string query = @"
            UPDATE Users
            SET NameSurname = @NameSurname, 
                UserName = @UserName, 
                Email = @Email, 
                MobileNumber = @MobileNumber
            WHERE UserId = @UserId";



                    using (var command = new SqlCommand(query, connection))
                    {
                        // Parametreler ekleniyor
                        command.Parameters.AddWithValue("@NameSurname", user.NameSurname); // user yerine updatedUser yok
                        command.Parameters.AddWithValue("@UserName", user.UserName); // user yerine updatedUser yok
                        command.Parameters.AddWithValue("@Email", user.Email); // user yerine updatedUser yok
                        command.Parameters.AddWithValue("@MobileNumber", user.MobileNumber); // user yerine updatedUser yok
                        command.Parameters.AddWithValue("@UserId", user.UserId); // user yerine updatedUser yok

                        var result = await command.ExecuteNonQueryAsync();

                        if (result > 0)
                        {
                            return true; // Güncelleme başarılı
                        }
                        else
                        {
                            return false; // Güncelleme başarısız
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Hata durumunda loglama yapılabilir
                Debug.WriteLine($"Veritabanı hatası: {ex.Message}");
                return false;
            }
        }

        public static async Task<bool> LoadUserFromDatabaseAsync(int userId)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                var query = "SELECT * FROM Users WHERE UserId = @UserId";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            // Kullanıcı verilerini Session'a yükle
                            CurrentUser = new User
                            {
                                UserId = reader.GetInt32(0),
                                NameSurname = reader.GetString(1),
                                UserName = reader.GetString(2),
                                MobileNumber = reader.GetString(3),
                                Email = reader.GetString(4),

                            };

                            // Session bilgilerinin de güncellenmesi
                            CurrentUserId = CurrentUser.UserId;
                            CurrentUserEmail = CurrentUser.Email;
                            CurrentUserName = CurrentUser.NameSurname;

                            return true; // Kullanıcı başarıyla yüklendi
                        }
                    }
                }
            }
            
            return false; // Kullanıcı bulunamadı

        }
           // Bu metod yalnızca User tipinde bir parametre alıyor
    public static void UpdateSessionUserInfoWithUser(User user)
        {
            CurrentUser = user;
        }




        // Bu metod farklı parametreler alıyorsa, farklı bir adla tanımlanabilir
        public static void UpdateSessionUserInfoWithDetails(int userId, string nameSurname, string userName, string mobileNumber, string email)
        {
            CurrentUser = new User
            {
                UserId = userId,
                NameSurname = nameSurname,
                UserName = userName,
                MobileNumber = mobileNumber,
                Email = email
            };
        }
        public static void UpdateSessionUserInfo(User user)
        {
            CurrentUser = user;
            CurrentUserId = user.UserId;
            CurrentUserEmail = user.Email;
            CurrentUserName = user.NameSurname;
        }

        public static void Logout()
        {
            // Session bilgilerini temizle
            CurrentUser = null;
            CurrentUserId = 0;
            CurrentUserEmail = string.Empty;
            CurrentUserName = string.Empty;

            // Gerekirse başka temizlik işlemleri yapılabilir (örneğin, oturum verilerinin silinmesi)
        }

    }
}
