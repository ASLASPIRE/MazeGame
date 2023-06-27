using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Globals
{
    public enum Difficulty
    {
        Easy,
        Medium,
        Hard
    }

    public enum Vocab
    {
        Chemistry,
        Biology,
        FoodWeb,
        PartsOfTheCell
    }

    public static TextAsset vocabJson;
    public static string vocabSet = "PartsOfTheCell"; // to be deprecated
    public static Dictionary<string,int> powerup_amts = new Dictionary<string,int>() {
        {"slow_time", 3},
        {"multiplier", 3}
    };
    public static Difficulty difficulty = Difficulty.Hard;
    public static Vocab vocabList = Vocab.Chemistry;

}
