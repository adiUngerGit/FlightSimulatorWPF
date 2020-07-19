using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.ComponentModel;
using System.Timers;
using System.Diagnostics;

namespace FlightSimulatorApp
{
    class Model : IModel
    {
        //private Mutex mtx = new Mutex();
        private Singleton singleton = Singleton.Instance;
        private static Mutex mtx;// = new Mutex();
        public event PropertyChangedEventHandler PropertyChanged;

        private IClient client;
        volatile Boolean stop;
        private string timeOut;
        //map
        private double latitude;
        private double longitude;

        public void not_stop()
        {
            this.stop = false;
        }
        public string TimeOut
        {
            set
            {
                this.timeOut = value;
                NotifyPropertyChanged("TimeOut");

            }
            get

            {
                return this.timeOut;
            }
        }
        //map properties
        public double Latitude
        {
            get
            {
                return latitude;
            }
            set
            {

                if (value > 90)
                {
                    latitude = 90;
                    error["latitude"] = true;
                    NotifyPropertyChanged("error");

                }
                else if (value < -90)
                {
                    latitude = -90;
                    error["latitude"] = true;
                    NotifyPropertyChanged("error");

                }
                else
                {
                    latitude = value;
                    error["latitude"] = false;
                }
                NotifyPropertyChanged("location");

            }
        }
        public double Longitude
        {
            get
            {
                return longitude;
            }
            set
            {
                if (value > 180)
                {
                    longitude = 180;
                    error["longitude"] = true;
                    NotifyPropertyChanged("error");

                }
                else if (value < -180)
                {
                    longitude = -180;
                    error["longitude"] = true;
                    NotifyPropertyChanged("error");

                }
                else
                {
                    longitude = value;
                    error["longitude"] = false;
                }
                NotifyPropertyChanged("location");

            }
        }
        //slider board
        private double heading;
        private double vertical_speed;
        private double ground_speed;
        private double air_speed;
        private double altitude;
        private double roll;
        private double pitch;
        private double altimeter;

        public  Dictionary<string, bool> error = new Dictionary<string, bool>();
    
        //slider board properties
        public double Heading { get { return heading; }
            set
            {
                    heading = value;
                    error["heading"] = false;
               
                NotifyPropertyChanged("heading");
            }
        } //instrumentation/heading-indicator/indicated-heading-deg\n
        public double Vertical_speed
        {
            get
            {
                return vertical_speed;
            }
            set
            {

                    vertical_speed = value;
                    error["vertical_speed"] = false;
               
                NotifyPropertyChanged("vertical_speed");
            }
        } ///instrumentation/gps/indicated-vertical-speed\n
        public double Ground_speed
        {
            get
            {
                return ground_speed;
            }
            set
            {
                    ground_speed = value;
                    error["ground_speed"] = false;
               
                NotifyPropertyChanged("ground_speed");
            }
        }///instrumentation/gps/indicated-ground-speed-kt\n
        public double Air_speed
        {
            get
            {
                return air_speed;
            }
            set
            {
                    air_speed = value;
                    error["air_speed"] = false;
                NotifyPropertyChanged("air_speed");
            }
        }///instrumentation/airspeed-indicator/indicated-speed-kt'
        public double Altitude
        {
            get
            {
                return altitude;
            }
            set
            {

                    altitude = value;
                    error["altitude"] = false;
                
                NotifyPropertyChanged("altitude");

            }
        }  ///instrumentation/gps/indicated-altitude-ft\n
        public double Roll
        {
            get
            {
                return roll;
            }
            set
            {
                    roll = value;
                    error["roll"] = false;
                
                NotifyPropertyChanged("roll");
            }
        }  ///instrumentation/attitude-indicator/internal-roll-deg'\n
        public double Pitch
        {
            get
            {
                return pitch;
            }
            set
            {
                    pitch = value;
                    error["pitch"] = false;
                NotifyPropertyChanged("pitch");

            }
        } ///instrumentation/attitude-indicator/internal-pitch-deg\n
        public double Altimeter
        {
            get
            {
                return altimeter;
            }
            set
            {
                        altimeter = value;
                        error["altimeter"] = false;
                    NotifyPropertyChanged("altimeter");
            }
        } ////instrumentation/altimeter/indicated-altitude-ft\n
        //property streeing wheel

        public Model(IClient c)
        {
            this.client = c;
            stop = true;
         //   this.connect("127.0.0.1", 5402);
            mtx = new Mutex();
        }
        public void connect(string ip, int port)
        {
            stop = false;
            this.client.connect(ip, port);
         
              
           

        }
      

        public void disconnect()
        {
            this.stop = true;
      //      this.client.dissconect();
        }
        public void start()
        {

            new Thread(delegate ()
            {
                DateTime start = DateTime.UtcNow;

                while (!stop)
                {

                    string s = "";
                    mtx.WaitOne();

                    foreach (var pair in singleton.getSliderBoard().ToList())
                    {
                    //    temp = singleton.getSliderBoard();
                        
                        if (!stop)
                        {


                            try
                            {

                                //  new Thread(delegate ()
                                // {
                                //mtx.WaitOne();
                                s = client.read("get " + pair.Key);
                                this.readingClient(pair.Key, s);
                                //mtx.ReleaseMutex();

                                /*  if (pair.Key.Contains("/controls/flight/rudder"))
                                  {
                                      Console.WriteLine("rudder " + s);
                                  }
                                  if (pair.Key.Contains("/controls/flight/elevator"))
                                  {
                                      Console.WriteLine("elavator " + s);
                                  }
                                  Console.WriteLine();*/
                                /*   if (pair.Key.Contains("/instrumentation/altimeter/indicated-altitude-ft"))
                                   {
                                       Console.WriteLine("altimeter " + s);
                                   }*/

                                //}).Start();
                            }
                            catch (Exception ex)
                            {
                                if (ex is TimeoutException || ex is System.InvalidOperationException)
                                {
                                    TimeOut = "Error while reading the server";
                                    //Thread.Sleep(5000);
                                    DateTime end = DateTime.UtcNow;
                                    TimeSpan timeDiff = end - start;
                                    if (timeDiff.Seconds >= 30)
                                    {
                                        //throw new System.TimeoutException();
                                        TimeOut = "found problems in the server";
                                        //Thread.Sleep(3000);

                                        this.client.dissconect();
                                        disconnect();
                                    }
                                }
                            }


                        }



                    }
                    start = DateTime.UtcNow;
                    mtx.ReleaseMutex();

                    lock (error)
                    {
                        Heading = singleton.getSliderBoard()["/instrumentation/heading-indicator/indicated-heading-deg\n"];
                        Vertical_speed = singleton.getSliderBoard()["/instrumentation/gps/indicated-vertical-speed\n"];
                        Ground_speed = singleton.getSliderBoard()["/instrumentation/gps/indicated-ground-speed-kt\n"];
                        Air_speed = singleton.getSliderBoard()["/instrumentation/airspeed-indicator/indicated-speed-kt\n"];
                        Altitude = singleton.getSliderBoard()["/instrumentation/gps/indicated-altitude-ft\n"];
                        Roll = singleton.getSliderBoard()["/instrumentation/attitude-indicator/internal-roll-deg\n"];
                        Pitch = singleton.getSliderBoard()["/instrumentation/attitude-indicator/internal-roll-deg\n"];
                        Altimeter = singleton.getSliderBoard()["/instrumentation/altimeter/indicated-altitude-ft\n"];
                        Latitude = singleton.getSliderBoard()["/position/latitude-deg\n"];
                        Longitude = singleton.getSliderBoard()["/position/longitude-deg\n"];
                    }                     // Thread.Sleep(250);// read the data in 4Hz
                }



            }).Start();

        }
        public void readingClient(string path, string s)
        {

            /*    string str = "get " + s;
                byte[] read = Encoding.ASCII.GetBytes(str);
                t.GetStream().Write(read, 0, read.Length);
                byte[] buffer = new byte[1024];
                t.GetStream().Read(buffer, 0, 1024);
                string data = Encoding.ASCII.GetString(buffer, 0, buffer.Length);
                */
            //  string[] data1 = data.Split('\n');
            // data = data1[0];
            try
            {
                    singleton.set(path, s);
                TimeOut = "";
              
            }
            catch (System.FormatException)
            {
                TimeOut = "Error in data";
                //Thread.Sleep(5000);
            }


        }
    
        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }


        public void MoveThrottle(double throttle, string path)
        {
            try
            {
                mtx.WaitOne();
                client.write(path, throttle.ToString());
                mtx.ReleaseMutex();
            }
            catch (Exception ex)
            {
                if (ex is TimeoutException || ex is System.InvalidOperationException)
                {
                    TimeOut = "Error while writing to the server";
                    //Thread.Sleep(5000);
                }
            }
        }

        public void MoveAileron(double aileron, string path)
        {
            try
            {
                mtx.WaitOne();
                client.write(path, aileron.ToString());
                mtx.ReleaseMutex();
            }
            catch (Exception ex)
            {
                if (ex is TimeoutException || ex is System.InvalidOperationException)
                {
                    TimeOut = "Error while writing to the server";
                    //Thread.Sleep(5000);
                }
            }
        }

        public void moveRudder(double rudder)
        {
            try
            {


                if (!stop)
                {
                    mtx.WaitOne();
                    client.write("/controls/flight/rudder", rudder.ToString());
                    mtx.ReleaseMutex();
                }
            }
            catch (Exception ex)
            {
                if (ex is TimeoutException || ex is System.InvalidOperationException)
                {
                    TimeOut = "Error while writing to the server";
                    //Thread.Sleep(5000);
                }
            }
        }
        public void moveElevator(double elevator)
        {
            try
            {
                if (!stop)
                {
                    mtx.WaitOne();
                    client.write("/controls/flight/elevator", elevator.ToString());
                    mtx.ReleaseMutex();
                }
            }
                catch (Exception ex)
            {
                if (ex is TimeoutException || ex is System.InvalidOperationException)
                {
                    TimeOut = "Error while writing server";
                    //Thread.Sleep(5000);
                }
            }
        }
        public Dictionary<string, bool> returnError()
        {
            //mtx.WaitOne();
             return this.error;
           // mtx.ReleaseMutex();

        }
    }

}
