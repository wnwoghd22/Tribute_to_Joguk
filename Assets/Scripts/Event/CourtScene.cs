using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CourtScene : Event
{
    private TestimonyManager theTM;
    protected Testimony testimony;

    protected void Testimony()
    {
        //theTM.
    }
    protected void Interrogation()
    {
        
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        theTM = FindObjectOfType<TestimonyManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
