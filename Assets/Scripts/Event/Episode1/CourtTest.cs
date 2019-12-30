using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CourtTest : TestimonyScene
{
    [SerializeField][TextArea(2,2)]
    private string switch0;
    [SerializeField][TextArea(2,2)]
    private string switch1;

    protected override void Start()
    {
        base.Start();
        testimony.logCount = -1;
    }

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
                if (testimony.logCount == -1)
                    StartChoice(choices[0]);
                else
                    StartChoice(choices[1]);
                yield return waitExit;
                switch(Answer)
                {
                    case 0:
                        testimony.logCount = -2;
                        StartDialogue(dialogs[4]);
                        SetTestimony(Count, switch0);
                        yield return waitExit;
                        break;
                    case 1:
                        testimony.logCount = Count;
                        StartDialogue(dialogs[5]);
                        SetTestimony(Count, switch1);
                        yield return waitExit;
                        break;
                    case 2:
                        StartDialogue(dialogs[6]);
                        yield return waitExit;
                        break;
                }
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
