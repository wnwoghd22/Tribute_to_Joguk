using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Testimony
{
    public Dialog testimony;
    public int logCount; //답이 없을 경우 -1을 넣는다.
    public int itemID;
}