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
        private static int groupPlayerCount = 0;

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

        public async Task SendMessage(string message, string userName, string answer, string group)
        {
            bool isAnswer = false;
            if (message.ToLower() == answer) isAnswer = true;
            await Clients.Group(group).SendAsync("ReceiveMessage", isAnswer, message, userName);
        }

        public override async Task OnConnectedAsync()
        {
            string role = this.Context.GetHttpContext().Request.Query["role"];
            string group = (role == "crocodile") ? (groupCrocodileCount++).ToString() : (groupPlayerCount++).ToString();
            bool isGroupComplete = false;
            if (!dict.ContainsKey(group))
            {
                dict.Add(group, new List<string> { Context.ConnectionId });
            }
            else
            {
                dict[group].Add(Context.ConnectionId);
                isGroupComplete = true;
            }
            await Groups.AddToGroupAsync(Context.ConnectionId, group);
            await Clients.Group(group).SendAsync("ReceiveGroup", group, isGroupComplete);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            foreach (var d in dict)
            {
                if (d.Value.Contains(Context.ConnectionId))
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
