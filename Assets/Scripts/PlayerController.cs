using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
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
    private Rigidbody2D myRigidbody;
    [SerializeField]
    float spd;  
    public string SceneName { get; set; }

    [SerializeField]
    private Animator EffectTrigger;
    private Animator EmotionTrigger;
    [SerializeField]
    private GameObject Attorney;
    [SerializeField]
    private GameObject Prosecutor;
    [SerializeField]
    private GameObject Judge;
    [SerializeField]
    private GameObject Witness;
    [SerializeField]
    private GameObject Company;
    [SerializeField]
    private GameObject Talk;
    private Dictionary<emotion, string> EmoDict
        = new Dictionary<emotion, string>
    {
        { emotion.normal,"normal" },
        { emotion.point, "point" },
        { emotion.interrogate, "interrogate" },
        { emotion.embarrassed, "embarrassed" },
        { emotion.doubt, "doubt" },
        { emotion.check, "check" },
        { emotion.bang, "bang" },
        {emotion.agree, "agree" },
    };

    public void SetCharacterActive(who _character)
    {
        Attorney.SetActive(_character == who.Attorney);
        Prosecutor.SetActive(_character == who.Prosecutor);
        Judge.SetActive(_character == who.Judge);
        Witness.SetActive(_character == who.Witness);
        Company.SetActive(_character == who.Company);
        Talk.SetActive(_character == who.Talk);
        switch (_character)
        {
            case who.None:
                EmotionTrigger = null;
                break;
            case who.Attorney:
                EmotionTrigger = Attorney.GetComponent<Animator>();
                break;
            case who.Prosecutor:
                EmotionTrigger = Prosecutor.GetComponent<Animator>();
                break;
            case who.Judge:
                EmotionTrigger = Judge.GetComponent<Animator>();
                break;
            case who.Witness:
                EmotionTrigger = Witness.GetComponent<Animator>();
                break;
            case who.Company:
                EmotionTrigger = Company.GetComponent<Animator>();
                break;
            case who.Talk:
                EmotionTrigger = Talk.GetComponent<Animator>();
                break;
            default:
                break;
        }
    }
    public void SetEmotionTrigger(emotion _e)
    {
        if(EmotionTrigger != null & _e != emotion.None)
        {
            ResetEmotionTrigger();
            EmotionTrigger.SetTrigger(EmoDict[_e]);
        }          
    }
    public void SetEffectTrigger(string _s)
    {
        EffectTrigger.SetTrigger(_s);
    }
    public void ResetEmotionTrigger()
    {
        EmotionTrigger.ResetTrigger("normal");
        EmotionTrigger.ResetTrigger("doubt");
        EmotionTrigger.ResetTrigger("talk");
    }

    public IEnumerator Move(bool _direction, int _count = 1)
    {
        int dir = _direction ? 1 : -1; //true = right, false = left
        for(int i = 0; i < 30; i++)
        {
            yield return null;
            myRigidbody.velocity = new Vector2(dir * spd * _count, 0);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
