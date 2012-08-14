using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase.Command;
using Newtonsoft.Json;

namespace _21SocketServer.Command
{
    public class JOIN : JsonSubCommandBase<_21Session,int>
    {
        protected override void ExecuteJsonCommand(_21Session session, int seat)
        {
            Game game = session.Server.Game;
            lock (game)
            {
                if (game.IsStart)
                {
                    session.SendResponse("ERROR 牌局已经开始，请稍后！");
                    return;
                }
                if (game.PlayerList.Count >= 4)
                {
                    session.SendResponse("ERROR 牌局人数已满，请稍后！");
                    return;
                }
                var temp = (from p in game.PlayerList
                             where p.Session == session
                             select p).SingleOrDefault();
                if (temp != null)
                {
                    session.SendResponse("ERROR 你已经加入了牌局！");
                    return;
                }
                temp = (from p in game.PlayerList
                        where p.Seat == seat
                        select p).SingleOrDefault();
                if (temp != null)
                {
                    session.SendResponse("ERROR 该座位上已有人！");
                    return;
                }
                
                Player player = new Player();
                player.Session = session;
                session.Player = player;
                player.Seat = seat;
                player.IsStart = false;
                game.PlayerList.Add(player);

                game.SendPlayerInfo();
            }
        }

     
    }
}
