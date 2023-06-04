using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.SignalR;
using task6x.Models;
using System.Text;

namespace task6x.Hubs
{
    public class CrocodileHub : Hub
    {
        private static string _answer = "";
        private static string _color = "#444";

        public void SendAnswer(string answer)
        {
            _answer = answer.ToUpper();
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
