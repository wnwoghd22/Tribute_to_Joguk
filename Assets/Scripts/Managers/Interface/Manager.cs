using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Manager
{
    void HandleInput();
    void Enter(UI ui);
    void Exit(bool _b = true); //true - 작업을 완전히 끝내고 상태를 전이함. false - 상태만 전이함.
}
