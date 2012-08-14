using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase.Command;

namespace _21SocketServer.Command
{
    public class STAND : StringCommandBase<_21Session>
    {
        public override void ExecuteCommand(_21Session session, StringCommandInfo commandInfo)
        {
            var game = session.Server.Game;
            lock (game)
            {
                if (!session.Player.IsTurn)
                {
                    session.SendResponse("ERROR 你不是当前行动者！");
                    return;
                }

                session.Player.IsTurn = false;
                session.SendResponse("WAIT_OTHER");

                if (session.Player.IsDealer)
                {
                    game.End();
                    session.Server.Game = new Game();
                    return;
                }

                Player nextPlayer = null;
                while (nextPlayer == null || nextPlayer.IsOut)
                {
                    nextPlayer = (from p in game.PlayerList
                                  where p.Seq == session.Player.Seq + 1
                                  select p).SingleOrDefault();
                    if (nextPlayer == null)
                    {
                        nextPlayer = game.PlayerList[0];
                        if (nextPlayer.IsOut)
                        {
                            game.End();
                            session.Server.Game = new Game();
                            return;
                        }
                    }
                }
                
                nextPlayer.Session.SendResponse("WAIT");
                nextPlayer.IsTurn = true;
            }
        }
    }
}
