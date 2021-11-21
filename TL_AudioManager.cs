using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class TL_AudioManager : MonoBehaviour
{
    //Mixers
    public AudioMixer MusicMixer;
    public AudioMixer SoundEffectMixer;

    //Sliders
    public Slider MusicSlider;
    public Slider SFXSlider;


    //Adjust the volume of the background music
    public void SetMusicVolume(float SliderValue)
    {
        MusicMixer.SetFloat("MusicVolume", Mathf.Log10(SliderValue) * 20f);
    }

    //Adjust the volume of the sound effects
    public void SetSoundEffectVolume(float SliderValue)
    {
        SoundEffectMixer.SetFloat("SoundEffectVolume", Mathf.Log10(SliderValue) * 20f);
    }

    void Awake()
    {
        //This gameobject does not get destroyed when it loads into a new scene
        DontDestroyOnLoad(transform.root.gameObject);
    }
}
