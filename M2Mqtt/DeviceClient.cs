using System;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace com.ibm.iotf.client.device
{
    class DeviceClient
    {

        public void connect()
        {
            // SUBSCRIBER

            // create client instance
            MqttClient client = new MqttClient(IPAddress.Parse(MQTT_BROKER_ADDRESS));

            // register to message received
            client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;

            string clientId = Guid.NewGuid().ToString();
            client.Connect(clientId);

            // subscribe to the topic "/home/temperature" with QoS 2
            client.Subscribe(new string[] { "/home/temperature" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });

        }

        static void Main(string[] args)
        {
            DeviceClient deviceClient = new DeviceClient();
            deviceClient.connect();


        }
    }
}
