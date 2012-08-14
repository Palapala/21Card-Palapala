using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase.Command;
using System.Collections;
using Newtonsoft.Json;
using _21SocketServer;

namespace _21SocketServer.Command
{
    public class POKER : StringCommandBase<_21Session>
    {
        public override void ExecuteCommand(_21Session session, StringCommandInfo commandInfo)
        {
            //var list = new List<PokerCard>();
            ////list.Add(new PokerCard() { Card = 4 });
            //Random r = new Random(DateTime.Now.Millisecond);
            //for (int i = 0; i < 14; i++)
            //{
            //    PokerCard card = new PokerCard() { Card = r.Next(14) };
            //    while (list.Contains(card))
            //        card = new PokerCard() { Card = r.Next(14) };

            //    list.Add(card);
            //}

            CardLib lib = new CardLib();
            lib.Shuffle();
            List<JDeal> list = new List<JDeal>();
            for (int i = 1; i <= 2; i++) 
            {
                JDeal deal = new JDeal()
                {
                    Seat = 1,
                    Card = lib.List[(i - 1) * 4]
                };
                list.Add(deal);

                for (int j = 2; j <= 4; j++)
                {
                    deal = new JDeal()
                    {
                        Seat = j,
                        Card = null
                    };
                    list.Add(deal);
                }
                
            }

            session.SendResponse(string.Format("POKER {0}", JsonConvert.SerializeObject(new { List = list })));
        }

        public class PokerCard
        {
            public int Card { get; set; }

            public override bool Equals(object obj)
            {
                if (!(obj is PokerCard))
                    return false;

                PokerCard card = (PokerCard)obj;
                return card.Card == this.Card;
            }

            public override string ToString()
            {
                return Card.ToString();
            }
        }
    }
}
