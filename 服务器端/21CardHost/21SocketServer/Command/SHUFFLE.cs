using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase.Command;
using Newtonsoft.Json;

namespace _21SocketServer.Command
{
    /// <summary>
    /// 派牌
    /// </summary>
    public class SHUFFLE : StringCommandBase<_21Session>
    {
        public override void ExecuteCommand(_21Session session, StringCommandInfo commandInfo)
        {
            CardLib lib = new CardLib();
            lib.Shuffle();

            session.SendResponse(string.Format("SHUFFLE {0}", JsonConvert.SerializeObject(new { List = lib.List })));
        }
    }
}
