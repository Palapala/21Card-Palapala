using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase.Command;
using Newtonsoft.Json;

namespace _21SocketServer.Command
{
    public class HIT : StringCommandBase<_21Session>
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
                var card = game.CardLib.List[0];
                session.Player.CardList.Add(card);
                game.CardLib.List.RemoveAt(0);

                JHitResult result = new JHitResult()
                {
                    Card = card,
                    Point = session.Player.Point,
                    IsExplode = session.Player.IsExplode,
                    Seat = session.Player.Seat
                };

                session.SendResponse(string.Format("HIT {0}", JsonConvert.SerializeObject(new { result = result })));
                result.Point = 0;
                foreach (var player in game.PlayerList) 
                {
                    if (player.Session == session)
                        continue;

                    player.Session.SendResponse(string.Format("HIT {0}", JsonConvert.SerializeObject(new { result = result })));
                }

                if (session.Player.IsExplode)
                {
                    session.Player.IsTurn = false;
                    session.SendResponse("WAIT_OTHER");
                    if (session.Player.IsDealer)
                    {
                        game.End();
                        game.Server = null;
                        session.Server.Game = new Game();
                        session.Server.Game.Server = session.Server;
                    }
                    else
                    {
                        var nextPlayer = (from p in game.PlayerList
                                          where p.Seq == session.Player.Seq + 1
                                          select p).SingleOrDefault();
                        if (nextPlayer == null)
                            nextPlayer = game.PlayerList[0];
                        nextPlayer.Session.SendResponse("WAIT");
                        nextPlayer.IsTurn = true;
                    }
                }
            }
        }
    }
}
