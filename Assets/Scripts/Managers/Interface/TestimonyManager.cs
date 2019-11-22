using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestimonyManager : MonoBehaviour, Manager
{
    private bool keyActivated = false;
    private int count = -1;
    public int Count { get => count; }

    [SerializeField]
    private Text text;
    [SerializeField]
    private Text text_middle;
    public SpriteRenderer rendererSprite;

    private List<Event> events;
    public void SetEvents(Event[] _events)
    {
        for (int i = 0; i < _events.Length; i++)
            events.Add(_events[i]);
    }
    private string description;
    private List<string> listSentences;
    private List<Dialog.emotion> listEmotion;
    private int logNum; //정답 조회용
    private int ItemID; //정답 조회용

    public Animator Character;
    public Animator Dialog;

    public string typesound;
    public string entersound;
    
    private UI ui;
    private WaitForSeconds waitTime = new WaitForSeconds(0.01f);

    public void Enter(UI _ui)
    {
        ui = _ui;
        if (count == -1)
        {

        }
    }
    public void Exit() //통제권 넘김
    {
        text.text = "";
        //ui.SetPlayerMove(true);
    }
    private void AssignTestimony(Testimony _testimony) //심문 내용 넣기
    {
        description = _testimony.name;
        for (int i = 0; i < _testimony.testimony.infos.Length; i++)
        {
            listSentences[i] = _testimony.testimony.infos[i].sentence;
            listEmotion[i] = _testimony.testimony.infos[i].emotion;
        }
    }
    private void ClearTestimony() //심문 종료, 모든 변수 초기화
    {

        count = 0;
        listSentences.Clear();
        Character.SetBool("Appear", false);
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
                    StopAllCoroutines();
                    //ui.SetBase(); 
                    //확인 대사 후 첫번째부터
                }
                else
                {
                    StopAllCoroutines();
                    StartCoroutine(StartTextCoroutine());
                }
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                //심문...?
                //잠깐! 대사 띄우기
                ui.GetEvent(events[count]);
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (count < listSentences.Count - 1)
                {
                    count++;
                    //다음 대사
                }
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (count > 0)
                {
                    count--;
                    //이전 대사
                }
            }
        }
    }

    public void ShowText(string[] _sentences)
    {
        for (int i = 0; i < _sentences.Length; i++)
        {
            listSentences.Add(_sentences[i]);
        }

        StartCoroutine(StartTextCoroutine());
    }
    IEnumerator StartInterrogationCoroutine()
    {
        keyActivated = false;

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

        text_middle.text = "";

        yield return waitTime;
    }

    IEnumerator StartTextCoroutine()
    {
        keyActivated = true;

        for (int i = 0; i < listSentences[count].Length; i++)
        {
            text.text += listSentences[count][i];
            if (i % 7 == 1)
            {
                ui.PlaySound(entersound);
            }
            yield return waitTime;
        }
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
