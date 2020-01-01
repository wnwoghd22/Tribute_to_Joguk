using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour, Manager
{    
    [SerializeField]
    private Text text;
    [SerializeField]
    private Text whoIs;

    [SerializeField]
    private Text text_only;

    private List<string> listName;
    private List<string> listSentences;
    private List<emotion> listEmotion;
    private List<who> listWho;   
    private List<effect> listEffect;

    private List<Dialog> listLog;

    private int count; //대화의 길이, 진행상황

    [SerializeField]
    private Animator Dialog;
    [SerializeField]
    private Animator Text_only;
    private WaitForSeconds waitLog = new WaitForSeconds(0.01f);
    private WaitForSeconds waitType = new WaitForSeconds(.1f);
    private WaitForSeconds waitEffect = new WaitForSeconds(0.5f);
    
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
        listEffect = new List<effect>();
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
        count = 0;

        for ( int i=0; i < dialogue.sentence.Length; i++)
        {
            listSentences.Add(dialogue.sentence[i]);
            listName.Add(dialogue._name[i]);
            listEmotion.Add(dialogue._emotion[i]);
            listWho.Add(dialogue._who[i]);
            listEffect.Add(dialogue._effect[i]);
        }
        Dialog.SetBool("Appear", true);        
        StartCoroutine(StartDialogueCoroutine());
    }

    IEnumerator StartDialogueCoroutine()
    {
        if(listEffect[count] != effect.None) 
        {
            ui.Effect(listEffect[count]);
            yield return waitEffect;
        }       //effect 추가...enum effect 정의 후 만들기. 흔들리는 효과 등등
        ui.SetCutTrigger(listWho[count]);
        ui.SetCharacter(listWho[count]);
        ui.SetEmotionTrigger(listEmotion[count]);
        whoIs.text += listName[count];
        keyActivated = true;

        for (int i=0; i < listSentences[count].Length; i++)
        {
            text.text += listSentences[count][i];
            if(i % 7 == 1)
            {
                ui.SetEmotionTrigger("talk");
                ui.PlaySound(entersound);
            }
            yield return waitLog;
        }
    }
    IEnumerator StartTextCoroutine()
    {
        keyActivated = true;

        for (int i = 0; i < listSentences[count].Length; i++)
        {
            text_only.text += listSentences[count][i];          
            ui.PlaySound(typesound);           
            yield return waitType;
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
            listEffect.Clear();
            Dialog.SetBool("Appear", false);
            Text_only.SetBool("Appear", false);
            //ui.SetPlayerMove(true);
        }
    }
}
