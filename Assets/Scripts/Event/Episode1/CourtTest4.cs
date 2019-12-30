using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CourtTest4 : TestimonyScene
{
    bool[] _trigger = { false, false, false, false, false, false };
    protected override IEnumerator InterrogationCoroutine(int _i)
    {
        _trigger[_i] = true;
        switch (_i)
        {
            case 0:
                yield return new WaitForSeconds(1f);
                StartInterrogation(dialogs[0]);
                yield return waitExit;
                if (_trigger[0] & _trigger[1] & _trigger[2] & _trigger[3] & _trigger[4] & _trigger[5])
                {
                    flag = false;
                    break;
                }
                FadeOut();
                yield return waitTime;
                break;
            case 1:
                yield return new WaitForSeconds(1f);
                StartInterrogation(dialogs[1]);
                yield return waitExit;
                if (_trigger[0] & _trigger[1] & _trigger[2] & _trigger[3] & _trigger[4] & _trigger[5])
                {
                    flag = false;
                    break;
                }
                FadeOut();
                yield return waitTime;
                break;
            case 2:
                yield return new WaitForSeconds(1f);
                StartInterrogation(dialogs[2]);
                yield return waitExit;
                if (_trigger[0] & _trigger[1] & _trigger[2] & _trigger[3] & _trigger[4] & _trigger[5])
                {
                    flag = false;
                    break;
                }
                FadeOut();
                yield return waitTime;
                break;
            case 3:
                yield return new WaitForSeconds(1f);
                StartInterrogation(dialogs[3]);
                yield return waitExit;
                if (_trigger[0] & _trigger[1] & _trigger[2] & _trigger[3] & _trigger[4] & _trigger[5])
                {
                    flag = false;
                    break;
                }
                FadeOut();
                yield return waitTime;
                break;
            case 4:
                yield return new WaitForSeconds(1f);
                StartInterrogation(dialogs[4]);
                yield return waitExit;
                if (_trigger[0] & _trigger[1] & _trigger[2] & _trigger[3] & _trigger[4] & _trigger[5])
                {
                    flag = false;
                    break;
                }
                FadeOut();
                yield return waitTime;
                break;
            case 5:
                yield return new WaitForSeconds(1f);
                StartInterrogation(dialogs[5]);
                yield return waitExit;
                if (_trigger[0] & _trigger[1] & _trigger[2] & _trigger[3] & _trigger[4] & _trigger[5])
                {
                    flag = false;                   
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
        StartInterrogation(dialogs[6]);
        yield return waitExit;

        //panalty

        FadeOut();
        ExitCoroutine();
    }
}
