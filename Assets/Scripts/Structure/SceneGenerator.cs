using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SceneGenerator : MonoBehaviour
{
    private UI ui;

    [SerializeField]
    private Event[] dialogs;
    [SerializeField]
    private Event adduce;
    [SerializeField]
    private Map[] map;
    [SerializeField]
    private Sprite background;
    [SerializeField]
    private GameObject[] clues; //위치를 담을 수 있게 해야 하는가?

    // Start is called before the first frame update
    void Start()
    {
        ui = FindObjectOfType<UI>();
        ui.SetScene(dialogs, adduce, map, background);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
