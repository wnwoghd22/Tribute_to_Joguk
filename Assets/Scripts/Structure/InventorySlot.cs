using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image icon;
    //public Text item_name;
    //public Text item_count;
    public GameObject selected_Item;
    public int itemID;
    public List<Sprite> details;

    public void AddItem(Item _item)
    {
        //item_name.text = _item.itemName;
        icon.sprite = _item.itemIcon;
        itemID = _item.itemID;
        if (_item.Detail != null)
            for (int i = 0; i < _item.Detail.Count; i++)
                details.Add(_item.Detail[i]);
    }
    public void RemoveItem()
    {
        //item_name.text = "";
        //item_count.text = "";
        icon.sprite = null;
    }
}
