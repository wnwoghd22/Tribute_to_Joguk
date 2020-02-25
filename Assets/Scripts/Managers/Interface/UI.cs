using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    #region Singlton
    static public UI instance;

    private void Awake()
    {
        Player = FindObjectOfType<PlayerController>();
        theB = FindObjectOfType<BaseManager>();
        theAM = FindObjectOfType<AudioManager>();
        theCM = FindObjectOfType<ChoiceManager>();
        theDM = FindObjectOfType<DialogManager>();
        theTM = FindObjectOfType<TestimonyManager>();
        theIM = FindObjectOfType<InvestigationManager>();
        theMenu = FindObjectOfType<Menu>();

        theFM = FindObjectOfType<FadeManager>();
        theIV = FindObjectOfType<Inventory>();

        theMM = FindObjectOfType<MapSelectManager>();
        theBGM = FindObjectOfType<BGMManager>();
        theCut = FindObjectOfType<ImageCutManager>();
        theGM = FindObjectOfType<GameManager>();
        title = FindObjectOfType<Title>();

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
    //interface
    private Title title;
    private BaseManager theB;
    private DialogManager theDM;
    private TestimonyManager theTM;
    private ChoiceManager theCM;
    private MapSelectManager theMM;
    private InvestigationManager theIM;
    private Inventory theIV;
    private Menu theMenu;
    //sub
    private FadeManager theFM;
    private AudioManager theAM;
    private BGMManager theBGM;  
    private ImageCutManager theCut;
    private GameManager theGM;
    [SerializeField]
    private GameObject mainCamera;
    [SerializeField]
    private GameObject eventSystem;
    #endregion
    private PlayerController Player;
    private Event @event;
    private List<Map> @map;

    private Dictionary<who, string> WhoDict
        = new Dictionary<who, string>
        {
            { who.None, "Off" }, //Off는 외부에서만 호출 가능하게 한다.
            { who.Attorney, "Attorney" },
            { who.Prosecutor, "Prosecutor" },
            { who.Judge, "Judge" },
            { who.Witness, "Witness" },
            { who.Company, "Company" },
            { who.Talk, "Talk" },
            { who.Court, "Court" }
        };

    private Manager currentManager;
    private void ChangeManager(Manager _manager, bool _b = true) //true - 이벤트를 완전히 종료, false - 상태만 전이
    {
        if (currentManager != null)
            currentManager.Exit(_b);
        currentManager = _manager;
        currentManager.Enter(this);
    }
    private Stack<Manager> _stack;
    public void PopState() => ChangeManager(_stack.Pop());
    public void SetBase() => ChangeManager(theB);

    // Start is called before the first frame update
    void Start()
    {
        _stack = new Stack<Manager>();
        @map = new List<Map>();
        @event = null;
    }

    // Update is called once per frame
    void Update()
    {      
        currentManager.HandleInput();
    }

    public void SetHUDactive(bool _b) //체력 표시. 추후 제작
    {

    }

    public void Move(bool _dir, int _c = 1)
    {
        Player.Move(_dir, _c); //필요한가?
    }
    public void Effect(effect _e = effect.None) //패러미터를 두 개 받는 걸로 할까
    {
        switch (_e)
        {
            case effect.HoldIt:
                Player.SetEffectTrigger("HoldIt!");
                break;
            case effect.Objection:
                Player.SetEffectTrigger("Objection!");
                break;
            case effect.TakeThat:
                Player.SetEffectTrigger("TakeThat!");
                break;
            case effect.Quake:
                theCut.SetTrigger("Quake");
                break;
            default:
                break;
        }
    }
    public void SetCharacter(who _c = who.None) => Player.SetCharacterActive(_c);
    public void SetEmotionTrigger(emotion _e = emotion.normal) => Player.SetEmotionTrigger(_e);
    public void SetEmotionTrigger(string _parameter) => Player.SetEmotionTrigger(_parameter);
    public void ClearAll()
    {
        SetCharacter(who.None);
        SetCutTrigger("Off");
        SetCutActive(false);
    }
    #region Event
    public bool IsEvent => @event != null;
    public void GetEvent(Event _event) => @event = _event;
    public void ClearEvent() => @event = null;
    public void StartEvent()
    {
        SetCharacter();
        @event.Excute();
    }//player stop, start event
    public void ExitEvent(bool _b = true) //false - stack
    {
        ClearEvent();
        if (_b == true)
            SetBase();
        else
            PopState();
    }
    public void StartDialogue(Dialog _log, bool exit = true) //exit = true - 이벤트를 완전히 종료
    {
        ChangeManager(theDM, exit);
        theDM.ShowDialogue(_log);
    }
    public void StartText(string _log, bool exit = true)
    {
        ChangeManager(theDM, exit);
        theDM.ShowText(_log);
    }
    public void StartChoice(Choice _choice, bool exit = true)
    {
        ChangeManager(theCM, exit);
        theCM.ShowChoice(_choice);
    }
    public void StartTestimony(Testimony _testimony)
    {
        ChangeManager(theTM);
        theTM.StartTestimony(_testimony);
    }
    public void SetCutTrigger(string _s)
    {
        theCut.ResetTrigger();
        theCut.SetTrigger(_s);
    }
    public void SetCutTrigger(who _who)
    {
        theCut.ResetTrigger();
        if (_who != who.None)
            theCut.SetTrigger(WhoDict[_who]);
    }
    public void ChangeCut(Sprite _sprite)
    {
        theCut.ChangeCut(_sprite);
    }
    
    public void SetCutActive(bool _b)
    {
        theCut.SetCutActive(_b);
    }
    public bool IsExcuting => (object)currentManager != theB;
    public int Result => theCM.Result;
    #endregion
    #region Map
    public bool IsMap => @map != null;
    public void SetMap(Map[] _map) => @map.AddRange(_map);
    public List<Map> GetMap() => map;
    public void ClearMap() => @map.Clear();
    public void ChangeMap(string _name)
    {
        FadeOut();
        PlayerSceneName = _name;

        theGM.LoadStart();

        SceneManager.LoadScene(PlayerSceneName);
    }
    public void StartMapSelect()
    {
        _stack.Push(currentManager);
        ChangeManager(theMM);
    }
    public string PlayerSceneName { get => Player.SceneName; private set => Player.SceneName = value; }
    #endregion
    #region Inventory
    public void GoToInventory(ReturnType type = ReturnType.None)
    {
        _stack.Push(currentManager);
        theIV.SetType(type);
        ChangeManager(theIV, false); //상태만 전이
    }
    public void Adduce(Choice _c,ReturnType type = ReturnType.Both)
    {
        //_stack.Push(currentManager);
        theIV.SetType(type);
        ChangeManager(theIV);
        theIV.Adduce(_c);
    }
    public int ReturnValue => theIV.ReturnValue;
    public void AddItem(Item _item) => theIV.AddItem(_item); //아이템 추가
    public void GetItem(int _itemID) => theIV.GetItem(_itemID);
    #endregion
    #region Testimony
    public void CallTestimony(int _c)
    {
        ChangeManager(theTM);
        theTM.ShowText(_c);
    }
    public void CallObjection(int _result)
    {
        ChangeManager(theTM);
        theTM.SetObjection(_result);
    }
    public void ChangeWitness(RuntimeAnimatorController _a)
    {
        Player.ChangeWitness(_a);
    }
    #endregion
    #region Menu
    public void ActivateMenu() => ChangeManager(theMenu);
    #endregion
    #region investigation
    public void Investigate(bool _abs = false) //false = 조사 장면에서 빠져나오기 가능
    {
        _stack.Push(currentManager);
        theIM.SetAbs(_abs);
        ChangeManager(theIM);
    }
    public void Search()
    {
        _stack.Push(currentManager);
        StartEvent();
    }
    #endregion
    #region Title
    public void StartAsTitle() => ChangeManager(title);
    public void BackToTitle()
    {
        ClearAll();
        Destroy(Player);
        Destroy(mainCamera);
        Destroy(eventSystem);
        Destroy(this.gameObject);
        SceneManager.LoadScene("Title");
    }
    #endregion

    #region AudioManager
    public void PlaySound(string _name) => theAM.Play(_name);
    #endregion
    #region BGM
    public void PlayBGM(int i) => theBGM.Play(i);
    public void StopBGM() => theBGM.Stop();
    #endregion
    #region FadeManager
    public void FadeIn(float _speed = 0.02f) => theFM.FadeIn(_speed);
    public void FadeOut(float _speed = 0.02f) => theFM.FadeOut(_speed);
    public void Flash(float _speed = 0.1f) => theFM.Flash(_speed);
    public void FlashIn(float _speed = 0.02f) => theFM.FlashIn(_speed);
    public void FlashOut(float _speed = 0.02f) => theFM.FlashOut(_speed);
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
