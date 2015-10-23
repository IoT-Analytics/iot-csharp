using ConsoleApplication2.Properties;
using System;
using System.Configuration;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using System.Threading;
using uPLibrary.Networking.M2Mqtt.Exceptions;

using log4net;

namespace com.ibm.iotf.client.device.sample
{
    ///<author>Kaberi Singh, kabsingh@in.ibm.com</author>
    /// <date>28/08/2015 09:05:05 </date>
    /// <summary>
    ///     Sample device client for auto generation of data
    /// </summary>
    public class SampleDeviceClientAutoData
    {
       
        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();
            ILog log = log4net.LogManager.GetLogger(typeof(SampleDeviceClientAutoData));
            DeviceClient deviceClient = new DeviceClient("j82zgk", "TestType", "9663155111", "token", "y1&_!98TlxScdk?hqP");
            deviceClient.connect();
            
            deviceClient.subscribeCommand("testcmd","json",0);
            deviceClient.commandCallback += processCommand;

            for (int i = 0; i < 10; i++)
            {
                String data = "{time:" + DateTime.Now.ToString() + "}";
                deviceClient.publishEvent("temp", "json", data, 2);
                Thread.Sleep(1000);
            }
            
            deviceClient.disconnect();
       }

        public static void processCommand(String cmdName, string format, string data) {
            //Console.WriteLine("Sample Device Client : Sample Command " + cmdName + " " + "format: " + format + "data: " + data);
        }
        
         
    }


}
