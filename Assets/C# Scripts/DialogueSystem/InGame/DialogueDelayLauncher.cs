using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueDelayLauncher : MonoBehaviour
{
    [SerializeField]
    private SingleDialogueController targetDialogue;

    private float delay = 5f;

    private void Start()
    {
        DOTween.Sequence()
            .AppendInterval(delay)
            .OnComplete( () => targetDialogue.Activate());
    }
}
