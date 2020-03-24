using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MapSelectManager : MonoBehaviour, Manager
{
    private UI ui;

    [SerializeField]
    private GameObject[] buttons;
    [SerializeField]
    private Text[] mapName;
    [SerializeField]
    private Image mapImage;
    private List<Map> mapList;

    private int count;
    public string result { get; private set; }

    private Animator myAnimator;

    //public bool choicing;
    private bool keyInput;

    public void Enter(UI _ui)
    {
        ui = _ui;
        mapList.AddRange(ui.GetMap());

        ShowChoice();
    }
    public void Exit(bool _b = true)
    {
        count = 0;
        for (int i = 0; i < 4; i++)
        {
            buttons[i].SetActive(false);
            mapName[i].text = "";
        }
        mapList.Clear();
        result = "";
        myAnimator.SetTrigger("disappear");
        keyInput = false;
        //choicing = false;
        StopAllCoroutines();
    }
    public void HandleInput()
    {
        if (keyInput)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (count > 0)
                    count--;
                else
                    count = mapList.Count - 1;
                Selection();
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (count < mapList.Count - 1)
                    count++;
                else
                    count = 0;
                Selection();
            }
            else if (Input.GetKeyDown(KeyCode.X))
            {
                ui.PopState();
            }
            else if (Input.GetKeyDown(KeyCode.Z))
            {
                result = mapList[count].sceneName;
                ui.ChangeMap(result);

                ui.SetBase();
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        ui = FindObjectOfType<UI>();
        myAnimator = GetComponent<Animator>();
        mapList = new List<Map>();
    }

    public void ShowChoice()
    {
        //choicing = true;
        count = 0;
        for (int i = 0; i < mapList.Count; i++)
        {  
            buttons[i].SetActive(true);
            mapName[i].text = mapList[i].mapName;
        }
        myAnimator.SetTrigger("appear");
        StartCoroutine(ChoiceCoroutine());
        Selection();       
    }
    
    IEnumerator ChoiceCoroutine()
    {
        yield return new WaitForSeconds(0.2f);

        keyInput = true;
    }
    
    public void Selection()
    {
        Color color = buttons[0].GetComponent<SpriteRenderer>().color;
        color.a = 0.5f;
        for (int i = 0; i <= mapList.Count; i++)
        {
            buttons[i].GetComponent<SpriteRenderer>().color = color;
        }
        color.a = 1f;
        buttons[count].GetComponent<SpriteRenderer>().color = color;
        mapImage.sprite = mapList[count].mapImage;
    }
}
