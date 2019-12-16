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

    public Text text;
    public SpriteRenderer rendererSprite;
    public Text whoIs;

    private List<string> listSentences;
    private List<Dialog.emotion> listEmotion;
    private List<string> listName;

    private int count; //대화의 길이, 진행상황

    public Animator Character;
    public Animator Dialog;

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
        whoIs.text = "";
        listSentences = new List<string>();
        listEmotion = new List<Dialog.emotion>();
        listName = new List<string>();
    }

    public void ShowText(string[] _sentences)
    {
        onlyText = true;

        for (int i = 0; i < _sentences.Length; i++)
        {
            listSentences.Add(_sentences[i]);
        }

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
        }
        //Character.SetBool("Appear", true);
        Dialog.SetBool("Appear", true);
        
        StartCoroutine(StartDialogueCoroutine());

    }

    IEnumerator StartDialogueCoroutine()
    {
        if (count > 0)
        {
            if (listName[count] != listName[count - 1])
            {
                whoIs.text = "";

                //Character.SetBool("Change", true);
                Dialog.SetBool("Appear", false);
                yield return new WaitForSeconds(0.1f);

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
            whoIs.text += listName[count];
            //rendererSprite.sprite = listSprite[count];
        }

        keyActivated = true;

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
            text.text += listSentences[count][i];
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
        }
    }
    public void Enter(UI _ui)
    {
        ui = _ui;
    }
    public void Exit(bool _b) //대화 종료, 모든 변수 초기화
    {
        text.text = "";
        whoIs.text = "";
        count = 0;
        listSentences.Clear();
        //listSprite.Clear();
        listName.Clear();
        //Character.SetBool("Appear", false);
        Dialog.SetBool("Appear", false);
        //ui.SetPlayerMove(true);
    }
}
