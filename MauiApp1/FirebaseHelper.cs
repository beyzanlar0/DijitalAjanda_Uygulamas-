//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Firebase.Database;
//using Firebase.Database.Query;

//namespace MauiApp1
//{
//    public class FirebaseHelper
//    {
//        private readonly FirebaseClient firebase;

//        // Firebase bağlantısı için URL'yi parametre olarak alıyoruz
//        public FirebaseHelper()
//        {
//         //   firebase = new FirebaseClient("https://vschatapp-88e8d-default-rtdb.firebaseio.com/");
//        }


//        // Test mesajını Firebase'e gönder
//        public async Task<bool> SendMessageAsync(int fromUserId, int toUserId, string messageText)
//        {
//            try
//            {
//                var result = await firebase
//                    .Child("messages")  // Mesajlar kısmına veri ekliyoruz
//                    .PostAsync(new
//                    {
//                        FromUserId = fromUserId,
//                        ToUserId = toUserId,
//                        MessageText = messageText,
//                        SentTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")  // Şu anki tarihi gönderiyoruz
//                    });

//                // Mesaj başarıyla gönderildiyse true döndürüyoruz
//                return true;
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Mesaj gönderme hatası: {ex.Message}");
//                return false;  // Hata durumunda false döndürüyoruz
//            }
//        }

//    }
//}
