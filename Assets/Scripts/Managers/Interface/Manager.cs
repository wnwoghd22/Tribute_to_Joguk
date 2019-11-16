using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Manager
{
    void HandleInput();
    void Enter(UI ui);
    void Exit();
}
