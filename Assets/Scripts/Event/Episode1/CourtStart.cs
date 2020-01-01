using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CourtStart : Event
{
    private int answer;

    protected override IEnumerator EventCoroutine()
    {
        GetItem(90001);
        GetItem(91002);

        GetItem(10001);
        GetItem(10004);
        GetItem(11001);
        GetItem(11002);

        ClearAll();
        ChangeCut("Court");
        FadeIn();
        yield return waitTime;
        ChangeCut("Gavel");
        yield return waitTime;

        StartDialogue(dialogs[0]);
        yield return waitExit;

        bool _b = true;
        while(_b)
        {
            StartChoice(choices[0]);
            yield return waitExit;
            switch(Answer)
            {
                case 0:
                    StartDialogue(dialogs[1]);
                    yield return waitExit;
                    break;
                case 1:
                    _b = false;
                    break;
            }
        }

        StartDialogue(dialogs[2]);
        yield return waitExit;

        _b = true;
        answer = 11002;
        while(_b)
        {
            AdducePerson(choices[1]);
            yield return waitExit;

            if (Adduced == answer)
                break;
            else
            {
                StartDialogue(dialogs[3]);
                yield return waitExit;
            }           
        }

        StartDialogue(dialogs[4]);
        yield return waitExit;

        answer = 10004;
        while(_b)
        {
            AdduceClue(choices[2]);
            yield return waitExit;

            if (Adduced == answer)
                break;
            else
            {
                StartDialogue(dialogs[5]);
                yield return waitExit;
            }
        }

        StartDialogue(dialogs[6]);
        yield return waitExit;

        GetItem(10002);
        GetItem(10003);
        GetItem(10005);

        StartDialogue(dialogs[8]);
        yield return waitExit;

        FadeOut();
        yield return waitTime;
        FadeIn();

        StartDialogue(dialogs[7]);
        yield return waitExit;

        FadeOut();
        yield return waitTime;
        NextEvent(nextEvent);
    }
}
