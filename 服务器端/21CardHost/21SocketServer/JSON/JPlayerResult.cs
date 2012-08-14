using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _21SocketServer
{
    public class JPlayerResult
    {
        public List<JCard> CardList {get;set;}
        //座次
        public int Seat { get; set; }
        //牌型(点数、爆、五龙）
        public string CardResult { get; set; }
        //胜负(庄家没有自身的胜负，只有每个玩家才有胜负）
        public bool IsWin { get; set; }
    }
}
