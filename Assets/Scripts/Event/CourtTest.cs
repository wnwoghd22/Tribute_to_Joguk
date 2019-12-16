using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CourtTest : CourtScene
{
    protected override IEnumerator InterrogationCoroutine(int _i)
    {
        switch (_i)
        {
            case 0:
                yield return new WaitForSeconds(1f);
                StartDialogue(dialogs[0]);
                yield return new WaitUntil(() => !IsExcuting());
                break;
            case 1:
                yield return new WaitForSeconds(1f);
                StartDialogue(dialogs[1]);
                yield return new WaitUntil(() => !IsExcuting());
                break;
            case 2:
                yield return new WaitForSeconds(1f);
                StartDialogue(dialogs[2]);
                yield return new WaitUntil(() => !IsExcuting());
                break;
            case 3:
                yield return new WaitForSeconds(1f);
                StartDialogue(dialogs[3]);
                yield return new WaitUntil(() => !IsExcuting());
                break;
            case 4:
                yield return new WaitForSeconds(1f);
                StartDialogue(dialogs[4]);
                yield return new WaitUntil(() => !IsExcuting());
                break;
            default:
                break;
        }
        ExitIterrogation();
    }

    protected override IEnumerator ObjectionCoroutine(int _i, int _ID)
    {
        throw new System.NotImplementedException();
    }
}
