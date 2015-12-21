using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ITL.Enabler.API;

namespace LXR.Counter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Forecourt _forecourt;
        PointCollection points = new PointCollection();
        public MainWindow()
        {
            InitializeComponent();
            InitializeMyComponent();
        }


        private void InitializeMyComponent()
        {
            _forecourt = new Forecourt();

            _forecourt.OnServerEvent += _forecourt_OnServerEvent;
            _forecourt.OnConnectAsyncResult += _forecourt_OnConnectAsyncResult;
        }

        void _forecourt_OnConnectAsyncResult(object sender, ConnectCompletedEventArgs e)
        {
            this.Dispatcher.BeginInvoke(
                (Action)(
                () =>
                {
                    if (e.ConnectResult == ApiResult.Ok)
                    {
                        ConnectionSucceed();
                    }
                    else
                    {
                        ConnectionFailed(e.ConnectResult);
                    }
                }
                )
                );
        }

        /// <summary>
        /// Catch server offline event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _forecourt_OnServerEvent(object sender, EventArgs e)
        {
            this.Dispatcher.BeginInvoke(
                (Action)(() =>
                {
                    Message.Content = String.Format("Server has {0}", _forecourt.IsConnected? "Connected":"Disconnected");
                })
                );
        }

        /// <summary>
        /// Show the logon panel
        /// </summary>
        /// <param name="StoryBoardName"></param>
        void StartAnimation(String StoryBoardName)
        {
            Storyboard currentStoryBoard;
            try
            {
                currentStoryBoard = this.FindResource(StoryBoardName) as Storyboard;
                currentStoryBoard.Begin(this, false);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Error:" + ex.Message);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            StartAnimation("show_logon");
            
        }
   
        private void MenuItem_Logon_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_Logoff_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_Exit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_About_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Logon_StoryBoard_Completed(object sender, EventArgs e)
        {
            this.Dispatcher.BeginInvoke(
                (Action)(()=>
                {
                    Message.Content = "Please insert id and password";
                }
                )
                );
        }

        private void Button_Logon_Clicked(object sender, RoutedEventArgs e)
        {
            try
            {
                Message.Content = "Connecting...";
                _forecourt.ConnectAsync(txt_ServerIP.Text, Convert.ToInt32(txt_TerminalID.Text), "TestTerminal", txt_Password.Text, true);
            }
            catch (System.Exception ex)
            {
                Message.Content = "Failed to connect to server";
            }
        }

        private void Button_Exit_Clicked(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ConnectionSucceed()
        {
            LogonPanel.Visibility = Visibility.Hidden;
            Message.Content = String.Format("Connected to Server {0}", txt_ServerIP.Text);
            LogonData.SaveData(txt_ServerIP.Text, Convert.ToInt32(txt_TerminalID.Text), txt_Password.Text);
        }

        private void ConnectionFailed(ApiResult ConnectResult)
        {
            Message.Content = "Failed to connect to server: "+ ConnectResult.ToString();
        }


       
    
    }
}
