using System.Linq;
using D3Sharp.Core.Accounts;
using D3Sharp.Net;
using D3Sharp.Net.Packets;
using D3Sharp.Utils;
using bnet.protocol.authentication;

namespace D3Sharp.Core.Services
{
    [Service(serviceID: 0x1, serviceName: "bnet.protocol.authentication.AuthenticationServer")]
    public class AuthenticationService:AuthenticationServer, IServerService
    {
        protected static readonly Logger Logger = LogManager.CreateLogger();
        public Client Client { get; set; }

        public override void Logon(Google.ProtocolBuffers.IRpcController controller, LogonRequest request, System.Action<LogonResponse> done)
        {
            Logger.Trace("LogonRequest(): " + request.Email);
            Client.Account = AccountsManager.GetAccount(request.Email);

            var builder = bnet.protocol.authentication.LogonResponse.CreateBuilder()
                .SetAccount(Client.Account.BnetAccountID)
                .SetGameAccount(Client.Account.BnetGameAccountID);

            done(builder.Build());
        }

        public override void ModuleMessage(Google.ProtocolBuffers.IRpcController controller, ModuleMessageRequest request, System.Action<bnet.protocol.NoData> done)
        {
            throw new System.NotImplementedException();
        }
    }
}
