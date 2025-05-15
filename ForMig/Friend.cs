using System.ComponentModel.DataAnnotations.Schema;

namespace ForMig
{
    public class Friend
    {
        
            public int Id { get; set; }

            public int UserId1 { get; set; }  // Arkadaşlığı başlatan
            public int UserId2 { get; set; }  // Eklenen kişi


        [ForeignKey("UserId1")]
        public User User1 { get; set; }

        [ForeignKey("UserId2")]
        public User User2 { get; set; }

    }


}
