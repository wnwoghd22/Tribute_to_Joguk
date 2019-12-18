using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CourtTest : CourtScene
{
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
                yield return new WaitUntil(() => !IsExcuting);
                break;
            case 1:
                yield return new WaitForSeconds(1f);
                StartInterrogation(dialogs[1]);
                yield return new WaitUntil(() => !IsExcuting);
                break;
            case 2:
                yield return new WaitForSeconds(1f);
                StartInterrogation(dialogs[2]);
                yield return new WaitUntil(() => !IsExcuting);
                break;
            case 3:
                yield return new WaitForSeconds(1f);
                StartInterrogation(dialogs[3]);
                yield return new WaitUntil(() => !IsExcuting);
                break;
            case 4:
                yield return new WaitForSeconds(1f);
                StartInterrogation(dialogs[4]);
                yield return new WaitUntil(() => !IsExcuting);
                break;
            default:
                break;
        }
        ExitCoroutine();
    }

    protected override IEnumerator WrongAnswerCoroutine()
    {
        throw new System.NotImplementedException();
    }
}
