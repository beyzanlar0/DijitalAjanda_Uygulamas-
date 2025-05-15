using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1
{
    public class MessageDto
    {
        public int FromUserId { get; set; }
        public int ToUserId { get; set; }
        public string MessageText { get; set; }
        public DateTime SentTime { get; set; }
    }

}
