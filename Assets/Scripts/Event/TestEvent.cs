using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEvent : Event
{
    protected override IEnumerator EventCoroutine()
    {
        Debug.Log("asdf");
        StartDialogue(dialogs[0]);

        yield return new WaitUntil(() => !IsExcuting());
    }
    protected override void Start()
    {
        base.Start();
        SetEvent();
    }
}
