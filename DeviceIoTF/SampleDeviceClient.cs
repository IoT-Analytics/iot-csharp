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
    /// <author>Kaberi Singh, kabsingh@in.ibm.com</author>
    /// <date>28/08/2015 09:05:05 </date>
    /// <summary>
    ///     A sample device client
    /// </summary>
    public class SampleDeviceClient 
    {
       
        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();
            ILog log = log4net.LogManager.GetLogger(typeof(SampleDeviceClient));
            DeviceClient deviceClient = new DeviceClient("j82zgk", "TestType", "9663155111", "token", "y1&_!98TlxScdk?hqP");

            deviceClient.connect();
            deviceClient.publishEvent("temp", "json", "{temp:23}", 2);
            deviceClient.subscribeCommand("testcmd","json",0);
            deviceClient.commandCallback += processCommand; 

            deviceClient.disconnect();
       }

        public static void processCommand(String cmdName, string format, string data) {
            // Console.WriteLine("Sample Device Client : Sample Command " + cmdName + " " + "format: " + format + "data: " + data);
        }
     
    }

}
