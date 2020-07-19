using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

using System.Data.SqlClient;
using System.Windows;

namespace FlightSimulatorApp
{
    class ViewModel : INotifyPropertyChanged
    {
        private Singleton singleton = Singleton.Instance;
    
        private IModel model;


        private double throttle;
        private double alieron;
        private double rudder;
        private double elevator;
        public event PropertyChangedEventHandler PropertyChanged;
        public double Throttle { get { return throttle; } set { throttle = value; NotifyPropertyChanged("throttle"); } }
        public double Aileron { get { return alieron; } set { alieron = value; NotifyPropertyChanged("alieron"); } }
        public double Rudder { get { return rudder; } set { rudder = value; NotifyPropertyChanged("rudder"); } }
        public double Elevtor { get { return elevator; } set { elevator = value; NotifyPropertyChanged("elevator"); } }

        private bool isConnect = true; 
        private int port;
        private string ip;
        public int Port { get { return port; } set { port = value; } }
        public string Ip { get { return ip; } set { ip = value; } }


        public ViewModel(IModel m)
        {
            this.model = m;
            model.PropertyChanged +=
            delegate (Object sender, PropertyChangedEventArgs e) {
                NotifyPropertyChanged("VM_" + e.PropertyName);
            };


        }

        public void NotifyPropertyChanged(string propName) {
            if (this.PropertyChanged != null)
            {
               
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
        public string VM_error
        {
            set
            {

            }
            get
            {
                string s = "error in ";
                bool enter = false;
                Dictionary<string, bool> temp = model.returnError();
                for (int i = 0; i < temp.Count(); i++)
                {
                    
                    if (temp.ToList()[i].Value)
                    {
                        enter = true;
                        s += temp.ToList()[i].Key + "  ";
                    }
                    
                    
                }
                if (enter)
                {
                    return s;
                }
                return "ready";
            }
        }
        // Properties
        public double VM_heading
        {   
            set
            {

            }
            get
            { 
                return model.Heading;
            }
        }
        public double VM_vertical_speed
        {
            set
            {

            }
            get
             {
            
                return model.Vertical_speed;
            }
        }
        public double VM_ground_speed
        {
            set
            {

            }
            get
            {

                return model.Ground_speed;
            }
        }
        public double VM_air_speed
        {
            set
            {

            }
            get
            {

                return model.Air_speed;
            }
        }
        public double VM_altitude
        {
            set
            {

            }
            get
            {

                return model.Altitude;
            }
        }
        public double VM_roll
        {
            set
            {

            }
            get
            {

                return model.Roll;
            }
        }
        public double VM_pitch
        {
            set
            {

            }
            get
            {
                return model.Pitch;
            }
        }
        public double VM_altimeter
        {
            set
            {

            }
            get
            {
                return model.Altimeter;
            }
        }
        public Location VM_location
        {
            set
            {

            }
            get
            {
              
                
                /*   if (model.Longitude < 180 && model.Longitude > -180)
                   {
                       x = model.Longitude;
                       Console.WriteLine(model.Latitude);

                   }
                   else

                   {
                       model.returnTempMap().TryGetValue("/position/longtitude-deg\n",out x);

                   }
                   if (model.Latitude < 90 && model.Latitude > -90)
                   {
                       y = model.Latitude;
                   }
                   else
                   {
                       model.returnTempMap().TryGetValue("/position/latitude-deg\n", out y);

                   }

                   Location l = new Location(y,x);*/
                Location l = new Location(model.Latitude, model.Longitude);
                return l;
            }
        }
        public void stop()
        { 
            this.model.disconnect();
        }
        public void not_stop ()
        {
            model.not_stop();
            model.start();
        }
        public void connect()
        {

            try
            {
                try
                {
                    this.model.connect( Ip, Port);
                    model.start();
                    this.isConnect = true;
                }
                catch
                {
                    this.isConnect = true;

                    MessageBox.Show("Ip and Port invalid, connection from system configuration");

                    var connectionString = ConfigurationManager.ConnectionStrings["MyDatabase"];
                    string con = connectionString.ToString();
                    string[] s = con.Split(',');
                    string port = s[1];
                    string ip = s[0];
                    try
                    {
                        this.model.connect( ip, Int32.Parse(port));
                        model.start();
                    }
                    catch
                    {
                        this.model.connect( "127.0.0, 1", 5402);
                        model.start();
                        this.isConnect = true;

                    }
                }
            }
            catch (System.Net.Sockets.SocketException)
            {
                model.TimeOut = "the server is closed";
                isConnect = false;
            }

        }
        public bool IsConnected()
        {
            return isConnect;
        }
        public double VM_slider_throttle
        {
            set
            {
                this.throttle = value;
                model.MoveThrottle(this.throttle, "/controls/engines/current-engine/throttle");

            }
            get
            {
                return throttle;
            }
        }
        public string VM_TimeOut
        {
            set
            {
               // this.timeOut = value;
            }
            get
            {
                string s = model.TimeOut;
                return s;
            }
        }
        public double VM_slider_alieron
        {
            set
            {
                this.alieron = value;
                model.MoveAileron(this.alieron, "/controls/flight/aileron");
            }
            get
            {
                return alieron;
            }
        }

        public int VM_port
        {
            set
            {
                this.port = value;
            }
        }
        public void moveJoystickR(double rudder)
        {
            model.moveRudder(rudder);
        }
        public void moveJoystickE(double elevator)
        {
            model.moveElevator(elevator);
        }

        public double VM_Rudder
        {
            set
            {
                this.rudder = value;
                moveJoystickR(rudder);
            }
            get
            {
                return rudder;
            }
        }
        public double VM_Elevaltor
        {
            set
            {
                this.elevator = value;
                moveJoystickE(elevator);
            }
            get
            {
                return elevator;
            }
        }



    }
}
