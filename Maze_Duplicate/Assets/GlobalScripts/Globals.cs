using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MenuScreenManager;

public class Globals
{
    public static TextAsset vocabJson;
    public static string vocabSet = "PartsOfTheCell"; // to be deprecated
    public static Dictionary<string,int> powerup_amts = new Dictionary<string,int>() {
        {"slow_time", 3},
        {"multiplier", 3}
    };
    public static Difficulty difficulty = Difficulty.Easy;
    public static Vocab vocabList = Vocab.PartsOfTheCell;

}
