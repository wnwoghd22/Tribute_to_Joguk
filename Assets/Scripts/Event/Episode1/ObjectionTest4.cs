using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectionTest4 : Event
{
    protected override IEnumerator EventCoroutine()
    {
        StartDialogue(dialogs[0]);
        yield return waitExit;
        FadeOut();
        yield return waitTime;
        ClearAll();

        FadeIn();
        yield return waitTime;
        StartDialogue(dialogs[1]);
        ChangeCut(0);
        SetCutActive(true);
        yield return new WaitForSeconds(0.1f); //깜빡이기
        SetCutActive(false);
        yield return waitExit;
        yield return waitTime;

        StartDialogue(dialogs[2]);
        SetCutActive(true);
        yield return new WaitForSeconds(0.1f); //깜빡이기
        SetCutActive(false);
        yield return new WaitForSeconds(0.1f); //깜빡이기
        SetCutActive(true);
        yield return new WaitForSeconds(0.1f); //깜빡이기
        SetCutActive(false);
        yield return waitExit;
        ChangeCut("Court");
        yield return waitTime;

        StartDialogue(dialogs[3]);
        yield return waitExit;

        while(true)
        {
            AdducePerson(choices[0]);
            yield return waitExit;

            if (Adduced == 11003)
                break;
            else
            {
                StartDialogue(dialogs[4]);
                yield return waitExit;
            }
        }
        StartDialogue(dialogs[5]);
        yield return waitExit;

        FadeOut();
        yield return waitTime;

        NextEvent(nextEvent);
    }
}
