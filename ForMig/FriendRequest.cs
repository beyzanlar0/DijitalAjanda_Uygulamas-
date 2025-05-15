namespace ForMig
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
