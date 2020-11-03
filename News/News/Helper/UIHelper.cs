using System;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Xamarin.Forms;

namespace News.Helper
{
    public class UIHelper
    {
        //Display A Progress Dialog untill the task sent as a parameter gets completed
        public static void ShowProgressDialog(Task TaskName, string DisplayMessage)
        {
            try
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    using (UserDialogs.Instance.Loading(DisplayMessage, null, null, true, MaskType.Black))
                    {
                        await Task.Run(async () => await TaskName);
                       
                        UserDialogs.Instance.HideLoading();
                    }
                    //UserDialogs.Instance.ShowLoading(DisplayMessage, MaskType.Black);
                    //    await Task.Run(async () => await TaskName);
                    //    UserDialogs.Instance.HideLoading();
                });
            }
            catch (Exception ex)
            {
              
            }
        }
    }
}
