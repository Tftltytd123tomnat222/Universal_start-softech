using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace App_Music
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        List<Song> songs;
        string url_search = "https://mp3.zing.vn/tim-kiem/bai-hat.html?q=";
        string match = "item-song";
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

        }
        private void Search()
        {

        }
        async private void TxtKeywords_QuerySubmitted(SearchBox sender, SearchBoxQuerySubmittedEventArgs args)
        {
            PrgLoading.IsActive = true;
            songs = new List<Song>();

            using (var client = new HttpClient())
            {

                    // get HTML về
                    // string html = await reponse.Content.ReadAsStringAsync();
                    string url = url_search + TxtKeywords.QueryText;

                    var response = await client.GetBufferAsync(new Uri(url));
               
                    string html = Encoding.UTF8.GetString(response.ToArray());

                    // position (vị trí con trỏ)
                    int pos = 0;
                    while (html.IndexOf( match, pos ) > 0)
                    {
                        // Đánh dấu vị trí cho nó (+ match.Length ::khi tìm nó sẽ kết thúc vòng lặp vô hạng tại vị trí .Length đóa )
                        pos = html.IndexOf( match, pos ) + match.Length;

                        // Đếm từ data-id=" là 9 kí tự
                        pos = html.IndexOf("data-id", pos ) + 9;
                        string id = html.Substring(pos, html.IndexOf("\"", pos) - pos);

                        // Đếm từ title="  là 7 kí tự
                        pos = html.IndexOf("title", pos) + 7;
                        string title = html.Substring(pos, html.IndexOf("\"", pos) - pos);


                        // Đếm từ title="  là 6 kí tự
                        pos = html.IndexOf("href", pos) + 6;
                        string href = html.Substring(pos, html.IndexOf("\"", pos) - pos);
                        songs.Add(new Song()
                        {
                            Id = id,
                            Title = title,
                            Href = "https://mp3.zing.vn" + href,
                            Info = "",
                            Url = ""   
                        });
                    }
            }

            lstPlayList.ItemsSource = songs;
            PrgLoading.IsActive = false;
        }


        async private Task<string> GetSource(string href)
        {
            using (var client = new HttpClient())
            {
                string html = await client.GetStringAsync(new Uri(href));
                int pos = html.IndexOf("data-xml");

                if(pos > 0)
                {
                    pos += 10;
                    string url_xml = "https://mp3.zing.vn/xhr" + html.Substring(pos, html.IndexOf("\"", pos) - pos);
                    string json = await client.GetStringAsync(new Uri(url_xml));

                    JsonObject root = JsonObject.Parse(json);
                    JsonObject data = root.GetNamedValue("data").GetObject();
                    JsonObject source = data.GetNamedValue("source").GetObject();

                    return "http:"+source.GetNamedString("128");
                }
            }
            return "";
        }
        async private void BtnPlay_Click(object sender, RoutedEventArgs e)
        {
            string href = (sender as AppBarButton).CommandParameter.ToString();
            Song s = songs.Where(x => x.Href.Equals(href)).FirstOrDefault();
            
            if (s.Url.Equals(""))
            {
                s.Url = await GetSource(s.Href);
            }
            //s.Url = await GetSource(s.Href);
            //await new MessageDialog(s.Url).ShowAsync();

            Player.Source = new Uri(s.Url);
        }

        async private void BtnDownload_Click(object sender, RoutedEventArgs e)
        {
            string href = (sender as AppBarButton).CommandParameter.ToString();
            Song s = songs.Where(x => x.Href.Equals(href)).FirstOrDefault();

            if (s.Url.Equals(""))
            {
                s.Url = await GetSource(s.Href);
            }

            StorageFile file = await KnownFolders.MusicLibrary.CreateFileAsync(s.Title + ".mp3", CreationCollisionOption.GenerateUniqueName);
            BackgroundDownloader downloader = new BackgroundDownloader();
            downloader.CreateDownload(new Uri(s.Url), file).AttachAsync();
        }
    }
}
