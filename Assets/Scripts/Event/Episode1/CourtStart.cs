using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CourtStart : Event
{
    protected override IEnumerator EventCoroutine()
    {
        FadeIn();

        ChangeCut("Gavel");
        yield return waitTime;

        StartDialogue(dialogs[0]);
        yield return waitExit;

        bool b = true;
        while(b)
        {
            StartChoice(choices[0]);
            yield return waitExit;
            switch(Result)
            {
                case 0:
                    StartDialogue(dialogs[1]);
                    yield return waitExit;
                    break;
                case 1:
                    b = false;
                    break;
            }
        }

        StartDialogue(dialogs[2]);
        yield return waitExit;
    }
}
