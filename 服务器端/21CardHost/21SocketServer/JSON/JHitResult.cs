using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _21SocketServer
{
    public class JHitResult
    {
        public JCard Card { get; set; }
        //点数
        public int Point { get; set; }
        //爆
        public bool IsExplode { get; set; }
        //座次
        public int Seat { get; set; }
    }
}
