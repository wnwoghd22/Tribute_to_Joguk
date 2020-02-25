using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestClue : Event
{
    [SerializeField]
    Map[] maps;

    protected override IEnumerator EventCoroutine()
    {
        StartDialogue(dialogs[0]);
        yield return waitExit;

        ExitEvent(false);
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        isClue = true;
        UI.instance.SetMap(maps);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
