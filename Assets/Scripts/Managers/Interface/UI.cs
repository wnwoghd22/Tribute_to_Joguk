using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    #region Singlton
    static public UI instance;

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
    #region Component
    private BaseManager theB;

    private FadeManager theFM;
    private AudioManager theAM;
    private DialogManager theDM;
    private TestimonyManager theTM;
    private ChoiceManager theCM;
    private MapSelectManager theMM;

    private Inventory theIV;

    private BGMManager theBGM;
    private Title title;
    private ImageCutManager theCut;
    #endregion
    private PlayerController Player;
    private Event @event;
    private ChangeMap @map;

    private Manager currentManager;
    public void ChangeManager(Manager _manager)
    {
        if (currentManager != null)
            currentManager.Exit();
        currentManager = _manager;
        currentManager.Enter(this);
    }

    public enum StateStack
    {
        BaseManager,
        Testimony,
    }
    public StateStack stateStack { get; private set; }
    public void SetTestimony(bool _b)
    {
        stateStack = _b ? StateStack.Testimony : StateStack.BaseManager;
    }
    private void SetBase()
    {
        ChangeManager(theB);
    }
    private void BackToTestimony()
    {
        ChangeManager(theTM);
    }
    public void ExitState()
    {
        if (stateStack == StateStack.BaseManager)
            SetBase();
        else if (stateStack == StateStack.Testimony)
            BackToTestimony();
    }

    // Start is called before the first frame update
    void Start()
    {
        Player = FindObjectOfType<PlayerController>();
        theB = FindObjectOfType<BaseManager>();
        theAM = FindObjectOfType<AudioManager>();
        theCM = FindObjectOfType<ChoiceManager>();
        theDM = FindObjectOfType<DialogManager>();
        theTM = FindObjectOfType<TestimonyManager>();

        theFM = FindObjectOfType<FadeManager>();
        theIV = FindObjectOfType<Inventory>();

        theMM = FindObjectOfType<MapSelectManager>();
        theBGM = FindObjectOfType<BGMManager>();
        theCut = FindObjectOfType<ImageCutManager>();
        title = FindObjectOfType<Title>();
        @event = null;
        @map = null;
    }

    // Update is called once per frame
    void Update()
    {      
        currentManager.HandleInput();
    }

    public void SetHUDactive(bool _b)
    {

    }

    public void Move(bool _dir, int _c = 1)
    {
        Player.Move(_dir, _c);
    }

    #region Event
    public bool IsEvent()
    {
        return @event != null;
    }
    public void GetEvent(Event _event)
    {
        @event = _event;
    }
    public void ClearEvent()
    {
        @event = null;
    }   
    public void StartEvent()
    {
        @event.Excute();
    } //player stop, start event
    public void ExitEvent()
    {

    }
    public void StartDialogue(Dialog _log)
    {
        ChangeManager(theDM);
        theDM.ShowDialogue(_log);
    }
    public void StartChoice(Choice _choice)
    {
        ChangeManager(theCM);
        theCM.ShowChoice(_choice);
    }
    public void ChangeCut(Sprite _sprite)
    {
        theCut.ChangeCut(_sprite);
    }
    public void SetCutActive(bool _b)
    {
        theCut.SetCutActive(_b);
    }
    public bool IsExcuting()
    {
        return (object)currentManager != theB;
    }
    public int GetResult()
    {
        return theCM.GetResult();
    }
    #endregion
    #region Map
    public bool IsMap()
    {
        return @map != null;
    }
    public void GetMap(ChangeMap _map)
    {
        @map = _map;
    }
    public void ClearMap()
    {
        @map = null;
    }
    public void StartChangeMap()
    {
        @map.Excute();
    } //player stop, start event
    public void ExitChangeMap()
    {
    }
    public void StartMapSelect()
    {
        ChangeManager(theMM);
    }
    public void SetPlayerSceneName(string _name)
    {
        Player.SetSceneName(_name);
    }
    public string GetPlayerSceneName()
    {
        return Player.GetSceneName();
    }
    public string GetSelection()
    {
        return theMM.GetResult();
    }
    #endregion

    #region AudioManager
    public void PlaySound(string _name)
    {
        theAM.Play(_name);
    }
    #endregion
    #region BGM
    public void PlayBGM(int i)
    {
        theBGM.Play(i);
    }
    public void StopBGM()
    {
        theBGM.Stop();
    }
    #endregion
    #region FadeManager
    public void FadeIn(float _speed = 0.02f)
    {
        theFM.FadeIn(_speed);
    }
    public void FadeOut(float _speed = 0.02f)
    {
        theFM.FadeOut(_speed);
    }
    public void Flash(float _speed = 0.1f)
    {
        theFM.Flash(_speed);
    }
    public void FlashIn(float _speed = 0.02f)
    {
        theFM.FlashIn(_speed);
    }
    public void FlashOut(float _speed = 0.02f)
    {
        theFM.FlashOut(_speed);
    }
    #endregion
    #region Inventory
    public void GoToInventory()
    {
        ChangeManager(theIV);
    }
    public void ReturnItem(Item _item)
    {
        theIV.ReturnItem(_item);
    }
    public void GetItem(int _itemID, int _c = 1)
    {
        theIV.GetItem(_itemID, _c);
    }
    #endregion

    #region Testimony
    private int testimony_count;
    public void ReturnToTestimony(int _count)
    {
        
    }

    #endregion

    #region Title
    public void StartAsTitle()
    {
        ChangeManager(title);
    }
    #endregion
    #region SaveLoad
    public void Save()
    {

    }
    public void Load()
    {

    }
    #endregion
}
