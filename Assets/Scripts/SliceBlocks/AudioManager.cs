using System;
using UnityEngine;

public class AudioManager : GenericSingletonClass<AudioManager>
{
    [SerializeField] private AudioSource startAudio;
    [SerializeField] private AudioSource successBlockAudio;
    [SerializeField] private AudioSource NormaBlockAudio;
    [SerializeField] private AudioSource luckyBlockAudio;

    private void Start() {
        GameManager.Instance.EndGame += SoundStartRound;
    }

    public void SoundStartRound() {
        startAudio.Play();
    }
    
    public void SoundSuccessBlock() {
        successBlockAudio.Play();
    }

    public void SoundNormalBlock() {
        NormaBlockAudio.Play();
    }

    public void SoundLuckyBlock() {
        luckyBlockAudio.Play();
    }

}
