using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class TestimonyManager : MonoBehaviour, Manager
{
    private bool keyActivated = false; //입력 가능 상태인가?
    private int count = -1; //증언이 몇 번째 장면인가?
    public int GetCount() => count;
    public void NextCount()
    {
        if(0 <= count && count < listSentences.Count)
            count++;
        else if(count == listSentences.Count)
            count = 0;
    }

    public enum InputState
    {
        interrogate,
        objection,
        testimony,
        back_to_zero,
    }
    private InputState state;
    public InputState GetState() => state;

    [SerializeField]
    private Text text;
    [SerializeField]
    private Text text_middle;
    public SpriteRenderer rendererSprite;
    
    private string description; //진입 시, ~증언 내용~
    private List<string> listSentences; //증언 내용
    private List<Dialog.emotion> listEmotion;
    private int logNum; //정답 조회용
    private int ItemID; //정답 조회용

    public Animator Character;
    public Animator Dialog;

    public string typesound;
    public string entersound;
    
    private UI ui;
    private WaitForSeconds waitTime = new WaitForSeconds(0.01f);
    // Start is called before the first frame update
    void Start()
    {
        count = -1;
        text.text = "";
        listSentences = new List<string>();
        listEmotion = new List<Dialog.emotion>();
    }
    public void Enter(UI _ui)
    {
        ui = _ui;
        state = InputState.testimony;
        Dialog.SetBool("Appear", true);
    }
    public void Exit() //통제권 넘김
    {
        text.text = "";
        //ui.SetPlayerMove(true);
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

        count = -1;
        listSentences.Clear();
        listEmotion.Clear();
        //Character.SetBool("Appear", false);
        Dialog.SetBool("Appear", false);
    }
    private void HoldTestimony()
    {
        text.text = "";
        //Character.SetBool("Appear", false);
        Dialog.SetBool("Appear", false);
    }
    public void HandleInput()
    {
        if (keyActivated)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                keyActivated = false;
                count++;
                text.text = "";
                ui.PlaySound(typesound);

                if (count == listSentences.Count)
                {
                    //확인 대사 후 첫번째부터
                    StopAllCoroutines();                   
                    HoldTestimony();
                    state = InputState.back_to_zero; //상태전이, count를 0으로 초기화하고 다시 돌아감.
                }
                else
                {
                    StopAllCoroutines();
                    StartCoroutine(StartTextCoroutine(count));
                }
            }
            if(count >= 0)
            {
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    //심문...?
                    //잠깐! 대사 띄우기
                    state = InputState.interrogate; //상태전이, 대사를 완료하고 count++
                }
                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    if (count < listSentences.Count - 1)
                    {
                        count++;
                        //다음 대사
                        StopAllCoroutines();
                        StartCoroutine(StartTextCoroutine(count));
                    }
                }
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    if (count > 0)
                    {
                        count--;
                        //이전 대사
                        StopAllCoroutines();
                        StartCoroutine(StartTextCoroutine(count));
                    }
                }
            }
        }
    }

    public void ShowText(int _c)
    {
        if (_c == -1)
        {
            StartCoroutine(StartInterrogationCoroutine());
        }
        else
            StartCoroutine(StartTextCoroutine(_c));
    }
    IEnumerator StartInterrogationCoroutine()
    {
        keyActivated = false;
        text.text = "";
        text_middle.text = "";

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
    }
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
    }
    //// Start is called before the first frame update
    //void Start()
    //{

    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}
}
