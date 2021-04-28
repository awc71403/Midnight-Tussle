using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{

    public static AudioManager instance;

    [SerializeField] private List<SFX> sfxList;
    [SerializeField] private List<Music> musicList;
    private AudioSource audioSource;

    private Dictionary<string, SFX> sfxMap = new Dictionary<string, SFX>();
    private Dictionary<string, Music> musicMap = new Dictionary<string, Music>();


    void Awake(){
        if (instance == null) {
            instance = this;
        }
        else if (instance != this) {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        foreach(SFX sfx in sfxList){
            sfxMap[sfx.name] = sfx;
        }

        foreach(Music music in musicList){
            musicMap[music.name] = music;
        }

        audioSource = GetComponent<AudioSource>();
    }



    public void PlayMusic(string musicName){
        if(musicMap.ContainsKey(musicName)){
            audioSource.clip = musicMap[musicName].song;
            audioSource.volume = musicMap[musicName].volume;
            audioSource.Play();
                       
        }
    }

    public void PlaySFX(string sfxName){
        if(sfxMap.ContainsKey(sfxName)){
            audioSource.PlayOneShot(sfxMap[sfxName].clip, sfxMap[sfxName].volume);
        }
    }


}

[System.Serializable]
public class SFX {
    public string name;
    public AudioClip clip;
    
    [Range(0,1)]
    public float volume = 1;
}

[System.Serializable]
public class Music {
    public string name;
    public AudioClip song;
    
    [Range(0,1)]
    public float volume = 1;
}
