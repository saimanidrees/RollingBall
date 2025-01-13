using GameData.MyScripts;
using UnityEngine;
using UnityEngine.UI;
public class SettingsScript : MonoBehaviour
{
    [SerializeField] private RectTransform[] buttons;
    [SerializeField] private Sprite soundOnSprite, soundOffSprite, musicOnSprite, musicOffSprite;
    private void OnEnable()
    {
        RefreshSoundSettings();
        RefreshMusicSettings();
    }
    public void SoundToggle()
    {
        SoundController.Instance.PlayBtnClickSound();
        PlayerPrefsHandler.SetSoundControllerBool("Sound", !PlayerPrefsHandler.GetSoundControllerBool("Sound"));
        RefreshSoundSettings();
    }
    public void MusicToggle()
    {
        SoundController.Instance.PlayBtnClickSound();
        if (PlayerPrefsHandler.GetSoundControllerBool("Music"))
        {
            PlayerPrefsHandler.SetSoundControllerBool("Music", false);
            SoundController.Instance.MuteBackgroundMusic();
        }
        else
        {
            PlayerPrefsHandler.SetSoundControllerBool("Music", true);
            SoundController.Instance.PlayBackgroundMusic();
        }
        RefreshMusicSettings();
    }
    private void RefreshSoundSettings()
    {
        if (PlayerPrefsHandler.GetSoundControllerBool("Sound"))
        {
            AudioListener.volume = 1;
            buttons[0].GetComponent<Image>().sprite = soundOnSprite;
        }
        else
        {
            if(!PlayerPrefsHandler.GetSoundControllerBool("Music"))
                AudioListener.volume = 0;
            buttons[0].GetComponent<Image>().sprite = soundOffSprite;
        }
    }
    private void RefreshMusicSettings()
    {
        if (PlayerPrefsHandler.GetSoundControllerBool("Music"))
        {
            AudioListener.volume = 1;
            buttons[1].GetComponent<Image>().sprite = musicOnSprite;
        }
        else
        {
            if(!PlayerPrefsHandler.GetSoundControllerBool("Sound"))
                AudioListener.volume = 0;
            buttons[1].GetComponent<Image>().sprite = musicOffSprite;
        }
    }
}