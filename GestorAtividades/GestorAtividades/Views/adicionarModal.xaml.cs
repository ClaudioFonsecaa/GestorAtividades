using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Android.App;
using Android.Graphics;
using Android.Views;
using GestorAtividades.Models;
using Plugin.LocalNotification;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static Android.Graphics.Paint;
using Color = Xamarin.Forms.Color;

namespace GestorAtividades.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class adicionar : ContentPage
    {

        string categoria;
        Color cor;
        int idNotificacao;

        public adicionar()
        {
            InitializeComponent();
        }

        public adicionar(string cat) //RECEBER CATEGORIA QUE VAMOS ADICIONAR
        {
            InitializeComponent();
            categoria = cat;
        }

        protected override void OnAppearing()
        {

            //Definir a minima data para a data atual
            etDate.MinimumDate = DateTime.Now;

            try
            {


                base.OnAppearing();



                switch (categoria)
                {
                    case "todo":

                        cor = Color.FromHex("#FF0000");
                        lbType.Text = "TO DO";
                        lbType.TextColor = cor;

                        break;
                    case "done":

                        cor = Color.FromHex("#008000");
                        lbType.Text = "DONE";
                        lbType.TextColor = cor;

                        break;
                    case "doing":

                        cor = Color.FromHex("#0000FF");
                        lbType.Text = "DOING";
                        lbType.TextColor = cor;

                        break;

                }

                frame.BorderColor = cor;
            }
            catch
            {

            }
        }



        async private void adicionarTarefa(object sender, EventArgs e) //Função para adicionar atividades
        {

            //VERIFICAR SE OS CAMPOS NAO ESTAO VAZIOS
            if (string.IsNullOrWhiteSpace(etNome.Text) || string.IsNullOrWhiteSpace(etDescricao.Text))
            {
                await DisplayAlert("Erro", "Preencha todos os campos pff!", "OK");

            }
            else
            {

                switch (categoria)
                {
                    case "todo":

                        //ADICIONAR atividade to do
                        await App.MyDatabase.InsertActivity(new Models.todoTable
                        {
                            Name = etNome.Text,
                            Description = etDescricao.Text,
                            DataFim = etDate.Date.ToString("dd/MM/yyyy"),
                            DataInicio = DateTime.Now.ToString("dd/MM/yyyy"),
                            Tempo = etTime.Time.ToString(),
                            Alarme = btAlarme.IsToggled,
                            NotificationID = Guid.NewGuid().GetHashCode()//gerar um código unique

                        }); ;

                        await DisplayAlert("Sucesso!", "Tarefa adicionada com sucesso!", "OK");
                        break;
                    case "done":

                        //ADICIONAR atividade to do
                        await App.MyDatabase.InsertActivity(new Models.doneTable
                        {
                            Name = etNome.Text,
                            Description = etDescricao.Text,
                            DataFim = etDate.Date.ToString("dd/MM/yyyy"),
                            DataInicio = DateTime.Now.ToString("dd/MM/yyyy"),
                            Tempo = etTime.Time.ToString(),
                            Alarme = btAlarme.IsToggled,
                            NotificationID = Guid.NewGuid().GetHashCode()//gerar um código unique

                        });
                        await DisplayAlert("Sucesso!", "Tarefa adicionada com sucesso!", "OK");
                        break;
                    case "doing":

                        //ADICIONAR atividade to do
                        await App.MyDatabase.InsertActivity(new Models.doingTable
                        {
                            Name = etNome.Text,
                            Description = etDescricao.Text,
                            DataFim = etDate.Date.ToString("dd/MM/yyyy"),
                            DataInicio = DateTime.Now.ToString("dd/MM/yyyy"),
                            Tempo = etTime.Time.ToString(),
                            Alarme = btAlarme.IsToggled,
                            NotificationID = Guid.NewGuid().GetHashCode()//gerar um código unique

                        });
                        await DisplayAlert("Sucesso!", "Tarefa adicionada com sucesso!", "OK");
                        break;

                }


                //SE O ALARME FOI ATIVADO
                if (btAlarme.IsToggled)
                {


                    switch (categoria)
                    {
                        case "todo":

                            //verificar o notification ID que foi posto na BD e por para uma variavel
                            List<todoTable> listaTodo = await App.MyDatabase.ReadTodoActivity();

                            foreach (var item in listaTodo)
                            {

                                idNotificacao = item.NotificationID;

                            }


                            break;
                        case "done":

                            //verificar o notification ID que foi posto na BD e por para uma variavel
                            List<doneTable> listaDone = await App.MyDatabase.ReadDoneActivity();

                            foreach (var item in listaDone)
                            {

                                idNotificacao = item.NotificationID;

                            }


                            break;
                        case "doing":

                            //verificar o notification ID que foi posto na BD e por para uma variavel
                            List<doingTable> listaDoing = await App.MyDatabase.ReadDoingActivity();

                            foreach (var item in listaDoing)
                            {

                                idNotificacao = item.NotificationID;

                            }

                            break;

                    }


                    

                    var notification = new NotificationRequest
                    {
                        BadgeNumber = 1,
                        Description = $"Acabou a deadline da atividade {etNome.Text} !",
                        Title = "Gestor Atividades",
                        ReturningData = categoria + "," + idNotificacao.ToString() + "," + etNome.Text, //ID NA TABELA
                        NotificationId = idNotificacao, //atraves disto cancelar o notification
                        NotifyTime = etDate.Date.AddMinutes(etTime.Time.TotalMinutes),

                    };

                    NotificationCenter.Current.Show(notification);

                }


                await Navigation.PopModalAsync();

            }





        }

        private async void fechar(object sender, EventArgs e)
        {

            //função para fechar o modal
            await Navigation.PopModalAsync();
        }





    }
}