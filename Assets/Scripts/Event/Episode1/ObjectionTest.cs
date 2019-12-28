using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectionTest : Event
{
    protected override IEnumerator EventCoroutine()
    {
        yield return waitTime;
        StartDialogue(dialogs[0]); //이의제기
        yield return waitExit;

        //증거제출 - 휴가명령서

        StartDialogue(dialogs[1]); //증거확인
        yield return waitExit;

        //증인 바꾸기



    }
}
