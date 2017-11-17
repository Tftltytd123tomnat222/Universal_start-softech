using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace App_ShowImages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        
        public MainPage()
        {
            this.InitializeComponent();
        }

        async private void BtnFolder_Click(object sender, RoutedEventArgs e)
        {
            var picker = new FolderPicker();
            picker.ViewMode = PickerViewMode.Thumbnail;
            picker.FileTypeFilter.Add("*");

            var folder = await picker.PickSingleFolderAsync();
            if(folder != null)
            {
                List<Picture> list = new List<Picture>();
                foreach (var file in await folder.GetFilesAsync())
                {
                    try
                    {
                        BitmapImage bitmap = new BitmapImage();
                        bitmap.SetSource(await file.OpenReadAsync());

                        list.Add(new Picture()
                        {
                            FileName = file.DisplayName,
                            FileSource = bitmap
                        });
                    }
                    catch {}
                }

                LstPictures.ItemsSource = list;
            }
        }
    }
}
