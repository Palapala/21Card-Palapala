using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace _21SocketServer
{
    public class Game
    {
        public Game()
        {
            IsStart = false;
            PlayerList = new List<Player>();
            CardLib = new CardLib();
            CardLib.Shuffle();
        }

        public _21Server Server { get; set; }
        public bool IsStart { get; set; }

        public List<Player> PlayerList { get; set; }

        public CardLib CardLib { get; set; }

        private List<JTablePlayer> getPlayerList()
        {
            List<JTablePlayer> list = new List<JTablePlayer>();

            foreach (var player in PlayerList)
            {
                JTablePlayer jPlayer = new JTablePlayer();
                jPlayer.Key = player.Session.IdentityKey;
                jPlayer.Seat = player.Seat;
                jPlayer.IsStart = player.IsStart;
                jPlayer.IsSelf = false;
                list.Add(jPlayer);
            }

            return list;
        }

        public void SendPlayerInfo()
        {
            List<JTablePlayer> list = getPlayerList();
            foreach (var player in PlayerList)
            {
                list.ForEach(m => m.IsSelf = false);

                var temp = (from p in list
                            where p.Key == player.Session.IdentityKey
                            select p).SingleOrDefault();
                temp.IsSelf = true;

                player.Session.SendResponse(string.Format("PLAYER_LIST {0}", JsonConvert.SerializeObject(new { list = list })));
            }
            var sessionList = Server.GetAllSession();
            list.ForEach(m => m.IsSelf = false);
            foreach (var session in sessionList)
            {
                var temp = (from p in PlayerList
                            where p.Session == session
                            select p).SingleOrDefault();

                if(temp == null)
                    session.SendResponse(string.Format("PLAYER_LIST {0}", JsonConvert.SerializeObject(new { list = list })));
            }
        }

        public void SendPlayerInfo(_21Session session)
        {
            List<JTablePlayer> list = getPlayerList();
            session.SendResponse(string.Format("PLAYER_LIST {0}", JsonConvert.SerializeObject(new { list = list })));
        }

        public void Start()
        {
            IsStart = true;

            Random r = new Random(DateTime.Now.Millisecond);
            int index = r.Next(PlayerList.Count);
            PlayerList[index].IsDealer = true;

            foreach (var player in PlayerList)
            {
                player.Seq = player.Seat - index;
                if (player.Seq < 0)
                    player.Seq += PlayerList.Count;

                player.CardList = new List<JCard>();
            }
            PlayerList = PlayerList.OrderBy(m => m.Seq).ToList();

            //发牌
            for (int i = 0; i < 2; i++)
            {
                foreach (var player in PlayerList)
                {
                    player.CardList.Add(this.CardLib.List[0]);
                    this.CardLib.List.RemoveAt(0);
                }
            }

            JGameInfo gameInfo = new JGameInfo();
            gameInfo.PlayerList = getPlayerList();
            for (int i = 0; i < PlayerList.Count; i++)
            {
                var sendPlayer = PlayerList[i];
                gameInfo.PlayerList.ForEach(m => m.IsSelf = false);

                var temp = (from p in gameInfo.PlayerList
                            where p.Key == sendPlayer.Session.IdentityKey
                            select p).SingleOrDefault();
                temp.IsSelf = true;

                gameInfo.DealList = new List<JDeal>();

                foreach (var player in PlayerList)
                {
                    JDeal deal = null;
                    if (player == sendPlayer)
                        deal = new JDeal()
                        {
                            Seat = player.Seat,
                            Card = player.CardList[0]
                        };
                    else
                        deal = new JDeal()
                        {
                            Seat = player.Seat,
                            Card = null
                        };

                    gameInfo.DealList.Add(deal);
                }

                foreach (var player in PlayerList)
                {
                    JDeal deal = new JDeal()
                    {
                        Seat = player.Seat,
                        Card = player.CardList[0]
                    };

                    gameInfo.DealList.Add(deal);
                }

                sendPlayer.Session.SendResponse(string.Format("GAME_START {0}", JsonConvert.SerializeObject(new { info = gameInfo })));
            }
        }

        public void Play()
        {
            for (int i = 0; i < PlayerList.Count; i++) 
            {
                var player = PlayerList[i];
                if (i == 1)
                {
                    player.Session.SendResponse("WAIT");
                    player.IsTurn = true;
                }
                else
                {
                    player.Session.SendResponse("WAIT_OTHER");
                    player.IsTurn = false;
                }
            }
        }

        public void End()
        {
            List<JPlayerResult> list = new List<JPlayerResult>();
            JPlayerResult dealer = null;
            foreach (var player in PlayerList)
            {
                JPlayerResult result = new JPlayerResult();
                result.CardList = player.CardList;
                result.Seat = player.Seat;
                if (player.IsExplode)
                    result.CardResult = "爆";
                else if (player.IsDragon)
                    result.CardResult = "五龙";
                else
                    result.CardResult = player.Point.ToString();

                if (!player.IsDealer) 
                {
                    switch (result.CardResult) 
                    {
                        case "爆":
                            result.IsWin = false;
                            break;
                        case "五龙":
                            result.IsWin = dealer.CardResult != "五龙";
                            break;
                        default:
                            int point = Convert.ToInt32(result.CardResult);
                            int dealerPoint = Convert.ToInt32(dealer.CardResult);
                            result.IsWin = point > dealerPoint;
                            break;
                    }
                }
                else{
                    dealer = result;
                }
            }

            foreach (var player in PlayerList)
            {
                player.Session.SendResponse(string.Format("GAME_END {0}", JsonConvert.SerializeObject(new { list = list })));
            }
        }



        public void LostPlayer(string key)
        {
            var player = (from p in PlayerList
                          where p.Session.IdentityKey == key
                          select p).SingleOrDefault();
            if (player == null)
                return;

            if (IsStart)
            {
                player.IsOut = true;
                foreach (var otherPlayer in PlayerList)
                {
                    if (otherPlayer == player)
                        continue;

                    otherPlayer.Session.SendResponse(string.Format("OUT_INFO {0}",player.Seat));
                }
            }
            else
            {
                PlayerList.Remove(player);
                SendPlayerInfo();
            }
        }
    }
}
