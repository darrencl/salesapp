using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SalesApp.Core.Interfaces;
using MvvmCross.Platform;
using MvvmCross.Platform.Droid.Platform;

namespace SalesApp.Droid.Services
{
    public class DialogService : IDialogService
    {
        Dialog dialog = null;
        public async Task<bool> Show(string message, string title)
        {
            return await Show(message, title, "OK", "Cancel");
        }

        public async Task<bool> Show(string message, string title, string confirmButton, string cancelButton)
        {
            bool buttonPressed = false;
            bool chosenOption = false;
            Application.SynchronizationContext.Post(_ =>
            {
                var mvxTopActivity = Mvx.Resolve<IMvxAndroidCurrentTopActivity>();
                Android.App.AlertDialog.Builder alertDialog = new AlertDialog.Builder(mvxTopActivity.Activity);
                alertDialog.SetTitle(title);
                alertDialog.SetMessage(message);
                alertDialog.SetNegativeButton(cancelButton, (s, args) =>
                {
                    chosenOption = false;
                });
                alertDialog.SetPositiveButton(confirmButton, (s, args) =>
                {
                    chosenOption = true;
                });

                dialog = alertDialog.Create();
                dialog.DismissEvent += (object sender, EventArgs e) =>
                {
                    buttonPressed = true;
                    dialog.Dismiss();
                };
                dialog.Show();
            }, null);
            while (!buttonPressed)
            {
                await Task.Delay(100);
            }
            return chosenOption;
        }
    }
    
}