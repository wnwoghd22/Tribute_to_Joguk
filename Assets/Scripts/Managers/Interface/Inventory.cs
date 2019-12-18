﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour, Manager
{
    public static Inventory instance;

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
    private GameObject go; //인벤토리 창 활성화
    private GameObject goOOC; //선택지 활성화...또는 증거 상세
    //public GameObject prefab_FloatingText;

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

    private bool stopKeyInput = false; //증거 상세 진입 시 키 입력 방지
    private bool preventExc; //중복입력 방지

    public enum ReturnType
    {
        None, //단순 열람
        Objection, //심문 장면
        Both, //둘 중 하나를 제시해야만 할 때
        Person, //사람을 제시해야만 할 때
        Clue, //증거를 제시해야만 할 때
    }
    private ReturnType returnType;
    public void SetType(ReturnType type) => returnType = type;
    private int ReturnValue;

    private WaitForSeconds waitTime = new WaitForSeconds(0.01f);
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        theDB = FindObjectOfType<DatabaseManager>();
        
        inventoryItemList = new List<Item>();
        inventoryTabList = new List<Item>();
        slots = tf_tab.GetComponentsInChildren<InventorySlot>(); //tab. 4X2
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
    public void GetItem(int _itemID, int _count = 1)
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
                    if (inventoryItemList[i].type == Item.ItemType.clue)
                        inventoryTabList.Add(inventoryItemList[i]);
                }
                break;
            case SelectedTab.Person:
                for (int i = 0; i < inventoryItemList.Count; i++)
                {
                    if (inventoryItemList[i].type == Item.ItemType.person)
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

        if (selectedItem >= slotCount)
            selectedItem = slotCount - 1;
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
        while (currentActivated == Activated.Item)
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
    
    private void ShowItem()
    {
        if (!tf_item.gameObject.activeSelf)
            ActivateItem();
        if(inventoryTabList.Count > 0)
        {
            name_text.text = inventoryTabList[selectedItem].itemName;
            description_Text.text = inventoryTabList[selectedItem].itemDescription;
            ReturnValue = inventoryItemList[selectedItem].itemID;
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
                    if (Input.GetKeyDown(KeyCode.X) && !preventExc)
                    {
                        if (returnType == ReturnType.None || returnType == ReturnType.Objection)
                           ui.ExitInventory();
                    }
                    else if (Input.GetKeyDown(KeyCode.W) && !preventExc)
                    {
                        if (returnType != ReturnType.Person && returnType != ReturnType.Clue)
                        {
                            SwitchTab(true);
                            ShowTab();
                        }
                    }
                    else if (Input.GetKeyDown(KeyCode.RightArrow))
                    {
                        if (selectedItem == slotCount - 1)
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
                    else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.UpArrow))
                    {
                        if (selectedItem > 3)
                            selectedItem = selectedItem + 4;
                        else
                            selectedItem = selectedItem - 4;
                        SelectedItem();
                    }
                    else if (Input.GetKeyDown(KeyCode.Z) && !preventExc)
                    {
                        ui.PlaySound(enter_sound);
                        
                        preventExc = true;

                        ShowItem();
                    }
                    break; //탭 활성화 시
                case Activated.Item:                   
                    if (Input.GetKeyDown(KeyCode.X) && !preventExc)
                    {
                        preventExc = true;
                        ui.PlaySound(cancel_sound);
                        StopAllCoroutines();
                        ShowTab();
                    }
                    else if (Input.GetKeyDown(KeyCode.W) && !preventExc)
                    {
                        if (returnType != ReturnType.Person && returnType != ReturnType.Clue)
                        {
                            SwitchTab(true);
                            ShowItem();
                        }
                    }
                    if (inventoryTabList.Count > 0)
                    {
                        if (Input.GetKeyDown(KeyCode.RightArrow))
                        {
                            if(inventoryTabList.Count > 0)
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
                        }
                    }
                    break; //아이템 활성화 시
                default:                   
                    break;
            }
            if (Input.GetKeyDown(KeyCode.S) && returnType != ReturnType.None && !preventExc)
            {
                if (returnType == ReturnType.Objection) //이의 제기 장면으로 진입
                    ui.CallObjection(ReturnValue);
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
        go.SetActive(true);
        if (returnType == ReturnType.Person)
            selectedTab = SelectedTab.Person;
        else
            selectedTab = SelectedTab.Clue;
        ActivateTab();
        SwitchTab();
        ShowTab();
    }
    public void Exit(bool _b = true)
    {
        returnType = ReturnType.None;
        currentActivated = Activated.Tab;
        ui.PlaySound(cancel_sound);
        StopAllCoroutines();
        go.SetActive(false);
    }
}
