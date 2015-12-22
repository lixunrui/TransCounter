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
        int currentTotalTransNum;

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
            _forecourt.Pumps.OnTransactionEvent += Pumps_OnTransactionEvent;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //TODO: Load data from file
            LogonData.LoadData();
            txt_ServerIP.Text = LogonData.Server;
            txt_TerminalID.Text = LogonData.TerminalID.ToString();
            txt_Password.Text = LogonData.TerminalPassword;
            ShowLogonPanel();

        }

        private void Window_Closed(object sender, EventArgs e)
        {
            _forecourt = null;
            this.Close();
        }

        void ShowLogonPanel()
        {
            if (_forecourt == null || !_forecourt.IsConnected)
            {
                LogonPanel.Visibility = Visibility.Visible;
                StartAnimation("show_logon");
            }
            else
            {
                LogonPanel.Visibility = Visibility.Hidden;
            }
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

        void StartTimer()
        {
            DateTime now = DateTime.Now;

            while (_forecourt != null && _forecourt.IsConnected)
            {
                DateTime current = DateTime.Now;
                TimeSpan elapsedSpan = new TimeSpan(current.Ticks - now.Ticks);
                Console.WriteLine("Current count {0} on Span {1}", currentTotalTransNum, elapsedSpan.Seconds);
                if (elapsedSpan.Seconds >= 30)
                {
                    UpdateNumbers(currentTotalTransNum);
                    Console.WriteLine("Number is {0}", currentTotalTransNum);
                    currentTotalTransNum = 0;
                    now = current;
                }
                Thread.Sleep(1000);
            }
        }

        void Pumps_OnTransactionEvent(object sender, PumpTransactionEventArgs e)
        {
            if (e.EventType == TransactionEventType.Completed)
            {
                currentTotalTransNum++;
            }
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
                        Thread timerThread = new Thread(StartTimer);
                        timerThread.Start();
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
                    if (!_forecourt.IsConnected)
                    {
                        StartAnimation("show_logon");
                    }
                    
                })
                );

            
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
            About about = new About();
            about.Owner = this;
            about.ShowDialog();
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
            ShowLogonPanel();
            ShowTerminalPanel();
            Message.Content = String.Format("Connected to Server {0}", txt_ServerIP.Text);
            LogonData.SaveData(txt_ServerIP.Text, Convert.ToInt32(txt_TerminalID.Text), txt_Password.Text);
            UpdateNumbers();
        }

        void ShowTerminalPanel()
        {
            lbl_Terminal_ID.Content = txt_TerminalID.Text;
        }

        void UpdateNumbers(int currentTotalTransNum=0)
        {
            if (_forecourt.IsConnected)
            {
                this.Dispatcher.BeginInvoke(
                  (Action)(() =>
                  {
                      Console.WriteLine("Invoking {0}", currentTotalTransNum);
                      lbl_Tot_Trans_Num.Content = "0";
                      lbl_Ave_Trans_Num.Content = currentTotalTransNum.ToString();
                  })
                  );
            }
      
        }

        private void ConnectionFailed(ApiResult ConnectResult)
        {
            Message.Content = "Failed to connect to server: "+ ConnectResult.ToString();
        }

    }
}
