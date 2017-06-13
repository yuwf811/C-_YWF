using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace YSocket
{
    public class YSocketServer
    {
        public void Connect()
        {
            //定义IP地址
            IPAddress local = IPAddress.Parse("127.0,0,1");
            IPEndPoint iep = new IPEndPoint(local, 13000);
            //创建服务器的socket对象
            Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            server.Bind(iep);
            server.Listen(20);
            server.BeginAccept(new AsyncCallback(Accept), server);  
        }

        void Accept(IAsyncResult iar)
        {
            //还原传入的原始套接字
            Socket MyServer = (Socket)iar.AsyncState;
            //在原始套接字上调用EndAccept方法，返回新的套接字
            Socket service = MyServer.EndAccept(iar);
        }
    }
}
