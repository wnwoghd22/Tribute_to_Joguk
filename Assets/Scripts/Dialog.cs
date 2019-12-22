using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialog
{
    public string name;
    public string[] sentence;
    public string[] _name;
    public emotion[] _emotion;
    public who[] _who;
}
public enum emotion
{
    normal,
    point,
    bang,
    embarrassed,
    doubt,
    interrogate,
    check,
}
public enum who
{
    None,
    Attorney,
    Prosecutor,
    Judge,
    Witness,
    Company,
    Talk,
}
