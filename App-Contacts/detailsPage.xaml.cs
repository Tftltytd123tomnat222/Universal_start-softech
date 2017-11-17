using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Popups;
using Windows.UI.Xaml.Media.Imaging;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace App_Contacts
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class detailsPage : Page
    {
        Contact c;
        int id = -1;
        public detailsPage()
        {
            this.InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if(e.Parameter != null)
            {
                // Edit
                id = int.Parse(e.Parameter.ToString());
                c = App.contacts.Where(x => x.Id == id).FirstOrDefault();
            }
            else
            {
                // Add new
                c = new Contact();
                // ẩn nút Delete
                BtnDelete.Visibility = Visibility.Collapsed;
            }
            // lấy contact đổ lên trên form => 
            this.DataContext = c;
        }
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            // nếu ngta thêm mới
            if ( id == -1)
            {
                // add
                App.contacts.Add(c);
            }
            App.SaveXml();
            this.Frame.GoBack();
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }

        async private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new ContentDialog()
            {
                Title = "Delete?",
                Content = "Are you sure to delete this contacts?",
                CloseButtonText = "No",
                PrimaryButtonText = "Yes"
            };
            if( await dialog.ShowAsync() == ContentDialogResult.Primary)
            {
                //  Delete
                App.contacts.Remove(c);
                App.SaveXml();
                this.Frame.GoBack();
            }
        }

        private void LoadAvartar()
        {
            try
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.UriSource = new Uri(TxtAvatar.Text);
                ImgAvatar.Source = bitmap;
            }
            catch (Exception)
            {

            }
        }
        private void TxtAvatar_TextChanged(object sender, TextChangedEventArgs e)
        {
            LoadAvartar();
        }
        
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadAvartar();
        }
    }
}
