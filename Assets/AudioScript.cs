using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
 
public class AudioScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private AudioMixer myMixer;
    [SerializeField] private Slider MusicSlider;
    //[SerializeField] private Slider sfxVolumeSlider;

    private void start()
    {
        SetMusicVolume();
    }
    public void SetMusicVolume()
    {
        float volume = MusicSlider.value;
        myMixer.SetFloat("Music", Mathf.Log10(volume)*20);
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }
    
    private void LoadVolume()
    {
        MusicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        SetMusicVolume();
    }
    /*public void SetSFXVolume()
    {
        float volume = sfxVolumeSlider.value; 
        myMixer.SetFloat("SFX", Mathf.Log10(Volume)*20)
    }*/
}
