using GestorAtividades.Models;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GestorAtividades.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class donePage : ContentPage
    {
        public donePage()
        {
            InitializeComponent();



        }

        protected override async void OnAppearing()
        {
            try
            {
                base.OnAppearing();
                doneCollectionView.ItemsSource = await App.MyDatabase.ReadDoneActivity();

            }
            catch
            {

            }
        }

   

        private async void mudar(object sender, EventArgs e)
        {

            //variaveis locais que ficam temporariamente com a atividade escolhida
            Models.doneTable doneActivity;
            var item = sender as SwipeItem;
            var atividade = item.CommandParameter as doneTable;
            doneActivity = atividade as doneTable;
            
            var result = await DisplayAlert("Mudar o estado", "Escolha o estado atual da tarefa",  "DOING", "TO DO");

            switch (result) {

                case true:

                    await App.MyDatabase.InsertActivity(new Models.doingTable
                    {
                        Name = doneActivity.Name,
                        Description = doneActivity.Description,
                        DataInicio = doneActivity.DataInicio,
                        DataFim = doneActivity.DataFim,
                        Tempo = doneActivity.Tempo,
                        Alarme = doneActivity.Alarme,
                        NotificationID = doneActivity.NotificationID,
                    });
                    break;

                case false:

                    await App.MyDatabase.InsertActivity(new Models.todoTable
                    {
                        Name = doneActivity.Name,
                        Description = doneActivity.Description,
                        DataInicio = doneActivity.DataInicio,
                        DataFim = doneActivity.DataFim,
                        Tempo = doneActivity.Tempo,
                        Alarme = doneActivity.Alarme,
                        NotificationID = doneActivity.NotificationID,
                    });

                   

                    break;

            }


            await App.MyDatabase.DeleteActivity(doneActivity);
            doneCollectionView.ItemsSource = await App.MyDatabase.ReadDoneActivity(); //atualizar

        }

        private async void adicionar(object sender, EventArgs e)
        {
            string categoria = "done";

        
            //REDIRECIONAR PARA OUTRA PAGINA
            await Navigation.PushModalAsync(new adicionar(categoria));
        }

        private async void apagar(object sender, EventArgs e)
        {

            var item = sender as SwipeItem;
            var atividade = item.CommandParameter as doneTable; //passar o item recebido para tipo atividade DONE

            var result = await DisplayAlert("Atenção", $"Tem a certeza que pertende apagar {atividade.Name} da lista?", "Sim", "Não");
            if (result)
            {

                await App.MyDatabase.DeleteActivity(atividade); //apagar o campo da tabela 
                doneCollectionView.ItemsSource = await App.MyDatabase.ReadDoneActivity(); //atualizar
            }
        }

        private async void ver(object sender, EventArgs e)
        {

            var item = sender as SwipeItem;
            var atividade = item.CommandParameter as doneTable; //passar o item recebido para tipo atividade DONE

            int id = atividade.Id;//por numa variavel o id da tarefa
            string categoria = "done";


            //REDIRECIONAR PARA OUTRA PAGINA
            await Navigation.PushModalAsync(new verModal(categoria, id, atividade));//enviar categoria e id
        }

        private async void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            doneCollectionView.ItemsSource = await App.MyDatabase.SearchDoneActivity(e.NewTextValue);
        }
    }
}