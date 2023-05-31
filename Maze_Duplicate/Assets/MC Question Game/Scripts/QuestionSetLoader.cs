using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

public static class QuestionSetLoader 
{
    /**
    *   Load a list of questions from a path to a json in the assets folder
    *   @param path: The path to the json file, relative to the assets folder
    *   @return: The list of questions in the Json file, represente through
    *       a List of Question objects
    **/
    //public static List<Question> QuestionListFromJsonLocal(string path) 
    //{
    //    string questionListString = File.ReadAllText(_absolutePathFromRelative(path));
    //    List<Question> questions = JsonConvert.DeserializeObject<List<Question>>(questionListString);
    //    foreach (Question q in questions) {
    //        if (q.SourceLink != "") {
    //            q.SourceLink = _urlFromRelativePath(q.SourceLink);
    //        }
    //    }
    //    return questions;
    //}

    //private static string _absolutePathFromRelative(string relPath) 
    //{
    //    return $"{Application.dataPath}/" + relPath;
    //}

    //private static string _urlFromRelativePath(string relPath)
    //{
    //    return "file://" + _absolutePathFromRelative(relPath);
    //}
}