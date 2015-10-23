using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace com.ibm.iotf.client.app.sample
{
    /// <author>Kaberi Singh, kabsingh@in.ibm.com</author>
    /// <date>28/08/2015 09:05:05 </date>
    /// <summary>
    ///     Sample application client
    /// </summary>
    class SampleAppClient
    {

        private static string orgId = "j82zgk";
        private static string appId = "9663155111";
        private static string apiKey = "a-j82zgk-mwum96l85s";
        private static string authToken = "JmbckABL5ixSu-DdRq";

        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();
            ILog log = log4net.LogManager.GetLogger(typeof(SampleAppClient));
            try
            {
                string data = "name:foo,cpu:60,mem:50";
                string deviceType = "TestType";
                string deviceId = "9663155111";
                string evt = "temp";
                string format = "json";

                ApplciationClient applicationClient = new ApplciationClient(orgId, appId, apiKey, authToken);
                applicationClient.connect();

                log.Info("Connected sucessfully to app id : " + appId);

                applicationClient.commandCallback += processCommand;
                applicationClient.eventCallback += processEvent;
                applicationClient.deviceStatusCallback += processDeviceStatus;
                applicationClient.appStatusCallback += processAppStatus;

                applicationClient.subscribeToDeviceStatus();
                applicationClient.subscribeToApplicationStatus();
                applicationClient.subscribeToDeviceEvents(deviceType, deviceId, evt, format, 0);
                applicationClient.publishCommand(deviceType, deviceId, "testcmd", "json", data, 0);

                applicationClient.disconnect();
            }
            catch (Exception)
            {
                // ignore
            }
        }

        public static void processCommand(String cmdName, string format, string data)
        {
            // Console.WriteLine("Sample Application Client : Sample Command : " + cmdName + " format : " + format + " data : " + data);
        }

        public static void processEvent(String eventName, string format, string data)
        {
            // Console.WriteLine("Sample Application Client : Sample Event : " + eventName + " format : " + format + " data : " + data);
        }

        public static void processDeviceStatus(String deviceType, string deviceId, string data)
        {
            // Console.WriteLine("Sample Application Client : Sample Device status : " + deviceType + " format : " + deviceId + " data : " + data);
        }

        public static void processAppStatus(String appId, String data)
        {
            // Console.WriteLine("Sample Application Client : Sample App Status : " + appId + " data : " + data);
        }
    }
}
