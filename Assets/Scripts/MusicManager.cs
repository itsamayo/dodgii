using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{

    public AudioSource music;
    public AudioClip[] songs;

    public Button muteUnmuteButton;

    public Image muteButtonImage;
    public Image unmuteButtonImage;

    bool musicIsPlaying;
    bool hasStarted;

    public int startingPitch = 4;
    public int timeToDecreasePitch = 5;

    // Start is called before the first frame update
    void Start()
    {        
        music = GetComponent<AudioSource>();
        Button muteUnmute = muteUnmuteButton.GetComponent<Button>();
        muteUnmute.onClick.AddListener(MuteUnmute);
        if (PlayerPrefs.GetInt("musicmuted") == 1)
        {
            music.mute = true;
            muteButtonImage.enabled = true;
            unmuteButtonImage.enabled = false;
        }
        else
        {
            music.mute = false;
            muteButtonImage.enabled = false;
            unmuteButtonImage.enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        hasStarted = FindObjectOfType<GameManage>().hasStarted;

        if (!music.mute && !musicIsPlaying)
        {            
            musicIsPlaying = true;
            int randomSongIndex = Random.Range(0, 6);
            music.PlayOneShot(songs[randomSongIndex]);
            music.loop = true;
        }

        if (musicIsPlaying && !hasStarted)
        {
            music.volume = 0.05f;
            music.pitch = 0.5f;
        }

        if (musicIsPlaying && hasStarted)
        {
            music.volume = 0.1f;
            music.pitch = 1.0f;
        }

        // Cool music pitch change during godMode - disabled for now until I can 
        //if (hasStarted && musicIsPlaying && FindObjectOfType<PlayerController>() != null) 
        //{
        //    if (FindObjectOfType<PlayerController>().godMode)
        //    {
        //        while (music.pitch > 0.5) 
        //        {
        //            music.pitch = Time.deltaTime * startingPitch / timeToDecreasePitch;
        //        }
                
        //    }
        //    else {
        //        music.pitch = 1;
        //    }
        //}
    }

    void MuteUnmute() 
    {
        music.mute = !music.mute;
        if (music.mute)
        {
            PlayerPrefs.SetInt("musicmuted", 1);
            muteButtonImage.enabled = true;
            unmuteButtonImage.enabled = false;
        }
        else
        {
            PlayerPrefs.SetInt("musicmuted", 0);
            muteButtonImage.enabled = false;
            unmuteButtonImage.enabled = true;
        }
    }
}
