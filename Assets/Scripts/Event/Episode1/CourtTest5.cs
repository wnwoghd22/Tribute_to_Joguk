using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CourtTest5 : TestimonyScene
{
    HashSet<int> AnswerSet = new HashSet<int>() { 10003, 10005 };
    protected override IEnumerator InterrogationCoroutine(int _i)
    {
        switch (_i)
        {
            case 0:
                yield return new WaitForSeconds(1f);
                StartInterrogation(dialogs[0]);
                yield return waitExit;
                StartChoice(choices[0]);
                yield return waitExit;
                if (Answer == 0)
                    StartDialogue(dialogs[1]);
                else
                    StartDialogue(dialogs[6]);
                yield return waitExit;
                FadeOut();
                yield return waitTime;
                break;
            case 1:
                yield return new WaitForSeconds(1f);
                StartInterrogation(dialogs[2]);
                yield return waitExit;
                StartChoice(choices[0]);
                yield return waitExit;
                if (Answer == 0)
                    StartDialogue(dialogs[3]);
                else
                    StartDialogue(dialogs[6]);
                yield return waitExit;
                FadeOut();
                yield return waitTime;
                break;
            case 2:
                yield return new WaitForSeconds(1f);
                StartInterrogation(dialogs[4]);
                yield return waitExit;
                StartChoice(choices[0]);
                yield return waitExit;
                if (Answer == 0)
                    StartDialogue(dialogs[5]);
                else
                    StartDialogue(dialogs[6]);
                yield return waitExit;
                yield return waitTime;
                break;          
            default:
                break;
        }
        ExitCoroutine();
    }

    protected override IEnumerator WrongAnswerCoroutine()
    {
        HashSet<int> vs = new HashSet<int>();
        vs.Add(Adduced);

        yield return new WaitForSeconds(1f);
        StartInterrogation(dialogs[7]);
        yield return waitExit;

        Adduce(choices[1]);
        yield return waitExit;

        vs.Add(Adduced);

        if (vs.Equals(AnswerSet) & Count == 2)
            flag = false;
        else
        {
            StartDialogue(dialogs[8]);
            yield return waitExit;
            FadeOut();
        }

        ExitCoroutine();
    }
}
