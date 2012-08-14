using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase;

namespace _21SocketServer
{
    public class _21Session : AppSession<_21Session>
    {
        public _21Server Server
        {
            get
            {
                return AppServer as _21Server;
            }
        }

        public override void StartSession()
        {
            base.StartSession();
            lock (Server.Game)
            {
                if (Server.Game.IsStart)
                    SendResponse("ERROR 牌局已经开始，请稍后！");
                else
                    Server.Game.SendPlayerInfo(this);
            }
        }

        public Player Player { get; set; }

        public List<JCard> CardList { get; set; }

        public int Point
        {
            get
            {
                int total = 0;
                int ACount = 0;
                foreach (var card in CardList)
                {
                    switch (card.Number)
                    {
                        case 1:
                            total += 11;
                            ACount++;
                            break;
                        case 11:
                        case 12:
                        case 13:
                            total += 10;
                            break;
                        default:
                            total += card.Number;
                            break;
                    }
                }
                while (ACount > 0 && total > 21)
                {
                    total -= 10;
                    ACount--;
                }

                return total;
            }
        }
        public bool IsExplode
        {
            get
            {
                return Point > 21;
            }
        }
        public bool IsDragon
        {
            get
            {
                if (IsExplode)
                    return false;
                return CardList.Count >= 5;
            }
        }
    }
}
