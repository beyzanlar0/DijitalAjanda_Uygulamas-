using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1
{
    public class FriendRequest
    {
        public int Id { get; set; }
        public string FromUser { get; set; }
        public string ToUser { get; set; }
        public string Status { get; set; }
        public DateTime RequestDate { get; set; }
        public bool IsAccepted { get; set; }
    }
}
