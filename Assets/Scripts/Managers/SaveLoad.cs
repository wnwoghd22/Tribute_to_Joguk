using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;

public class SaveLoad : MonoBehaviour
{
    [System.Serializable]
    public class Data
    {
        public List<int> playerItemInventory;
        public List<int> playerItemCount;
        public List<int> playerEquipItem;

        public string mapName;
        public string sceneName;

        public List<bool> swList;
        public List<string> swNameList;
        public List<string> varNameList;
        public List<float> varNumberList;
        public void Clear()
        {
            playerItemInventory.Clear();
            playerItemCount.Clear();
            playerEquipItem.Clear();

            swList.Clear();
            swNameList.Clear();
            varNameList.Clear();
            varNameList.Clear();           
        }
    }

    private PlayerController player;
    private DatabaseManager theDB;
    private Inventory theIV;
    private FadeManager theFM;
    private UI ui;

    public Data data;
    
    public void CallSave()
    {
        theDB = FindObjectOfType<DatabaseManager>();
        player = FindObjectOfType<PlayerController>();
        theIV = FindObjectOfType<Inventory>();
        ui = FindObjectOfType<UI>();
        
        Debug.Log("Save Suceed");

        data.Clear();

        for(int i = 0; i < theDB.var_name.Length; i++)
        {
            data.varNameList.Add(theDB.var_name[i]);
            data.varNumberList.Add(theDB.var[i]);
        }
        for (int i = 0; i < theDB.switch_name.Length; i++)
        {
            data.swNameList.Add(theDB.switch_name[i]);
            data.swList.Add(theDB.switches[i]);
        }

        List<Item> itemList = theIV.SaveItem();
        for (int i = 0; i < itemList.Count; i++)
        {
            Debug.Log("인벤토리 저장 완료 : " + itemList[i].itemID);
            data.playerItemInventory.Add(itemList[i].itemID);
        }
        
        BinaryFormatter bf = new BinaryFormatter();
        FileStream fileStream = File.Create(Application.dataPath + "/SaveFile.dat");

        bf.Serialize(fileStream, data);
        fileStream.Close();

        Debug.Log(Application.dataPath + "저장 완료");
    }
    public void CallLoad()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream fileStream = File.Open(Application.dataPath + "/SaveFile.dat", FileMode.Open);

        if (fileStream != null && fileStream.Length > 0)
        {
            data = (Data)bf.Deserialize(fileStream);

            theDB = FindObjectOfType<DatabaseManager>();
            player = FindObjectOfType<PlayerController>();
            theIV = FindObjectOfType<Inventory>();
            //theFM = FindObjectOfType<FadeManager>();

            ui = FindObjectOfType<UI>();

            ui.FadeOut();
            
            ui.PlayerSceneName = data.sceneName;


            theDB.var = data.varNumberList.ToArray();
            theDB.var_name = data.varNameList.ToArray();
            theDB.switches = data.swList.ToArray();
            theDB.switch_name = data.swNameList.ToArray();

            List<Item> itemList = new List<Item>();

            for (int i = 0; i < data.playerItemInventory.Count; i++)
            {
                for (int x = 0; x < theDB.itemList.Count; x++)
                {
                    if (data.playerItemInventory[i] == theDB.itemList[x].itemID)
                    {
                        itemList.Add(theDB.itemList[x]);
                        Debug.Log("인벤토리 아이템을 로드했습니다 : " + theDB.itemList[x].itemID);
                        break;
                    }
                }
            }
            theIV.LoadItem(itemList);
        }

        else
        {
            Debug.Log("저장된 파일이 없습니다.");
        }
        fileStream.Close();        
    }
    IEnumerator WaitCoroutine()
    {
        yield return new WaitForSeconds(2f);

        GameManager theGM = FindObjectOfType<GameManager>();
        theGM.LoadStart();

        SceneManager.LoadScene(data.sceneName); //   
    }
}
