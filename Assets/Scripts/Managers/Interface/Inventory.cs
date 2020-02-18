using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour, Manager
{
    //public static Inventory instance;

    #region Variables
    private DatabaseManager theDB;
    private UI ui;

    public string key_sound;
    public string enter_sound;
    public string cancel_sound;
    public string open_sound;
    public string beep_sound;

    private List<Item> inventoryItemList; //플레이어가 모은 증거, 인물정보
    private List<Item> inventoryTabList; //활성화시 분류

    [SerializeField]
    private Text name_text;

    //탭
    [SerializeField]
    private Transform tf_tab;
    private InventorySlot[] slots; //item slots

    //상세
    [SerializeField]
    private Transform tf_item;
    [SerializeField]
    private Text description_Text;
    [SerializeField]
    private Image item_image;
    
    private Animator myAnimator;    
    [SerializeField]
    private GameObject detail; //선택지 활성화...또는 증거 상세

    private enum SelectedTab
    {
        Clue,
        Person,
    }
    private SelectedTab selectedTab;
    private int Page
    {
        get
        {
            if (selectedTab == SelectedTab.Clue)
                return page_clue;
            else
                return page_person;
        }
        set
        {
            if (selectedTab == SelectedTab.Clue)
                page_clue = value;
            else
                page_person = value;
        }
    }//페이지
    private int MaxPage => (inventoryTabList.Count - 1) / MAX_SLOTS_COUNT;
    private void NextPage()
    {
        if (MaxPage == 0)
            return;
        else if (Page == MaxPage)
            Page = 0;
        else
            Page++;
    }
    private void PreviousPage()
    {
        if (MaxPage == 0)
            return;
        else if (Page == 0)
            Page = MaxPage;
        else
            Page--;
    }
    private int page_clue;
    private int page_person;
    private int selectedItem
    {
        get
        {
            if (selectedTab == SelectedTab.Clue)
                return selectedClue;
            else
                return selectedPerson;
        }
        set
        {
            if (selectedTab == SelectedTab.Clue)
                selectedClue = value;
            else
                selectedPerson = value;
        }
    }
    private int selectedClue;
    private int selectedPerson;
    private int slotCount; //활성화된 슬롯 수
    private const int MAX_SLOTS_COUNT = 8; //최대 슬롯 수
    
    private Activated currentActivated;
    private enum Activated
    {
        Tab,
        Item,
        Detail,
    }
    private void ActivateTab()
    {
        StopAllCoroutines();
        tf_item.gameObject.SetActive(false);

        currentActivated = Activated.Tab;
        page_person = selectedPerson / MAX_SLOTS_COUNT;
        selectedPerson = selectedPerson % MAX_SLOTS_COUNT;
        page_clue = selectedClue / MAX_SLOTS_COUNT;
        selectedClue = selectedClue % MAX_SLOTS_COUNT;

        tf_tab.gameObject.SetActive(true);
    }
    private void ActivateItem()
    {
        StopAllCoroutines();
        tf_tab.gameObject.SetActive(false);

        currentActivated = Activated.Item;
        selectedPerson = page_person * MAX_SLOTS_COUNT + selectedPerson;
        selectedClue = page_clue * MAX_SLOTS_COUNT + selectedClue;

        tf_item.gameObject.SetActive(true);
    }

    private int detail_index;
    private List<Sprite> details;
    private void ActivateDetail()
    {
        detail_index = 0;
        currentActivated = Activated.Detail;
        details.AddRange(slots[selectedItem].details);
        SetDetail();
        detail.SetActive(true);
    }
    private void SetDetail()
    {
        detail.GetComponent<Image>().sprite = details[detail_index];
    }
    private void ExitDetail()
    {
        detail.SetActive(false);
        detail.GetComponent<Image>().sprite = null;
        details.Clear();
        currentActivated = Activated.Item;
    }

    private bool stopKeyInput = false; //증거 상세 진입 시 키 입력 방지
    private bool preventExc; //중복입력 방지
    
    //제시해야만 할 때
    private ReturnType returnType;
    public void SetType(ReturnType type) => returnType = type;
    public int ReturnValue { get; private set; }
    private string question;
    [SerializeField]
    private Text question_Text;
    [SerializeField]
    private GameObject go; //제시해야 할 때

    private WaitForSeconds waitTime = new WaitForSeconds(0.01f);
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        //instance = this;
        theDB = FindObjectOfType<DatabaseManager>();
        myAnimator = GetComponent<Animator>();
        
        inventoryItemList = new List<Item>();
        inventoryTabList = new List<Item>();
        details = new List<Sprite>();
        slots = tf_tab.GetComponentsInChildren<InventorySlot>(); //tab. 4X2
        go.SetActive(false);
        detail.SetActive(false);
    }
    public List<Item> SaveItem()
    {
        return inventoryItemList;
    }
    public void LoadItem(List<Item> _item)
    {
        inventoryItemList = _item;
    }
    public void AddItem(Item _item) //? 정크데이터
    {
        inventoryItemList.Add(_item);
    }
    public void GetItem(int _itemID)
    {
        for(int i = 0; i < theDB.itemList.Count; i++) //데이터베이스 검색
        {
            if(_itemID == theDB.itemList[i].itemID) //데이터베이스 발견
            {
                inventoryItemList.Add(theDB.itemList[i]); //아이템 추가
                return;
            }
        }
    }

    private void RemoveSlot()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].RemoveItem();
            slots[i].gameObject.SetActive(false);
        }
    } //인벤토리 초기화
    private void SwitchTab(bool _b = false)
    {
        inventoryTabList.Clear();
        
        if(_b == true)
        {
            if (selectedTab == SelectedTab.Clue)
                selectedTab = SelectedTab.Person;
            else
                selectedTab = SelectedTab.Clue;
        }
    
        switch (selectedTab)
        {
            case SelectedTab.Clue:
                for (int i = 0; i < inventoryItemList.Count; i++)
                {
                    if (inventoryItemList[i].type == ItemType.clue)
                        inventoryTabList.Add(inventoryItemList[i]);
                }
                break;
            case SelectedTab.Person:
                for (int i = 0; i < inventoryItemList.Count; i++)
                {
                    if (inventoryItemList[i].type == ItemType.person)
                        inventoryTabList.Add(inventoryItemList[i]);
                }
                break;
        } //탭에 따라 분류, 인벤토리 리스트에 추가
    } //증거 - 인물정보 간 탭 변경

    private void ShowTab()
    {
        if (!tf_tab.gameObject.activeSelf)
            ActivateTab();

        RemoveSlot();

        ShowPage();
        SelectedItem();
    }//탭 활성화, 페이지가 넘어갈 때
    private void ShowPage()
    {
        slotCount = -1;

        for (int i = Page * MAX_SLOTS_COUNT; i < inventoryTabList.Count; i++)
        {
            slotCount = i - (Page * MAX_SLOTS_COUNT);
            slots[slotCount].gameObject.SetActive(true);
            slots[slotCount].AddItem(inventoryTabList[i]);

            if (slotCount == MAX_SLOTS_COUNT - 1)
                break;
        } //인벤토리 리스트의 내용을 인벤토리 창에 추가
    }//페이지가 바뀌거나 탭이 바뀔 때 슬롯 내용 변경, 접근 금지
    private void SelectedItem()
    {
        StopAllCoroutines();

        if (selectedItem > slotCount)
            selectedItem = slotCount;
        if (slotCount > -1)
        {
            Color color = slots[0].selected_Item.GetComponent<Image>().color;
            color.a = 0f;
            for (int i = 0; i <= slotCount; i++)
                slots[i].selected_Item.GetComponent<Image>().color = color;

            name_text.text = inventoryTabList[selectedItem].itemName;
            ReturnValue = inventoryTabList[selectedItem].itemID;
            StartCoroutine(SelectedItemEffectCoroutine());
        }
        else
            name_text.text = "아이템이 없습니다.";
    }//슬롯 선택 표시(점멸 효과), 페이지 변경 없이 슬롯만 바뀔 때
    private IEnumerator SelectedItemEffectCoroutine()
    {
        while (currentActivated == Activated.Tab)
        {
            Color color = slots[selectedItem].selected_Item.GetComponent<Image>().color;
            while (color.a < 0.5f)
            {
                color.a += 0.03f;
                slots[selectedItem].selected_Item.GetComponent<Image>().color = color;
                yield return waitTime;
            }
            while (color.a > 0f)
            {
                color.a -= 0.03f;
                slots[selectedItem].selected_Item.GetComponent<Image>().color = color;
                yield return waitTime;
            }
            yield return waitTime;
        }

    }//선택된 슬롯 점멸효과, 접근 금지

    public void Adduce(Choice _q)
    {
        question_Text.text = "";
        ui.SetCutTrigger(_q._who);
        ui.SetCharacter(_q._who);
        ui.SetEmotionTrigger(_q._emotion);
        question = _q.sentence;
    
        StartCoroutine(TypingQuestion());
    }
    IEnumerator TypingQuestion()
    {
        for (int i = 0; i < question.Length; i++)
        {
            question_Text.text += question[i];
            yield return waitTime;
        }
    }

    private void ShowItem()
    {
        if (!tf_item.gameObject.activeSelf)
            ActivateItem();
        if(inventoryTabList.Count > 0)
        {
            name_text.text = inventoryTabList[selectedItem].itemName;
            description_Text.text = inventoryTabList[selectedItem].itemDescription;
            item_image.sprite = inventoryTabList[selectedItem].itemIcon;
            ReturnValue = inventoryTabList[selectedItem].itemID;
        }
        else
        {
            name_text.text = "빈 슬롯";
            if (selectedTab == SelectedTab.Clue)
                description_Text.text = "증거가 없습니다.";
            else
                description_Text.text = "인물정보가 없습니다.";
        }
        //이미지도 집어넣어야 함
    }//슬롯 활성화
    
    public void HandleInput()
    {
        if (!stopKeyInput)
        {
            switch (currentActivated)
            {
                case Activated.Tab:
                    if (Input.GetKeyDown(KeyCode.X) & !preventExc)
                    {
                        if (returnType == ReturnType.None | returnType == ReturnType.Objection | returnType == ReturnType.Adduce)
                            ui.PopState();
                    }
                    else if (Input.GetKeyDown(KeyCode.W) & !preventExc)
                    {
                        if (returnType != ReturnType.Person & returnType != ReturnType.Clue)
                        {
                            SwitchTab(true);
                            ShowTab();
                        }
                    }
                    else if (Input.GetKeyDown(KeyCode.RightArrow))
                    {
                        if (selectedItem == slotCount)
                        {
                            NextPage();
                            selectedItem = 0;
                            ShowTab();
                        }
                        else if (selectedItem % 4 == 3)
                        {
                            NextPage();
                            selectedItem = selectedItem - 3;
                            ShowTab();
                        }
                        else if (selectedItem % 4 < 3)
                        {
                            selectedItem++;
                            SelectedItem();
                        }
                    }
                    else if (Input.GetKeyDown(KeyCode.LeftArrow))
                    {
                        if (selectedItem % 4 == 0)
                        {
                            PreviousPage();
                            selectedItem = selectedItem + 3;
                            ShowTab();
                        }
                        if (selectedItem % 4 > 0)
                        {
                            selectedItem--;
                            SelectedItem();
                        }
                    }
                    else if (Input.GetKeyDown(KeyCode.DownArrow) | Input.GetKeyDown(KeyCode.UpArrow))
                    {
                        if (selectedItem < 4)
                            selectedItem = selectedItem + 4;
                        else
                            selectedItem = selectedItem - 4;
                        SelectedItem();
                    }
                    else if (Input.GetKeyDown(KeyCode.Z) & !preventExc)
                    {
                        ui.PlaySound(enter_sound);

                        preventExc = true;

                        ShowItem();
                    }
                    break; //탭 활성화 시
                case Activated.Item:
                    if (Input.GetKeyDown(KeyCode.X) & !preventExc)
                    {
                        preventExc = true;
                        ui.PlaySound(cancel_sound);
                        StopAllCoroutines();
                        ShowTab();
                    }
                    else if (Input.GetKeyDown(KeyCode.W) & !preventExc)
                    {
                        if (returnType != ReturnType.Person & returnType != ReturnType.Clue)
                        {
                            SwitchTab(true);
                            ShowItem();
                        }
                    }
                    if (inventoryTabList.Count > 0)
                    {
                        if (Input.GetKeyDown(KeyCode.RightArrow))
                        {
                            if (inventoryTabList.Count > 0)
                            {
                                if (selectedItem == inventoryTabList.Count - 1)
                                    selectedItem = 0;
                                else
                                    selectedItem++;
                                ui.PlaySound(key_sound);
                                ShowItem();
                            }
                        }
                        else if (Input.GetKeyDown(KeyCode.LeftArrow))
                        {
                            if (inventoryTabList.Count > 0)
                            {
                                if (selectedItem == 0)
                                    selectedItem = inventoryTabList.Count - 1;
                                else
                                    selectedItem--;
                                ui.PlaySound(key_sound);
                                ShowItem();
                            }
                        }
                        else if (Input.GetKeyDown(KeyCode.Z) && !preventExc)
                        {
                            //증거 확대.
                            if (slots[selectedItem].details.Count > 0)
                                ActivateDetail();
                        }
                    }
                    break; //아이템 활성화 시
                case Activated.Detail:
                    if(Input.GetKeyDown(KeyCode.X) & !preventExc)
                    {
                        ExitDetail();
                    }
                    if(details.Count > 0)
                    {
                        if (Input.GetKeyDown(KeyCode.RightArrow))
                        {
                            if (detail_index < details.Count)
                            {
                                detail_index++;
                                SetDetail();
                            }
                        }
                        else if (Input.GetKeyDown(KeyCode.LeftArrow))
                        {
                            if (detail_index > 0)
                            {
                                detail_index--;
                                SetDetail();
                            }
                        }
                    }                  
                    break; //상세 확인
                default:
                    break;
            }
            if (Input.GetKeyDown(KeyCode.S) & !preventExc & currentActivated != Activated.Detail)
            {               
                switch (returnType)
                {
                    case ReturnType.None:
                        break;
                    case ReturnType.Objection:
                        ui.CallObjection(ReturnValue);//이의 제기 장면으로 진입
                        break;
                    case ReturnType.Adduce:
                    case ReturnType.Both:
                    case ReturnType.Person:
                    case ReturnType.Clue:
                        ui.PopState();
                        break;
                }
                //다른 장면에서의 함수를 만들 필요가 있다
            }
            if (Input.GetKeyUp(KeyCode.Z) || Input.GetKeyUp(KeyCode.X) || Input.GetKeyUp(KeyCode.W)) //중복 실행 방지
                preventExc = false;
        }
    }
    public void Enter(UI _ui)
    {
        ui = _ui;
        ui.PlaySound(open_sound);
        ReturnValue = -1;
        selectedClue = 0;
        selectedPerson = 0;

        switch (returnType)
        {
            case ReturnType.None:
            case ReturnType.Objection:
            case ReturnType.Adduce:
                selectedTab = SelectedTab.Clue;
                break;
            case ReturnType.Both:
            case ReturnType.Clue:
                selectedTab = SelectedTab.Clue;
                go.SetActive(true);
                break;
            case ReturnType.Person:
                selectedTab = SelectedTab.Person;
                go.SetActive(true);
                break;           
        }
        myAnimator.SetBool("appear", true);
        ActivateTab();
        SwitchTab();
        ShowTab();
    }
    public void Exit(bool _b = true)
    {
        myAnimator.SetBool("appear", false);
        returnType = ReturnType.None;
        currentActivated = Activated.Tab;
        ui.PlaySound(cancel_sound);
        StopAllCoroutines();
        go.SetActive(false);
    }
}
public enum ReturnType
{
    None, //단순 열람
    Objection, //심문 장면
    Adduce, //탐정 파트
    Both, //둘 중 하나를 제시해야만 할 때
    Person, //사람을 제시해야만 할 때
    Clue, //증거를 제시해야만 할 때
}
