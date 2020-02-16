using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvestigationManager : MonoBehaviour, Manager
{
    [SerializeField]
    private GameObject cursor;

    public void Enter(UI ui)
    {
        throw new System.NotImplementedException();
    }

    public void Exit(bool _b = true)
    {
        throw new System.NotImplementedException();
    }

    public void HandleInput()
    {
        HandleMovement();
    }
    public void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 move = new Vector3(horizontal, vertical, 0);
        Vector3 position = cursor.transform.position;
        position += move * 3 * Time.deltaTime;
        Vector3 viewPos = Camera.main.ViewportToWorldPoint(cursor.transform.position);
        {
            if (viewPos.x < 0f) viewPos.x = 0f;
            if (viewPos.x > 1f) viewPos.x = 1f;
            if (viewPos.y < 0f) viewPos.y = 0f;
            if (viewPos.y > 1f) viewPos.y = 1f;
        }
        cursor.transform.position = Camera.main.WorldToViewportPoint(viewPos);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
