using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEvent : Event
{
    protected override IEnumerator EventCoroutine()
    {
        StartDialogue(dialogs[0]);
        yield return waitExit;

        FadeOut();
        yield return waitTime;
        ChangeCut("Lobby");
        yield return waitTime;
        FadeIn();
        yield return waitTime;

        StartText(texts[0]);
        yield return waitExit;
        ChangeCut("Off");

        ChangeCut("Court");
        yield return waitTime;
        ChangeCut("Gavel");
        yield return waitTime;
        StartDialogue(dialogs[1]);
        yield return waitExit;
        FadeOut();
        yield return waitTime;

        NextEvent(nextEvent);
    }
    protected override void Start()
    {
        base.Start();
        SetEvent();
    }
}
