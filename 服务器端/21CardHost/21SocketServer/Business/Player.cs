using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _21SocketServer
{
    public class Player
    {
        public _21Session Session { get; set; }
        //座次
        public int Seat { get; set; }
        //是否开始
        public bool IsStart { get; set; }


        //--------------游戏状态--------------
        //初始化成功，接受到START_COMPLETE之后
        public bool IsInit { get; set; }
        //是否庄家
        public bool IsDealer { get; set; }
        //是否掉线
        public bool IsOut { get; set; }
        //顺次
        public int Seq { get; set; }
        //是否当前回合者
        public bool IsTurn {get;set;}
        ////是否停止加牌
        //public bool IsStop { get; set; }
        //手牌
        public List<JCard> CardList { get; set; }
        //点数
        public int Point
        {
            get {
                if (CardList == null)
                    return 0;

                int total = 0;
                int As = 0;
                foreach (var card in CardList)
                {
                    switch (card.Number)
                    { 
                        case 1:
                            total += 11;
                            As++;
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
                while (total > 21 && As > 0)
                {
                    total -= 10;
                    As--;
                }
                return total;
            }
        }
        //爆？
        public bool IsExplode { get { return Point > 21; } } 
        //五龙
        public bool IsDragon
        {
            get
            {
                if (CardList == null)
                    return false;
                if (!IsExplode && CardList.Count >= 5)
                    return true;
                else
                    return false;
            }
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Player))
                return false;
            Player player = (Player)obj;
            return this.Session.Equals(player.Session);
        }
    }
}
