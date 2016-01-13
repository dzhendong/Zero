
using System;
using System.Collections.Generic;
using System.Text;
using ILvYou.Zero.Config;
using ILvYou.Zero.Logs;
using ILvYou.Zero.Utility;
using ZeroMQ;
using ZeroMQ.Monitoring;

namespace ILvYou.Zero.Base
{
    /// <summary>
    /// 抽象服务
    /// </summary>
    public abstract class AbstractService
    {
        #region Field
        private Dictionary<string, ZmqContext> contexts;
        private Dictionary<string, ZmqSocket> sockets;
        private Dictionary<string, ZmqMonitor> monitors;
        private Dictionary<string, ZeroRoute> routes;

        private volatile bool inited;
        public Processor processor;
        #endregion

        #region APIs
        public void Declare()
        {
            if (!this.inited)
            {
                ZeroLog.LogInfo("producer inited....");

                contexts = new Dictionary<string, ZmqContext>();
                sockets = new Dictionary<string, ZmqSocket>();
                monitors = new Dictionary<string, ZmqMonitor>();

                List<ZeroRoute> rules = XMLExchange.GetInstance().Configs;

                routes = new Dictionary<string, ZeroRoute>(rules.Count);

                foreach (var config in rules)
                {
                    ZmqContext context = ZmqContext.Create();
                    ZmqSocket socket = context.CreateSocket(PreSocketType());
                    ZmqMonitor monitor = context.CreateMonitor();

                    socket.SendTimeout = new TimeSpan(0, 0, config.SendTimeout);
                    socket.MaxMessageSize = config.MaxMessageSize;

                    contexts.Add(config.ExchangeName, context);
                    sockets.Add(config.ExchangeName, socket);
                    monitors.Add(config.ExchangeName, monitor);

                    routes.Add(config.ExchangeName, config);

                    Bind(socket, config);
                    Connect(socket, config);

                    ZeroLog.LogInfo("producer inited...." + config.ExchangeName);
                }

                processor = new Processor(contexts, sockets, monitors, routes);

                this.inited = true;
            }
        }

        public Processor TProcessor
        {
            get
            {
                return processor;
            }
        }

        public void Close(string exchangeName)
        {
            ZeroLog.LogInfo("producer close....");

            ZmqContext context = contexts[exchangeName];
            ZmqSocket socket = sockets[exchangeName];
            ZmqMonitor monitor = monitors[exchangeName];

            monitor.Dispose();
            socket.Dispose();
            context.Dispose();

            contexts.Remove(exchangeName);
            sockets.Remove(exchangeName);
            monitors.Remove(exchangeName);
        }

        public ZmqSocket this[string exchangeName]
        {
            get
            {
                return sockets[exchangeName];
            }
        }

        public virtual SocketType PreSocketType()
        {
            return SocketType.PUB;
        }

        public abstract void Bind(ZmqSocket socket, ZeroRoute config);

        public abstract void Connect(ZmqSocket socket, ZeroRoute config);
        #endregion
    }
}
