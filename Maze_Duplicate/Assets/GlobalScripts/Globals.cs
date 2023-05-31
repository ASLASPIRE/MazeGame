using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Globals
{
    public static TextAsset vocabJson;
    public static string vocabSet = "PartsOfTheCell";
    public static Dictionary<string,int> powerup_amts = new Dictionary<string,int>() {
        {"slow_time", 3},
        {"multiplier", 3}
    };
}
