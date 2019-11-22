using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CourtScene : Event
{
    private TestimonyManager theTM;
    protected Testimony testimony;
    [SerializeField]
    protected Event[] interrogations;
    protected Dialog backToTheFirst;

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
        theTM.SetEvents(interrogations);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
