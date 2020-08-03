using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Windows;
using ArtShareClient.Classes;
using Microsoft.WindowsAPICodePack.Dialogs;
using Newtonsoft.Json;

namespace ArtShareClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool Localhost = false;
        private Config Config;
        private string Url => Localhost ? "http://localhost:5002/api/" : Config?.ApiUrl ?? "http://localhost:5002/api/";

        public MainWindow()
        {
            InitializeComponent();

            Config = JsonConvert.DeserializeObject<Config>(File.ReadAllText("config.json"));
        }

        private void SubmitBtn_Click(object sender, RoutedEventArgs e)
        {
            var username = UsernameBox.Text;
            var furAffinity = FurAffinityBox.Text;
            var twitter = TwitterBox.Text;
            var discord = DiscordBox.Text;

            if (username.Length <= 0)
            {
                MessageBox.Show("Please enter username");
                return;
            }

            var user = new Customer
            {
                Username = username,
                FurAffinity = furAffinity,
                Twitter = twitter,
                Discord = discord
            };

            var response = ApiPostRequest("addCustomer", JsonConvert.SerializeObject(user));

            MessageBox.Show("API-Response: " + response);
        }

        private string ApiPostRequest(string action, string data)
        {
            try
            {
                var req = (HttpWebRequest)WebRequest.Create($"{Url}Main/{action}");

                req.ContentType = "application/json";
                req.Method = "POST";

                var b = Encoding.UTF8.GetBytes(data);
                req.ContentLength = b.Length;

                using (var dataStream = req.GetRequestStream())
                {
                    dataStream.Write(b, 0, b.Length);

                    using (var response = req.GetResponse())
                    {
                        using (var dataStream2 = response.GetResponseStream())
                        {
                            if (dataStream2 != null)
                            {
                                using (var reader = new StreamReader(dataStream2))
                                {
                                    var responseFromServer = reader.ReadToEnd();

                                    Debug.WriteLine($@"Response: {responseFromServer}");

                                    return responseFromServer;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return string.Empty;
            }

            return string.Empty;
        }

        public string ApiGetRequest(string call, string param)
        {
            try
            {
                using (var wc = new WebClient())
                {
                    return wc.DownloadString($"{Url}Main/{call}/?{param}");
                }
            }
            catch (Exception)
            {
                return default;
            }
        }

        private void UploadZipBtn_Click(object sender, RoutedEventArgs e)
        {
            var name = ZipFileUsernameBox.Text;

            if (name.Length <= 0)
            {
                MessageBox.Show("Please enter username");
                return;
            }

            var dialog = new CommonOpenFileDialog();
            dialog.DefaultExtension = ".zip";
            dialog.Filters.Add(new CommonFileDialogFilter("zip archive | *.zip", ".zip"));

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                var zip = Convert.ToBase64String(File.ReadAllBytes(dialog.FileName));

                var r = ApiPostRequest("addZipForUser", JsonConvert.SerializeObject(new UserZipModel
                {
                    Username = name,
                    ZipFile = zip
                }));

                MessageBox.Show(r);
            }
        }

        private void UnzipOnServerBtn_Click(object sender, RoutedEventArgs e)
        {
            var name = ZipFileUsernameBox.Text;

            if (name.Length <= 0)
            {
                MessageBox.Show("Please enter username");
                return;
            }

            MessageBox.Show(ApiGetRequest("unzipZip", $"username={name}"));
        }

        private void OpenWebForUser_Click(object sender, RoutedEventArgs e)
        {
            var name = ZipFileUsernameBox.Text;

            if (name.Length <= 0)
            {
                MessageBox.Show("Please enter username");
                return;
            }

            Process.Start($"https://art.mohregregs.de/?user={name}");
        }
    }
}
