using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(BoxCollider2D))]
public abstract class Event : MonoBehaviour
{
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
    protected bool isActive = false; //false = start event automatically, 조사용 이벤트인가? 자동 이벤트인가?
    protected WaitForSeconds waitTime = new WaitForSeconds(1f);

    // Start is called before the first frame update
    protected virtual void Start()
    {
        EventHandler = FindObjectOfType<UI>();
    }
    protected virtual void OnTriggerEnter2D(Collider2D collision) //조사화면에서 커서를 맞출 때.
    {
        if (!flag && collision.gameObject.tag == "Player")
        {
            EventHandler.GetEvent(this);
            if (!isActive)
                EventHandler.StartEvent();
        }       
    }
    protected virtual void OnTriggerExit2D(Collider2D collision) //커서가 벗어날 때
    {
        if (collision.gameObject.tag == "Player")
        {
            EventHandler.ClearEvent();
        }
    }
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
    protected abstract IEnumerator EventCoroutine();

    protected void StartDialogue(Dialog _d) //대사 진입.
    {
        EventHandler.StartDialogue(_d);    
    }
    protected void StartChoice(Choice _c) //선택 분기 진입.
    {
        EventHandler.StartChoice(_c);
    }
    protected void StartTestimony(Testimony _t) //할당 후 개시 글자가 움직이는 애니메이션.
    {
        EventHandler.AssignTestimony(_t);
        //애니메이션 코드를 여기에.
    }
    protected void ShowTestimony(int _i)
    {
        EventHandler.CallTestimony(_i);
    }
    protected bool IsExcuting()
    {
        return EventHandler.IsExcuting();
    }
    protected int GetResult()
    {
        return EventHandler.GetResult();
    }

    protected void MoveRight(string _name, int _count = 1)
    {
        EventHandler.Move(true, _count);
    }
    protected void MoveLeft(string _name, int _count = 1)
    {
        EventHandler.Move(false, _count);
    }
    protected void ActWait()
    {
        //목소리는?
        EventHandler.Action(0);
    }
    protected void ActObjection()
    {
        //목소리는?
        EventHandler.Action(1);
    }
    protected void ActTakeThis()
    {
        //목소리는?
        EventHandler.Action(2);
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
    protected void SetCutActive(bool _b)
    {
        EventHandler.SetCutActive(_b);
    }
    protected void PlayBGM(int _track)
    {
        EventHandler.PlayBGM(_track);
    }

    protected void GetItem(int _itemID, int _count = 1)
    {
        EventHandler.GetItem(_itemID, _count);
    }
   
    protected void ExitEvent()
    {

        Debug.Log("flag" + flag);
    }
    protected void NextEvent(Event _e)
    {
        _e.SetEvent();
    }
}
