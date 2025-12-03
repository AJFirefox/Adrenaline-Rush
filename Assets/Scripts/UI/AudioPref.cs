using UnityEngine;
using UnityEngine.UI;

public class AudioPref : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public string sliderName;

    private Slider slider;
       void Start()
    {
        slider = GetComponent<Slider>();
        slider.value = PlayerPrefs.GetFloat(sliderName, 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetVolume(float newVolume)
    {
        PlayerPrefs.SetFloat(sliderName, newVolume);
    }
}
