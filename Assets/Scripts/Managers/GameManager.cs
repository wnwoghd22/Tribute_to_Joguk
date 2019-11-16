using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private PlayerController player;
    private CameraManager theCamera;
    //private FadeManager theFM;
    private UI ui;

    public void LoadStart()
    {
        StartCoroutine(LoadWaitCoroutine());
    }
    IEnumerator LoadWaitCoroutine()
    {
        yield return new WaitForSeconds(0.5f);

        player = FindObjectOfType<PlayerController>();
        theCamera = FindObjectOfType<CameraManager>();
        ui = FindObjectOfType<UI>();

        theCamera.target = GameObject.Find("Player");
        theCamera.transform.position = new Vector3(theCamera.target.transform.position.x,
                                                   theCamera.target.transform.position.y,
                                                   theCamera.transform.position.z);


        ui.FadeIn();
    }
}
