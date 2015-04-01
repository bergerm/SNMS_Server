using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

using SNMS_Server.Plugins;
using SNMS_Server.Connection;

namespace SNMS_Server.Factories
{
    class AccountFactory
    {
        public static List<Account> Build(Plugin plugin, NetworkStream stream)
        {
            ProtocolMessage getAccountsMessage = new ProtocolMessage();
            getAccountsMessage.SetMessageType(ProtocolMessageType.PROTOCOL_MESSAGE_GET_ACCOUNTS);

            getAccountsMessage.AddParameter(plugin.GetID());

            ConnectionHandler.SendMessage(stream, getAccountsMessage);
            ProtocolMessage responseMessage = ConnectionHandler.GetMessage(stream);

            List<Account> accounts = Account.ParseMessage(responseMessage, plugin);

            return accounts;
        }
    }
}
