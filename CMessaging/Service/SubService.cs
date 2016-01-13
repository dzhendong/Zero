
using System;
using System.Collections.Generic;
using ILvYou.Zero.Config;
using ILvYou.Zero.Logs;
using ILvYou.Zero.Threads;
using ILvYou.Zero.Utility;

namespace ILvYou.Zero.Service
{
    public class SubService
    {
        #region Ctor
        private static SubService subService = new SubService();
        private Dictionary<string, SubThread> pools = new Dictionary<string, SubThread>();

        private volatile bool inited;

        public static SubService GetInstance()
        {
            return SubService.subService;
        }

        private SubService()
        {
        }
        #endregion

        #region APIs
        public void Declare()
        {
            if (!this.inited)
            {
                ZeroLog.LogInfo("consumer inited....");

                List<ZeroRoute> rules = XMLExchange.GetInstance().Configs;

                foreach (var config in rules)
                {
                    SubThread processor = new SubThread();
                    processor.Config = config;
                    processor.ExchangeName = config.ExchangeName;

                    pools.Add(config.ExchangeName, processor);

                    ZeroLog.LogInfo("consumer inited...." + config.ExchangeName);
                }

                this.inited = true;
            }
        }

        private SubThread this[string exchangeName]
        {
            get
            {
                return pools[exchangeName];
            }
        }

        public void Start(string exchangeName)
        {
            ZeroLog.LogInfo("consumer start...." + exchangeName);

            SubThread thd = this[exchangeName];
            thd.TogglePause(false);
            thd.Start();
        }

        public void Stop(string exchangeName)
        {
            SubThread thd = this[exchangeName];

            if (thd != null)
            {
                try
                {
                    ZeroLog.LogInfo("consumer stop....");

                    thd.TogglePause(true);

                    //TODO
                    //线程销毁和重入
                    //thd.Interrupt();
                }
                catch (Exception ex)
                {
                    ZeroLog.LogInfo("consumer stop...." + ex.Message);
                }
                finally
                {
                }
            }
        }
        #endregion
    }
}
