using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Acr.UserDialogs;
using News.Helper;
using Newtonsoft.Json;
using Plugin.Media;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace News
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewsPage : TabbedPage
    {
        private const string url = "http://newsapi.org/v2/everything?q=bitcoin&from=2020-11-01&sortBy=publishedAt&apiKey=79ccb5b55f694076b86586c90ffbc5eb";

        private HttpClient _Client = new HttpClient();

        private NewsDTO _news = new NewsDTO();

        bool isLoad = true;

        public NewsPage()
        {
            InitializeComponent();

            uploadImg.Clicked += UploadImg_Clicked;

            CurrentPageChanged += NewsPage_CurrentPageChanged;

            newsListView.ItemTapped += NewsListView_ItemTapped;

        }

        private void NewsListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var selectedData = e.Item as Article;

            NavigationHelper.GotoNextPage(new WebView(selectedData.url));

        }

        private void NewsPage_CurrentPageChanged(object sender, EventArgs e)
        {
            try
            {
                var tabbedPage = (TabbedPage)sender;

                var Title = tabbedPage.CurrentPage.Title;

                if (Title == "News")
                {
                    SyncNewsData();
                }
            }
            catch (Exception ex)
            {

            }
        }

        private async void UploadImg_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    await DisplayAlert("No Camera", "No Camera Avaliable", "OK");
                    return;
                }

                await CrossMedia.Current.Initialize();
                var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium,
                    Directory = "ProfileImages",
                    Name = String.Format("PHOTO.TRANSACTION.{0}" + ".jpg", DateTime.Now.ToString("MMM_dd_yyyy_HHmmss")),
                });

                if (file == null)
                {
                    return;
                }

                Device.BeginInvokeOnMainThread(async () =>
                {
                    //await DisplayAlert("File Location", file.Path, "OK");
                    await Task.Delay(1);
                    Image image = new Image();
                    image.Aspect = Aspect.AspectFit;
                    image.HeightRequest = 40;
                    image.WidthRequest = 40;
                    image.HorizontalOptions = LayoutOptions.CenterAndExpand;
                    image.VerticalOptions = LayoutOptions.CenterAndExpand;

                    //DependencyService.Get<IImageHelper>().CompressImage(file.Path);

                    //byte[] imgBytes = System.IO.File.ReadAllBytes(file.Path);
                    //string _img = Convert.ToBase64String(imgBytes);

                    //image.Source = ImageSource.FromStream(() =>
                    //{
                    //    var stream = file.GetStream();
                    //    _imgPath.Add(file.Path);
                    //    file.Dispose();
                    //    return stream;
                    //});

                    //var tap = new TapGestureRecognizer();
                    //tap.Tapped += Image_Tapped;
                    //tap.NumberOfTapsRequired = 1;
                    //image.GestureRecognizers.Add(tap);
                    //slBolImageContainer.Children.Add(image);
                    //_bolImgString.Add(_img);
                });
            }
            catch (Exception ex)
            {

            }
        }

        private async void SyncNewsData()
        {
            try
            {
                await Task.Run(async () => {

                    while (isLoad)
                    {
                        try
                        {
                            var current = Connectivity.NetworkAccess;
                            var profiles = Connectivity.ConnectionProfiles;
                            if (current == NetworkAccess.Internet || profiles.Contains(ConnectionProfile.WiFi))
                            {

                                UIHelper.ShowProgressDialog(Task.Run(async() =>
                                {
                                    await LoadNewsData();

                                }), "Loading Data..");

                            }

                            await Task.Delay(10000);

                        }
                        catch (Exception ex)
                        {
                           
                        }
                    }
                });

            }
            catch (Exception ex)
            {

            }
        }

        private async Task LoadNewsData()
        {
            try
            {
                var client = new System.Net.Http.HttpClient();

                var response = await client.GetAsync(string.Format(url));

                string contactsJson = await response.Content.ReadAsStringAsync();

                var Items = JsonConvert.DeserializeObject<NewsDTO>(contactsJson);

                Device.BeginInvokeOnMainThread(() =>
                {
                    newsListView.ItemsSource = Items.articles;
                });

              

            }
            catch (Exception ex)
            {

            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            isLoad = true;

            //await LoadNewsData();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            isLoad = false;
        }
    }

}
