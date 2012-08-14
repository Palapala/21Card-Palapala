using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase.Command;

namespace _21SocketServer.Command
{
    public class START_COMPLETE : StringCommandBase<_21Session>
    {
        public override void ExecuteCommand(_21Session session, StringCommandInfo commandInfo)
        {
            var game = session.Server.Game;
            lock (game) 
            {
                session.Player.IsInit = true;

                bool isAllInit = true;
                foreach(var player in game.PlayerList)
                {
                    if(!player.IsInit)
                    {
                        isAllInit = false;
                        break;
                    }
                }

                if(isAllInit)
                {
                    game.Play();
                }
            }
        }
    }
}
