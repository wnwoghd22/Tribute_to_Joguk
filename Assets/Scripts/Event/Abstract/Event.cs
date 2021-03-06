﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
[RequireComponent(typeof(BoxCollider2D))]
public abstract class Event : MonoBehaviour
{
    [SerializeField]
    public string EventName { get; protected set; }
    [SerializeField][TextArea(1,2)]
    protected string[] texts;
    [SerializeField]
    protected Dialog[] dialogs;
    [SerializeField]
    protected Choice[] choices;
    [SerializeField]
    protected Sprite[] cutImages;
    [SerializeField]
    protected Event nextEvent;

    private UI EventHandler;
    protected bool flag = false;
    protected bool isActive = false; //false = start event automatically
    protected bool isClue = false; //조사화면용 이벤트인가?

    private bool IsExcuting => EventHandler.IsExcuting;
    protected int Answer => EventHandler.Result;
    protected int Adduced => EventHandler.ReturnValue;
    
    protected WaitForSeconds waitTime = new WaitForSeconds(1f);
    protected WaitUntil waitExit;
    
    // Start is called before the first frame update
    protected virtual void Start()
    {
        EventHandler = FindObjectOfType<UI>();
        waitExit = new WaitUntil(() => !EventHandler.IsExcuting);
    }
    protected virtual void OnTriggerEnter2D(Collider2D collision) //조사화면에서 커서를 맞출 때.
    {
        if (isClue && collision.gameObject.tag == "cursor")
        {
            EventHandler.GetEvent(this);
        }       
    }
    protected virtual void OnTriggerExit2D(Collider2D collision) //커서가 벗어날 때
    {
        if (collision.gameObject.tag == "cursor")
        {
            EventHandler.ClearEvent();
        }
    }
    protected abstract IEnumerator EventCoroutine();

    protected void SetEvent()
    {
        EventHandler.GetEvent(this);
        if (!isActive)
            EventHandler.StartEvent();
    }
    public void Excute()
    {
        flag = true;
        StartCoroutine(EventCoroutine());
    }

    protected void ShowMenu()
    {
        ExitEvent(); //모든 이벤트 종료, 클리어
        EventHandler.ActivateMenu();
    }

    protected void StartDialogue(Dialog _d) //대사 진입.
    {
        EventHandler.StartDialogue(_d);    
    }
    protected void StartInterrogation(Dialog _d) //심문 진입.
    {
        EventHandler.StartDialogue(_d, false);
    }
    protected void StartText(string _s) //단순 텍스트
    {
        EventHandler.StartText(_s);
    }
    protected void StartTestimony(Testimony _t) //할당 후 개시 글자가 움직이는 애니메이션.
    {
        EventHandler.StartTestimony(_t);
        //애니메이션 코드를 여기에.
    }
    protected void ShowTestimony(int _i) //i번째 증언
    {
        EventHandler.CallTestimony(_i);
    }

    protected void StartChoice(Choice _c) //선택 분기 진입.
    {
        EventHandler.StartChoice(_c);
    }
    protected void Adduce(Choice _c) //제시 장면으로 진입
    {
        EventHandler.Adduce(_c,ReturnType.Both);
    }
    protected void AdducePerson(Choice _c) //인물 제시
    {
        EventHandler.Adduce(_c, ReturnType.Person);
    } 
    protected void AdduceClue(Choice _c) //증거 제시
    {
        EventHandler.Adduce(_c, ReturnType.Clue);
    }
    protected void ChangeWitness(RuntimeAnimatorController _a)
    {
        EventHandler.ChangeWitness(_a);
    }
    protected void MoveRight(string _name, int _count = 1)
    {
        EventHandler.Move(true, _count);
    }
    protected void MoveLeft(string _name, int _count = 1)
    {
        EventHandler.Move(false, _count);
    }
    protected void HoldIt()
    {
        //목소리는? - 애니메이터에 넣어버리자!
        EventHandler.Effect(effect.HoldIt);
    }
    protected void Objection()
    {
        //목소리는?
        EventHandler.Effect(effect.Objection);
    }
    protected void TakeThat()
    {
        //목소리는?
        EventHandler.Effect(effect.TakeThat);
    }

    protected void Flash(float _speed = 0.1f)
    {
        EventHandler.Flash(_speed);
    }
    protected void FadeOut(float _speed = 0.02f)
    {
        EventHandler.FadeOut(_speed);
    }
    protected void FadeIn(float _speed = 0.02f)
    {
        EventHandler.FadeIn(_speed);
    }
    protected void ChangeCut(int num)
    {
        EventHandler.ChangeCut(cutImages[num]);
    }
    protected void ChangeCut(string _s)
    {
        EventHandler.SetCutTrigger(_s);
    }
    protected void SetCutActive(bool _b)
    {
        EventHandler.SetCutActive(_b);
    }
    protected void PlayBGM(int _track)
    {
        EventHandler.PlayBGM(_track);
    }
    protected void ClearAll()
    {
        EventHandler.ClearAll();
    }

    protected void GetItem(int _itemID)
    {
        EventHandler.GetItem(_itemID);
    }
   
    protected void ExitEvent(bool _b = true) //단순 이벤트 종료. 증거 수집 등 false - stack.Pop()
    {
        EventHandler.ExitEvent(_b);
    } 
    protected void NextEvent(Event _e) //다음 이벤트로
    {
        ExitEvent();
        _e.SetEvent();
    }
    protected void NextEvent(string _s) //다른 맵으로
    {
        if (_s == "Title")
        {
            EventHandler.BackToTitle();
            return;
        }
        else
        {
            ExitEvent();
            EventHandler.ChangeMap(_s);
        }
       
    }
}
