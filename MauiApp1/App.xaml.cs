using SQLite;

namespace MauiApp1
{
    public partial class App : Application
    {
        public static User CurrentUser { get; set; }
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
            
        }
        public static SQLiteAsyncConnection Database { get; private set; }

      

    }
}
