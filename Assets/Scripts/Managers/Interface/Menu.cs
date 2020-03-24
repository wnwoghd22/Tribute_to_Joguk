using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour, Manager
{
    private List<Event> talk_events; //각 장면마다 이벤트들을 배치. 
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
    [SerializeField]
    private GameObject[] talk_panel;
    [SerializeField]
    private Text[] talk_name;
    private WaitForSeconds waitTime = new WaitForSeconds(0.01f);

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
                            state = State.talk;
                            TalkAppear();
                            myAnimator.SetTrigger("talkAppear");

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
                if(Input.GetKeyDown(KeyCode.DownArrow))
                {
                    if (talk_index < talk_events.Count - 1)
                        talk_index++;
                    else
                        talk_index = 0;
                    StopAllCoroutines();
                    TalkSelection();
                }
                else if(Input.GetKeyDown(KeyCode.UpArrow))
                {
                    if (talk_index > 0)
                        talk_index--;
                    else
                        talk_index = talk_events.Count - 1;
                    StopAllCoroutines();
                    TalkSelection();
                }
                else if(Input.GetKeyDown(KeyCode.Z))
                {
                    ui.GetEvent(talk_events[talk_index]);
                    ui.StartEvent();
                }
                else if(Input.GetKeyDown(KeyCode.X))
                {
                    state = State.menu;
                    myAnimator.SetTrigger("talkDisappear");
                }
                break;
        }
        
    }

    // Start is called before the first frame update
    void Start()
    {
        myAnimator = GetComponent<Animator>();
        talk_events = new List<Event>();
        for (int i = 0; i < 5; i++)
        {
            talk_panel[i].gameObject.SetActive(false);
            talk_name[i].text = "";
        }
        index = 0;
        state = State.menu;
    }
    public void SetDialogues(Event[] _events) => talk_events.AddRange(_events);
    public void SetAdduce(Event _event) => clue_event = _event;
      
    // Update is called once per frame
    void Update()
    {

    }

    void TalkAppear()
    {
        for(int i = 0; i < talk_events.Count; i++)
        { 
            talk_panel[i].SetActive(true);
            talk_name[i].text = talk_events[i].EventName;
        }
        TalkSelection();
    }
    void TalkSelection()
    {
        Color _color = talk_panel[0].GetComponent<Image>().color;
        _color.a = .8f;
        for (int i = 0; i < talk_events.Count; i++)
            talk_panel[i].GetComponent<Image>().color = _color;

        StartCoroutine(TalkSelectCoroutine());
    }
    IEnumerator TalkSelectCoroutine()
    {
        Color _color = talk_panel[0].GetComponent<Image>().color;
        while (state == State.talk)
        {
            while(_color.a < 1f)
            {
                _color.a += 0.03f;
                talk_panel[talk_index].GetComponent<Image>().color = _color;
                yield return waitTime;
            }
            while (_color.a > .5f)
            {
                _color.a -= 0.03f;
                talk_panel[talk_index].GetComponent<Image>().color = _color;
                yield return waitTime;
            }
            yield return waitTime;
        }
    }
}
