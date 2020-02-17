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
    private int Index
    {
        get => index;
        set
        {
            if (index + value > 3) index = 0;
            else if (index + value < 0) index = 3;
            else index += value;

        }
    }
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
    }

    public void HandleInput()
    {
        switch (state)
        {
            case State.menu:
                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    Index++;
                    myAnimator.SetTrigger("button" + index);
                }
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    Index--;
                    myAnimator.SetTrigger("button" + index);
                }
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    switch (index)
                    {
                        case 0: //map
                            break;
                        case 1: //investigate
                            break;
                        case 2: //talk
                            break;
                        case 3: //clue

                            break;
                    }
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
