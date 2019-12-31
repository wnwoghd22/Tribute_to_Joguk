using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CourtEnd : Event
{
    [SerializeField]
    RuntimeAnimatorController Amolang;

    protected override IEnumerator EventCoroutine()
    {
        ChangeCut("Lobby");
        FadeIn();

        StartDialogue(dialogs[0]);
        yield return waitExit;

        ChangeWitness(Amolang);
        StartDialogue(dialogs[1]);
        yield return waitExit;

        FadeOut();
        yield return waitTime;
        FadeIn();

        NextEvent("Title");
    }
}
