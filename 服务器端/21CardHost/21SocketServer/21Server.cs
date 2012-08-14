using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase;

namespace _21SocketServer
{
    public class _21Server : AppServer<_21Session>
    {
        //public object SyncObj = new object();

        //public _21Session GetSessionByUserName(string username)
        //{
        //    return GetSessions(m => m.UserName == username).SingleOrDefault() as _21Session;
        //}

        public List<_21Session> GetAllSession()
        {
            return GetSessions(m => true).ToList();
        }

        protected override void OnSocketSessionClosed(object sender, SuperSocket.SocketBase.SocketSessionClosedEventArgs e)
        {
            base.OnSocketSessionClosed(sender, e);
            lock (Game)
            {
                Game.LostPlayer(e.IdentityKey);
            }
        }

        protected override void OnStartup()
        {
            base.OnStartup();
            Game = new Game();
            Game.Server = this;
        }

        public Game Game { get; set; }
    }
}
