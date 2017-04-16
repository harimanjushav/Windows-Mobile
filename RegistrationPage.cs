using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Text.RegularExpressions;
using System.IO;
using Newtonsoft.Json.Linq;
using Microsoft.Phone.Net.NetworkInformation;
using System.Windows.Input;

namespace AppName
{
    public partial class RegisterPage : PhoneApplicationPage
    {

        private string name, displayName, email, password,deviceId;

        public RegisterPage()
        {
            InitializeComponent();
        }

        //method to hide the placeholder text in First Name field
        private void firstName_GotFocus(object sender, RoutedEventArgs e)
        {
            firstNamePlaceholder.Visibility = Visibility.Collapsed;
        }

        //method to display the placeholder text if the first name filed is empty
        private void firstName_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox)
            {
                var textbox = sender as TextBox;
                if (string.IsNullOrEmpty(textbox.Text))
                {
                    firstNamePlaceholder.Visibility = Visibility.Visible;
                }
                else
                {
                    firstNameTextBox.Text = char.ToUpper(firstNameTextBox.Text[0]) + firstNameTextBox.Text.Substring(1);                  
                }
            }
        }

        //method to hide the placeholder text in last Name field
        private void lastName_GotFocus(object sender, RoutedEventArgs e)
        {
            lastNamePlaceHolder.Visibility = Visibility.Collapsed;
        }

        //method to display the placeholder text if the Last Name filed is empty
        private void lastName_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox)
            {
                var textbox = sender as TextBox;
                if (string.IsNullOrEmpty(textbox.Text))
                {
                    lastNamePlaceHolder.Visibility = Visibility.Visible;
                }
                else
                {
                    lastNameTextBox.Text = char.ToUpper(lastNameTextBox.Text[0]) + lastNameTextBox.Text.Substring(1);
                }
            }
        }

        //method to hide the placeholder text in Display Name field
        private void displayName_GotFocus(object sender, RoutedEventArgs e)
        {
            displayNamePlaceHolder.Visibility = Visibility.Collapsed;
        }

        //method to display the placeholder text if the Display Name filed is empty
        private void displayName_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox)
            {
                var textbox = sender as TextBox;
                if (string.IsNullOrEmpty(textbox.Text))
                {
                    displayNamePlaceHolder.Visibility = Visibility.Visible;
                }                
            }
        }

        //method to hide the placeholder text in email address field
        private void email_GotFocus(object sender, RoutedEventArgs e)
        {
            emailAddressPlaceHolder.Visibility = Visibility.Collapsed;
        }

        //method to display the placeholder text if the email filed is empty
        private void email_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox)
            {
                var textbox = sender as TextBox;
                if (string.IsNullOrEmpty(textbox.Text))
                {
                    emailAddressPlaceHolder.Visibility = Visibility.Visible;
                }
            }
        }
        //method to hide the placeholder text in password address field
        private void password_GotFocus(object sender, RoutedEventArgs e)
        {
            passwordPlaceHolder.Visibility = Visibility.Collapsed;
        }

        //method to display the placeholder text if the email filed is empty
        private void password_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox)
            {
                var textbox = sender as PasswordBox;
                if (string.IsNullOrEmpty(textbox.Password))
                {
                    passwordPlaceHolder.Visibility = Visibility.Visible;
                }
            }
        }

        //method to hide the placeholder text in password address field
        private void confirmPassword_GotFocus(object sender, RoutedEventArgs e)
        {
            confirmPwdPlaceHolder.Visibility = Visibility.Collapsed;
        }

        //method to display the placeholder text if the email filed is empty
        private void confirmPassword_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox)
            {
                var textbox = sender as PasswordBox;
                if (string.IsNullOrEmpty(textbox.Password))
                {
                    confirmPwdPlaceHolder.Visibility = Visibility.Visible;
                }
            }
        }

        private void registerBtn_Click(object sender, RoutedEventArgs e)
        {
            string pattern = @"^[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?";
            //checking email validation
            System.Text.RegularExpressions.Match emailMatch = Regex.Match(emailAddressTextBox.Text.Trim(), pattern, RegexOptions.IgnoreCase);


            var namePattern = new Regex("^[a-zA-Z0-9 ]*$");

            //code for checking first name field exists
            if (firstNameTextBox.Text.Trim().Length == 0)
            {
                MessageBox.Show("Please enter a first name.");
                firstNameTextBox.Focus();

            }//code fro checking display name exists or not
            else if (displayNameTextBox.Text.Trim().Length == 0)
            {
                MessageBox.Show("Please enter a display name.");
                displayNameTextBox.Focus();

            }//code for checking valid first name or not
            else if(!namePattern.IsMatch(firstNameTextBox.Text))
            {
                MessageBox.Show("Please enter a valid name.");
                firstNameTextBox.Focus();
            }//code for checking valid first name or not
            else if (!namePattern.IsMatch(lastNameTextBox.Text))
            {
                MessageBox.Show("Please enter a valid name.");
                lastNameTextBox.Focus();
            } //code for checking email field is entered or not.
            else if (emailAddressTextBox.Text.Trim().Length == 0)
            {
                MessageBox.Show("Please enter your email address.");
                emailAddressTextBox.Focus();

            }//checking valid email or not
            else if (!emailMatch.Success)
            {
                MessageBox.Show("Please enter a valid email address.");
                emailAddressTextBox.Focus();

            }//checking password is entered or not
            else if (passwordTextBox.Password.Length == 0)
            {
                MessageBox.Show("Please enter your password.");
                passwordTextBox.Focus();

            }//checking password minimum length
            else if (passwordTextBox.Password.Length < 6)
            {
                MessageBox.Show("Password should contain at least 6 characters.");
                passwordTextBox.Focus();

            }//checking password max length
            else if (passwordTextBox.Password.Length > 16)
            {
                MessageBox.Show("Password should contain maximum 16 characters.");
                passwordTextBox.Focus();
            }
            else if (confirmPasswordTextBox.Password.Length == 0)
            {
                MessageBox.Show("Please enter confirm password.");
                confirmPasswordTextBox.Focus();
            }
            else if (confirmPasswordTextBox.Password != passwordTextBox.Password)
            {
                MessageBox.Show("Confirm password and password must be same.");
                confirmPasswordTextBox.Focus();
            }
            else
            {//sending user details to server

                name = firstNameTextBox.Text.Trim() + " " + lastNameTextBox.Text.Trim();
                name = name.Trim();

                displayName = displayNameTextBox.Text.Trim();
                email = emailAddressTextBox.Text.Trim();
                password = passwordTextBox.Password;

                byte[] myDeviceID = (byte[])Microsoft.Phone.Info.DeviceExtendedProperties.GetValue("DeviceUniqueId");
                deviceId = Convert.ToBase64String(myDeviceID);

                sendRegisterRequest();
            }
        }


        //code for request from server
        private void sendRegisterRequest()
        {       //url to request
            this.Focus();

            layerControl.Visibility = Visibility.Visible;

            progressBar.Visibility = Visibility.Visible;
            this.progressBar.IsIndeterminate = true;

            var url = MainPage.webServiceURL + "userRegistration";
           
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.Method = "POST";
            webRequest.ContentType = string.Format("multipart/form-data; boundary={0}", boundary);

            // Start the request
            webRequest.BeginGetRequestStream(new AsyncCallback(registerRequestCallBack), webRequest);
        }

        void registerRequestCallBack(IAsyncResult asynchronousResult)
        {//code for retriving old userid

            //code for posting data to httpwebrequest
            HttpWebRequest webRequest = (HttpWebRequest)asynchronousResult.AsyncState;
            // End the stream request operation
            Stream postStream = webRequest.EndGetRequestStream(asynchronousResult);
            // Create the post data
            // Demo POST data 


            Dictionary<string, object> data = new Dictionary<string, object>()
                  {
                    {"name", name},
                    {"displayName",displayName},
                    {"email",email},
                    {"password",password},
                    {"osVersion","0.1"},
                    {"appVersion","0.1"},
                    {"deviceType","3"},
                    {"deviceId",deviceId}
                  };
            writeMultipartObject(postStream, data);

            postStream.Close();
            // Start the web request
            webRequest.BeginGetResponse(new AsyncCallback(GetRegisterResponseCallback), webRequest);
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
        private void GetRegisterResponseCallback(IAsyncResult results)
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
                                MessageBox.Show("" + message);

                                NavigationService.GoBack();

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

        private void firstNameTextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {//setting focus on password textbox
                lastNameTextBox.Focus();
            }
        }

        private void lastNameTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {//setting focus on password textbox
                displayNameTextBox.Focus();
            }
        }

        private void displayNameTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {//setting focus on password textbox
                emailAddressTextBox.Focus();
            }
        }

        private void passwordTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {//setting focus on password textbox
                confirmPasswordTextBox.Focus();
            }
        }

        private void emailAddressTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {//setting focus on password textbox
                passwordTextBox.Focus();
            }
        }

        private void confirmPasswordTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {//setting focus on password textbox
                this.Focus();
            }
        }

      

    }
}