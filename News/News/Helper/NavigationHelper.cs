using System;
using Xamarin.Forms;
namespace News
{
    public class NavigationHelper
    {
        public static void GotoNextPage(Page nextPage)
        {
            Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(nextPage));
        }

        public static void GotoPreviousPage()
        {
            Application.Current.MainPage.Navigation.PopModalAsync();
        }

        public static void GoBackTwoPages()
        {
            for (int i = 0; i < 2; i++)
            {
                Application.Current.MainPage.Navigation.PopModalAsync();
            }
        }
    }
}
