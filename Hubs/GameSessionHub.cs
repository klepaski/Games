using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using System.Web;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Http.Connections.Features;

namespace task6x.Hubs
{
    public class GameSessionHub : Hub
    {
        private static Dictionary<string, List<string>> dict = new(); //{ group : {game, user1, user2} }
        private static int groupCount = 0;


        public async Task CreateSession(string game, string username)
        {
            string group = (groupCount++).ToString();
            dict.Add(group, new List<string> { game, username });
            await Clients.All.SendAsync("NewGameSession", game, username);
        }

        public async Task JoinSession(string userToJoin, string username)
        {
            foreach (var d in dict)
            {
                if (d.Value.Contains(userToJoin) && d.Value.Count == 2)
                {
                    d.Value.Add(username);
                    await Clients.All.SendAsync("DeleteGameSession", userToJoin);
                    return;
                }
            }
        }

        public override async Task OnConnectedAsync()
        {
            string? username = this.Context.GetHttpContext().Request.Query["username"];
            foreach (var d in dict)
            {
                await Clients.Caller.SendAsync("NewGameSession", d.Value[0], d.Value[1]);
                if (d.Value.Contains(username))
                {
                    bool isGroupComplete = (d.Value.Count > 2) ? true : false;
                    await Groups.AddToGroupAsync(Context.ConnectionId, d.Key);
                    await Clients.Group(d.Key).SendAsync("ReceiveGroup", d.Key, isGroupComplete);
                }
            }
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            string? username = this.Context.GetHttpContext().Request.Query["username"];
            await Clients.All.SendAsync("DeleteGameSession", username);
            foreach (var d in dict)
            {
                if (d.Value.Contains(username))
                {
                    await Clients.Group(d.Key).SendAsync("CloseGroup");
                    dict.Remove(d.Key);
                    break;
                }
            }
            await base.OnDisconnectedAsync(exception);
        }
    }
}
