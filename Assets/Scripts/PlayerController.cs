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

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
