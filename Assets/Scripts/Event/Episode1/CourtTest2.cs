using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CourtTest2 : TestimonyScene
{
    [SerializeField]
    string switch0;
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
                if(testimony.logCount == -1)
                {
                    StartInterrogation(dialogs[3]);
                    SetTestimony(Count, switch0);
                    testimony.logCount = Count;
                }
                else
                    StartInterrogation(dialogs[4]);
                yield return waitExit;
                FadeOut();
                yield return waitTime;
                break;
            case 4:
                yield return new WaitForSeconds(1f);
                StartInterrogation(dialogs[5]);
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
        yield return new WaitForSeconds(1f);
        StartInterrogation(dialogs[6]);
        yield return waitExit;

        //panalty

        ExitCoroutine();
    }
}
