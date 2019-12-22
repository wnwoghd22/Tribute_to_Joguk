using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
[RequireComponent(typeof(BoxCollider2D))]
public abstract class Event : MonoBehaviour
{
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
    protected bool isActive = false; //false = start event automatically, 조사용 이벤트인가? 자동 이벤트인가?

    protected bool IsExcuting => EventHandler.IsExcuting;
    protected int Result => EventHandler.Result;
    
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
    protected void StartChoice(Choice _c) //선택 분기 진입.
    {
        EventHandler.StartChoice(_c);
    }
    protected void StartTestimony(Testimony _t) //할당 후 개시 글자가 움직이는 애니메이션.
    {
        EventHandler.AssignTestimony(_t);
        //애니메이션 코드를 여기에.
    }
    protected void ShowTestimony(int _i) //i번째 증언
    {
        EventHandler.CallTestimony(_i);
    }
    protected void Adduce() //제시 장면으로 진입
    {
        EventHandler.GoToInventory(Inventory.ReturnType.Both);
    }
    protected void AdducePerson() //인물 제시
    {
        EventHandler.GoToInventory(Inventory.ReturnType.Person);
    } 
    protected void AdduceClue() //증거 제시
    {
        EventHandler.GoToInventory(Inventory.ReturnType.Clue);
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
        //목소리는? - 애니메이터에 넣어버리자!
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

    protected void GetItem(int _itemID, int _count = 1)
    {
        EventHandler.GetItem(_itemID, _count);
    }
   
    protected void ExitEvent()
    {
        EventHandler.ExitEvent();
    } //단순 이벤트 종료. 증거 수집 등
    protected void NextEvent(Event _e) //다음 이벤트로
    {
        ExitEvent();
        _e.SetEvent();
    }
    protected void NextEvent(string _s) //다른 맵으로
    {
        ExitEvent();
        EventHandler.ChangeMap(_s);
    }
}
