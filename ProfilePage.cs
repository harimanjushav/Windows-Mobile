using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.IO;
using Newtonsoft.Json.Linq;
using Microsoft.Phone.Net.NetworkInformation;
using System.IO.IsolatedStorage;

using Facebook;
namespace QPalls
{
    public partial class ProfilePage : PhoneApplicationPage
    {
        public static string facebookId, facebookAccessToken;
        public static string appToken, pushId;
        //variables for social networking
        
        bool isFacebook = true;

        public ProfilePage()
        {
            InitializeComponent();
            ImageBrush brush1 = new ImageBrush();
            ImageBrush brush2 = new ImageBrush();

            brush2.ImageSource = new BitmapImage(new Uri(@"/Images/qFacebook_deactive.png", UriKind.Relative));
            facebookBtn.Background = brush2;
           

            //sending request to server to get user details
           
        }

        private void menuButton_Click(object sender, EventArgs e)
        {
            //NavigationService.Navigate(new Uri("/MenuPage.xaml", UriKind.Relative));
            NavigationService.GoBack();
        }

        private void editButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/EditProfilePage.xaml", UriKind.Relative));
        }

        private void qCreatedTab_Click(object sender, RoutedEventArgs e)
        {
            phoneBgImg.Source = new BitmapImage(new Uri(@"/Images/Profile/activity-tab-active.png", UriKind.Relative));
            facebookBgImg.Source = null;
            followingBgImg.Source = null;
            followersBgImg.Source = null;

            SolidColorBrush ForeBrush = new SolidColorBrush();
            ForeBrush.Color = Colors.White;

            qCreatedCountLabel.Foreground = ForeBrush;
            qCreatedLabel.Foreground = ForeBrush;

            SolidColorBrush greenBrush = new SolidColorBrush();
            greenBrush.Color = Color.FromArgb(255, 153, 154, 138);

            qReceivedCountLabel.Foreground = greenBrush;
            qReceivedLabel.Foreground = greenBrush;
            followingCountLabel.Foreground = greenBrush;
            followingLabel.Foreground = greenBrush;
            followersCountLabel.Foreground = greenBrush;
            followersLabel.Foreground = greenBrush;
           
        }

        private void qReceivedTab_Click(object sender, RoutedEventArgs e)
        {
            phoneBgImg.Source = null;
            facebookBgImg.Source = new BitmapImage(new Uri(@"/Images/Profile/activity-tab-active.png", UriKind.Relative));
            followingBgImg.Source = null;
            followersBgImg.Source = null;

            SolidColorBrush ForeBrush = new SolidColorBrush();
            ForeBrush.Color = Colors.White;

            qReceivedCountLabel.Foreground = ForeBrush;
            qReceivedLabel.Foreground = ForeBrush;

            SolidColorBrush greenBrush = new SolidColorBrush();
            greenBrush.Color = Color.FromArgb(255, 153, 154, 138);

            qCreatedCountLabel.Foreground = greenBrush;
            qCreatedLabel.Foreground = greenBrush;
            followingCountLabel.Foreground = greenBrush;
            followingLabel.Foreground = greenBrush;
            followersCountLabel.Foreground = greenBrush;
            followersLabel.Foreground = greenBrush;
           
        }

        private void followingTab_Click(object sender, RoutedEventArgs e)
        {
            phoneBgImg.Source = null;
            facebookBgImg.Source = null;
            followingBgImg.Source = new BitmapImage(new Uri(@"/Images/Profile/activity-tab-active.png", UriKind.Relative));
            followersBgImg.Source = null;

            SolidColorBrush ForeBrush = new SolidColorBrush();
            ForeBrush.Color = Colors.White;

            followingCountLabel.Foreground = ForeBrush;
            followingLabel.Foreground = ForeBrush;

            SolidColorBrush greenBrush = new SolidColorBrush();
            greenBrush.Color = Color.FromArgb(255, 153, 154, 138);

            qReceivedCountLabel.Foreground = greenBrush;
            qReceivedLabel.Foreground = greenBrush;
            qCreatedCountLabel.Foreground = greenBrush;
            qCreatedLabel.Foreground = greenBrush;
            followersCountLabel.Foreground = greenBrush;
            followersLabel.Foreground = greenBrush;

        }

        private void followersTab_Click(object sender, RoutedEventArgs e)
        {
            phoneBgImg.Source = null;
            facebookBgImg.Source = null;
            followingBgImg.Source = null;
            followersBgImg.Source = new BitmapImage(new Uri(@"/Images/Profile/activity-tab-active.png", UriKind.Relative));

            SolidColorBrush ForeBrush = new SolidColorBrush();
            ForeBrush.Color = Colors.White;

            followersCountLabel.Foreground = ForeBrush;
            followersLabel.Foreground = ForeBrush;

            SolidColorBrush greenBrush = new SolidColorBrush();
            greenBrush.Color = Color.FromArgb(255, 153, 154, 138);

            qReceivedCountLabel.Foreground = greenBrush;
            qReceivedLabel.Foreground = greenBrush;
            followingCountLabel.Foreground = greenBrush;
            followingLabel.Foreground = greenBrush;
            qCreatedCountLabel.Foreground = greenBrush;
            qCreatedLabel.Foreground = greenBrush;

        }



        //code for request from server
        private void sendUserDetailsRequest()
        {       //url to request
            this.Focus();

            layerControl.Visibility = Visibility.Visible;

            progressBar.Visibility = Visibility.Visible;
            this.progressBar.IsIndeterminate = true;

            var url = MainPage.webServiceURL + "userProfile";

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.Method = "POST";
            webRequest.ContentType = string.Format("multipart/form-data; boundary={0}", boundary);

            // Start the request
            webRequest.BeginGetRequestStream(new AsyncCallback(userDetailsRequestCallBack), webRequest);
        }

        void userDetailsRequestCallBack(IAsyncResult asynchronousResult)
        {//code for retriving old userid

            //code for posting data to httpwebrequest
            HttpWebRequest webRequest = (HttpWebRequest)asynchronousResult.AsyncState;
            // End the stream request operation
            Stream postStream = webRequest.EndGetRequestStream(asynchronousResult);
            // Create the post data
            // Demo POST data 

            Dictionary<string, object> data = new Dictionary<string, object>()
                  {
                    {"userId", MainPage.userId},
                    {"accessToken",MainPage.appToken}
                  };

            writeMultipartObject(postStream, data);

            postStream.Close();
            // Start the web request
            webRequest.BeginGetResponse(new AsyncCallback(GetUserDetailsResponseCallback), webRequest);
        }

        public Dictionary<string, object> parameters { get; set; }
        string boundary = "----------" + DateTime.Now.Ticks.ToString();
        public void writeMultipartObject(Stream stream, object data)
        {
            StreamWriter writer = new StreamWriter(stream);
            if (data != null)
            {
                foreach (var entry in data as Dictionary<string, object>)
                {
                    WriteEntry(writer, entry.Key, entry.Value);
                }
            }
            writer.Write("--");
            writer.Write(boundary);
            writer.WriteLine("--");
            writer.Flush();
        }
        //code for binding each element
        private void WriteEntry(StreamWriter writer, string key, object value)
        {
            if (value != null)
            {
                writer.Write("--");
                writer.WriteLine(boundary);
                //code for binding image  
                if (value is byte[])
                {
                    byte[] ba = value as byte[];
                    //code for setting image name
                    writer.WriteLine(@"Content-Disposition: form-data; name=""{0}""; filename=""{1}""", key, "sentPhoto.png");                   
                    //code for setting type of the image
                    writer.WriteLine(@"Content-Type: image/png");
                    //code fro setting length of the image
                    writer.WriteLine(@"Content-Length: " + ba.Length);
                    writer.WriteLine();
                    writer.Flush();
                    Stream output = writer.BaseStream;
                    output.Write(ba, 0, ba.Length);
                    output.Flush();
                    writer.WriteLine();
                }
                else
                {//code for variables concatinating 
                    writer.WriteLine(@"Content-Disposition: form-data; name=""{0}""", key);
                    writer.WriteLine();
                    writer.WriteLine(value.ToString());
                }
            }
        }
        //code for response from server
        private void GetUserDetailsResponseCallback(IAsyncResult results)
        {//exceptions
            Dispatcher.BeginInvoke(() =>
            {
                layerControl.Visibility = Visibility.Collapsed;

                progressBar.Visibility = Visibility.Collapsed;
                progressBar.IsIndeterminate = false;
            });

            try
            {
                var request = (HttpWebRequest)results.AsyncState;
                var response = request.EndGetResponse(results);

                using (var stream = response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {//storing response into a string
                    string contents = reader.ReadToEnd();

                    //json parsing
                    JObject o;
                    try
                    {
                        o = JObject.Parse(contents);
                        //reading data from response to variables
                        string message = (string)o["message"];
                        int result = (int)o["result"];

                        if (result == 1)
                        {

                            Dispatcher.BeginInvoke(() =>
                            {

                                string imgLink = (string)o["thumb"];

                                Uri uri1 = new Uri(imgLink, UriKind.Absolute);
                                BitmapImage imgSource1 = new BitmapImage(uri1);
                                profileImage.Source = imgSource1;

                                displayNameLabel.Text = (string)o["displayName"];
                                bioDescLabel.Text = (string)o["biography"];

                                emailLabel.Text = (string)o["email"];
                                locationLabel.Text = (string)o["location"];

                                int gender = (int)o["gender"];

                                if (gender == 1)
                                {
                                    genderLabel.Text = "Male";
                                }
                                else if (gender == 2)
                                {
                                    genderLabel.Text = "Female";
                                }
                                else
                                {
                                    genderLabel.Text = "";
                                }

                               

                            });

                        }
                        else
                        {
                            Dispatcher.BeginInvoke(() =>
                            {//Displaying error message from server
                                MessageBox.Show("" + message);
                            });
                        }
                    }
                    catch (Exception)
                    {
                        Dispatcher.BeginInvoke(() =>
                        {
                            /* Guide.BeginShowMessageBox(" ", "Sorry - there seems to be a problem.  Please contact us or try again later.", new string[] { "Report a bug", "Cancel" }, 1, MessageBoxIcon.None,
                              new AsyncCallback(OnMessageBoxClosed), null);
                           */
                            MessageBox.Show("Sorry - there seems to be a problem.");
                        });
                    }
                }
            }
            catch (WebException )
            {
                Dispatcher.BeginInvoke(() =>
                {//making progressbar disable

                    //code for checking network is available on device or not.
                    bool isAvailable = DeviceNetworkInformation.IsNetworkAvailable;
                    if (isAvailable == false)
                    {
                        MessageBox.Show("Sorry - there seems to be a problem with the network connection.  Please try again later.");
                    }
                    else
                    {//custom messagebox
                        /*  Guide.BeginShowMessageBox(" ", "Sorry - there seems to be a problem.  Please contact us or try again later.", new string[] { "Report a bug", "Cancel" }, 1, MessageBoxIcon.None,
                          new AsyncCallback(OnMessageBoxClosed), null);*/
                        MessageBox.Show("Sorry - there seems to be a problem.");
                    }
                });
            }

        }

        private void PhoneApplicationPage_Loaded_1(object sender, RoutedEventArgs e)
        {
            sendUserDetailsRequest();

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

            if (IsolatedStorageSettings.ApplicationSettings.Contains("fbAccessToken"))
            {
                facebookAccessToken = IsolatedStorageSettings.ApplicationSettings["fbAccessToken"] as string;
            }
            else
            {
                facebookAccessToken = "";
            }

            ImageBrush brush1 = new ImageBrush();

            if (facebookAccessToken == "")
            {
                if (isFacebook)
                {
                    brush1.ImageSource = new BitmapImage(new Uri(@"/Images/qFacebook_deactive.png", UriKind.Relative));
                    facebookBtn.Background = brush1;
                    isFacebook = false;
                }
            }

        }

        private void facebookBtn_Click(object sender, RoutedEventArgs e)
        {

            if (facebookAccessToken == "")
            {
                //Navigate the user to FB page so the  user can authenticate the app with permissions .
                NavigationService.Navigate(new Uri("/FacebookLoginPage.xaml", UriKind.Relative));
            }
            else
            {
                ImageBrush brush1 = new ImageBrush();                 
                //fb token is available 
                if (isFacebook)
                {
                    MessageBoxResult m = MessageBox.Show("Are you sure you would like to dis-associate Facebook link with ApplicationName account?", "", MessageBoxButton.OKCancel);
                    if (m == MessageBoxResult.OK)
                    {

                        //removing data from database    
                            IsolatedStorageSettings.ApplicationSettings.Remove("fbAccessToken");
                            facebookAccessToken = "";

                    }
                    brush1.ImageSource = new BitmapImage(new Uri(@"/Images/qFacebook_deactive.png", UriKind.Relative));
                    facebookBtn.Background = brush1;
                    isFacebook = false;
                }
                else
                {
                    brush1.ImageSource = new BitmapImage(new Uri(@"/Images/Profile/icon-facebook.png", UriKind.Relative));
                    facebookBtn.Background = brush1;

                    isFacebook = true;
                }
            }
           


        }
     


    }
}