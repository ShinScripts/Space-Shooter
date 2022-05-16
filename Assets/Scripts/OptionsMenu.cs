using UnityEngine;
using UnityEngine.Audio;

public class OptionsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;


    public void SetVolume(float volume)
    {
        //* Om volumen är lika med eller mindra än -35, s'tt värdet till -80
        if (volume <= -35)
        {
            volume = -80;
        }
        //* Ändrar audiomixerns värde till volume
        audioMixer.SetFloat("musicVolume", volume);
    }

    public void Fullscreen(bool is_fullscreen)
    {
        //* Full screen = boolean av is_fullscreen (true or false)
        Screen.fullScreen = is_fullscreen;
        //* Om den är true, sätt fullscreen till true med upplösningen till 1080p
        if (is_fullscreen)
        {
            Screen.SetResolution(1920, 1080, true);
        }
        //* Om den är false, sätt fullscreen till false med upplösningen till 720p
        else
        {
            Screen.SetResolution(1280, 720, false);
        }
    }
}
