using System.IO;
using System.Numerics;

namespace task6x.Services
{
    public enum Category
    {
        profession, nature, other, people, character
    };

    public class WordList
    {
        private static string[] PROFESSION =
        {
            "plumber", "firefighter", "psychiatrist", "prosecutor", "sculptor", "director", "cynologist", "cosmonaut",
            "chemist", "stewardess", "beekeeper", "designer", "electrician", "archaeologist", "veterinarian", "lumberjack", //дровосек
            "programmer", "taxi driver", "salesman", "cook", "top model", "designer", "cyclist", "agent", "actor"
        };

        private static string[] NATURE =
        {
            "chameleon", "crab", "starfish", "skunk", "sloth", "chihuahua", "raccoon", "shrimp", "ladybug", "platypus", 
            "hummingbird", "beaver", "pelican", "stork", "spider", "dinosaur", "jellyfish", "snail", "turkey", "porcupine", 
            "scorpion", "fox", "cow", "bear", "sheep", "frog", "whale", "worm", "cabbage", "pine", "octopus", "fly"
        };

        private static string[] OTHER =
        {
            "antivirus", "spray", "tulle", "salary", "closet", "tandem", "photoshop", "wikipedia", "Facebook", "curler", 
            "gay parade", "journey ", "parking", "garbage", "subway", "toilet", "prison", "dictaphone", "pizza", "sushi", 
            "rock", "weightlessness", "bathroom", "ranch", "shawarma", "restaurant", "bar", "testament", "cellar", "supermarket",
            "watermelon", "ice cream", "night", "planet", "olympiad", "note", "interview", "borscht", "lightning", "drum"
        };

        private static string[] PEOPLE =
        {
            "Grigory Leps", "Napoleon Bonaparte", "Lady Gaga", "Yuri Kuklachev", "Michael Jackson", "Yuri Gagarin", "Isaac Newton",
            "Timati", "Jackie Chan", "Andrey Malakhov", " Albert Einstein", "Alexander Pushkin", "Verka Serduchka", "Vladimir Lenin", 
            "Karl Marx", "Steve Jobs", "Bill Gates", "Van Gogh", "Brad Pitt", "Alexander Rosenbaum", "Leonardo DiCaprio"
        };

        private static string[] CHARACTER =
        {
            "Chukchi", "Chuck Norris", "centaur", "geisha", "Terminator", "Hottabych", "transvestite", "Catdog", "Batman", "homeless", 
            "Baba Yaga", "Shrek", "alien", "Superman", "Homer Simpson", "macho", "hobbit", "Dr. House", "Jack Sparrow", "mermaid", 
            "Winnie the Pooh", "Freddy Krueger", "gnome", "Kolobok", "smurf", "Peppa pig", "millionaire", "womanizer", "zombie", 
            "paparazzi", "intellectual", "sultan", "king", "kamikaze", "tourist"
        };

        public static List<string> GenerateWords(List<Category> categories)
        {
            var allWords = new List<string>();

            foreach (var c in categories)
            {
                allWords.AddRange(c switch
                {
                    Category.profession => PROFESSION,
                    Category.people => PEOPLE,
                    Category.other => OTHER,
                    Category.nature => NATURE,
                    Category.character => CHARACTER,
                });
            }
            var shuffled = allWords.Distinct().OrderBy(x => System.Guid.NewGuid().ToString()).ToList();
            return shuffled;
        }
    }
}
