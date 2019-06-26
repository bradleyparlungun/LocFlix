using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace LocFlix.Uwp.Helpers
{
    class MessageDialogManager
    {
        public enum Types
        {
            Error = 0,
            Success = 1,
            Information = 2,
            None = 3
        }

        public static async Task MessageDialog(string message, Types types = Types.None)
        {
            var messageDialog = types != Types.None ? new MessageDialog(types.ToString() + ": " + message) : new MessageDialog(message);

            messageDialog.Commands.Add(new UICommand("Ok", null));

            messageDialog.DefaultCommandIndex = 0;

            await messageDialog.ShowAsync();
        }
    }
}
