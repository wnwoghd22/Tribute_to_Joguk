using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(BoxCollider2D))]
public abstract class ChangeMap : MonoBehaviour
{
    [SerializeField]
    protected string transferMapName;
    [SerializeField]
    protected string transferSceneName;
    private string currentSceneName;
    private UI ChangeHandler;

    [SerializeField]
    private Animator doorAnim;

    protected bool IsActive = false; //상호작용 객체인가?(true: press C then activate, false: activate automatically)
    protected bool door = false; //문이 있는가?
    protected void SetValue(bool isActive, bool isDoor)
    {
        IsActive = isActive;
        door = isDoor;
    }

    // Start is called before the first frame update
    void Start()
    {
        ChangeHandler = FindObjectOfType<UI>();
        currentSceneName = gameObject.scene.name;
    }
    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")            
        {
            ChangeHandler.GetMap(this);
            if (!IsActive)
                ChangeHandler.StartChangeMap();
        }       
    }  
    protected void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            ChangeHandler.ClearMap();
        }
    }
    protected IEnumerator TransferCoroutine()
    {        
        if (door)
        {
            doorAnim.SetBool("Open", true);            
        }
        ChangeHandler.FadeOut();
        yield return new WaitForSeconds(.5f);

        if (door)
        {
            doorAnim.SetBool("Open", false);           
        }
        yield return new WaitForSeconds(.5f);
        //ChangeHandler.PlayerSceneName = transferSceneName;
        SceneManager.LoadScene(transferSceneName);
        ChangeHandler.FadeIn();
        yield return new WaitForSeconds(0.5f);               
        
    }

    protected abstract IEnumerator ChangeCoroutine();
    
    protected void StartSelection()
    {        
        ChangeHandler.StartMapSelect();
    }
    protected string GetResult()
    {
        return ChangeHandler.GetSelection();
    }  
    protected bool IsTransfer()
    {
        return transferSceneName != currentSceneName;
    }
    public void Excute()
    {
        StartCoroutine(ChangeCoroutine());
    }
    protected bool IsExcuting()
    {
        return ChangeHandler.IsExcuting;
    }
    protected void ExitEvent()
    {
    }
}
