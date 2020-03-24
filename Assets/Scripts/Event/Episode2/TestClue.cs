using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestClue : Clue
{
    protected override void Start()
    {
        base.Start();
        PosX = 0f;
        PosY = 0f;
        SetPos(PosX, PosY);
    }

    protected override IEnumerator EventCoroutine()
    {
        StartDialogue(dialogs[0]);
        yield return waitExit;

        ExitEvent(false);
    }
}
