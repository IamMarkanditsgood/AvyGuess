using UnityEngine.UI;

public class MainMenu : BasicScreen
{
    public Button settings;
    public Button game1;
    public Button game2;
    public Button game3;
    public Button analytics;
    public Button profile;
    public Button info;

    public override void Subscribe()
    {
        base.Subscribe();
        settings.onClick.AddListener(Settings);
        game1.onClick.AddListener(Game1);
        game2.onClick.AddListener(Game2);
        game3.onClick.AddListener(Game3);
        analytics.onClick.AddListener(Analytics);
        profile.onClick.AddListener(Profile);
        info.onClick.AddListener(Info);
    }
    public override void UnSubscribe()
    {
        base.UnSubscribe();
        settings.onClick.RemoveListener(Settings);
        game1.onClick.RemoveListener(Game1);
        game2.onClick.RemoveListener(Game2);
        game3.onClick.RemoveListener(Game3);
        analytics.onClick.RemoveListener(Analytics);
        profile.onClick.RemoveListener(Profile);
        info.onClick.RemoveListener(Info);
    }

    public override void ResetScreen()
    {
    }

    public override void SetScreen()
    {
    }

    private void Settings()
    {
        UIManager.Instance.ShowScreen(ScreenTypes.Settings);
    }
    private void Game1()
    {
        UIManager.Instance.ShowScreen(ScreenTypes.Game1Home);
    }
    private void Game2()
    {
        UIManager.Instance.ShowScreen(ScreenTypes.Game2Home);
    }
    private void Game3()
    {
        UIManager.Instance.ShowScreen(ScreenTypes.Game3Home);
    }
    private void Analytics()
    {
        UIManager.Instance.ShowScreen(ScreenTypes.Analitics, false);
    }
    private void Profile()
    {
        UIManager.Instance.ShowScreen(ScreenTypes.Profile);
    }
    private void Info()
    {
        UIManager.Instance.ShowScreen(ScreenTypes.Info);
    }
}
