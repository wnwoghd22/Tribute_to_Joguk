using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Clue : Event
{
    public float PosX { get; protected set; }
    public float PosY { get; protected set; }

    protected override void Start()
    {
        base.Start();
        isClue = true;
    }

    protected void SetPos(float _x, float _y)
    {
        Vector3 pos = Camera.main.ViewportToWorldPoint(this.gameObject.transform.position);
        pos.x = _x;
        pos.y = _y;
        this.gameObject.transform.position = pos;
    }
}
