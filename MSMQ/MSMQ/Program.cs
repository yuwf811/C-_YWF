using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace MSMQ
{
    class Program
    {
        static MessageQueue mq;

        static void Main(string[] args)
        {
            Console.WriteLine("Please input sth");
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

            while (true)
            {
                string input = Console.ReadLine();
                mq.Send(input);
            }
        }
    }
}
