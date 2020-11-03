using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Acr.UserDialogs;
using News.Helper;
using Xamarin.Forms;

namespace News
{
    public partial class WebView : ContentPage
    {
        public WebView(string url)
        {
            InitializeComponent();

            menuLogo.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        NavigationHelper.GotoPreviousPage();
                    });
                })
            });

            Device.BeginInvokeOnMainThread(() =>
            {
                UIHelper.ShowProgressDialog(Task.Run(() =>
                {
                    loadURL.Source = url;

                }), "Loading Data..");

            });

        }
    }
}
