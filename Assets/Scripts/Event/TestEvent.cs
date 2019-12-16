using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEvent : Event
{
    protected override IEnumerator EventCoroutine()
    {
        StartDialogue(dialogs[0]);

        yield return new WaitUntil(() => !IsExcuting());

        FadeOut();

        NextEvent(nextEvent);
    }
    protected override void Start()
    {
        base.Start();
        SetEvent();
    }
}
