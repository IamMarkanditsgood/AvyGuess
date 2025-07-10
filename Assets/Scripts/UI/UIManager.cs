using System;
using System.Linq;
using UnityEngine;

[Serializable]
public class UIManager
{
    [SerializeField] private BasicScreen[] _screens;
    [SerializeField] private BasicPopup[] _popups;
    [SerializeField] private ScreenTypes _firstLoadedScreen;

    public static UIManager Instance { get; private set; }

    public void Init()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        InitScreens();

        Subscribe();

        if (_firstLoadedScreen != ScreenTypes.Default)
        {
            ShowScreen(_firstLoadedScreen);
        }
    }

    public void Destroy()
    {
        if (Instance != null)
        {
            Instance = null;
        }
        UnSubscribe();
    }

    public void Subscribe()
    {
        foreach (var screen in _screens)
        {
            screen.Subscribe();
        }
    }

    public void UnSubscribe()
    {
        foreach (var screen in _screens)
        {
            screen.UnSubscribe();
        }
    }

    private void InitScreens()
    {
        foreach (var screen in _screens)
        {
            screen.Init();
        }
    }

    public BasicScreen GetScreen(ScreenTypes ScreenType)
    {
        var screen = _screens.FirstOrDefault(s => s.ScreenType == ScreenType);

        if (screen == null)
        {
            Debug.LogWarning($"Popup of type {ScreenType} was not found.");
        }

        return screen;
    }

    public BasicPopup GetPopup(PopupTypes popupType)
    {
        var popup = _popups.FirstOrDefault(p => p.PopupType == popupType);

        if (popup == null)
        {
            Debug.LogWarning($"Popup of type {popupType} was not found.");
        }

        return popup;
    }

    public void ShowScreen(ScreenTypes screenType, bool closeScreensBefore = true)
    {
        if (closeScreensBefore)
        {
            CloseAllScreens();
        }

        foreach (var screen in _screens)
        {
            if(screen.ScreenType == screenType)
            {
                Debug.Log(screenType);
                screen.Show();
            }
        }
    }

    public void ShowPopup(PopupTypes popupType)
    {

        foreach (var popup in _popups)
        {
            if (popup.PopupType == popupType)
            {
                popup.Show();
            }
        }
    }

    public void HideScreen(ScreenTypes screenType)
    {
        foreach (var screen in _screens)
        {
            if (screen.ScreenType == screenType)
            {
                screen.Hide();
            }
        }
    }

    public void HidePopup(PopupTypes popupType)
    {

        foreach (var popup in _popups)
        {
            if (popup.PopupType == popupType)
            {
                popup.Hide();
            }
        }
    }

    public void CloseAllScreens()
    {
        foreach (var screen in _screens)
        {
            screen.Hide();
        }
    }
}