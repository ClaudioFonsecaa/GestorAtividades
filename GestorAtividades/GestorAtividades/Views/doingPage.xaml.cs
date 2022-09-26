using GestorAtividades.Models;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GestorAtividades.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class doingPage : ContentPage
    {
        public doingPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            try
            {
                base.OnAppearing();
                doingCollectionView.ItemsSource = await App.MyDatabase.ReadDoingActivity();

            }
            catch
            {

            }
        }

        private async void adicionar(object sender, EventArgs e)
        {
            string categoria = "doing";

            //REDIRECIONAR PARA OUTRA PAGINA
            await Navigation.PushModalAsync(new adicionar(categoria));
        }

        private async void mudar(object sender, EventArgs e)
        {


            //variaveis locais que ficam temporariamente com a atividade escolhida
            Models.doingTable doingActivity;
            var item = sender as SwipeItem;
            var atividade = item.CommandParameter as doingTable;
            doingActivity = atividade as doingTable;

            var result = await DisplayAlert("Mudar o estado", "Escolha o estado atual da tarefa",  "TO DO", "DONE");

            switch (result)
            {

            
                case true:

                    await App.MyDatabase.InsertActivity(new Models.todoTable
                    {
                        Name = doingActivity.Name,
                        Description = doingActivity.Description,
                        DataInicio = doingActivity.DataInicio,
                        DataFim = doingActivity.DataFim,
                        Tempo = doingActivity.Tempo,
                        Alarme = doingActivity.Alarme,
                        NotificationID = doingActivity.NotificationID,
                    });

  
                    break;

                case false:

                    await App.MyDatabase.InsertActivity(new Models.doneTable
                    {
                        Name = doingActivity.Name,
                        Description = doingActivity.Description,
                        DataInicio = doingActivity.DataInicio,
                        DataFim = doingActivity.DataFim,
                        Tempo = doingActivity.Tempo,
                        Alarme = doingActivity.Alarme,
                        NotificationID = doingActivity.NotificationID,
                    });

                    break;


            }

            await App.MyDatabase.DeleteActivity(doingActivity);
            doingCollectionView.ItemsSource = await App.MyDatabase.ReadDoingActivity(); //atualizar

        }
        private async void apagar(object sender, EventArgs e)
        {

            var item = sender as SwipeItem;
            var atividade = item.CommandParameter as doingTable; //passar o item recebido para tipo atividade DOING

            var result = await DisplayAlert("Atenção", $"Tem a certeza que pertende apagar {atividade.Name} da lista?", "Sim", "Não");
            if (result)
            {

                await App.MyDatabase.DeleteActivity(atividade); //apagar o campo da tabela 
                doingCollectionView.ItemsSource = await App.MyDatabase.ReadDoingActivity(); //atualizar
            }
        }

        private async void ver(object sender, EventArgs e)
        {
            var item = sender as SwipeItem;
            var atividade = item.CommandParameter as doingTable; //passar o item recebido para tipo atividade DOING

            int id = atividade.Id;//por numa variavel o id da tarefa
            string categoria = "doing";


            //REDIRECIONAR PARA OUTRA PAGINA
            await Navigation.PushModalAsync(new verModal(categoria, id, atividade));//enviar categoria e id
        }

        private async void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            doingCollectionView.ItemsSource = await App.MyDatabase.SearchDoingActivity(e.NewTextValue);
        }
    }
}