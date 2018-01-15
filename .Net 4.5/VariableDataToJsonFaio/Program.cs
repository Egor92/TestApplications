using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace VariableDataToJsonFaio
{
    class Program
    {
        static void Main(string[] args)
        {
            var variableData = new VariableData
            {
                ImagesCountInTurn = 4,
                TurnCountInQueue  = 5,
                StartHintCount = 5,
                HintIncrementOnGuessedCount = 5,
                ColoredImageChance = 0.7,
                ColorNames = new List<string>
                {
                    "Red",
                    "Orange",
                    "Yellow",
                    "Green",
                    "Teal",
                    "Blue",
                    "Purple",
                    "Pink",
                    "White",
                    "Gray",
                    "Black",
                    "Brown",
                },
                RightChoiseDelay = TimeSpan.FromSeconds(1),
                WrongChoiseDelay = TimeSpan.FromSeconds(3),
                PageBackgroundUri = "CompiledResources/Background.jpg",
                RightChoiseColorString = "#0F0",
                WrongChoiseColorString = "#F00",
                StartGameTime = new TimeSpan(0, 0, 0, 30),
                RightChoiseTimeIncrement = TimeSpan.FromSeconds(1),
                WrongChoiseTimeDecrement = TimeSpan.FromSeconds(5),
                RightChoiseInARowTimeIncrement = TimeSpan.FromSeconds(0.5),
                MaxAvailableTimeIncrement = TimeSpan.FromSeconds(2),
                DisplayedLocalHighscoreCount = 100,
                DisplayedWorldHighscoreCount = 50,
                InfromComboLowLimit = 2,
                PointsOnGuessedTurn = 100,
                PointsOnSecond = 1,
                PlayerNameMaxLength = 25,
                AvailablePlayerNameRegexPattern = @"^[\w\d\s\-\']+$"
            };

            var serializeObject = JsonConvert.SerializeObject(variableData);
            Console.Read();
        }
    }
}
