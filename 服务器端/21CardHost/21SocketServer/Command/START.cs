using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase.Command;

namespace _21SocketServer.Command
{
    public class START : StringCommandBase<_21Session>
    {
        public override void ExecuteCommand(_21Session session, StringCommandInfo commandInfo)
        {
            Game game = session.Server.Game;
            lock (game)
            {
                if (game.IsStart)
                {
                    session.SendResponse("ERROR 牌局已经开始，请稍后！");
                    return;
                }
                var temp = (from p in game.PlayerList
                            where p.Session == session
                            select p).SingleOrDefault();
                if (temp == null)
                {
                    session.SendResponse("ERROR 你还没有加入了牌局！");
                    return;
                }
                if (temp.IsStart)
                {
                    session.SendResponse("ERROR 你已经开始了！");
                    return;
                }
                temp.IsStart = true;

                game.SendPlayerInfo();

                var isStart = true;
                foreach (var player in game.PlayerList) 
                {
                    if (!player.IsStart)
                        isStart = false;
                }
                if (isStart && game.PlayerList.Count() > 1)
                    game.Start();
            }
        }
    }
}
