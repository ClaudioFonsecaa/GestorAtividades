using GestorAtividades.Models;
using Plugin.LocalNotification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GestorAtividades.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class editModal : ContentPage
    {

        string cat;
        int Id;
        int idNotificacao;

        Models.doneTable doneActivity;
        Models.doingTable doingActivity;
        Models.todoTable todoActivity;

        public editModal()
        {
            InitializeComponent();
        }

        public editModal(string categoria, int id, object atividade)
        {
            InitializeComponent();
            cat = categoria;
            Id = id;


            switch (categoria)
            {
                case "todo":

                    todoActivity = atividade as todoTable;

                    etState.SelectedIndex = 1;
                    etNome.Text = todoActivity.Name;
                    etDate.Date = DateTime.ParseExact(todoActivity.DataFim, "dd/MM/yyyy", null);
                    etTime.Time = TimeSpan.Parse(todoActivity.Tempo);
                    etDescricao.Text = todoActivity.Description;
                    btAlarme.IsToggled = todoActivity.Alarme;
                    idNotificacao = todoActivity.NotificationID;

                    break;
                case "done":

                    doneActivity = atividade as doneTable;

                    etState.SelectedIndex = 0;
                    etNome.Text = doneActivity.Name;
                    etDate.Date = DateTime.ParseExact(doneActivity.DataFim, "dd/MM/yyyy", null);
                    etTime.Time = TimeSpan.Parse(doneActivity.Tempo);
                    etDescricao.Text = doneActivity.Description;
                    btAlarme.IsToggled = doneActivity.Alarme;
                    idNotificacao = doneActivity.NotificationID;


                    break;
                case "doing":

                    doingActivity = atividade as doingTable;

                    etState.SelectedIndex = 2;
                    etNome.Text = doingActivity.Name;
                    etDate.Date = DateTime.ParseExact(doingActivity.DataFim, "dd/MM/yyyy", null);
                    etTime.Time = TimeSpan.Parse(doingActivity.Tempo);
                    etDescricao.Text = doingActivity.Description;
                    btAlarme.IsToggled = doingActivity.Alarme;
                    idNotificacao = doingActivity.NotificationID;

                    break;

            }


        }

        protected override void OnAppearing()
        {

            base.OnAppearing();


            //Definir a minima data para a data atual
            etDate.MinimumDate = DateTime.Now;

            
        }



        private async void btClose_Clicked(object sender, EventArgs e)
        {
            //função para fechar o modal
            await Navigation.PopModalAsync();
        }

        private async void btSave_Clicked(object sender, EventArgs e)
        {




            //VERIFICAR SE OS CAMPOS NAO ESTAO VAZIOS
            if (string.IsNullOrWhiteSpace(etNome.Text) || string.IsNullOrWhiteSpace(etDescricao.Text))
            {
                await DisplayAlert("Erro", "Preencha todos os campos pff!", "OK");

            }
            else
            {


                switch (cat)
                {
                    case "todo"://1

                        todoActivity.Name = etNome.Text;
                        todoActivity.Description = etDescricao.Text;
                        todoActivity.DataFim = etDate.Date.ToString("dd/MM/yyyy");
                        todoActivity.Tempo = etTime.Time.ToString();
                        todoActivity.Alarme = btAlarme.IsToggled;


                        if (etState.SelectedIndex != 1)
                        { //SE O ESTADO DA TAREFA FOI ALTERADO, APAGAR DA TABELA X E POR NA TABELA Y


                            //verificar para qual estado o utilizador quer alterar
                            switch (etState.SelectedIndex)
                            {
                                case 0://DONE

                                    await App.MyDatabase.InsertActivity(new Models.doneTable
                                    {
                                        Name = etNome.Text,
                                        Description = etDescricao.Text,
                                        DataFim = etDate.Date.ToString("dd/MM/yyyy"),
                                        Tempo = etTime.Time.ToString(),
                                        Alarme = btAlarme.IsToggled,
                                        NotificationID = todoActivity.NotificationID,
                                        DataInicio = todoActivity.DataInicio,
                                    });

                                    //como foi criada uma nova dizer qual o novo id da notificação
                                    idNotificacao = todoActivity.NotificationID;
                                    await App.MyDatabase.DeleteActivity(todoActivity);


                                    break;
                                case 2: //DOING

                                    await App.MyDatabase.InsertActivity(new Models.doingTable
                                    {
                                        Name = etNome.Text,
                                        Description = etDescricao.Text,
                                        DataFim = etDate.Date.ToString("dd/MM/yyyy"),
                                        Tempo = etTime.Time.ToString(),
                                        Alarme = btAlarme.IsToggled,
                                        NotificationID = todoActivity.NotificationID,
                                        DataInicio=todoActivity.DataInicio,
                                    });

                                    //como foi criada uma nova dizer qual o novo id da notificação
                                    idNotificacao = todoActivity.NotificationID;

                                    await App.MyDatabase.DeleteActivity(todoActivity);


                                    break;

                            }
                            int numModals = Application.Current.MainPage.Navigation.ModalStack.Count;

                            // FECHAR OS 2 MODALS
                            for (int currModal = 0; currModal < numModals; currModal++)
                            {
                                await Application.Current.MainPage.Navigation.PopModalAsync(false);
                            }


                        }
                        else//CASO NÃO EXISTE QUALQUER ALTERAÇÃO NO ESTADO DA TAREFA, ATUALIZAR SÓ OS CAMPOS
                        {

                            await App.MyDatabase.UpdateActivity(todoActivity);
                        }


                        break;
                    case "done"://0

                        doneActivity.Name = etNome.Text;
                        doneActivity.Description = etDescricao.Text;
                        doneActivity.DataFim = etDate.Date.ToString("dd/MM/yyyy");
                        doneActivity.Tempo = etTime.Time.ToString();
                        doneActivity.Alarme = btAlarme.IsToggled;

                        if (etState.SelectedIndex != 0)
                        { //SE O ESTADO DA TAREFA FOI ALTERADO, APAGAR DA TABELA X E POR NA TABELA Y


                            //verificar para qual estado o utilizador quer alterar
                            switch (etState.SelectedIndex)
                            {

                                case 1://TODO

                                    await App.MyDatabase.InsertActivity(new Models.todoTable
                                    {
                                        Name = etNome.Text,
                                        Description = etDescricao.Text,
                                        DataFim = etDate.Date.ToString("dd/MM/yyyy"),
                                        Tempo = etTime.Time.ToString(),
                                        Alarme = btAlarme.IsToggled,
                                        NotificationID = doneActivity.NotificationID,
                                        DataInicio = doneActivity.DataInicio,
                                    });

                                    //como foi criada uma nova dizer qual o novo id da notificação
                                    idNotificacao = doneActivity.NotificationID;

                                    await App.MyDatabase.DeleteActivity(doneActivity);


                                    break;
                                case 2: //DOING

                                    await App.MyDatabase.InsertActivity(new Models.doingTable
                                    {
                                        Name = etNome.Text,
                                        Description = etDescricao.Text,
                                        DataFim = etDate.Date.ToString("dd/MM/yyyy"),
                                        Tempo = etTime.Time.ToString(),
                                        Alarme = btAlarme.IsToggled,
                                        NotificationID = doneActivity.NotificationID,
                                        DataInicio=doneActivity.DataInicio,
                                    });

                                    //como foi criada uma nova dizer qual o novo id da notificação
                                    idNotificacao = doneActivity.NotificationID;

                                    await App.MyDatabase.DeleteActivity(doneActivity);


                                    break;

                            }

                            int numModals = Application.Current.MainPage.Navigation.ModalStack.Count;

                            // FECHAR OS 2 MODALS
                            for (int currModal = 0; currModal < numModals; currModal++)
                            {
                                await Application.Current.MainPage.Navigation.PopModalAsync(false);
                            }


                        }
                        else//CASO NÃO EXISTE QUALQUER ALTERAÇÃO NO ESTADO DA TAREFA, ATUALIZAR SÓ OS CAMPOS
                        {

                            await App.MyDatabase.UpdateActivity(doneActivity);
                        }


                        break;
                    case "doing"://2

                        doingActivity.Name = etNome.Text;
                        doingActivity.Description = etDescricao.Text;
                        doingActivity.DataFim = etDate.Date.ToString("dd/MM/yyyy");
                        doingActivity.Tempo = etTime.Time.ToString();
                        doingActivity.Alarme = btAlarme.IsToggled;

                        if (etState.SelectedIndex != 2)
                        { //SE O ESTADO DA TAREFA FOI ALTERADO, APAGAR DA TABELA X E POR NA TABELA Y


                            //verificar para qual estado o utilizador quer alterar
                            switch (etState.SelectedIndex)
                            {
                                case 0://DONE

                                    await App.MyDatabase.InsertActivity(new Models.doneTable
                                    {
                                        Name = etNome.Text,
                                        Description = etDescricao.Text,
                                        DataInicio = doingActivity.DataInicio,
                                        DataFim = etDate.Date.ToString("dd/MM/yyyy"),
                                        Tempo = etTime.Time.ToString(),
                                        Alarme = btAlarme.IsToggled,
                                        NotificationID = doingActivity.NotificationID,
                                        
                                    });

                                    //como foi criada uma nova dizer qual o novo id da notificação
                                    idNotificacao = doingActivity.NotificationID;

                                    await App.MyDatabase.DeleteActivity(doingActivity);


                                    break;
                                case 1://TODO

                                    await App.MyDatabase.InsertActivity(new Models.todoTable
                                    {
                                        Name = etNome.Text,
                                        Description = etDescricao.Text,
                                        DataInicio = doingActivity.DataInicio,
                                        DataFim = etDate.Date.ToString("dd/MM/yyyy"),
                                        Tempo = etTime.Time.ToString(),
                                        Alarme = btAlarme.IsToggled,
                                        NotificationID = doingActivity.NotificationID,
                                        
                                    });

                                    //como foi criada uma nova dizer qual o novo id da notificação
                                    idNotificacao = doingActivity.NotificationID;

                                    await App.MyDatabase.DeleteActivity(doingActivity);


                                    break;

                            }

                            int numModals = Application.Current.MainPage.Navigation.ModalStack.Count;

                            // FECHAR OS 2 MODALS
                            for (int currModal = 0; currModal < numModals; currModal++)
                            {
                                await Application.Current.MainPage.Navigation.PopModalAsync(false);
                            }


                        }
                        else//CASO NÃO EXISTE QUALQUER ALTERAÇÃO NO ESTADO DA TAREFA, ATUALIZAR SÓ OS CAMPOS
                        {
                            
                            await App.MyDatabase.UpdateActivity(doingActivity);

                        }

                        break;
                }


                //SE O ALARME FOI ATIVADO
                if (btAlarme.IsToggled)
                {

                    var notification = new NotificationRequest
                    {
                        BadgeNumber = 1,
                        Description = $"Acabou a deadline da atividade {etNome.Text} !",
                        Title = "Gestor Atividades",
                        ReturningData = cat + "," + idNotificacao+ "," + etNome.Text,//ID NA TABELA
                        NotificationId = idNotificacao , //atraves disto cancelar o notification
                        NotifyTime = etDate.Date.AddMinutes(etTime.Time.TotalMinutes),
                        //para testes, apresentar mensagem 5 seg depois
                        //NotifyTime = DateTime.Now.AddSeconds(10),
                    };

                    NotificationCenter.Current.Show(notification);

                }
                else if(!btAlarme.IsToggled) //DESATIVAR ALARME SE O TOOGLE ESTIVER OFF
                {

                    NotificationCenter.Current.Cancel(idNotificacao);
                }


                await DisplayAlert("Sucesso!", "Tarefa alterada com sucesso!", "OK");
                await Navigation.PopModalAsync();

            }



        }
    }
}