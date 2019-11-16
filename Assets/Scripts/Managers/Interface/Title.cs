using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Title : MonoBehaviour, Manager
{
    private UI ui;

    private string clickSound;

    private GameManager theGM;
    [SerializeField]
    private Text Description;
    
    [SerializeField]
    private GameObject[] gamePanels;

    private const int GAMESTART = 0, EXITGAME = 1; //추후 메뉴 추가 예정.
    private int selectedMenu;
    [SerializeField]
    private GameObject SelectPanel; // New Or Load?
    [SerializeField]
    private GameObject newGame_Panel;
    [SerializeField]
    private GameObject loadGame_Panel;
    private bool result; //New game? or Load Game?

    private WaitForSeconds waitTime = new WaitForSeconds(0.01f);

    private SelectedTab tab;
    private enum SelectedTab
    {
        AtFirst,
        MainMenu,
        StartGame,
    }

    private void Awake()
    {
        ui = FindObjectOfType<UI>();
        theGM = FindObjectOfType<GameManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        ui.StartAsTitle();
        ui.PlayBGM(2);
    }

    public void SetPlayerOn()
    {

    }
    public void StartGame()
    {
        for (int i = 0; i < DatabaseManager.instance.switches.Length; i++)
            DatabaseManager.instance.switches[i] = false;
        StartCoroutine(GameStartCoroutine());
    }
    IEnumerator GameStartCoroutine()
    {
        Exit();
        ui.FadeOut();
        ui.PlaySound(clickSound);
        yield return new WaitForSeconds(2f);
      
        ui.SetPlayerSceneName("Rooms");

        theGM.LoadStart();
        SetPlayerOn();
    }
    public void ExitGame()
    {
        ui.PlaySound(clickSound);
        Application.Quit();
    }

    public void HandleInput()
    {
        switch (tab)
        {
            case SelectedTab.AtFirst:
                if (Input.anyKeyDown)
                {
                    for (int i = 0; i < gamePanels.Length; i++)
                    {
                        gamePanels[i].SetActive(true);
                    }
                    SelectPanel.SetActive(false);
                    selectedMenu = GAMESTART;
                    
                    tab = SelectedTab.MainMenu;
                    SelectTab();
                }                  
                break;
            case SelectedTab.MainMenu:
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    if (selectedMenu < gamePanels.Length - 1)
                        selectedMenu++;
                    else
                        selectedMenu = 0;
                    StopAllCoroutines();
                    SelectTab();
                }
                else if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    if (selectedMenu > 0)
                        selectedMenu--;
                    else
                        selectedMenu = gamePanels.Length - 1;
                    StopAllCoroutines();
                    SelectTab();
                }
                else if (Input.GetKeyDown(KeyCode.Z))
                {
                    ui.PlaySound(clickSound);
                    switch(selectedMenu)
                    {
                        case GAMESTART:
                            tab = SelectedTab.StartGame;
                            SelectPanel.SetActive(true);
                            ShowChoice();
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
            case SelectedTab.StartGame:
                if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.UpArrow))
                {
                    result = !result;
                    StopAllCoroutines();
                    Selected();
                }                
                else if (Input.GetKeyDown(KeyCode.Z))
                {
                    ui.PlaySound(clickSound);
                    ui.SetBase();
                    if (result)
                        StartGame();
                    else
                    {
                        ui.Load();
                        SetPlayerOn();
                    }                        
                }
                else if (Input.GetKeyDown(KeyCode.X))
                {
                    ui.PlaySound(clickSound);
                    SelectPanel.SetActive(false);
                    tab = SelectedTab.MainMenu;
                    SelectTab();
                }
                break;
        }
    }

    private void SelectTab()
    {
        Color color = gamePanels[0].GetComponent<Image>().color;
        color.a = 0f;
        for (int i = 0; i < gamePanels.Length; i++)
        {
            gamePanels[i].GetComponent<Image>().color = color;
        }
        switch (selectedMenu)
        {
            case GAMESTART:
                Description.text = "유희를 시작합니다.(Z)";
                break;
            case EXITGAME:
                Description.text = "유희를 종료합니다.";
                break;
        }
        StartCoroutine(SelectedTabEffectCoroutine());
    }
    IEnumerator SelectedTabEffectCoroutine()
    {
        while (tab == SelectedTab.MainMenu)
        {
            Color color = gamePanels[selectedMenu].GetComponent<Image>().color;
            while (color.a < 0.5f)
            {
                color.a += 0.03f;
                gamePanels[selectedMenu].GetComponent<Image>().color = color;
                yield return waitTime;
            }
            while (color.a > 0f)
            {
                color.a -= 0.03f;
                gamePanels[selectedMenu].GetComponent<Image>().color = color;
                yield return waitTime;
            }
            yield return waitTime;
        }
    }//선택된 탭 점멸효과

    public void Selected()
    {
        Color color = newGame_Panel.GetComponent<Image>().color;
        color.a = 0f;
        if (result)
        {
            newGame_Panel.GetComponent<Image>().color = color;
            loadGame_Panel.GetComponent<Image>().color = color;
            StartCoroutine(SelectedResultEffectCoroutine());
            Description.text = "새로운 여정을 시작합니다.";
        }
        else
        {
            newGame_Panel.GetComponent<Image>().color = color;
            loadGame_Panel.GetComponent<Image>().color = color;
            StartCoroutine(SelectedResultEffectCoroutine());
            Description.text = "이전에 저장한 유희를 불러옵니다.";
        }
    }
    public IEnumerator SelectedResultEffectCoroutine()
    {
        while (tab == SelectedTab.StartGame)
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
    } //선택된 결과 점멸효과
    public void ShowChoice()
    {
        result = true;

        Selected();

        StartCoroutine(ShowChoiceCoroutine());

        tab = SelectedTab.StartGame;
    } //start question new or load?
    IEnumerator ShowChoiceCoroutine()
    {
        yield return new WaitForSeconds(0.01f);
    } 
   
    public void Enter(UI _ui)
    {
        ui = _ui;

        for (int i = 0; i < gamePanels.Length; i++)
        {
            gamePanels[i].SetActive(false);
        }
        Description.text = "아무 키나 누르십시오.";
    }
    public void Exit()
    {
        
    }
}
