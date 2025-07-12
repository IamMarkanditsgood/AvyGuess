using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Info : BasicScreen
{
    public Button privacy;
    public Button back;


    public override void Subscribe()
    {
        base.Subscribe();
        privacy.onClick.AddListener(Privacy);
        back.onClick.AddListener(Back);
    }

    public override void UnSubscribe()
    {
        base.UnSubscribe();

        privacy.onClick.RemoveListener(Privacy);
        back.onClick.RemoveListener(Back);

    }

    public override void ResetScreen()
    {
    }

    public override void SetScreen()
    {

    }
    private void Privacy()
    {
        Application.OpenURL("https://www.privacypolicygenerator.info/live.php?token");
    }
    private void Back()
    {
        UIManager.Instance.ShowScreen(ScreenTypes.MainMenu);
    }
}
