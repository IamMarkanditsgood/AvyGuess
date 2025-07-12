using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game3MainMenu : BasicScreen
{
    public Button home;

    public override void Subscribe()
    {
        base.Subscribe();
        home.onClick.AddListener(Home);
    }
    public override void UnSubscribe()
    {
        base.UnSubscribe();
        home.onClick.RemoveListener(Home);
    }
    public override void ResetScreen()
    {
    }

    public override void SetScreen()
    {
    }

    public void Home()
    {
        UIManager.Instance.ShowScreen(ScreenTypes.MainMenu);
    }
}
