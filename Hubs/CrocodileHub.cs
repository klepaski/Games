using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.SignalR;
using task6x.Models;
using System.Text;
using task6x.Services;

namespace task6x.Hubs
{
    public class CrocodileHub : Hub
    {
        private static string _color = "#444";
        private static List<string> _words;
        private static string _answer = "";

        public async Task SendCategories(string[] categories)
        {
            List<Category> items = categories.Select(a => (Category)Enum.Parse(typeof(Category), a)).ToList();
            _words = WordList.GenerateWords(items);
            _answer = _words[0].ToUpper();
            await Clients.Caller.SendAsync("chooseAnswer", _answer);
        }

        public async Task ChangeAnswer()
        {
            _words.RemoveAt(0);
            _answer = _words[0].ToUpper();
            await Clients.Caller.SendAsync("chooseAnswer", _answer);
        }

        public async Task SetAnswer()
        {
            await Clients.All.SendAsync("setAnswer", _answer);
        }

        public void SendColor(string color)
        {
            _color = color;
        }

        public async Task DrawLine(Data data)
        {
            await Clients.Others.SendAsync("addLine", data, _color);
        }

        public async Task SendMessage(string message, string userName)
        {
            bool isAnswer = false;
            if (message.ToUpper() == _answer) isAnswer = true;
            await Clients.All.SendAsync("ReceiveMessage", isAnswer, message, userName);
        }
    }
}
