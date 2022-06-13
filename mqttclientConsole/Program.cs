using uPLibrary.Networking;
using System.Text;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using System;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace mqttclientConsole
{
    public class Program
    {
        private static int mcount;
        static void Main(string[] args)
        {
            Console.WriteLine("***********PUBLISHER***********");

            //MqttClient client = new MqttClient("dev.rabbitmq.com", 1883, false, null);
            MqttClient client = new MqttClient("localhost");
            var state = client.Connect("rinhtt");
           
            var msg = new ServerContext();
            msg.ClientId = "rinhtt";
            msg.Value = "hello";
            msg.Url = "manage/devicelist/";         
            client.Publish("device" , UTF8Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(msg)));
            client.Subscribe(new[] { "response/default/rinhtt" }, new[] {(byte)0});

            client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;
            Console.ReadLine();

        }
        static void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            // handle message received 
            Console.WriteLine("Received = " + Encoding.UTF8.GetString(e.Message) + " on topic " + e.Topic);
            
        }


    }
    public class ServerContext
    {
        public string Url { get; set; }
        public string ClientId { get; set; }
        public string Token { get; set; }
        public object Value { get; set; }

        public T ParseObject<T>()
        {
            return ((Newtonsoft.Json.Linq.JObject)Value).ToObject<T>();
        }
        public List<T> ParseArray<T>()
        {
            var lst = new List<T>();
            foreach (var e in ((Newtonsoft.Json.Linq.JArray)Value))
            {
                lst.Add(e.ToObject<T>());
            }
            return lst;
        }
    }
}
