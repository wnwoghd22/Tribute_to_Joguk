using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CourtTest3 : TestimonyScene
{
    [SerializeField][TextArea(2,2)]
    string switch0;
    [SerializeField][TextArea(2, 2)]
    string switch1;
    protected override IEnumerator InterrogationCoroutine(int _i)
    {
        switch (_i)
        {
            case 0:
                yield return new WaitForSeconds(1f);
                StartInterrogation(dialogs[0]);
                yield return waitExit;
                FadeOut();
                yield return waitTime;
                break;
            case 1:
                yield return new WaitForSeconds(1f);
                StartInterrogation(dialogs[1]);
                yield return waitExit;
                FadeOut();
                yield return waitTime;
                break;
            case 2:
                yield return new WaitForSeconds(1f);
                StartInterrogation(dialogs[2]);
                yield return waitExit;
                FadeOut();
                yield return waitTime;
                break;
            case 3:
                yield return new WaitForSeconds(1f);
                StartInterrogation(dialogs[3]);
                yield return waitExit;
                StartChoice(choices[0]);
                yield return waitExit;
                switch (Answer)
                {
                    case 0:
                        StartInterrogation(dialogs[4]);
                        testimony.logCount = Count;
                        SetTestimony(Count, switch0);
                        yield return waitExit;
                        break;
                    case 1:
                        StartInterrogation(dialogs[5]);
                        testimony.logCount = -1;
                        SetTestimony(Count, switch1);
                        yield return waitExit;
                        break;
                    case 2:
                        StartInterrogation(dialogs[6]);
                        yield return waitExit;
                        break;
                }
                yield return waitTime;
                break;
            default:
                break;
        }
        ExitCoroutine();
    }

    protected override IEnumerator WrongAnswerCoroutine()
    {
        yield return new WaitForSeconds(1f);
        StartInterrogation(dialogs[7]);
        yield return waitExit;

        //panalty

        FadeOut();
        ExitCoroutine();
    }
}
