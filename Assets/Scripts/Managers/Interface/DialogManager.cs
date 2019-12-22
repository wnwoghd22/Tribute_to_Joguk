using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour, Manager
{
    #region Singleton

    public static DialogManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    #endregion Singleton

    [SerializeField]
    private Text text;
    [SerializeField]
    private Text whoIs;

    [SerializeField]
    private Text text_only;

    private List<string> listSentences;
    private List<emotion> listEmotion;
    private List<string> listName;
    private List<who> listWho;
    private Dictionary<who, string> WhoDict
        = new Dictionary<who, string>
        {
            { who.None, "Off" },
            { who.Attorney, "Attorney" },
            { who.Prosecutor, "Prosecutor" },
            { who.Judge, "Judge" },
            { who.Witness, "Witness" },
            { who.Company, "Company" },
            { who.Talk, "Talk" }
        };

    private int count; //대화의 길이, 진행상황

    [SerializeField]
    private Animator Dialog;
    [SerializeField]
    private Animator Text_only;
    
    public string typesound;
    public string entersound;

    //private AudioManager theAM;
    //public bool talking;
    private bool keyActivated = false;
    private bool onlyText = false;
    private UI ui;

    // Start is called before the first frame update
    void Start()
    {
        count = 0;
        text.text = "";
        text_only.text = "";
        whoIs.text = "";
        listSentences = new List<string>();
        listEmotion = new List<emotion>();
        listWho = new List<who>();
        listName = new List<string>();
    }

    public void ShowText(string _sentence)
    {
        onlyText = true;
        listSentences.Add(_sentence);
        Text_only.SetBool("Appear", true);
        StartCoroutine(StartTextCoroutine());
    }
    public void ShowDialogue(Dialog dialogue)
    {
        onlyText = false;

        for ( int i=0; i < dialogue.sentence.Length; i++)
        {
            listSentences.Add(dialogue.sentence[i]);
            listName.Add(dialogue._name[i]);
            listEmotion.Add(dialogue._emotion[i]);
            listWho.Add(dialogue._who[i]);
        }
        Dialog.SetBool("Appear", true);        
        StartCoroutine(StartDialogueCoroutine());
    }

    IEnumerator StartDialogueCoroutine()
    {
        /*
        if (count > 0)
        {
            if (listName[count] != listName[count - 1])
            {
                whoIs.text = "";

                //Character.SetBool("Change", true);
                Dialog.SetBool("Appear", false);
                yield return new WaitForSeconds(0.1f);
                ui.SetCutTrigger(WhoDict[listWho[count]]);
                ui.SetCharacter(listWho[count]);
                ui.SetEmotionTrigger(listEmotion[count]);
                Dialog.SetBool("Appear", true);
                whoIs.text += listName[count];

                //Character.SetBool("Change", false);
            }
            else
            {
                if (listEmotion[count] != listEmotion[count - 1])
                {
                    whoIs.text += listName[count];
                    //Character.SetBool("Change", true);
                    yield return new WaitForSeconds(0.1f);

                    //rendererSprite.sprite = listEmotion[count];
                    ui.SetEmotionTrigger(listEmotion[count]);
                    //Character.SetBool("Change", false);
                }
                else
                {
                    whoIs.text += listName[count];
                    yield return new WaitForSeconds(0.05f);
                }
            }
        }
        else
        {

            ui.SetCutTrigger(WhoDict[listWho[count]]);
            ui.SetCharacter(listWho[count]);
            ui.SetEmotionTrigger(listEmotion[count]);
            whoIs.text += listName[count];
            //rendererSprite.sprite = listSprite[count];
        }*/
        ui.SetCutTrigger(WhoDict[listWho[count]]);
        ui.SetCharacter(listWho[count]);
        ui.SetEmotionTrigger(listEmotion[count]);
        whoIs.text += listName[count];
        keyActivated = true;

        //effect 추가...enum effect 정의 후 만들기. 흔들리는 효과 등등
        for (int i=0; i < listSentences[count].Length; i++)
        {
            text.text += listSentences[count][i];
            if(i % 7 == 1)
            {
                ui.PlaySound(entersound);
            }
            yield return new WaitForSeconds(0.01f);
        }
    }
    IEnumerator StartTextCoroutine()
    {
        keyActivated = true;

        for (int i = 0; i < listSentences[count].Length; i++)
        {
            text_only.text += listSentences[count][i];
            if (i % 7 == 1)
            {
                ui.PlaySound(entersound);
            }
            yield return new WaitForSeconds(0.01f);
        }
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
                whoIs.text = "";
                ui.PlaySound(typesound);

                if (count == listSentences.Count)
                {
                    StopAllCoroutines();
                    ui.SetBase();
                }
                else
                {
                    StopAllCoroutines();
                    if (onlyText)
                        StartCoroutine(StartTextCoroutine());
                    else
                        StartCoroutine(StartDialogueCoroutine());
                }
            }
            if(Input.GetKeyDown(KeyCode.W))
            {
                ui.GoToInventory();
            }
        }
    }
    public void Enter(UI _ui)
    {
        ui = _ui;
    }
    public void Exit(bool _b) //대화 종료, 모든 변수 초기화
    {
        if(_b)
        {
            text.text = "";
            whoIs.text = "";
            count = 0;
            listSentences.Clear();
            listName.Clear();
            listWho.Clear();
            listEmotion.Clear();
            Dialog.SetBool("Appear", false);
            Text_only.SetBool("Appear", false);
            //ui.SetPlayerMove(true);
        }
    }
}
