using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Zork
{
    public class Program
    {


        private static void Main(string[] args) //void infront of main is return type so returns nothing (void), private is implied
        {
            const string defaultGameFilename = @"Content\Game.json";
            string gameFilename = (args.Length > 0 ? args[(int)CommandLineArguments.GameFilename] : defaultGameFilename);

            Game game = JsonConvert.DeserializeObject<Game>(File.ReadAllText(gameFilename));

            //Welcome message
            Console.WriteLine("Welcome to Zork!");
            game.Run();
            Console.WriteLine("Finished");

        }

        private enum CommandLineArguments
        { 
            GameFilename = 0
        }

    }
}
