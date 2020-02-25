using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour, Manager
{
    [SerializeField]
    private Event[] talk_events; //각 장면마다 이벤트들을 배치. 
    [SerializeField]
    private Event clue_event;

    private Animator myAnimator;
    private int index;
    private enum State
    {
        menu,
        talk,
    }
    private State state;
    private int talk_index;
    private UI ui;

    public void Enter(UI _ui)
    {
        ui = _ui;
        myAnimator.SetTrigger("appear");

        myAnimator.SetTrigger("button" + index);
    }

    public void Exit(bool _b = true)
    {
        myAnimator.SetTrigger("disappear");
        if(_b == true)
        {
            index = 0;
            state = State.menu;
        }
    }

    public void HandleInput()
    {
        switch (state)
        {
            case State.menu:
                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    if (index < 3) index++;
                    else index = 0;
                    myAnimator.SetTrigger("button" + index);
                }
                else if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    if (index > 0) index--;
                    else index = 3;
                    myAnimator.SetTrigger("button" + index);
                }
                else if (Input.GetKeyDown(KeyCode.Z))
                {
                    switch (index)
                    {
                        case 0: //map
                            ui.StartMapSelect();
                            break;
                        case 1: //investigate
                            ui.Investigate();
                            break;
                        case 2: //talk
                            break;
                        case 3: //clue
                            ui.GoToInventory(ReturnType.Adduce);
                            break;
                    }
                }
                else if(Input.GetKeyDown(KeyCode.W))
                {
                    index = 3;
                    ui.GoToInventory(ReturnType.Adduce);
                }
                break;
            case State.talk:
                break;
        }
        
    }

    // Start is called before the first frame update
    void Start()
    {
        myAnimator = GetComponent<Animator>();
        index = 0;
        state = State.menu;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
