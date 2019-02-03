using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestionData {

    public string questionText;
    public string imagePath;
    public bool hasImage;
    public bool diagram;
    public AnswerData[] answers;

}
