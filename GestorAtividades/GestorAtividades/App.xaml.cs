
using GestorAtividades.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.IO;
using Plugin.LocalNotification;

namespace GestorAtividades
{
    public partial class App : Application
    {



        private static SQLiteHelper db;
        public static SQLiteHelper MyDatabase
        {

            get
            {
                if (db == null)
                {
                    db = new SQLiteHelper(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "BD_GestorAtividades.db3"));
                }
                return db;
            }

        }

        public App()
        {
            InitializeComponent();

            
            MainPage = new AppShell();
           
        }


      

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
