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
    private Animator myAnimator;

    // Start is called before the first frame update
    void Start()
    {
        ui = FindObjectOfType<UI>();
        myAnimator = GetComponent<Animator>();
    }
    
    public void SetCutActive(bool _bool)
    {
        go.SetActive(_bool);
    }
    public void ChangeCut(Sprite _sprite)
    {
        go.GetComponent<Image>().sprite = _sprite;
    }
    public void SetTrigger(string _s)
    {
        myAnimator.SetTrigger(_s);
    }
    public void ResetTrigger()
    {
        myAnimator.ResetTrigger("Off");
        myAnimator.ResetTrigger("Attorney");
        myAnimator.ResetTrigger("Prosecutor");
        myAnimator.ResetTrigger("Judge");
        myAnimator.ResetTrigger("Court");
        myAnimator.ResetTrigger("Witness");
    }
}
