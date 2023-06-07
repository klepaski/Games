using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using System.Web;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Http.Connections.Features;

namespace task6x.Hubs
{
    public class HangmanHub : Hub
    {
        private static Dictionary<string, List<string>> dict = new();
        private static int groupHostCount = 0;

        public async Task SendWord(string word, string hint, string group)
        {
            word = word.ToUpper();
            string hiddenPart = new StringBuilder().Insert(0, "_", word.Length - 2).ToString();
            var displayWord = word.ToUpper().Remove(1, word.Length - 2).Insert(1, hiddenPart);
            await Clients.Group(group).SendAsync("ReceiveWord", displayWord, word, hint);
        }

        public async Task SendLetter(string letter, string username, string word, string displayWord, int tries, string group)
        {
            bool isRight = false;
            bool isOver = false;
            for (int i = 0; i < word.Length; i++)
            {
                if (word[i] == char.Parse(letter))
                {
                    isRight = true;
                    displayWord = displayWord.Remove(i, 1).Insert(i, letter);
                }
            }
            if (!isRight) tries++;
            if (tries == 6) displayWord = word;
            if (displayWord == word) isOver = true;
            await Clients.Group(group).SendAsync("ReceiveLetter", isOver, isRight, displayWord, tries, letter, username);
        }

        public override async Task OnConnectedAsync()
        {
            string username = this.Context.GetHttpContext().Request.Query["username"];
            string role = this.Context.GetHttpContext().Request.Query["role"];
            bool isGroupComplete = false;
            if (role == "host")
            {
                string group = (groupHostCount++).ToString();
                dict.Add(group, new List<string> { username });
                await Groups.AddToGroupAsync(Context.ConnectionId, group);
                await Clients.Caller.SendAsync("ReceiveGroup", group, isGroupComplete);
            }
            await base.OnConnectedAsync();
        }

        public async Task ChooseHost(string hostName, string userName)
        {
            foreach (var d in dict)
            {
                if (d.Value.Contains(hostName))
                {
                    if (d.Value.Count == 1)
                    {
                        await Groups.AddToGroupAsync(Context.ConnectionId, d.Key);
                        bool isGroupComplete = true;
                        await Clients.Group(d.Key).SendAsync("ReceiveGroup", d.Key, isGroupComplete);
                        dict[d.Key].Add(userName);
                        return;
                    }
                    else
                    {
                        await Clients.Caller.SendAsync("WrongHost", "This user is already playing.");
                        return;
                    }
                }
            }
            await Clients.Caller.SendAsync("WrongHost", "This user is absent.");
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            foreach (var d in dict)
            {
                if (d.Value.Contains(this.Context.GetHttpContext().Request.Query["username"]))
                {
                    await Clients.Group(d.Key).SendAsync("CloseGroup");
                    //foreach (var conn in d.Value)
                    //{
                    //    await Groups.RemoveFromGroupAsync(conn, d.Key);
                    //    Context.Abort();
                    //}
                    dict.Remove(d.Key);
                    break;
                }
            }
            await base.OnDisconnectedAsync(exception);
        }
    }
}
