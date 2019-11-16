using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event1 : Event
{
    public Dialog dialogue1;
    public Dialog dialogue2;

    private void Awake()
    {
        isActive = true;
    }

    protected override IEnumerator EventCoroutine()
    {       
        StartDialogue(dialogue1);

        yield return new WaitUntil(() => !IsExcuting());

        MoveRight("Player");
        
        yield return new WaitForSeconds(2f);

        Flash();

        StartDialogue(dialogue2);

        yield return new WaitUntil(() => !IsExcuting());

        ExitEvent();
    }
}
