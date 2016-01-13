using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ILvYou.Zero.API;
using ILvYou.Zero.Config;
using ZeroMQ;
using ZeroMQ.Monitoring;

namespace ILvYou.Zero.Base
{
    public class Processor : TProcessor
    {
        #region Field
        private Dictionary<string, ZmqContext> contexts;
        private Dictionary<string, ZmqSocket> sockets;
        private Dictionary<string, ZmqMonitor> monitors;
        private Dictionary<string, ZeroRoute> routes;
        #endregion

        #region Ctor
        public Processor(Dictionary<string, ZmqContext> contexts,
            Dictionary<string, ZmqSocket> sockets,
            Dictionary<string, ZmqMonitor> monitors,
            Dictionary<string, ZeroRoute> routes)
        {
            this.contexts = contexts;
            this.sockets = sockets;
            this.monitors = monitors;
            this.routes = routes;
        }

        private ZmqSocket this[string exchangeName]
        {
            get
            {
                return sockets[exchangeName];
            }
        }
        #endregion

        #region Field
        public Encoding ZeroEncode
        {
            get
            {
                return Encoding.UTF8;
            }
        }

        public TimeSpan Timeout(string exchangeName)
        {
            ZeroRoute route = routes[exchangeName];
            return new TimeSpan(0, 0, route.SendTimeout);
        }
        #endregion

        #region APIs
        public SendStatus Send(string exchangeName, byte[] buffer)
        {
            return this[exchangeName].Send(buffer);
        }

        public SendStatus Send(string exchangeName, string message)
        {
            return this[exchangeName].Send(message, ZeroEncode);
        }

        public SendStatus SendMore(string exchangeName, byte[] buffer)
        {
            return this[exchangeName].SendMore(buffer);
        }

        public SendStatus SendMore(string exchangeName, string message)
        {
            return this[exchangeName].SendMore(message, ZeroEncode);
        }

        public string Receive(string exchangeName)
        {
            return this[exchangeName].Receive(ZeroEncode);
        }

        public Frame ReceiveFrame(string exchangeName)
        {
            return this[exchangeName].ReceiveFrame();
        }

        public Frame ReceiveFrame(string exchangeName, Frame frame)
        {
            return this[exchangeName].ReceiveFrame(frame);
        }

        public SendStatus SendFrame(string exchangeName, Frame frame)
        {
            return this[exchangeName].SendFrame(frame);
        }

        public ZmqMessage ReceiveMessage(string exchangeName)
        {
            return this[exchangeName].ReceiveMessage();
        }

        public ZmqMessage ReceiveMessage(string exchangeName, ZmqMessage message)
        {
            return this[exchangeName].ReceiveMessage(message);
        }

        public ZmqMessage ReceiveMessage(string exchangeName, TimeSpan frameTimeout)
        {
            return this[exchangeName].ReceiveMessage(frameTimeout);
        }

        public ZmqMessage ReceiveMessage(string exchangeName, ZmqMessage message, TimeSpan frameTimeout)
        {
            return this[exchangeName].ReceiveMessage(message, frameTimeout);
        }

        public SendStatus SendMessage(string exchangeName, ZmqMessage message)
        {
            return this[exchangeName].SendMessage(message);
        }

        public int Receive(string exchangeName, byte[] buffer)
        {
            return this[exchangeName].Receive(buffer);
        }

        public int Receive(string exchangeName, byte[] buffer, SocketFlags flags)
        {
            return this[exchangeName].Receive(buffer, flags);
        }

        public byte[] Receive(string exchangeName, byte[] buffer, out int size)
        {
            return this[exchangeName].Receive(buffer, out size);
        }

        public byte[] Receive(string exchangeName, byte[] buffer, SocketFlags flags, out int size)
        {
            return this[exchangeName].Receive(buffer, flags, out size);
        }

        public int Send(string exchangeName, byte[] buffer, int size, SocketFlags flags)
        {
            return this[exchangeName].Send(buffer, size, flags);
        }

        public void Forward(string exchangeName, ZmqSocket destination)
        {
            this[exchangeName].Forward(destination);
        }

        public void AddTcpAcceptFilter(string exchangeName, string filter)
        {
            this[exchangeName].AddTcpAcceptFilter(filter);
        }

        public void ClearTcpAcceptFilter(string exchangeName)
        {
            this[exchangeName].ClearTcpAcceptFilter();
        }
        #endregion
    }
}
