using System.Collections;
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

    private List<Item> inventoryItemList;
    private List<Item> inventoryTabList; //활성화시 분류

    [SerializeField]
    private Text name_text;

    //탭
    [SerializeField]
    private Transform tf_tab;
    private InventorySlot[] slots; //item slots
    private int selectedSlot;

    //상세
    [SerializeField]
    private Transform tf_item;
    [SerializeField]
    private Text description_Text;

    [SerializeField]
    private GameObject go; //인벤토리 창 활성화
    private GameObject goOOC; //선택지 활성화
    //public GameObject prefab_FloatingText;

    private enum SelectedTab
    {
        item,
        person,
    }
    private SelectedTab selectedTab;
    private int page
    {
        get
        {
            if (selectedTab == SelectedTab.item)
                return page_item;
            else
                return page_person;
        }
        set
        {
            if (selectedTab == SelectedTab.item)
                page_item = value;
            else
                page_person = value;
        }
    }//페이지
    private int page_item;
    private int page_person;
    private int selectedItem
    {
        get
        {
            if (selectedTab == SelectedTab.item)
                return selectedClue;
            else
                return selectedPerson;
        }
        set
        {
            if (selectedTab == SelectedTab.item)
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
        tf_item.gameObject.SetActive(false);

        currentActivated = Activated.Tab;
        tf_tab.gameObject.SetActive(true);

        ShowTab();
    }
    private void ActivateItem()
    {
        tf_tab.gameObject.SetActive(false);

        currentActivated = Activated.Item;
        tf_item.gameObject.SetActive(true);

        ShowItem();
    }

    private bool stopKeyInput = false;
    private bool preventExc;

    private WaitForSeconds waitTime = new WaitForSeconds(0.01f);
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        theDB = FindObjectOfType<DatabaseManager>();

        ui = FindObjectOfType<UI>();
        
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
    public void ReturnItem(Item _item) //?
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

    public void RemoveSlot()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].RemoveItem();
            slots[i].gameObject.SetActive(false);
        }
    } //인벤토리 초기화

    public void ShowTab()
    {
        inventoryTabList.Clear();
        RemoveSlot();
        //selectedItem = 0;
        //page = 0;

        switch (selectedTab)
        {
            case SelectedTab.item:
                for (int i = 0; i < inventoryItemList.Count; i++)
                {
                    if (inventoryItemList[i].type == Item.ItemType.clue)
                        inventoryTabList.Add(inventoryItemList[i]);
                }
                break;
            case SelectedTab.person:
                for (int i = 0; i < inventoryItemList.Count; i++)
                {
                    if (inventoryItemList[i].type == Item.ItemType.person)
                        inventoryTabList.Add(inventoryItemList[i]);
                }
                break;
        } //탭에 따라 분류, 인벤토리 리스트에 추가

        ShowPage();

        SelectedItem();
    }//탭 활성화
    public void ShowPage()
    {
        slotCount = -1;

        for (int i = page * MAX_SLOTS_COUNT; i < inventoryTabList.Count; i++)
        {
            slotCount = i - (page * MAX_SLOTS_COUNT);
            slots[slotCount].gameObject.SetActive(true);
            slots[slotCount].AddItem(inventoryTabList[i]);

            if (slotCount == MAX_SLOTS_COUNT - 1)
                break;
        } //인벤토리 리스트의 내용을 인벤토리 창에 추가
    }
    public void SelectedItem()
    {
        StopAllCoroutines();
        if (slotCount > -1)
        {
            Color color = slots[0].selected_Item.GetComponent<Image>().color;
            color.a = 0f;
            for (int i = 0; i <= slotCount; i++)
                slots[i].selected_Item.GetComponent<Image>().color = color;
            name_text.text = inventoryTabList[selectedItem].itemName;
            StartCoroutine(SelectedItemEffectCoroutine());
        }
        else
            name_text.text = "아이템이 없습니다.";
    }//슬롯 선택 표시
    IEnumerator SelectedItemEffectCoroutine()
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

    }//선택된 슬롯 점멸효과
    
    public void ShowItem()
    {
        name_text.text = inventoryTabList[selectedItem].itemName;
        description_Text.text = inventoryTabList[selectedItem].itemDescription;
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
                        ui.ExitInventory();
                    }
                    else if (Input.GetKeyDown(KeyCode.RightArrow))
                    {
                        if (selectedItem % 4 == 3 && inventoryItemList.Count / 8 > page)
                        {
                            page++;
                            selectedItem = selectedItem - 3;
                        }
                        if (selectedItem % 4 < 3)
                            selectedItem++;
                    }
                    else if (Input.GetKeyDown(KeyCode.LeftArrow))
                    {
                        
                    }
                    else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.UpArrow))
                    {
                        if (selectedItem > 3)
                            selectedItem = selectedItem + 4;
                        else
                            selectedItem = selectedItem - 4;
                    }
                    else if (Input.GetKeyDown(KeyCode.Z))
                    {
                        ui.PlaySound(enter_sound);
                        
                        preventExc = true;

                        ActivateItem();
                    }
                    break; //탭 활성화 시
                case Activated.Item:
                    if (Input.GetKeyDown(KeyCode.I))
                    {
                        ui.ExitInventory();///////////////////////////////////////////////////////
                    }
                    else if (Input.GetKeyDown(KeyCode.X))
                    {
                        preventExc = true;
                        ui.PlaySound(cancel_sound);
                        StopAllCoroutines();
                        currentActivated = Activated.Tab;
                        ShowTab();
                    }
                    if (inventoryTabList.Count > 0)
                    {
                        if (Input.GetKeyDown(KeyCode.DownArrow))
                        {
                            if (selectedItem + 3 > slotCount)
                            {
                                if (page < (inventoryTabList.Count - 1) / MAX_SLOTS_COUNT)
                                    page++;
                                else
                                    page = 0;
                                RemoveSlot();
                                ShowPage();
                                selectedItem = -3;
                            }

                            if (selectedItem < slotCount - 2)
                                selectedItem += 3;
                            else
                                selectedItem %= 3;
                            ui.PlaySound(key_sound);
                            SelectedItem();
                        }
                        else if (Input.GetKeyDown(KeyCode.UpArrow))
                        {
                            if (selectedItem - 3 < 0)
                            {
                                if (page != 0)
                                    page--;
                                else
                                    page = (inventoryTabList.Count - 1) / MAX_SLOTS_COUNT;
                                RemoveSlot();
                                ShowPage();
                            }

                            if (selectedItem > 2)
                                selectedItem -= 3;
                            else
                                selectedItem = slotCount;
                            /*
                            {
                                theAM.Play(cancel_sound);
                                StopAllCoroutines();
                                itemActivated = false;
                                tabActivated = true;
                                ShowTab();
                                return;
                            }*/
                            ui.PlaySound(key_sound);
                            SelectedItem();
                        }
                        else if (Input.GetKeyDown(KeyCode.RightArrow))
                        {
                            if (selectedItem + 1 > slotCount)
                            {
                                if (page < (inventoryTabList.Count - 1) / MAX_SLOTS_COUNT)
                                    page++;
                                else
                                    page = 0;
                                RemoveSlot();
                                ShowPage();
                                selectedItem = -1;
                            }
                            if (selectedItem < slotCount)
                                selectedItem++;
                            else
                                selectedItem = 0;
                            ui.PlaySound(key_sound);
                            SelectedItem();
                        }
                        else if (Input.GetKeyDown(KeyCode.LeftArrow))
                        {
                            if (selectedItem - 1 < 0)
                            {
                                if (page != 0)
                                    page--;
                                else
                                    page = (inventoryTabList.Count - 1) / MAX_SLOTS_COUNT;
                                RemoveSlot();
                                ShowPage();
                            }
                            if (selectedItem > 0)
                                selectedItem--;
                            else
                                selectedItem = slotCount;
                            ui.PlaySound(key_sound);
                            SelectedItem();
                        }
                        else if (Input.GetKeyDown(KeyCode.Z) && !preventExc)
                        {
                            //증거 또는 인물 제시, 그에 따른 효과.
                        }
                    }
                    else if (Input.GetKeyDown(KeyCode.UpArrow))
                        {
                            preventExc = true;
                            ui.PlaySound(cancel_sound);
                            StopAllCoroutines();
                            currentActivated = Activated.Tab;
                            ShowTab();
                        }
                    else if (Input.GetKeyDown(KeyCode.Z))
                    {
                        //if(ui.)
                    }
                    break; //아이템 활성화 시
            }
            if (Input.GetKeyUp(KeyCode.Z) || Input.GetKeyUp(KeyCode.X) || Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.UpArrow)) //중복 실행 방지
                preventExc = false;            
        }
    }
    public void Enter(UI _ui)
    {
        ui = _ui;

        ui.PlaySound(open_sound);
        go.SetActive(true);
        selectedTab = SelectedTab.item;
        ActivateTab();
        ShowTab();
    }
    public void Exit(bool _b = true)
    {
        currentActivated = Activated.Tab;
        ui.PlaySound(cancel_sound);
        StopAllCoroutines();
        go.SetActive(false);
    }
}
