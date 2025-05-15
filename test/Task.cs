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
            public string Baslik { get; set; }  
            public string GorevAciklamasi { get; set; }  
            public DateTime TeslimTarihi { get; set; }  
            public bool TamamlandıMı { get; set; }

           
            public int UserId { get; set; }  
        

    }
}
