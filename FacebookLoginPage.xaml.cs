using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Facebook;
using System.IO.IsolatedStorage;

namespace QPalls
{
    public partial class FacebookLoginPage : PhoneApplicationPage
    {

        private string AppId;

        /// <summary>
        /// Extended permissions is a comma separated list of permissions to ask the user.
        /// </summary>
        /// <remarks>
        /// For extensive list of available extended permissions refer to 
        /// https://developers.facebook.com/docs/reference/api/permissions/
        /// </remarks>
        private const string ExtendedPermissions = "user_about_me,read_stream,email,publish_stream";

        private readonly FacebookClient _fb = new FacebookClient();

        public static string fbId, fbFirstName, fbLastName, fbDisplayName, fbEmail;


        public FacebookLoginPage()
        {
            InitializeComponent();

            AppId=MainPage.facebookId;

            progressGrid.Visibility = Visibility.Visible;
        }
       
        private Uri GetFacebookLoginUrl(string appId, string extendedPermissions)
        {
            var parameters = new Dictionary<string, object>();
            parameters["client_id"] = appId;
            parameters["redirect_uri"] = "https://www.facebook.com/connect/login_success.html";
            parameters["response_type"] = "token";
            parameters["display"] = "touch";

            // add the 'scope' only if we have extendedPermissions.
            if (!string.IsNullOrEmpty(extendedPermissions))
            {
                // A comma-delimited list of permissions
                parameters["scope"] = extendedPermissions;
            }

            return _fb.GetLoginUrl(parameters);
        }

        private void LoginSucceded(string accessToken)
        {
            var fb = new FacebookClient(accessToken);

            fb.GetCompleted += (o, e) =>
            {
                if (e.Error != null)
                {
                    Dispatcher.BeginInvoke(() =>  NavigationService.GoBack());
                   
                    return;
                }

                var result = (IDictionary<string, object>)e.GetResultData();

                 fbId = (string)result["id"];
                 fbFirstName =(string)result["first_name"];
                 fbLastName= (string)result["last_name"];
                 fbEmail=(string)result["email"];
                 fbDisplayName= (string)result["name"];

                 MainPage.isFacebookLogin = true;

              //  var url = string.Format("/Pages/FacebookInfoPage.xaml?access_token={0}&id={1}", accessToken, id);

               Dispatcher.BeginInvoke(() => NavigationService.GoBack());
            };

            fb.GetAsync("me");
        }

        private void webBrowser_Loaded(object sender, RoutedEventArgs e)
        {
            var loginUrl = GetFacebookLoginUrl(AppId, ExtendedPermissions);
            webBrowser.Navigate(loginUrl);
        }

        private void webBrowser_Navigated(object sender, NavigationEventArgs e)
        {

            progressGrid.Visibility = Visibility.Collapsed;

            FacebookOAuthResult oauthResult;
            if (!_fb.TryParseOAuthCallbackUrl(e.Uri, out oauthResult))
            {
                return;
            }

            if (oauthResult.IsSuccess)
            {
                webBrowser.Visibility = Visibility.Collapsed;

                var accessToken = oauthResult.AccessToken;

                //creating object for isolated storage
                IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
                // txtInput is a TextBox defined in XAML.
                if (!settings.Contains("fbAccessToken"))
                {//adding data to isolated storage
                    settings.Add("fbAccessToken", accessToken);
                }
                else
                {//adding data to isolated storage
                    settings["fbAccessToken"] = accessToken;
                }

                //saving isolated storage data
                settings.Save();

                MainPage.facebookAccessToken = accessToken;

                LoginSucceded(accessToken);
            }
            else
            {
                // user cancelled
                NavigationService.GoBack();
            }
        }

        private void webBrowser_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            progressGrid.Visibility = Visibility.Collapsed;
        }
    }
}