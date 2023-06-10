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
        public async Task SetGroup(string group)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, group);
        }

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
    }
}
