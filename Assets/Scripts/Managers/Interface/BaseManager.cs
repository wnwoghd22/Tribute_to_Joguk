using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseManager : MonoBehaviour, Manager
{
    private UI ui;

    public void Enter(UI _ui)
    {
        ui = _ui;
        //HUD on
    }
    public void Exit(bool _b = true)
    {
        //HUD off
    }
    public void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.I))
            ui.GoToInventory();
        else if (Input.GetKeyDown(KeyCode.C)) //InterAct.
        {
            if (ui.IsEvent())
                ui.StartEvent();
            else if (ui.IsMap)
                ui.StartChangeMap();
        }
        return;
    }

    // Start is called before the first frame update
    void Start()
    {       
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
