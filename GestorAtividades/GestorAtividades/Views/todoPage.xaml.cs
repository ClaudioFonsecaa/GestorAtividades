using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using GestorAtividades.Models;
using static SQLite.SQLite3;

namespace GestorAtividades.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class todoPage : ContentPage
    {
        public todoPage()
        {
            InitializeComponent();
        }


        protected override async void OnAppearing()
        {
            try
            {
                base.OnAppearing();
                todoCollectionView.ItemsSource = await App.MyDatabase.ReadTodoActivity();

            }
            catch
            {

            }
        }

        private async void adicionar(object sender, EventArgs e)
        {
            string categoria = "todo";

            //REDIRECIONAR PARA OUTRA PAGINA
            await Navigation.PushModalAsync(new adicionar(categoria));
        }

        private async void apagar(object sender, EventArgs e)
        {

            var item = sender as SwipeItem;
            var atividade = item.CommandParameter as todoTable; //passar o item recebido para tipo atividade TO DO

            var result = await DisplayAlert("Atenção", $"Tem a certeza que pertende apagar {atividade.Name} da lista?", "Sim", "Não");
            if (result)
            {

                await App.MyDatabase.DeleteActivity(atividade); //apagar o campo da tabela 
                todoCollectionView.ItemsSource = await App.MyDatabase.ReadTodoActivity(); //atualizar
            }
        }

        private async void mudar(object sender, EventArgs e)
        {


            //variaveis locais que ficam temporariamente com a atividade escolhida
            Models.todoTable todoActivity;
            var item = sender as SwipeItem;
            var atividade = item.CommandParameter as todoTable;
            todoActivity = atividade as todoTable;


            var result = await DisplayAlert("Mudar o estado", "Escolha o estado atual da tarefa", "DOING", "DONE");

            switch (result)
            {

                case true:

                    await App.MyDatabase.InsertActivity(new Models.doingTable
                    {
                        Name = todoActivity.Name,
                        Description = todoActivity.Description,
                        DataInicio = todoActivity.DataInicio,
                        DataFim = todoActivity.DataFim,
                        Tempo = todoActivity.Tempo,
                        Alarme = todoActivity.Alarme,
                        NotificationID = todoActivity.NotificationID,
                    });

                    break;

                case false:

                    await App.MyDatabase.InsertActivity(new Models.doneTable
                    {
                        Name = todoActivity.Name,
                        Description = todoActivity.Description,
                        DataInicio = todoActivity.DataInicio,
                        DataFim = todoActivity.DataFim,
                        Tempo = todoActivity.Tempo,
                        Alarme = todoActivity.Alarme,
                        NotificationID = todoActivity.NotificationID,
                    });

                    break;
            }

            await App.MyDatabase.DeleteActivity(todoActivity);
            todoCollectionView.ItemsSource = await App.MyDatabase.ReadTodoActivity(); //atualizar  

        }

        private async void ver(object sender, EventArgs e)
        {
            var item = sender as SwipeItem;
            var atividade = item.CommandParameter as todoTable; //passar o item recebido para tipo atividade DONE

            int id = atividade.Id;//por numa variavel o id da tarefa
            string categoria = "todo";


            //REDIRECIONAR PARA OUTRA PAGINA
            await Navigation.PushModalAsync(new verModal(categoria, id, atividade));//enviar categoria e id
        }

        private async void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            todoCollectionView.ItemsSource = await App.MyDatabase.SearchTodoActivity(e.NewTextValue);
        }
    }
}