
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Firebase.Database;
//using Firebase.Database.Query;
//using Microsoft.Maui.Storage;

//namespace MauiApp1
//{
//    public class FirebaseService
//    {

//        private readonly FirebaseClient firebaseClient;


//        private FirebaseHelper firebaseHelper;

//        public FirebaseService(string firebaseUrl)
//        {
//            // Firebase URL'nizi burada kullanıyorsunuz
//            firebaseClient = new FirebaseClient(firebaseUrl);
//        }


//        // Mesaj gönderme
//        // Mesaj gönderme
//        public async Task<bool> SendMessageAsync(MessageDto messageDTO)
//        {
//            try
//            {
//                var message = new MessageDto
//                {
//                    FromUserId = messageDTO.FromUserId,
//                    ToUserId = messageDTO.ToUserId,
//                    MessageText = messageDTO.MessageText,
//                    SentTime = messageDTO.SentTime
//                };

//                // Firebase'e mesaj ekliyoruz
//                await firebaseClient
//                    .Child("messages")
//                    .PostAsync(message);

//                return true;
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Hata: {ex.Message}");
//                return false;
//            }
//        }

//        // Tüm mesajları alma
//        public async Task<List<MessageDto>> GetAllMessagesAsync()
//        {
//            var messages = await firebaseClient
//                .Child("messages")
//                .OnceAsync<MessageDto>();

//            List<MessageDto> messageList = new List<MessageDto>();

//            foreach (var message in messages)
//            {
//                // FirebaseMessage'dan MessageDTO'ya dönüştürüyoruz
//                var messageDTO = new MessageDto
//                {
//                    FromUserId = message.Object.FromUserId,
//                    ToUserId = message.Object.ToUserId,
//                    MessageText = message.Object.MessageText,
//                    SentTime = message.Object.SentTime
//                };

//                messageList.Add(messageDTO);
//            }

//            return messageList;
//        }

//        // Kullanıcıya ait mesajları alma
//        public async Task<List<MessageDto>> GetMessagesByUserAsync(int userId)
//        {
//            var messages = await firebaseClient
//                .Child("messages")
//                .OrderBy("FromUserId") // or OrderBy("ToUserId") gibi farklı sıralamalar yapılabilir
//                .EqualTo(userId)
//                .OnceAsync<MessageDto>();

//            List<MessageDto> messageList = new List<MessageDto>();

//            foreach (var message in messages)
//            {
//                // FirebaseMessage'dan MessageDTO'ya dönüştürüyoruz
//                var messageDTO = new MessageDto
//                {
//                    FromUserId = message.Object.FromUserId,
//                    ToUserId = message.Object.ToUserId,
//                    MessageText = message.Object.MessageText,
//                    SentTime = message.Object.SentTime
//                };

//                messageList.Add(messageDTO);
//            }

//            return messageList;
//        }
//    }
//}

