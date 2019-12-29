using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectionTest : Event
{
    [SerializeField]
    RuntimeAnimatorController Amolang;
    protected override IEnumerator EventCoroutine()
    {
        yield return waitTime;
        StartDialogue(dialogs[0]); //이의제기
        yield return waitExit;

        //증거제출 - 근무일지
        GetItem(10006);

        StartDialogue(dialogs[1]); //증거확인
        yield return waitExit;

        //잠깐!
        HoldIt();
        ChangeWitness(Amolang);
        GetItem(11004);
        //증인 바꾸기
        yield return waitTime;

        StartDialogue(dialogs[2]);
        yield return waitExit;

        FadeOut();
        yield return waitTime;
        FadeIn();

        StartDialogue(dialogs[3]);
        yield return waitExit;

        FadeOut();
        yield return waitTime;
        NextEvent(nextEvent);
    }
}
