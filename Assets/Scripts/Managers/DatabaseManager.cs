using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseManager : MonoBehaviour
{
    static public DatabaseManager instance;

    public string[] var_name;
    public float[] var;

    public string[] switch_name;
    public bool[] switches;

    public List<Item> itemList = new List<Item>();
   
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        #region itemList
        itemList.Add(new Item(00001, "세월호 뱃지", "인권 변호사의 상징과도 같은 것. 나를 진정한 변호사로 만들어 준다."));
        itemList.Add(new Item(10002, "생강절임", "마력 10회복"));
        itemList.Add(new Item(10003, "연어초밥", "체력 50회복"));
        itemList.Add(new Item(10004, "미소장국", "마력 30회복"));
        itemList.Add(new Item(11001, "마법상자", "무작위 회복"));

        itemList.Add(new Item(20001, "단도 미사일", "짧은 검. 기본 무기"));       
        itemList.Add(new Item(20101, "제로식 아머", "가벼운 갑옷. 기본 갑옷"));
        itemList.Add(new Item(20201, "제로식 건틀렛", "가벼운 장갑. 기본 갑옷"));
        itemList.Add(new Item(20301, "제로식 팬츠", "가벼운 바지. 기본 갑옷"));
        itemList.Add(new Item(20401, "제로식 부츠", "가벼운 신발. 기본 갑옷"));

        itemList.Add(new Item(21001, "시간의 반지", "느린 속도로 마력을 회복시켜주는 반지"));
        itemList.Add(new Item(30001, "고대유물 조각1", "반으로 쪼개진 유물의 파편"));
        itemList.Add(new Item(30002, "고대유물 조각2", "반으로 쪼개진 유물의 파편"));
        itemList.Add(new Item(30003, "고대유물", "고대유물"));
        #endregion
    }

    public void UseItem(int _itemID)
    {
        switch(_itemID)
        {
            case 10001:
                break;
            case 10002:
                break;
            default:
                break;
        }
    }   
}
