using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _21SocketServer
{
    public class JCard
    {
        public JCardType Type { get; set; }
        public int Number { get; set; }

        public override string ToString()
        {
            string text = Number.ToString();
            switch (text)
            { 
                case "1":
                    text = "A";
                    break;
                case "11":
                    text = "J";
                    break;
                case "12":
                    text = "Q";
                    break;
                case "13":
                    text = "K";
                    break;
            }
            return string.Format("{0}{1}", Type, text);
        }
    }

    public enum JCardType
    { 
        方块 = 1,
        梅花 = 2,
        红桃 = 3,
        黑桃 = 4
    }
}
