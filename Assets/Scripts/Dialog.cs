using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialog
{
    public struct info
    {
        public string sentence;
        public string name;
        public emotion emotion;
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

    public string name;
    [TextArea(1, 2)]
    public info[] infos;
}
