using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using GestorAtividades.Droid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestorAtividades;
using Xamarin.Essentials;
using Xamarin.Forms;

[assembly: Dependency(typeof(AndroidAlert))]
namespace GestorAtividades.Droid
{
    public class AndroidAlert : IAlert
    {
        public Task<string> Display(string title, string message, string firstButton, string secondButton, string cancel)
        {
            var taskCompletionSource = new TaskCompletionSource<string>();
            var alertBuilder = new AlertDialog.Builder(Platform.CurrentActivity);

            alertBuilder.SetTitle(title);
            alertBuilder.SetMessage(message);

            alertBuilder.SetPositiveButton(firstButton, (senderAlert, args) =>
            {
                taskCompletionSource.SetResult(firstButton);
            });

            alertBuilder.SetNegativeButton(secondButton, (senderAlert, args) =>
            {
                taskCompletionSource.SetResult(secondButton);
            });

            alertBuilder.SetNeutralButton(cancel, (senderAlery, args) =>
            {
                taskCompletionSource.SetResult(cancel);
            });

            var alertDialog = alertBuilder.Create();
            alertDialog.Show();

            return taskCompletionSource.Task;
        }
    }
}