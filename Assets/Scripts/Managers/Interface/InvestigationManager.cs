using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvestigationManager : MonoBehaviour, Manager
{
    [SerializeField]
    private GameObject cursor;
    private float[] cursorPos = { 0f, 0f };

    private UI ui;

    public void Enter(UI _ui)
    {
        ui = _ui;
        ui.ClearEvent();

        ActivateCursor(true);

    }

    public void Exit(bool _b = true)
    {
        ActivateCursor(false);
        if (_b == true)
        {
            cursorPos[0] = 0.5f; cursorPos[1] = 0.5f;
        }
    }

    public void HandleInput()
    {
        HandleMovement();

        if(Input.GetKeyDown(KeyCode.Z))
        {
            if (ui.IsEvent) //이벤트가 있는가?
                ui.StartEvent(); //이벤트 진입
            //else
                //특별한 것은 없다
        }
        if(Input.GetKeyDown(KeyCode.X))
        {
            //나가기
        }
    }
    public void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 move = new Vector3(horizontal, vertical, 0);

        Vector3 position = cursor.transform.position;
        position += move * 3 * Time.deltaTime;

        Vector3 viewPos = Camera.main.ViewportToWorldPoint(position);
        {
            if (viewPos.x < 0f) viewPos.x = 0f;
            if (viewPos.x > 1f) viewPos.x = 1f;
            if (viewPos.y < 0f) viewPos.y = 0f;
            if (viewPos.y > 1f) viewPos.y = 1f;
        }
        cursor.transform.position = Camera.main.WorldToViewportPoint(viewPos);
    }
    void SetCursor(float x = 0.5f, float y = 0.5f)
    {
        Vector3 viewPos = Camera.main.ViewportToWorldPoint(cursor.transform.position);
        {
            viewPos.x = x;
            viewPos.y = y;
        }
        cursor.transform.position = Camera.main.WorldToViewportPoint(viewPos);
    }
    float[] GetCursor()
    {
        Vector3 viewPos = Camera.main.ViewportToWorldPoint(cursor.transform.position);
        float[] result = { viewPos.x, viewPos.y };
        return result;
    }
    void ActivateCursor(bool _active)
    {
        cursor.gameObject.SetActive(_active);
        if (_active == true)
            SetCursor(cursorPos[0], cursorPos[1]);
        else
            cursorPos = GetCursor();
    }

    void Start()
    {
        cursor.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
