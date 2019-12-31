using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectionTest5 : Event
{
    [SerializeField]
    RuntimeAnimatorController Otoke;
    protected override IEnumerator EventCoroutine()
    {
        StartDialogue(dialogs[0]);
        yield return waitExit;

        while(true)
        {
            StartChoice(choices[0]);
            yield return waitExit;
            if (Answer == 1)
                break;
            else
            {
                StartDialogue(dialogs[1]);
                yield return waitExit;
            }
        }

        StartDialogue(dialogs[2]);
        yield return waitExit;

        while(true)
        {
            AdduceClue(choices[1]);
            yield return waitExit;
            if (Adduced == 10004)
                break;
            else
            {
                StartDialogue(dialogs[3]);
                yield return waitExit;
            }
        }

        StartDialogue(dialogs[4]);
        yield return waitExit;
        FadeOut();
        yield return waitTime;
        ClearAll();
        FadeIn();
        yield return waitTime;

        StartDialogue(dialogs[5]); //회상
        yield return waitExit;
        FadeOut();
        yield return waitTime;
        StartDialogue(dialogs[6]);
        FadeIn();
        yield return waitExit;

        FadeOut();
        yield return waitTime;
        ChangeWitness(Otoke);
        FadeIn();
        StartDialogue(dialogs[7]);
        yield return waitExit;

        //무죄 애니메이션
        ChangeCut("Innocent");
        yield return waitTime;

        StartDialogue(dialogs[8]);
        yield return waitExit;

        FadeOut();
        yield return waitTime;
        NextEvent(nextEvent);
    }
}
