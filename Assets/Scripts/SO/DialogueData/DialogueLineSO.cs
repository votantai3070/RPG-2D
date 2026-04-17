using UnityEngine;

[CreateAssetMenu(fileName = "Line - ", menuName = "RPG Setup/Dialogue Data/ New Line Data")]
public class DialogueLineSO : ScriptableObject
{
    [Header("Dialogue info")]
    public string dialogueGroupName;
    public DialogueSpeakerSO speaker;

    [Header("Text Options")]
    [TextArea] public string[] textLine;

    [Header("Answer setup")]
    public bool playCanAnswer; // should be true, if play can make a choice
    public DialogueLineSO[] answerLine;

    public string GetRandomLine()
    {
        return textLine[Random.Range(0, textLine.Length)];
    }
}
