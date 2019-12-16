using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MapSelectManager : MonoBehaviour, Manager
{
    private UI ui;

    private string[] mapList;
    [SerializeField]
    private GameObject[] mapShape;

    private int count;
    private int result;
    private int current;

    public Text text;

    public Animator map;

    //public bool choicing;
    private bool keyInput;

    #region Singlton

    public static MapSelectManager instance;

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
    #endregion Singlton

    // Start is called before the first frame update
    void Start()
    {
        ui = FindObjectOfType<UI>();

        //text.text = "";
        //for ( int i = 0; i < mapList.Length; i++)
        //{
        //    mapShape[i].SetActive(false);
        //}
        //keyInput = false;
    }

    void Update()
    {
        //Debug.Log(mapList[result]);
    }

    public void ShowChoice()
    {
        //choicing = true;
        result = 0;
        for (int i = 0; i < mapList.Length; i++)
        {
            if (ui.GetPlayerSceneName() == mapList[i])
            {
                result = i;
                current = i;
            }               
            count = i;
            mapShape[i].SetActive(true);
            text.text = mapList[result];
        }

        map.SetBool("Appear", true);
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
        Color color = mapShape[0].GetComponent<Image>().color;
        color.a = 0.5f;
        for (int i = 0; i <= count; i++)
        {
            mapShape[i].GetComponent<Image>().color = color;
        }
        color.a = 1f;
        mapShape[result].GetComponent<Image>().color = color;
    }
    public string GetResult()
    {
        return mapList[result];
    }
    
    public void HandleInput()
    {
        if (keyInput)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (result > 0)
                    result--;
                else
                    result = count;
                text.text = mapList[result];
                Selection();
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (result < count)
                    result++;
                else
                    result = 0;
                text.text = mapList[result];
                Selection();
            }
            else if (Input.GetKeyDown(KeyCode.Z))
            {
                ui.SetBase();
            }
            else if (Input.GetKeyDown(KeyCode.C))
            {
                result = current;
                ui.SetBase();
            }
        }
    }
    public void Enter(UI _ui)
    {
        ui = _ui;

        ShowChoice();
    }
    public void Exit(bool _b = true)
    {
        count = 0;
        //result = 0;
        map.SetBool("Appear", false);
        for (int i = 0; i < mapList.Length; i++)
        {
            mapShape[i].SetActive(false);
        }
        keyInput = false;
        //choicing = false;
        StopAllCoroutines();
    }
}
