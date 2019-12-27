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

    public Sprite[] sprites;

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
        itemList.Add(new Item(90001, "세월호 뱃지", "인권 변호사의 상징과도 같은 것. 나를 진정한 변호사로 만들어 준다."));
        itemList.Add(new Item(91002, "레드준표", "한 때 \'모래시계 검사\'로 이름을 날리던 실력있는 검사.", ItemType.person));
        //Episode1
        itemList.Add(new Item(10001, "사건 기록", "피해자, 좌측과 우측 두부 각각 외상. 좌측은 경미, 우측은 깊은 두개골 파열로,우측의 두부외상이 사인이었을 것으로 추정. 사망추정시각은 11시 50분경."));
        itemList.Add(new Item(10002, "CCTV 사진", "12시경 오토케가 찍혀있는 사진. 룸 문 앞을 찍고 있다.", sprites[0]));
        itemList.Add(new Item(10003, "범행현장의 사진", "피해자가 쓰러져있는 사진. 피해자의 주위가 지나치게 깨끗하다.", sprites[1]));
        itemList.Add(new Item(10004, "재떨이", "사건에 사용된 둔기. 피고인의 오른손 지문이 찍혀있다."));
        itemList.Add(new Item(10005, "담배꽁초", "피해자의 밑에 깔려있던 한개비의 꽁초. 피해자의 타액은 검출되지 않았다."));

        itemList.Add(new Item(11001, "오토케 미노가시", "이번 사건의 피고인. 경찰 업무 중 과잉대처로 인한 살인 혐의를 받고 있다.", ItemType.person));
        itemList.Add(new Item(11002, "우라기리", "이번 사건의 피해자. 두부외상으로 사망했다.", ItemType.person));
        itemList.Add(new Item(11003, "이츠모 시다바리", "주요 증인. 술집에서 종업원으로 일하고 있다.", ItemType.person));
        itemList.Add(new Item(11004, "아몰랑 메소라시", "오토케의 동료 경찰. 오토케의 무죄를 믿고 있다.", ItemType.person));
        

        
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
