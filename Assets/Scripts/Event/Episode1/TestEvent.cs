using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEvent : Event
{
    protected override IEnumerator EventCoroutine()
    {
        #region CutIn
        StartDialogue(dialogs[0]);
        yield return waitExit;

        FadeOut();
        yield return waitTime;
        SetCutActive(true);
        ChangeCut(0);
        yield return waitTime;
        FadeIn();
        yield return waitTime;

        StartDialogue(dialogs[1]);
        yield return waitExit;

        FadeOut();
        yield return waitTime;
        ChangeCut(1);
        yield return waitTime;
        FadeIn();

        StartDialogue(dialogs[2]);
        yield return waitExit;

        FadeOut();
        yield return waitTime;
        SetCutActive(false);
        #endregion

        ChangeCut("Lobby");
        yield return waitTime;
        FadeIn();
        yield return waitTime;

        StartText(texts[0]);
        yield return waitExit;
        
        StartDialogue(dialogs[3]);

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
