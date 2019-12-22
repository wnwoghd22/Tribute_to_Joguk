using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Title : MonoBehaviour, Manager
{
    private UI ui;

    private string clickSound;

    [SerializeField]
    private GameObject __AtFirst;

    [SerializeField]
    private GameObject __Select; //Start Or Exit?
    [SerializeField]
    private GameObject startGame_Panel;
    [SerializeField]
    private GameObject exit_Panel;
    private const bool GAMESTART = true, EXITGAME = false;

    [SerializeField]
    private GameObject __Choice; // New Or Load?
    [SerializeField]
    private GameObject newGame_Panel;
    [SerializeField]
    private GameObject loadGame_Panel;
    private const bool NEWGAME = true, LOADGAME = false;

    private bool result;
    
    [SerializeField]
    private GameObject __Episode;
    [SerializeField]
    private Text episodeText;
    [SerializeField]
    private Sprite[] EpisodeCut;
    private string[] episode_description =
    {
        "제 1 화\n\n역전 조무사",
        "제 2 화\n\n상처뿐인 역전",
        "제 3 화\n\n역전 주식회사",

    };
    private int _index;

    private WaitForSeconds waitTime = new WaitForSeconds(0.01f);

    private SelectedTab tab;
    private enum SelectedTab
    {
        AtFirst,
        Select, //start or exit
        Choice, //new or load
        Episode,
    }

    void Awake()
    {
        ui = FindObjectOfType<UI>();
    }
    void Start()
    {
        ui.StartAsTitle();
    }
    public void Enter(UI _ui)
    {
        ui = _ui;
        SwitchTab(SelectedTab.AtFirst);
    }
    public void Exit(bool _b = true)
    {

    }
    public void HandleInput()
    {
        switch (tab)
        {
            case SelectedTab.AtFirst:
                if (Input.anyKeyDown)
                {
                    SwitchTab(SelectedTab.Select);
                    result = GAMESTART;                    
                    SelectTab();
                }
                break;
            case SelectedTab.Select:
                if (Input.GetKeyDown(KeyCode.DownArrow) | Input.GetKeyDown(KeyCode.UpArrow))
                {
                    result = !result;
                    StopAllCoroutines();
                    SelectTab();
                }              
                else if (Input.GetKeyDown(KeyCode.Z))
                {
                    //ui.PlaySound(clickSound);
                    switch (result)
                    {
                        case GAMESTART:
                            SwitchTab(SelectedTab.Choice);
                            ChoiceTab();
                            break;
                        case EXITGAME:
                            ExitGame();
                            break;
                    }
                }
                else if (Input.GetKeyDown(KeyCode.X))
                {
                    ui.PlaySound(clickSound);
                }
                break;
            case SelectedTab.Choice:
                if (Input.GetKeyDown(KeyCode.DownArrow) | Input.GetKeyDown(KeyCode.UpArrow))
                {
                    result = !result;
                    StopAllCoroutines();
                    ChoiceTab();
                }
                else if (Input.GetKeyDown(KeyCode.Z))
                {
                    ui.PlaySound(clickSound);
                    if (result)
                    {
                        _index = 0;
                        SwitchTab(SelectedTab.Episode);
                        ui.SetCutActive(true);
                        ChooseEpisode(_index);
                    }
                    else
                    {
                        ui.Load();
                        SetPlayerOn();
                    }
                }
                else if (Input.GetKeyDown(KeyCode.X))
                {
                    ui.PlaySound(clickSound);
                    result = NEWGAME;
                    SwitchTab(SelectedTab.Select);
                    SelectTab();
                }
                break;
            case SelectedTab.Episode:
                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    if (_index < 4)
                    {
                        _index++;
                        ChooseEpisode(_index);
                    }
                }
                else if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    if (_index > 0)
                    {
                        _index--;
                        ChooseEpisode(_index);
                    }
                }
                else if (Input.GetKeyDown(KeyCode.Z))
                {
                    StartGame("Episode"+(_index+1));
                }
                else if (Input.GetKeyDown(KeyCode.X))
                {
                    result = NEWGAME;
                    ui.SetCutActive(false);
                    SwitchTab(SelectedTab.Choice);
                    ChoiceTab();
                }
                break;
        }
    }

    private void SwitchTab(SelectedTab _selected)
    {
        tab = _selected;
        __AtFirst.SetActive(tab == SelectedTab.AtFirst);
        __Select.SetActive(tab == SelectedTab.Select);
        __Choice.SetActive(tab == SelectedTab.Choice);
        __Episode.SetActive(tab == SelectedTab.Episode);
    }

    private void StartGame(string _s)
    {
        for (int i = 0; i < DatabaseManager.instance.switches.Length; i++)
            DatabaseManager.instance.switches[i] = false;
        StartCoroutine(GameStartCoroutine(_s));
    }
    IEnumerator GameStartCoroutine(string _map)
    {
        Exit();
        ui.PlaySound(clickSound);
        ui.FadeOut();
        yield return new WaitForSeconds(2f);
        ui.ChangeMap(_map);
    }
    private void ExitGame()
    {
        ui.PlaySound(clickSound);
        Application.Quit();
    }

    private void SelectTab()
    {
        Color color = startGame_Panel.GetComponent<Image>().color;
        color.a = 0f;
        startGame_Panel.GetComponent<Image>().color = color;
        exit_Panel.GetComponent<Image>().color = color;
        StartCoroutine(SelectedTabEffectCoroutine());
    } //탭 선택
    IEnumerator SelectedTabEffectCoroutine()
    {
        while (tab == SelectedTab.Select)
        {
            while(result)
            {
                Color color = startGame_Panel.GetComponent<Image>().color;
                while (color.a < 0.5f)
                {
                    color.a += 0.03f;
                    startGame_Panel.GetComponent<Image>().color = color;
                    yield return waitTime;
                }
                while (color.a > 0f)
                {
                    color.a -= 0.03f;
                    startGame_Panel.GetComponent<Image>().color = color;
                    yield return waitTime;
                }
                yield return waitTime;
            }
            while(!result)
            {
                Color color = exit_Panel.GetComponent<Image>().color;
                while (color.a < 0.5f)
                {
                    color.a += 0.03f;
                    exit_Panel.GetComponent<Image>().color = color;
                    yield return waitTime;
                }
                while (color.a > 0f)
                {
                    color.a -= 0.03f;
                    exit_Panel.GetComponent<Image>().color = color;
                    yield return waitTime;
                }
                yield return waitTime;
            }           
        }
        
    }//선택된 탭 점멸효과

    private void ChoiceTab()
    {
        Color color = newGame_Panel.GetComponent<Image>().color;
        color.a = 0f;
        
        newGame_Panel.GetComponent<Image>().color = color;
        loadGame_Panel.GetComponent<Image>().color = color;
        StartCoroutine(SelectedResultEffectCoroutine());        
    }
    IEnumerator SelectedResultEffectCoroutine()
    {
        while (tab == SelectedTab.Choice)
        {
            while(result)
            {
                Color color = newGame_Panel.GetComponent<Image>().color;
                while (color.a < 0.5f)
                {
                    color.a += 0.03f;
                    newGame_Panel.GetComponent<Image>().color = color;
                    yield return waitTime;
                }
                while (color.a > 0f)
                {
                    color.a -= 0.03f;
                    newGame_Panel.GetComponent<Image>().color = color;
                    yield return waitTime;
                }
                yield return waitTime;
            }
            while(!result)
            {
                Color color = loadGame_Panel.GetComponent<Image>().color;
                while (color.a < 0.5f)
                {
                    color.a += 0.03f;
                    loadGame_Panel.GetComponent<Image>().color = color;
                    yield return waitTime;
                }
                while (color.a > 0f)
                {
                    color.a -= 0.03f;
                    loadGame_Panel.GetComponent<Image>().color = color;
                    yield return waitTime;
                }
                yield return waitTime;
            }           
        }
    } //선택된 탭 점멸효과
    
    private void ChooseEpisode(int _i)
    {
        ui.ChangeCut(EpisodeCut[_i]);
        episodeText.text = episode_description[_i];
    }

    public void SetPlayerOn()
    {

    }
}
