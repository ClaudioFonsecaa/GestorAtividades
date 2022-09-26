
using GestorAtividades.Models;
using GestorAtividades.Views;
using Plugin.LocalNotification;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace GestorAtividades
{
    public partial class AppShell : Xamarin.Forms.Shell
    {

        public Dictionary<string, Type> Routes { get; private set; } = new Dictionary<string, Type>();
        public ICommand HelpCommand => new Command<string>(async (url) => await Launcher.OpenAsync(url));


        public AppShell()
        {
            InitializeComponent();
            RegisterRoutes();
            BindingContext = this;

            NotificationCenter.Current.NotificationReceived += Current_NotificationReceived;
            //NotificationCenter.Current.NotificationTapped += Current_NotificationTapped; PARA IOS

        }

        

        //private async void Current_NotificationTapped(NotificationTappedEventArgs e)
        //{


        //    string[] subs = e.Data.Split(',');//dividir a informação enviada

        //    string CAT = subs[0];
        //    int NotificationID = Int32.Parse(subs[1]);//converter a informação que vem em integer
        //    string nome = subs[2];


        //    //verificar em todas as BD e desligar toogle
        //    await App.MyDatabase.UpdateTodoAlarmeByID(NotificationID);
        //    await App.MyDatabase.UpdateDoneAlarmeByID(NotificationID);
        //    await App.MyDatabase.UpdateDoingAlarmeByID(NotificationID);


        //    Device.BeginInvokeOnMainThread(() =>
        //    {
        //        DisplayAlert("Aviso", $"Acabou a deadline da atividade {nome} !", "Entendido");
        //    });


        //}

        private async void Current_NotificationReceived(NotificationReceivedEventArgs e)
        {
            
            string[] subs = e.Data.Split(',');//dividir a informação enviada

            string CAT = subs[0];
            int NotificationID = Int32.Parse(subs[1]);//converter a informação que vem em integer
            string nome = subs[2];

            
            //verificar em todas as BD e desligar toogle
            await App.MyDatabase.UpdateTodoAlarmeByID(NotificationID);
            await App.MyDatabase.UpdateDoneAlarmeByID(NotificationID);
            await App.MyDatabase.UpdateDoingAlarmeByID(NotificationID);


            Device.BeginInvokeOnMainThread(() =>
            {
                DisplayAlert("Aviso", $"Acabou a deadline da atividade {nome} !", "Entendido");
            });

            //PROCURAR FUNÇÕES PARA QUANDO INICIAR MOSTRAR DISPLAY ALERT 


        }


        void RegisterRoutes()
        {
            /*
            Routes.Add("donedetails", typeof(donedetails));
            Routes.Add("tododetails", typeof(tododetails));
            Routes.Add("doingdetails", typeof(doingdetails));*/

            foreach (var item in Routes)
            {
                Routing.RegisterRoute(item.Key, item.Value);
            }
        }


    }
}
