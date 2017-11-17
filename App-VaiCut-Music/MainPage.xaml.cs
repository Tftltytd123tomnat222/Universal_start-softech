using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace App_VaiCut_Music
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        List<Music_Item> playList;

        public MainPage()
        {
            this.InitializeComponent();
        }

        async private void BtnBrowser_Click(object sender, RoutedEventArgs e)
        {
            var picker = new FolderPicker();
            picker.ViewMode = PickerViewMode.List;

            // vị trí bắt đầu khi fush ra
            picker.SuggestedStartLocation = PickerLocationId.MusicLibrary;

            // lọc theo kiểu
            picker.FileTypeFilter.Clear();
            picker.FileTypeFilter.Add(".mp3");

            var folder = await picker.PickSingleFolderAsync();

            playList = new List<Music_Item>();
            foreach (var file in await folder.GetFilesAsync())
            {
                var props = await file.Properties.GetMusicPropertiesAsync();
                playList.Add(new Music_Item()
                {
                    FileName = file.DisplayName,
                    FilePath = file.Path,
                    Duration = props.Duration.ToString("hh\\:mm\\:ss"),
                    Bitrate = (props.Bitrate / 1000)+"kbps"
                });
            }
            LstPlayList.ItemsSource = playList;
        }


        async private void PlayMusic(string path)
        {
            
        }
        private void LstPlayList_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            string path = LstPlayList.SelectedValue.ToString();
            PlayMusic(path);
        }
    }
}
