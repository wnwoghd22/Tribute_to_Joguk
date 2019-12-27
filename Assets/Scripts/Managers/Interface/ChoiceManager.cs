using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceManager : MonoBehaviour, Manager
{
    #region singleton

    static public ChoiceManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }        
    }

    #endregion
    #region variables
    private UI ui;
    private BaseManager theB;
    //private AudioManager theAM;
    private string question;
    private List<string> answerList;
    //[SerializeField]
    //private GameObject go; //for GUI control
    [SerializeField]
    private Text question_Text;
    [SerializeField]
    private Text[] answer_Text;
    [SerializeField]
    private GameObject[] answerPanel;

    private Animator anim;

    public string keySound;
    public string enterSound;

    //public bool choicing;
    private bool keyInput; //키 처리 활성화
    private int count;
    public int Result { get; private set; } //선택 결과

    private WaitForSeconds waitTime = new WaitForSeconds(0.01f);
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        //theAM = FindObjectOfType<AudioManager>();
        anim = GetComponent<Animator>();
        answerList = new List<string>();
        for (int i = 0; i <= 3; i++)
        {
            answer_Text[i].text = "";
            answerPanel[i].SetActive(false);
        }
        question_Text.text = "";
    }

    public void ShowChoice(Choice _choice)
    {
        ui.SetCutTrigger(_choice._who);
        ui.SetCharacter(_choice._who);
        ui.SetEmotionTrigger(_choice._emotion);
        question = _choice.sentence;

        Result = 0;
        for (int i = 0; i < _choice.answers.Length; i++)
        {
            answerList.Add(_choice.answers[i]);
            answerPanel[i].SetActive(true);
            count = i;
        }
        anim.SetBool("appear", true);
        StartCoroutine(ChoiceCoroutine());
        Selection();
    }

    IEnumerator ChoiceCoroutine()
    {
        yield return new WaitForSeconds(0.2f);

        StartCoroutine(TypingQuestion());
        for (int i = 0; i <= count; i++)
        {
            yield return waitTime;
            StartCoroutine(TypingAnswer(i));
        }
        yield return new WaitForSeconds(0.5f);

        keyInput = true;
    }
    IEnumerator TypingQuestion()
    {
        for(int i = 0; i < question.Length; i++)
        {
            question_Text.text += question[i];
            yield return waitTime;
        }
    }
    IEnumerator TypingAnswer(int j)
    {
        for (int i = 0; i < answerList[j].Length; i++)
        {
            answer_Text[j].text += answerList[j][i];
            yield return waitTime;
        }
    }
    
    public void Selection()
    {
        Color color = answerPanel[0].GetComponent<Image>().color;
        color.a = 0f;

        for(int i = 0; i <= count; i++)
        {
            answerPanel[i].GetComponent<Image>().color = color;
        }
        color.a = .5f;
        answerPanel[Result].GetComponent<Image>().color = color;
    }

    public void HandleInput()
    {
        if (keyInput)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                ui.PlaySound(keySound);
                if (Result > 0)
                    Result--;
                else
                    Result = count;
                Selection();
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                ui.PlaySound(keySound);
                if (Result < count)
                    Result++;
                else
                    Result = 0;
                Selection();
            }
            else if (Input.GetKeyDown(KeyCode.Z))
            {
                ui.PlaySound(keySound);
                
                ui.SetBase();
                keyInput = false;
            }
        }
    }
    public void Enter(UI _ui)
    {
        ui = _ui;
        for (int i = 0; i <= 3; i++)
        {
            answer_Text[i].text = "";
            answerPanel[i].SetActive(false);
        }
    }
    public void Exit(bool _b = true)
    {
        question_Text.text = "";

        for (int i = 0; i <= count; i++)
        {
            answer_Text[i].text = "";
            //answerPanel[i].SetActive(false);
        }
        anim.SetBool("appear", false);

        answerList.Clear();
    }
}
