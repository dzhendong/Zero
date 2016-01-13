using System.Collections.Generic;
using System.Text;
using ILvYou.Zero.Base;
using ILvYou.Zero.Config;
using ILvYou.Zero.Logs;
using ILvYou.Zero.Utility;
using ZeroMQ;
using ZeroMQ.Monitoring;

namespace ILvYou.Zero.Service
{
    public class PubService :AbstractService
    {
        #region Ctor
        private static PubService pubService = new PubService();

        public static PubService GetInstance()
        {
            return PubService.pubService;
        }

        private PubService()
        {
        }
        #endregion

        #region APIs
        public override void Bind(ZmqSocket socket, ZeroRoute config)
        {
            ZeroLog.LogInfo("pub bind....");

            foreach (var endPoint in config.ConnectEndPoints())
            {
                socket.Bind(endPoint);

                ZeroLog.LogInfo("pub bind...." + endPoint);
            } 
        }

        public override void Connect(ZmqSocket socket, ZeroRoute config)
        {
        }
        #endregion
    }
}
