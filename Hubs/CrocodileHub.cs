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
        public async Task SendCategories(string[] categories)
        {
            List<Category> items = categories.Select(a => (Category)Enum.Parse(typeof(Category), a)).ToList();
            var words = WordList.GenerateWords(items).ToArray();
            await Clients.Caller.SendAsync("chooseAnswer", words);
        }

        public async Task SetGroup(string group)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, group);
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
    }
}
