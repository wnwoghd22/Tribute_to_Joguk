using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event2 : Event
{
    public Dialog dialogue1;
    public Dialog dialogue2;
    public Choice choice;

    private void Awake()
    {
        isActive = true;
    }

    protected override IEnumerator EventCoroutine()
    {
        StartChoice(choice);

        yield return new WaitUntil(() => !IsExcuting());

        switch (GetResult())
        {
            case 0:
                ExitEvent();
                break;
            case 1:
                StartDialogue(dialogue1);
                yield return new WaitUntil(() => !IsExcuting());
                ExitEvent();
                break;
            case 2:
                StartDialogue(dialogue2);
                yield return new WaitUntil(() => !IsExcuting());
                ExitEvent();
                break;
        }

        yield return new WaitForSeconds(1f);

        flag = false;
    }
}
