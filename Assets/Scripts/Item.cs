using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public int itemID; //중복 불가. DB key 값으로 사용
    public string itemName; //중복 가능. 찢어진 페이지(정보 부족), 찢어진 페이지(알고보니 주요증거) 등
    public string itemDescription; //아이템 설명 (찢겨 나간 부분의 내용은 알 수 없다) 등
    public Sprite itemIcon;
    public ItemType type;

    public enum ItemType
    {
        person, //인물 정보
        clue, //증거품
    }

    public Item(int _ID, string _name, string _Des)
    {
        itemID = _ID;
        itemName = _name;
        itemDescription = _Des;

        itemIcon = Resources.Load("ItemIcon/" + _ID.ToString(), typeof(Sprite)) as Sprite;
    }
}
