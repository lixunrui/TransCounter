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

        void _forecourt_OnServerEvent(object sender, EventArgs e)
        {
            throw new NotImplementedException();
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
            //reverse.RenderTransform = form;

            //const double margin = 10;
            //double xMin = margin;
            //double xMax = canGraph.Width - margin;

            //double yMin = margin;
            //double yMax = canGraph.Height - margin;

            //const double step = 5;

            //// make the x axis
            //GeometryGroup xAxis = new GeometryGroup();
            //xAxis.Children.Add(new LineGeometry(new Point(0, yMax), new Point(canGraph.Width, yMax)));

            //for (double x = xMin + step; x <= canGraph.Width - step; x+= step )
            //{
            //    xAxis.Children.Add(new LineGeometry(new Point(x, yMax - margin/2), new Point(x,yMax + margin/2)));
            //}

            //Path xAxisPath = new Path();
            //xAxisPath.StrokeThickness = 1;
            //xAxisPath.Stroke = Brushes.Black;
            //xAxisPath.Data = xAxis;

            //canGraph.Children.Add(xAxisPath);

            //// Make the Y ayis.
            //GeometryGroup yaxis = new GeometryGroup();
            //yaxis.Children.Add(new LineGeometry(
            //    new Point(xMin, 0), new Point(xMin, canGraph.Height)));
            //for (double y = step; y <= canGraph.Height - step; y += step)
            //{
            //    yaxis.Children.Add(new LineGeometry(
            //        new Point(xMin - margin / 2, y),
            //        new Point(xMin + margin / 2, y)));
            //}

            //Path yaxis_path = new Path();
            //yaxis_path.StrokeThickness = 1;
            //yaxis_path.Stroke = Brushes.Black;
            //yaxis_path.Data = yaxis;

            //canGraph.Children.Add(yaxis_path);

          //  LoadData();
        }

#region Item Events        
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
#endregion


        private void Logon_StoryBoard_Completed(object sender, EventArgs e)
        {

        }

        private void Button_Logon_Clicked(object sender, RoutedEventArgs e)
        {
            try
            {

                Message.Content = "Connecting...";
                _forecourt.ConnectAsync(".", 3, "Terminal", "password", true);
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
        }

        private void ConnectionFailed(ApiResult ConnectResult)
        {
            Message.Content = "Failed to connect to server: "+ ConnectResult.ToString();
        }

     

        //void LoadData()
        //{
        //    int index = 10;
        //    int x = 0;
         
        //    do 
        //    {

        //        Polyline line = new Polyline();
        //        line.Stroke = Brushes.Red;
        //        line.StrokeThickness = 1;
        //        Random r = new Random();

        //        double data = r.Next(100, 400);
                
        //        // the data point is (x:index(time), y:data)

        //        if (points.Count > 10)
        //        {
        //            points.RemoveAt(0);
        //        }

        //        points.Add(new Point(index, data));
        //        x++;
        //        index+=5;

        //        line.Points = points;

        //        canGraph.Children.Add(line);
             
        //    } while (x < 20);

        //    Console.WriteLine();
        //}

        //private void Button_Click(object sender, RoutedEventArgs e)
        //{
        //    double x = Convert.ToDouble(txt_x.Text);
        //    double y = Convert.ToDouble(txt_y.Text);
        //    points.Add(new Point(x, y));

        //    Draw();
        //}

        //void Draw()
        //{
        //    Polyline line = new Polyline();
        //    line.Stroke = Brushes.Red;
        //    line.StrokeThickness = 1;

        //    line.Points = points;

        //    canGraph.Children.Add(line);
        //}
    }
}
