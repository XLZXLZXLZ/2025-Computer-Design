using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NewActorData", menuName = "ScriptableObject/SingleDialogue/Config", order = 0)]
public class SingleDialogueConfig : ScriptableObject
{
    public List<SingleDialogueActorData> Actors = new();
    public TextAsset Dialogue;
}