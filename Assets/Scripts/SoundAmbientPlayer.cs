using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundAmbientPlayer : MonoBehaviour
{
    [SerializeField] private float _delayTime;
    [SerializeField] private AudioSource _source;
    [SerializeField] private List<AudioClip> _clips;

    private void Awake()
    {
        StartCoroutine(PlayClipCoroutine());
    }

    private IEnumerator PlayClipCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(_delayTime);
            PlayClip();
        }
    }

    private void PlayClip()
    {
        int currentIndex = Random.Range(0, _clips.Count);
        _source.clip = _clips[currentIndex];
        _source.Play();
    }
}
