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
    private Music current = null;

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
            StartCoroutine(Crossfade(musicMap[musicName]));         
        }
    }

    public void PlaySFX(string sfxName){
        if(sfxMap.ContainsKey(sfxName)){
            audioSource.PlayOneShot(sfxMap[sfxName].clip, sfxMap[sfxName].volume);
        }
    }

    IEnumerator Crossfade(Music next){
        float duration = .5f;
        float elapsed = 0;

        if(current != null){
            while(elapsed < duration){
                float volume = Mathf.Lerp(current.volume, 0, elapsed / duration);
                audioSource.volume = volume;
                elapsed += Time.deltaTime;
                yield return null;
            }
            elapsed = 0;
        }

        duration = 1;
        current = next;
        audioSource.clip = current.song;
        audioSource.volume = current.volume;
        audioSource.loop = current.loop;
        audioSource.Play();

        while(elapsed < duration){
            float volume = Mathf.Lerp(0, current.volume, elapsed / duration);
            audioSource.volume = volume;
            elapsed += Time.deltaTime;
            yield return null;
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
    public bool loop;
    
    [Range(0,1)]
    public float volume = 1;
}
