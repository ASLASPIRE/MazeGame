
[System.Serializable]
public class Question
{
    //these variables are case sensitive and must match the strings "Word" and "Link" in the JSON.
    public string Subject;
    public string Word;
    public string Link;
}

[System.Serializable]
public class Questions
{
    //Questions is case sensitive and must match the string "questions" in the JSON.
    public Question[] questions;
}