using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FlightSimulatorApp
{
    public sealed class Singleton
    {
        private static Singleton instance = null;
       // private Dictionary<string, double> steeringWheel = new Dictionary<string, double>();
        //private Dictionary<string, double> map = new Dictionary<string, Double>();
        private Dictionary<string, double> sliderBoard = new Dictionary<string, double>();
        private Singleton()
        {
            this.addToMap();
            
        }
        public void addToMap()
        {
            sliderBoard.Add("/instrumentation/heading-indicator/indicated-heading-deg\n", 0.0);
            sliderBoard.Add("/instrumentation/gps/indicated-vertical-speed\n", 0.0);
            sliderBoard.Add("/instrumentation/gps/indicated-ground-speed-kt\n", 0.0);
            sliderBoard.Add("/instrumentation/airspeed-indicator/indicated-speed-kt\n", 0.0);
            sliderBoard.Add("/instrumentation/gps/indicated-altitude-ft\n", 0.0);
            sliderBoard.Add("/instrumentation/attitude-indicator/internal-roll-deg\n", 0.0);
            sliderBoard.Add("/instrumentation/attitude-indicator/internal-pitch-deg\n", 0.0);
            sliderBoard.Add("/instrumentation/altimeter/indicated-altitude-ft\n", 0.0);

            sliderBoard.Add("/controls/engines/current-engine/throttle\n", 0.0);
            sliderBoard.Add("/controls/flight/elevator\n", 0.0);
            sliderBoard.Add("/controls/flight/rudder\n", 0.0);
            sliderBoard.Add("/controls/flight/aileron\n", 0.0);

            sliderBoard.Add("/position/latitude-deg\n", 0.0);
            sliderBoard.Add("/position/longitude-deg\n", 0.0);
        }
        public Dictionary<string, double> getSliderBoard()
        {
            return this.sliderBoard;
        }
      /*  public Dictionary<string, double> getSteeringWheel()
        {
            return this.steeringWheel;
        }
        public Dictionary<string, double> getMap()
        {
            return this.map;
        }*/
        public void set(string s, string data)
        {

         //   try
           // {
               double dataInt = Convert.ToDouble(data);
                if (this.getSliderBoard().ContainsKey(s))
                {
              
                      sliderBoard[s] = dataInt;
                }
              
           // }
            
            //catch (System.FormatException e)
            //{
             //   Console.WriteLine("error: " + data + " path: " + s);

            //}
        }
        public static Singleton Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Singleton();
                }
                return instance;
            }
        }
    
    }
  
}
