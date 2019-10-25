using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{

    bool musicIsPlaying;

    public AudioSource sounds;
    public AudioClip coinCollectSound;
    public AudioClip biggerCoinCollectSound;
    public AudioClip godModeSound;
    public AudioClip obstacleDestroyedSound;
    public AudioClip deathSound;

    public Button muteUnmuteButton;

    public Image muteButtonImage;
    public Image unmuteButtonImage;

    // Start is called before the first frame update
    void Start()
    {
        sounds = GetComponent<AudioSource>();        
        Button muteUnmute = muteUnmuteButton.GetComponent<Button>();
        muteUnmute.onClick.AddListener(MuteUnmute);
        if (PlayerPrefs.GetInt("soundeffectsmuted") == 1)
        {
            sounds.mute = true;
            muteButtonImage.enabled = true;
            unmuteButtonImage.enabled = false;
        }
        else {
            sounds.mute = false;
            muteButtonImage.enabled = false;
            unmuteButtonImage.enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void MuteUnmute() 
    {
        sounds.mute = !sounds.mute;
        if (sounds.mute)
        {
            PlayerPrefs.SetInt("soundeffectsmuted", 1);
            muteButtonImage.enabled = true;
            unmuteButtonImage.enabled = false;
        }
        else {
            PlayerPrefs.SetInt("soundeffectsmuted", 0);
            muteButtonImage.enabled = false;
            unmuteButtonImage.enabled = true;
        }
    }

    public void PlayCoinCollectSound()
    {
        sounds.PlayOneShot(coinCollectSound);
    }

    public void PlayBiggerCoinCollectSound()
    {
        sounds.PlayOneShot(biggerCoinCollectSound);
    }

    public void PlayGodModeCollectSound()
    {
        sounds.PlayOneShot(godModeSound);
    }

    public void PlayObstacleDestroyedSound()
    {
        sounds.PlayOneShot(obstacleDestroyedSound);
    }

    public void PlayDeathSound()
    {
        sounds.PlayOneShot(deathSound);
    }
}
