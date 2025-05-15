using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace MauiApp1
{
    public class Task
    {
        
            public int Id { get; set; }
            public string GunlukGorev { get; set; }  
            public string HaftalikHedefler { get; set; }  
            public string AylikHedefler { get; set; }  
            public string YillikHedefler { get; set; }  
            public bool TamamlandiMi{ get; set; }


        

    }
}
