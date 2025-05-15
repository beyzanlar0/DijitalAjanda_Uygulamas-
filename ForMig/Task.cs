namespace ForMig
{
    public class Task
    {
        
            public int Id { get; set; }
            public string GunlukGorev { get; set; }  
            public string HaftalikHedefler { get; set; }  
            public string AylikHedefler { get; set; }  
            public string YillikHedefler { get; set; }  
            public bool TamamlandiMi{ get; set; }

        public DateTime SelectedDate { get; set; } = DateTime.Today;

        public int UserId { get; set; }  
        

    }
}
