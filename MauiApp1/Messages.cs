using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1
{
    public class Messages
    {
        public int Id { get; set; }
        public int FromUserId { get; set; }  // Yeni eklendi
        public int ToUserId { get; set; }
        public string FromEmail { get; set; }
        public string ToEmail { get; set; }
        public string MessageText { get; set; }
        public DateTime SentTime { get; set; }

        // Message'ın iki tarafı var: Gönderen ve alıcı
        public virtual User FromUser { get; set; }
        public virtual User ToUser { get; set; }

      
    }

}
