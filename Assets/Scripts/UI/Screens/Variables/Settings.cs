using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : BasicScreen
{
    public Button sound;
    public Button music;
    public Button vibration;
    public Button back;

    public Image soindImage;
    public Image musicImage;
    public Image vibrationImage;

    public Sprite buttonON;
    public Sprite buttonOFF;

    public override void Subscribe()
    {
        base.Subscribe();
        sound.onClick.AddListener(Sound);
        music.onClick.AddListener(Music);
        vibration.onClick.AddListener(Vibration);
        back.onClick.AddListener(Back);
    }

    public override void UnSubscribe()
    {
        base.UnSubscribe();
        sound.onClick.RemoveListener(Sound);
        music.onClick.RemoveListener(Music);
        vibration.onClick.RemoveListener(Vibration);
        back.onClick.RemoveListener(Back);
       
    }

    public override void ResetScreen()
    {
    }

    public override void SetScreen()
    {
        if(!SaveManager.PlayerPrefs.IsSaved("Sound"))
        {
            SaveManager.PlayerPrefs.SaveInt("Sound", 1);
        }
        else
        {
            if(SaveManager.PlayerPrefs.LoadInt("Sound") == 1)
            {
                soindImage.sprite = buttonON;
            }
            else
            {
                soindImage.sprite = buttonOFF;
            }
        }

        if (!SaveManager.PlayerPrefs.IsSaved("Music"))
        {
            SaveManager.PlayerPrefs.SaveInt("Music", 1);
        }
        else
        {
            if (SaveManager.PlayerPrefs.LoadInt("Music") == 1)
            {
                musicImage.sprite = buttonON;
            }
            else
            {
                musicImage.sprite = buttonOFF;
            }
        }

        if (!SaveManager.PlayerPrefs.IsSaved("Vibration"))
        {
            SaveManager.PlayerPrefs.SaveInt("Vibration", 1);
        }
        else
        {
            if (SaveManager.PlayerPrefs.LoadInt("Vibration") == 1)
            {
                vibrationImage.sprite = buttonON;
            }
            else
            {
                vibrationImage.sprite = buttonOFF;
            }
        }
    }

    private void Sound()
    {

        if (SaveManager.PlayerPrefs.LoadInt("Sound") == 1)
        {
            SaveManager.PlayerPrefs.SaveInt("Sound", 0);
            soindImage.sprite = buttonOFF;
        }
        else
        {
            SaveManager.PlayerPrefs.SaveInt("Sound", 1);
            soindImage.sprite = buttonON;
        }
    }

    private void Music()
    {
        if (SaveManager.PlayerPrefs.LoadInt("Music") == 1)
        {
            SaveManager.PlayerPrefs.SaveInt("Music", 0);
            musicImage.sprite = buttonOFF;
        }
        else
        {
            SaveManager.PlayerPrefs.SaveInt("Music", 1);
            musicImage.sprite = buttonON;
        }
    }
    private void Vibration()
    {
        if (SaveManager.PlayerPrefs.LoadInt("Vibration") == 1)
        {
            SaveManager.PlayerPrefs.SaveInt("Vibration", 0);
            vibrationImage.sprite = buttonOFF;
        }
        else
        {
            SaveManager.PlayerPrefs.SaveInt("Vibration", 1);
            vibrationImage.sprite = buttonON;
        }
    }
    private void Back()
    {
        UIManager.Instance.ShowScreen(ScreenTypes.MainMenu);
    }
}
