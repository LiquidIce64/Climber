using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioLoopController : MonoBehaviour
{
    [SerializeField] private AudioClip startClip;
    [SerializeField] private AudioClip loopClip;
    [SerializeField] private AudioClip endClip;
    private AudioSource source;
    private Coroutine loopCoroutine = null;
    private bool playing = false;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    private void PlayClip(AudioClip clip, bool loop = false)
    {
        source.Stop();
        source.clip = clip;
        source.loop = loop;
        source.Play();
    }

    [ContextMenu("Play")]
    public void Play()
    {
        if (playing) return;
        if (loopCoroutine != null) StopCoroutine(loopCoroutine);
        PlayClip(startClip);
        loopCoroutine = StartCoroutine(Loop());
        playing = true;
    }

    private IEnumerator Loop()
    {
        yield return new WaitForSeconds(startClip.length);
        PlayClip(loopClip, true);
    }

    [ContextMenu("Stop")]
    public void Stop()
    {
        if (!playing) return;
        if (loopCoroutine != null)
        {
            StopCoroutine(loopCoroutine);
            loopCoroutine = null;
        }
        PlayClip(endClip);
        playing = false;
    }
}
