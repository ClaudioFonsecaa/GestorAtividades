using GestorAtividades.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestorAtividades;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GestorAtividades.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class verModal : ContentPage
    {

        string categoria;
        int Id=0;
        Models.doneTable doneActivity;
        Models.doingTable doingActivity;
        Models.todoTable todoActivity;

        public verModal()
        {
            InitializeComponent();

        }

        public verModal(string cat, int id, object atividade) //RECEBER CATEGORIA QUE VAMOS ADICIONAR
        {
            InitializeComponent();
            categoria = cat;
            Id = id;
            

            switch (categoria)
            {
                case "todo":

                    todoActivity = atividade as todoTable;
                    estado.Text = "TO DO";
                    estado.TextColor = Color.Red;
                    borda.BorderColor = Color.Red;

                    break;
                case "done":

                    doneActivity = atividade as doneTable; 
                    estado.Text = "DONE";
                    estado.TextColor = Color.Green;
                    borda.BorderColor = Color.Green;


                    break;
                case "doing":

                    doingActivity = atividade as doingTable;
                    estado.Text = "DOING";
                    estado.TextColor = Color.Blue;
                    borda.BorderColor = Color.Blue;

                    break;

            }
        }

        protected override async void OnAppearing()
        {
            try
            {
                base.OnAppearing();

               
                switch (categoria)
                {
                    case "todo":

                        dataCollectionView.ItemsSource = await App.MyDatabase.ReadIDTodoActivity(categoria, Id);

                        break;
                    case "done":

                        dataCollectionView.ItemsSource = await App.MyDatabase.ReadIDDoneActivity(categoria, Id);


                        break;
                    case "doing":

                        dataCollectionView.ItemsSource = await App.MyDatabase.ReadIDDoingActivity(categoria, Id);

                        break;

                }


                
                
               

            }
            catch
            {

            }
        }

        private async void btEdit_Clicked(object sender, EventArgs e)
        {

            //REDIRECIONAR PARA OUTRA PAGINA
            

            switch (categoria)
            {
                case "todo":

                    //todoActivity = sender as todoTable;

                    await Navigation.PushModalAsync(new editModal(categoria, Id, todoActivity));

                    break;
                case "done":

                    //doneActivity = sender as doneTable;

                    await Navigation.PushModalAsync(new editModal(categoria, Id, doneActivity));


                    break;
                case "doing":

                    //doingActivity = sender as doingTable;

                    await Navigation.PushModalAsync(new editModal(categoria, Id, doingActivity));

                    break;

            }


        }

        private async void btDelete_Clicked(object sender, EventArgs e)
        {

            switch (categoria)
            {
                case "todo":


                    var resultA = await DisplayAlert("Atenção", $"Tem a certeza que pertende apagar {todoActivity.Name} da lista?", "Sim", "Não");
                    if (resultA)
                    {
                        await App.MyDatabase.DeleteActivity(todoActivity); //apagar o campo da tabela 
                        await Navigation.PopModalAsync();
                    }

                    break;

                case "done":

                    var resultB = await DisplayAlert("Atenção", $"Tem a certeza que pertende apagar {doneActivity.Name} da lista?", "Sim", "Não");
                    if (resultB)
                    {
                        await App.MyDatabase.DeleteActivity(doneActivity); //apagar o campo da tabela 
                        await Navigation.PopModalAsync();
                    }



                    break;
                case "doing":

                    var resultC = await DisplayAlert("Atenção", $"Tem a certeza que pertende apagar {doingActivity.Name} da lista?", "Sim", "Não");
                    if (resultC)
                    {
                        await App.MyDatabase.DeleteActivity(doingActivity); //apagar o campo da tabela 
                        await Navigation.PopModalAsync();
                    }

                    break;

            }


        }

        private async void btClose_Clicked(object sender, EventArgs e)
        {
            //função para fechar o modal
            await Navigation.PopModalAsync();
        }
    }
}