using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Windows.Media.Animation;

namespace FlightSimulatorApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ViewModel vm;
        public MainWindow()
        {
            InitializeComponent();
          
            vm = new ViewModel(new Model(new Client()));
            var window = new connection();
            window.ShowDialog();
            if (window.Port != null)
            {
                vm.Port = Int32.Parse(window.Port);
            }
         
            vm.Ip = window.Ip;
            DataContext = vm;
            //joystickView.Da
            joystickV.DataContext = vm;
            vm.connect();
       
            
            //Joystick joy = new Joystick();
            //joy.DataContext = vm;
            //  joy.DataContext = vm.
            // joy.Rudder
            /*  Dictionary<string, TextBlock> d = new Dictionary<string, TextBlock>();
              d["heading"] = heading;

              vm.PropertyChanged += (o, s) =>
              {
                  if (s.PropertyName.StartsWith("error"))
                  {
                      foreach (בעייתיים)
                      {
                          TextBlock textBlock = d[שם של משתנה בעייתי];
                          textBlock.Text = "error";
                      }
                  }
              };*/
        }
        private void ProgressBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }



        private void Slider_Aileron_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

        private void Slider_Throttle_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }
        private void Click_Disconnect(object sender, RoutedEventArgs e)
        {
            vm.stop();
            Button b = sender as Button;
            b.Background = Brushes.LightPink;
            buttonConnect.Background = Brushes.LightGray;

        }

        private void Click_Connect(object sender, RoutedEventArgs e)
        {
            //if (firstInConnect)
            //       {
            //         vm.connect();
            //         this.firstInConnect = false;
            //           }
            //        else
            //            { 
            if (!vm.IsConnected())
            {
                vm.connect();
            }
                vm.not_stop();
//            }
            Button b = sender as Button;
            b.Background = Brushes.LightPink;
            buttonDisConnect.Background = Brushes.LightGray;
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }







        /*  private void mouseDown(object sender, MouseButtonEventArgs e)
          {

              Joystick joy = (sender as Joystick);
              this.centerX = joy.Width / 2;
              this.centerY = joy.Height / 2;
              double x = e.GetPosition(joy).X;
              double y = e.GetPosition(joy).Y;

              if (x < (this.centerX + joy.KnobBase.Width / 2) &&
                  x > (this.centerX - joy.KnobBase.Width / 2) && y < (this.centerY + joy.KnobBase.Height / 2) && y > (this.centerY - joy.KnobBase.Height / 2))
              {

                  this.mousePressed = true;
              }
              //  {
              //if (e.LeftButton == MouseButtonState.Pressed)
              //{
              //      MouseDownLoaction = e.Location();
              //    Console.WriteLine("mouse");
              //}

              //  Console.WriteLine("mouse pressed");
              // startX= e.GetPosition(sender as Joystick).X;
              //startY = e.GetPosition(sender as Joystick).Y;
              // mousePressed = true;
              //   }
          }

          private void J_Loaded(object sender, RoutedEventArgs e)
          {

          }



          /* private void mouseDown(object sender, TouchEventArgs e)
           {
               if (!mousePressed)
               {
                   mousePressed = true;
               }
           }

          private void mouseMove(object sender, MouseEventArgs e)
          {
              Joystick joy = (sender as Joystick);

              if (e.GetPosition(joy).X < joy.Width - (joy.KnobBase.Width / 2) && e.GetPosition(joy).X  > 0 + (joy.KnobBase.Height / 2) 
                  && e.GetPosition(joy).Y  < joy.Height -(joy.KnobBase.Width / 2) && e.GetPosition(joy).Y > 0 + (joy.KnobBase.Height / 2) 
                  && this.mousePressed)
              {
                  joy.knobPosition.X = e.GetPosition(joy).X - this.centerX;
                  joy.knobPosition.Y = e.GetPosition(joy).Y - this.centerY;
              }
              endX = joy.knobPosition.X;
              endY = joy.knobPosition.Y;
              double rudderX = (endX - this.centerX) / (joy.Width / 2);
              double elelvatorY = -1 * ((endY - this.centerY) / (joy.Height / 2));
              this.vm.moveJoystick(rudderX, elelvatorY);
              //if (e.button)
              //Console.WriteLine("mouse move");
              //this.j.knobPosition.X = 100;
              //this.j.knobPosition.Y = 200;
              //     mousePressed = false;
              // vm.moveJoystick(0, 0);
          }
          private void mouseUp(object sender, MouseButtonEventArgs e)
          {
              Joystick joy = (sender as Joystick);
              if (e.GetPosition(joy).X < joy.Width && e.GetPosition(joy).X > 0 
                  && e.GetPosition(joy).Y < joy.Height && e.GetPosition(joy).Y > 0 
                  && this.mousePressed)
              {
                  endX = e.GetPosition(joy).X;
                  endY = e.GetPosition(joy).Y;
              }
              else
              {
                  endX = joy.knobPosition.X;
                  endY = joy.knobPosition.Y;
              }
              double rudderX = (endX - this.centerX) / (joy.Width / 2);
              double elelvatorY = -1 * ((endY - this.centerY) / (joy.Height/2));
              this.vm.moveJoystick(rudderX, elelvatorY);
             // Storyboard story = this.FindResource("CenterKnob") as Storyboard;
             // story.Begin();
            //  Joystick joy = (sender as Joystick);


              //   joy.knobPosition.X = rudderX;
              // joy.knobPosition.Y = elelvatorY;
             // Console.WriteLine(endX);
            //  Console.WriteLine(endY);

              this.mousePressed = false;
            //  joy.knobPosition.X = this.centerX;
              joy.knobPosition.Y = this.centerY;

              //     joy.knobPosition.X = ;      //      if (mousePressed)
              //    {
              //  Joystick r = (sender as Joystick);
              //endX = 0.5;//e.GetPosition(r).X;
              //  endY = 0.5;// e.GetPosition(r).Y;
              //  double lineLength = length(x,y,x1,y1);
              //   double longestDiagnoal = length(0, 0, r.Width, r.Height);
              //  double speed = 10 * lineLength / longestDiagnoal;
              //  int angle = 0;
              //  vm.moveRobot(speed, angle); }
              //    Console.WriteLine("mouse up if");
              //  vm.moveJoystick(endX, endY);
              //      }
          }*/
    }
}
  
