using System;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Configuration;
using ConsoleApplication2.Properties;

using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using uPLibrary.Networking.M2Mqtt.Exceptions;


namespace com.ibm.iotf.client.device
{
    public class NewDeviceClient : AbstractClient
    {
        private static string evt = Settings.Default.evt;
        private string[] sub = { evt };
        private byte[] qos = { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE };
        protected string authKeyPassed;
        private NewDeviceClient publisherClient;

        public NewDeviceClient()
        {
            this.clientId = "d" + CLIENT_ID_DELIMITER + getOrgId() + CLIENT_ID_DELIMITER + getDeviceType() + CLIENT_ID_DELIMITER + getDeviceId();
        }

        /**
         * Returns the orgid for this client
         * 
         * @return orgid String orgid
         * 						
         */
        public String getOrgId()
        {
            if (authKeyPassed != null && !authKeyPassed.Trim().Equals("") && !authKeyPassed.Equals("quickstart"))
            {
                if (authKeyPassed.Length >= 8)
                {
                    return authKeyPassed.Substring(2, 8);
                }
            }
            return authKeyPassed;
        }

        public String getDeviceType()
        {
            return Settings.Default["deviceType"].ToString();
        }

        public String getDeviceId()
        {
            string id;
            try
            {
                id = Settings.Default["id"].ToString();
            }
            catch (SettingsPropertyNotFoundException e)
            {
                Console.WriteLine("Property Id not set!");
                throw e;
            }
            return id;
        }

        public void connect()
        {
            base.connect();
            /*if (!getOrgId().equals("quickstart"))
            {
                subscribeToCommands();
            }*/
        }

        /*private void subscribeToCommands()
        {
            try
            {
                mqttClient.subscribe("iot-2/cmd/+/fmt/" + getFormat(), 2);
            }
            catch (MqttException e)
            {
                e.printStackTrace();
            }
        }*/

        public String getFormat()
        {
            if (Settings.Default["format"] != null)
            {
                String format = Settings.Default["format"].ToString();
                return format;
            }
            else
                return "json";
        }

        public void publishEvent()
        {
            publisherClient = new NewDeviceClient();
            publisherClient.setSubscriber(false);
            publisherClient.connect();
            Console.WriteLine("Publisher connected to topic [" + evt + "]");
            int evtCount = 1;
            
            while (evtCount-- != 0) 
            {
                string msg = DateTime.Now.ToString();

                publisherClient.MqttMsgPublish(evt, msg, qosLevel);
                Thread.Sleep(1000);
            }
        }

        public MqttClient getMqttClient()
        {
            return mqttClient;
        }

        private void OnConnectionOnMqttMsgPublishReceived(object o, MqttMsgPublishEventArgs args)
        {
            MqttMsgReceived(args);
        }

        private void MqttMsgReceived(MqttMsgPublishEventArgs e)
        {

            Console.WriteLine("*** Message Received.");
            Console.WriteLine("*** Topic: " + e.Topic);
            Console.WriteLine("*** Message: " + System.Text.UTF8Encoding.UTF8.GetString(e.Message));
            Console.WriteLine("");
        }

        public void MqttMsgPublish(String topic, String msg, byte qosLevel)
        {
            mqttClient.MqttMsgPublished += client_MqttMsgPublished;
            mqttClient.Publish(topic, Encoding.UTF8.GetBytes(msg), qosLevel, false);
            Console.WriteLine("Published to topic [" + topic + "] msg[" + msg + "]");
        }

        void client_MqttMsgPublished(object sender, MqttMsgPublishedEventArgs e)
        {
            Console.WriteLine("*** Message published : " + e.MessageId);
        }


        public void MqttMsgSubscribe(string[] sub, byte[] qos)
        {
            mqttClient.Subscribe(sub, qos);
            // register to message received
            mqttClient.MqttMsgPublishReceived += client_EventPublished;
            mqttClient.MqttMsgSubscribed += client_MqttMsgSubscribed;

        }

        void client_MqttMsgSubscribed(object sender, MqttMsgSubscribedEventArgs e)
        {
            Console.WriteLine("*** Message subscribed : " + e.MessageId);
        }

        void client_EventPublished(Object sender, uPLibrary.Networking.M2Mqtt.Messages.MqttMsgPublishEventArgs e)
        {
            try
            {
                Console.WriteLine("*** Message Received.");
                Console.WriteLine("*** Topic: " + e.Topic);
                Console.WriteLine("*** Message: " + System.Text.UTF8Encoding.UTF8.GetString(e.Message));
                Console.WriteLine("");
            }
            catch (InvalidCastException ex)
            {
            }
        }

        public void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            // handle message received
            string result = System.Text.Encoding.UTF8.GetString(e.Message);
            Console.WriteLine("Result : " + result);
        }

        
    }
}
