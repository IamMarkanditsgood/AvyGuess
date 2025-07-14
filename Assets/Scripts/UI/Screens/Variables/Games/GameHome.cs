using UnityEngine;
using UnityEngine.UI;

public class GameHome : BasicScreen
{

    [SerializeField] private Button continueGame;
    [SerializeField] private Button newGame;
    [SerializeField] private Button home;

    [SerializeField] private ScreenTypes _teamScreen;

    public override void Subscribe()
    {
        base.Subscribe();
        continueGame.onClick.AddListener(Continue);
        newGame.onClick.AddListener(NewGame);
        home.onClick.AddListener(Home);
    
    }
    public override void UnSubscribe()
    {
        base.UnSubscribe();
        continueGame.onClick.RemoveListener(Continue);
        newGame.onClick.RemoveListener(NewGame);
        home.onClick.RemoveListener(Home);
    }
    public override void ResetScreen()
    {
    }

    public override void SetScreen()
    {
    }
    private void Continue()
    {
        UIManager.Instance.ShowScreen(_teamScreen);
    }
    private void NewGame()
    {
        UIManager.Instance.ShowScreen(_teamScreen);
    }
    private void Home()
    {
        UIManager.Instance.ShowScreen(ScreenTypes.MainMenu);
    }
}
