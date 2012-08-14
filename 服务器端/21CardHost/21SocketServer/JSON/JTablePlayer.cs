using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _21SocketServer
{
    public class JTablePlayer
    {
        //对应Session的Key
        public string Key { get; set; }
        //座次
        public int Seat { get; set; }
        //是否开始
        public bool IsStart { get; set; }
        //是否自己
        public bool IsSelf { get; set; }
        //是否庄家
        public bool IsDealer { get; set; }

    }
}
