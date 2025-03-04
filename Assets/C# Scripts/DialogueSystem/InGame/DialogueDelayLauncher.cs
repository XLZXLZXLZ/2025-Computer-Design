using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueDelayLauncher : MonoBehaviour
{
    [SerializeField]
    private SingleDialogueController targetDialogue;

    [SerializeField]
    private float delay = 3f;

    private void Start()
    {
        DOTween.Sequence()
            .AppendInterval(delay)
            .OnComplete( () => targetDialogue.Activate());
    }
}
