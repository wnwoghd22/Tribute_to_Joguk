using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    static public BGMManager instance;

    public AudioClip[] clips;
    private AudioSource source;

    private WaitForSeconds waitTIme = new WaitForSeconds(0.01f);

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
        source = GetComponent<AudioSource>();
    }
    public void Play(int _track, float _Vol = 1)
    {
        source.volume = _Vol;
        source.clip = clips[_track];
        source.Play();
    }
    public void Pause()
    {
        source.Pause();
    }
    public void Unpause()
    {
        source.UnPause();
    }
    public void SetVolume(float vol)
    {
        source.volume = vol;
    }
    public void Stop()
    {
        source.Stop();
    }
    public void FadeOutMusic()
    {
        StopAllCoroutines();
        StartCoroutine(FadeOutMusicCoroutine());
    }
    public void FadeInMusic()
    {
        StopAllCoroutines();
        StartCoroutine(FadeInMusicCoroutine());
    }

    IEnumerator FadeOutMusicCoroutine()
    {
        for(float i = 1.0f; i > 0f; i -= 0.01f)
        {
            source.volume = i;
            yield return waitTIme;
        }
    }
    IEnumerator FadeInMusicCoroutine()
    {
        for (float i = 0f; i < 1.0f; i += 0.01f)
        {
            source.volume = i;
            yield return waitTIme;
        }
    }
}
