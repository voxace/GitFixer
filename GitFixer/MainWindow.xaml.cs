using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;

namespace GitFixer
{
    public partial class MainWindow : Window
    {
        bool loaded = false;
        string logon;
        string configPath;
        string userConfigPath;
        string proxyInfo = "";
        bool fixed1 = false;
        bool fixed2 = false;

        public MainWindow()
        {
            InitializeComponent();
            logon = Environment.UserName;
            configPath = $@"C:\Users\{logon}\.gitconfig";
        }

        // Open File Button
        private void BtnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (Directory.Exists(@"U:\"))
            {
                openFileDialog.InitialDirectory = @"U:\";
            }

            openFileDialog.Title = "Navigate to the Project Folder";

            if (openFileDialog.ShowDialog() == true)
            {
                string directoryPath = System.IO.Path.GetDirectoryName(openFileDialog.FileName);
                if (File.Exists(directoryPath + @"\.git\config"))
                {
                    userConfigPath = directoryPath + @"\.git\config";
                    btnOpenFile.Content = "File loaded: \n" + userConfigPath;
                    btnOpenFile.IsEnabled = false;
                    loaded = true;
                }
                else
                {
                    MessageBox.Show("Incorrect file loaded or not a Git Repository. Please load a Git Repository that contains a Visual Studio Solution.");
                }
            }
        }

        // Fix Button
        private void FixButton_Click(object sender, RoutedEventArgs e)
        {
            if (GitPasswordTextBox.Password == "" || GitUsernameTextBox.Text == "" ||SchoolPasswordTextBox.Password == "" || SchoolUsernameTextBox.Text == "")
            {
                MessageBox.Show("Please fill out the username/password boxes.");
            }
            else if (loaded == false)
            {
                MessageBox.Show("Please load a Visual Studio Solution file.");
            }
            else
            {
                // Fix global config by adding proxy
                if(File.Exists(configPath))
                {
                    string configText = File.ReadAllText(configPath);
                    if (configText.Contains("proxy"))
                    {
                        MessageBox.Show("Proxy information already added.");
                        fixed1 = true;
                    }
                    else
                    {
                        proxyInfo = $"[http]\n\tproxy = http://{SchoolUsernameTextBox.Text}%40detnsw:{SchoolPasswordTextBox.Password}@proxy.det.nsw.edu.au:8080\n" +
                           $"[https]\n\tproxy = http://{SchoolUsernameTextBox.Text}%40detnsw:{SchoolPasswordTextBox.Password}@proxy.det.nsw.edu.au:8080\n";

                        File.AppendAllText(configPath, proxyInfo);
                        fixed1 = true;
                    }                   
                }
                else
                {
                    MessageBox.Show("Cannot fix. GitHub config file missing.");
                }

                // Fix user config by adding username and password
                string userConfigText = File.ReadAllText(userConfigPath);
                if (userConfigText.Contains("https://github.com/"))
                {
                    userConfigText = userConfigText.Replace("https://github.com/", $"https://{GitUsernameTextBox.Text}:{GitPasswordTextBox.Password}@github.com/");
                    File.WriteAllText(userConfigPath, userConfigText);
                    fixed2 = true;
                }
                else
                {
                    MessageBox.Show("Remote origin information missing or already updated.");
                }
            }
            if(fixed1 && fixed2)
            {
                MessageBox.Show("GitHub Fix Was Successful!");
            }
        }

        // Input Validation
        #region Input Validation
        private void GitUsernameTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (GitPasswordTextBox.Password == "" || GitUsernameTextBox.Text == "" || SchoolPasswordTextBox.Password == "" || SchoolUsernameTextBox.Text == "")
            {
                FixButton.IsEnabled = false;
            }
        }

        private void GitPasswordTextBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (GitPasswordTextBox.Password == "" || GitUsernameTextBox.Text == "" || SchoolPasswordTextBox.Password == "" || SchoolUsernameTextBox.Text == "")
            {
                FixButton.IsEnabled = false;
            }
        }

        private void SchoolUsernameTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (GitPasswordTextBox.Password == "" || GitUsernameTextBox.Text == "" || SchoolPasswordTextBox.Password == "" || SchoolUsernameTextBox.Text == "")
            {
                FixButton.IsEnabled = false;
            }
        }

        private void SchoolPasswordTextBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (GitPasswordTextBox.Password == "" || GitUsernameTextBox.Text == "" || SchoolPasswordTextBox.Password == "" || SchoolUsernameTextBox.Text == "")
            {
                FixButton.IsEnabled = false;
            }
        }
        #endregion
    }
}
