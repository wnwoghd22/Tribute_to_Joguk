﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Testimony
{
    public string name;
    public Dialog testimony;
    public int logCount;
    public int itemID;
}
public class TestimonyManager : MonoBehaviour, Manager
{
    private bool keyActivated = false;
    private int count = 0;
    public int Count { get => count; }

    [SerializeField]
    private Text text;
    [SerializeField]
    private Text text_middle;
    public SpriteRenderer rendererSprite;

    private List<string> listSentences;
    private List<Dialog.emotion> listEmotion;
    private int logNum; //정답 조회용
    private int ItemID; //정답 조회용
        
    public Animator Character;
    public Animator Dialog;

    public string typesound;
    public string entersound;
    
    private bool onlyText = false;
    private UI ui;

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
                    if (onlyText)
                        StartCoroutine(StartTextCoroutine());
                    else
                        StartCoroutine(StartDialogueCoroutine());
                }                   
            }  
            if (Input.GetKeyDown(KeyCode.Q))
            {
                
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (count < listSentences.Count - 1)
                {
                    count++;
                    //다음 대사
                }
            }
            if(Input.GetKeyDown(KeyCode.LeftArrow))
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

        for (int i = 0; i < dialogue.infos.Length; i++)
        {
            listSentences.Add(dialogue.infos[i].sentence);
        }
        Character.SetBool("Appear", true);
        Dialog.SetBool("Appear", true);

        StartCoroutine(StartDialogueCoroutine());

    }

    IEnumerator StartDialogueCoroutine()
    {
        if (count > 0)
        {
            if (listEmotion[count] != listEmotion[count - 1])
            {
                Character.SetBool("Change", true);
                yield return new WaitForSeconds(0.1f);

                //rendererSprite.sprite = listEmotion[count];
                Character.SetBool("Change", false);
            }
        }

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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
