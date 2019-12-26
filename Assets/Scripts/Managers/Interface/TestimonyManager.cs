using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestimonyManager : MonoBehaviour, Manager
{
    private bool keyActivated = false; //입력 가능 상태인가?
    public int Count { get; private set; }
    public void NextCount()
    {
        if(0 <= Count && Count < listSentences.Count)
            Count++;
        else if(Count == listSentences.Count)
            Count = 0;
    }

    public enum State
    {
        interrogate,
        objection,
        testimony,
        back_to_zero,
    }
    public State state { get; private set; }

    [SerializeField]
    private Text text;
    [SerializeField]
    private Text text_middle;
    [SerializeField]
    private Animator Dialog;

    private string description; //진입 시, ~증언 내용~
    private List<string> listSentences; //증언 내용
    private List<emotion> listEmotion;
    private int ItemID; //정답 조회용
    private int answer;
    public bool Answer => ItemID == answer;
    private bool hold_box = false;


    public string typesound;
    public string entersound;
    
    private UI ui;
    private WaitForSeconds waitTime = new WaitForSeconds(0.01f);

    // Start is called before the first frame update
    void Start()
    {
        Count = -1;
        text.text = "";
        listSentences = new List<string>();
        listEmotion = new List<emotion>();
    }
    public void Enter(UI _ui)
    {
        ui = _ui;
        state = State.testimony;
        if(Count >= 0 && !hold_box)
        {
            Dialog.SetBool("Appear", true);
        }
        if (hold_box)
            hold_box = false;
    }
    public void Exit(bool _b = true) //통제권 넘김
    {
        if (_b == true)
            ClearTestimony();
        else
            HoldTestimony();
        //ui.SetPlayerMove(true);
    }
    public void HandleInput()
    {
        if (keyActivated)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                keyActivated = false;
                Count++;
                text.text = "";
                ui.PlaySound(typesound);

                if (Count == listSentences.Count)
                {
                    //확인 대사 후 첫번째부터
                    StopAllCoroutines();
                    //HoldTestimony();
                    state = State.back_to_zero; //상태전이, count를 0으로 초기화하고 다시 돌아감.
                }
                else
                {
                    StopAllCoroutines();
                    StartCoroutine(StartTextCoroutine(Count));
                }
            }
            if (Count >= 0)
            {
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    //심문...?
                    //잠깐! 대사 띄우기
                    state = State.interrogate; //상태전이, 대사를 완료하고 count++
                } //심문
                else if (Input.GetKeyDown(KeyCode.W))
                {
                    hold_box = true;
                    ui.GoToInventory(ReturnType.Objection);
                } //법정기록 열람
                else if (Input.GetKeyDown(KeyCode.RightArrow)) 
                {
                    if (Count < listSentences.Count - 1)
                    {
                        Count++;                       
                        StopAllCoroutines();
                        StartCoroutine(StartTextCoroutine(Count));
                    }
                } //다음 대사
                else if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    if (Count > 0)
                    {
                        Count--;                        
                        StopAllCoroutines();
                        StartCoroutine(StartTextCoroutine(Count));
                    }
                } //이전 대사
            }
        }
    }

    public void AssignTestimony(Testimony _testimony) //심문 내용 넣기
    {
        description = _testimony.testimony.name;
        for (int i = 0; i < _testimony.testimony.sentence.Length; i++)
        {
            listSentences.Add(_testimony.testimony.sentence[i]);
            listEmotion.Add(_testimony.testimony._emotion[i]);
        }
    }
    private void ClearTestimony() //심문 종료, 모든 변수 초기화
    {

        Count = -1;
        listSentences.Clear();
        listEmotion.Clear();
        //Character.SetBool("Appear", false);
        Dialog.SetBool("Appear", false);
    }
    private void HoldTestimony() //심문 중단, 상태 전이
    {
        //Character.SetBool("Appear", false);
        if(!hold_box)
            Dialog.SetBool("Appear", false);
    }

    public void ShowText(int _c)
    {
        if (_c == -1)
            StartCoroutine(StartInterrogationCoroutine());
        else if (_c == listSentences.Count)
            state = State.back_to_zero;
        else
            StartCoroutine(StartTextCoroutine(_c));
    } //count번째 심문 대사
    IEnumerator StartInterrogationCoroutine()
    {
        keyActivated = false;
        text.text = "";
        text_middle.text = "";

        Dialog.SetBool("Appear", true);

        //심문개시 글자 애니메이션 띄우기

        for (int i = 0; i < description.Length; i++)
        {
            text_middle.text += description[i];
            if (i % 7 == 1)
            {
                ui.PlaySound(entersound);
            }
            yield return waitTime;
        }
        yield return waitTime;
        keyActivated = true;
    } //심문개시 글자 띄우기, 
    IEnumerator StartTextCoroutine(int _c)
    {
        keyActivated = true;
        text_middle.text = "";
        text.text = "";

        for (int i = 0; i < listSentences[_c].Length; i++)
        {
            text.text += listSentences[_c][i];
            if (i % 7 == 1)
            {
                ui.PlaySound(entersound);
            }
            yield return waitTime;
        }
    } //심문 대사 띄우기
    public void SetObjection(int _r)
    {
        answer = _r;
        state = State.objection;
    } //이의 제기
}
