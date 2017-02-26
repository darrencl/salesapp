using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesApp.Core.Interfaces
{
    public interface IDialogService
    {
        //Shows a dialog to the user, with a chosen message and title
        Task<bool> Show(string message, string title);

        // Shows a dialog to the user, with a chosen message, title, confirm button and cancel button     
        Task<bool> Show(string message, string title, string confirmButton, string cancelButton);
    }
}
