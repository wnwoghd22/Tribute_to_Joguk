using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectionTest2 : Event
{
    [SerializeField]
    RuntimeAnimatorController Otoke;
    protected override IEnumerator EventCoroutine()
    {
        StartDialogue(dialogs[0]);
        yield return waitExit;
        FadeOut();

        yield return waitTime;
        ChangeWitness(Otoke);

        FadeIn();
        StartDialogue(dialogs[1]);
        yield return waitExit;

        FadeOut();
        yield return waitTime;

        NextEvent(nextEvent);
    }
}
