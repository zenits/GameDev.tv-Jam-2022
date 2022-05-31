using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    private enum FadeDirection
    {
        FadeFrom1To2,
        FadeFrom2To1
    }

    private static AudioManager _Instance;

    public static AudioManager Instance { get { return _Instance; } }

    AudioSource _source;
    [SerializeField] AudioSource music1;
    [SerializeField] AudioSource music2;

    FadeDirection _fadeDirection = FadeDirection.FadeFrom1To2;

    // Start is called before the first frame update
    void Start()
    {
        if (_Instance == null)
        {
            _Instance = this;
            music1.Play();
            DontDestroyOnLoad(this);
        }
        else
            Destroy(this.gameObject);
    }

    private void Awake()
    {
        _source = GetComponent<AudioSource>();
    }

    public void PlayOnce(AudioClip clip, float volume)
    {
        _source.PlayOneShot(clip, volume);
    }

    public void SwitchMusic()
    {
        StartCoroutine("MusicFade");
    }

    IEnumerator MusicFade()
    {
        AudioSource source;
        AudioSource target;
        if (_fadeDirection == FadeDirection.FadeFrom1To2)
        {
            source = music1;
            target = music2;
            _fadeDirection = FadeDirection.FadeFrom2To1;
        }
        else
        {
            source = music2;
            target = music1;
            _fadeDirection = FadeDirection.FadeFrom1To2;
        }
        target.volume = 0;
        target.Play();
        for (float i = 0f; i <= 1f; i = i + 0.05f)
        {
            source.volume = 1 - i;
            target.volume = i;
            yield return new WaitForSeconds(0.1f);
        }
        source.volume = 0;
        source.Pause();
        yield return 0;
    }
}
