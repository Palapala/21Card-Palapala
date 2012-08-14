using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _21SocketServer
{
    //发牌
    public class JDeal
    {
        //座次
        public int Seat { get; set; }
        //Card为null,表示暗牌
        public JCard Card { get; set; }
    }
}
