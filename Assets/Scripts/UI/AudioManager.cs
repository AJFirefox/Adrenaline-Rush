using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public AudioMixer masterMixer;
    public AudioSource backgroundMusic;
    public static AudioManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        masterMixer.SetFloat("MusicVolume", PlayerPrefs.GetFloat("Music",1));
        masterMixer.SetFloat("MasterVolume", PlayerPrefs.GetFloat("Master",1));

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
