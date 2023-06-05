using Microsoft.AspNetCore.SignalR;
using System.Linq;
using System.Text;

namespace task6x.Hubs
{
    public class HangmanHub : Hub
    {
        private static string _word = "";
        private static string _hint = "";
        private static string _displayWord = "";
        private static int tries = 0;

        public async Task SendWord(string word, string hint)
        {
            tries = 0;
            _word = word.ToUpper();
            _hint = hint;
            string hiddenPart = new StringBuilder().Insert(0, "_", _word.Length - 2).ToString();
            _displayWord = _word.Remove(1, _word.Length - 2).Insert(1, hiddenPart);
            await Clients.All.SendAsync("ReceiveWord", _displayWord);
        }

        public async Task SendLetter(string letter, string username)
        {
            bool isRight = false;
            bool isOver = false;
            for (int i = 0; i < _word.Length; i++)
            {
                if (_word[i] == char.Parse(letter))
                {
                    isRight = true;
                    _displayWord = _displayWord.Remove(i, 1).Insert(i, letter);
                }
            }
            if (!isRight) tries++;
            if (tries == 6) _displayWord = _word;
            if (_displayWord == _word) isOver = true;
            await Clients.All.SendAsync("ReceiveLetter", isOver, isRight, _displayWord, tries, letter, username);
        }

        public async Task WatchHint()
        {
            await Clients.All.SendAsync("ReceiveHint", _hint);
        }
    }
}
