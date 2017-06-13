using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Receiver
{
    class Program
    {
        private static MessageQueue mq;

        static void Main(string[] args)
        {
            string path = ".\\private$\\killf";
            if (MessageQueue.Exists(path))
            {
                mq = new MessageQueue(path);
            }
            else
            {
                mq = MessageQueue.Create(path);
            }

            mq.Formatter = new XmlMessageFormatter(new Type[] {typeof (string)});
            mq.ReceiveCompleted += MqReceiveCompleted;
            mq.BeginReceive();


        }

        private static void MqReceiveCompleted(object sender, ReceiveCompletedEventArgs e)
        {
            MessageQueue mq1 = (MessageQueue)sender;
            System.Messaging.Message message = mq1.EndReceive(e.AsyncResult);

            string str = message.Body.ToString();
            Console.WriteLine(str);

            mq.BeginReceive();
        }
    }
}
