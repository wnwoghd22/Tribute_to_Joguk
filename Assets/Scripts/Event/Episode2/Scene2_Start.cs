using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene2_Start : Event
{
    protected override IEnumerator EventCoroutine()
    {
        StartDialogue(dialogs[0]);
        yield return waitExit;

        //조사화면으로 넘어갈 것.
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        SetEvent();
    }   
}
