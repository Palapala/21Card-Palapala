using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SuperSocket.Common;
using SuperSocket.SocketEngine.Configuration;
using SuperSocket.SocketEngine;
using System.Configuration;

namespace _21CardHost
{
    public class SuperSocketHost
    {

        public void Run()
        {
            LogUtil.Setup();
            startSuperWebSocket();
        }

        private void startSuperWebSocket()
        {
            var serverConfig = ConfigurationManager.GetSection("socketServer") as SocketServiceConfig;
            if (!SocketServerManager.Initialize(serverConfig))
                return;

            if (!SocketServerManager.Start())
                SocketServerManager.Stop();
        }

    }
}