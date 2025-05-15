using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1
{
public static class Constants
    {
        public const string DatabaseFilename = "mydatabase.db3";

        public static string DatabasePath
        {
            get
            {
                string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                return Path.Combine(folderPath, DatabaseFilename);
            }
        }
    }

}
