using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.SignalR;
using task6x.Models;
using System.Text;
using task6x.Services;
using System.Collections.Concurrent;

namespace task6x.Hubs
{
    public class CrocodileHub : Hub
    {
        private static Dictionary<string, List<string>> dict = new();
        private static int groupCrocodileCount = 0;

        public async Task SendCategories(string[] categories)
        {
            List<Category> items = categories.Select(a => (Category)Enum.Parse(typeof(Category), a)).ToList();
            var words = WordList.GenerateWords(items).ToArray();
            await Clients.Caller.SendAsync("chooseAnswer", words);
        }

        public async Task SetAnswer(string answer, string group)
        {
            await Clients.Group(group).SendAsync("setAnswer", answer);
        }

        public async Task DrawLine(Data data, string color, string group)
        {
            await Clients.OthersInGroup(group).SendAsync("addLine", data, color);
        }

        public async Task SendMessage(string message, string role, string username, string answer, string group)
        {
            bool isAnswer = false;
            if (message.ToLower() == answer) isAnswer = true;
            await Clients.Group(group).SendAsync("ReceiveMessage", isAnswer, message, role, username);
        }

        public override async Task OnConnectedAsync()
        {
            string username = this.Context.GetHttpContext().Request.Query["username"];
            string role = this.Context.GetHttpContext().Request.Query["role"];
            bool isGroupComplete = false;
            if (role == "crocodile")
            {
                string group = (groupCrocodileCount++).ToString();
                dict.Add(group, new List<string> { username });
                await Groups.AddToGroupAsync(Context.ConnectionId, group);
                await Clients.Caller.SendAsync("ReceiveGroup", group, isGroupComplete);
            }
            await base.OnConnectedAsync();
        }

        public async Task ChooseCrocodile(string crocodileName, string userName)
        {
            foreach (var d in dict)
            {
                if (d.Value.Contains(crocodileName))
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
                        await Clients.Caller.SendAsync("WrongCrocodile", "This user is already playing.");
                        return;
                    }
                }
            }
            await Clients.Caller.SendAsync("WrongCrocodile", "This user is absent.");
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            foreach (var d in dict)
            {
                if (d.Value.Contains(this.Context.GetHttpContext().Request.Query["username"]))
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
