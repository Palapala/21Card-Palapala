using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _21SocketServer
{
    public class CardLib
    {
        public List<JCard> List { get; set; }

        public CardLib()
        {
            List = new List<JCard>();
            for (int i = 1; i <= 4; i++) 
            {
                for (int j = 1; j <= 13; j++) 
                {
                    JCard card = new JCard()
                    {
                        Type = (JCardType)i,
                        Number = j
                    };
                    List.Add(card);
                }
            }
        }

        public void Shuffle()
        {
            Random r = new Random(DateTime.Now.Millisecond);
            List<JCard> newList = new List<JCard>();
            for (int i = 0; i < 52; i++)
            {
                int index = r.Next(List.Count);
                newList.Add(List[index]);
                List.RemoveAt(index);
            }
            List = newList;
        }
    }
}
