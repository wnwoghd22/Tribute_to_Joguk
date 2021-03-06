﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialog
{
    public string name;
    [TextArea(1,2)]
    public string[] sentence;
    public string[] _name;
    public emotion[] _emotion;
    public who[] _who;
    public effect[] _effect;
}
public enum emotion
{
    None,
    normal, //평정
    serious, //심각
    confident, //당당
    point, //손가락질
    bang, //책상치기
    shocked, //충격
    embarrassed, //당황
    doubt, //의문
    akward, //멋쩍은 웃음
    check, //확인
    agree, //끄덕이기
    deny, //가로젓기
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
    Court,
}
public enum effect
{
    None,
    HoldIt,
    Objection,
    TakeThat,
    Quake,
}

