using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace VariableDataToJsonFaio
{
    public class VariableData
    {
        public int ImagesCountInTurn { get; set; }
        public int TurnCountInQueue { get; set; }
        public int StartHintCount { get; set; }
        public int HintIncrementOnGuessedCount { get; set; }
        public double ColoredImageChance { get; set; }
        public List<string> ColorNames { get; set; }
        public TimeSpan RightChoiseDelay { get; set; }
        public TimeSpan WrongChoiseDelay { get; set; }
        public string PageBackgroundUri { get; set; }
        public string RightChoiseColorString { get; set; }
        public string WrongChoiseColorString { get; set; }
        public TimeSpan StartGameTime { get; set; }
        public TimeSpan RightChoiseTimeIncrement { get; set; }
        public TimeSpan WrongChoiseTimeDecrement { get; set; }
        public TimeSpan RightChoiseInARowTimeIncrement { get; set; }
        public TimeSpan MaxAvailableTimeIncrement { get; set; }
        public int DisplayedLocalHighscoreCount { get; set; }
        public int DisplayedWorldHighscoreCount { get; set; }
        public int InfromComboLowLimit { get; set; }
        public int PointsOnGuessedTurn { get; set; }
        public int PointsOnSecond { get; set; }
        public int PlayerNameMaxLength { get; set; }
        public string AvailablePlayerNameRegexPattern { get; set; }
    }
}
