using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectionTest3 : Event
{
    [SerializeField]
    RuntimeAnimatorController Ochiru;
    protected override IEnumerator EventCoroutine()
    {
        yield return waitTime;
        StartDialogue(dialogs[0]);
        yield return waitExit;
        ClearAll();
        ChangeCut("Court");
        yield return waitTime;
        ChangeCut("Gavel3");
        yield return waitTime;

        StartDialogue(dialogs[1]);
        yield return waitExit;

        FadeOut();
        GetItem(11003);
        ChangeWitness(Ochiru);
        yield return waitTime;
        FadeIn();

        yield return waitTime;
        StartDialogue(dialogs[2]);
        yield return waitExit;

        FadeOut();
        yield return waitTime;

        NextEvent(nextEvent);
    }
}
