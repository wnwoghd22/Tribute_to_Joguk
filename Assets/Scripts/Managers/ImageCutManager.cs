using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageCutManager : MonoBehaviour
{
    #region Singlton
    static public ImageCutManager instance;

    private void Awake()
    {
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
    #endregion

    private UI ui;
    [SerializeField]
    private GameObject go;

    // Start is called before the first frame update
    void Start()
    {
        ui = FindObjectOfType<UI>();
    }
    
    public void SetCutActive(bool _bool)
    {
        go.SetActive(_bool);
    }
    public void ChangeCut(Sprite _sprite)
    {
        go.GetComponent<Image>().sprite = _sprite;
    }
}
