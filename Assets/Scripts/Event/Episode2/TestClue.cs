using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestClue : Event
{
    protected override IEnumerator EventCoroutine()
    {
        StartDialogue(dialogs[0]);
        yield return waitExit;

        ExitEvent();
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        isClue = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
