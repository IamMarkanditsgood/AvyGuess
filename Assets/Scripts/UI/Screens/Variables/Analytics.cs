using System;
using TMPro;
using UnityEngine.UI;

public class Analytics : BasicScreen
{
    public Button close;
    public TMP_Text name;
    public TMP_Text startData;

    public TMP_Text WhoAmIGamesPlayed;
    public TMP_Text AssociationsGamesPlayed;
    public TMP_Text ImageGamesPlayed;

    public TMP_Text WhoAmIGamesWined;
    public TMP_Text AssociationsGamesWined;
    public TMP_Text ImageGamesWined;

    public TMP_Text WhoAmIGamesLosed;
    public TMP_Text AssociationsGamesLosed;
    public TMP_Text ImageGamesLosed;

    public AvatarManager avatarManager;

    public override void Subscribe()
    {
        base.Subscribe();
        close.onClick.AddListener(Close);
    }

    public override void UnSubscribe()
    {
        base.UnSubscribe();
        close.onClick.RemoveListener(Close);
    }
    public override void ResetScreen()
    {
    }

    public override void SetScreen()
    {
        avatarManager.SetSavedPicture();
        name.text = SaveManager.PlayerPrefs.LoadString("PlayerName", "Player");

        string time = "In game since";
        time += GetData();
        startData.text = time;

        SetPlayedGames();
        SetWonGames();
        SetLosesGames();
    }

    private void SetPlayedGames()
    {
        WhoAmIGamesPlayed.text = SaveManager.PlayerPrefs.LoadInt("Game1Played", 0) + "games";
        AssociationsGamesPlayed.text = SaveManager.PlayerPrefs.LoadInt("Game2Played", 0) + "games";
        ImageGamesPlayed.text = SaveManager.PlayerPrefs.LoadInt("Game3Played", 0) + "games";
    }
    private void SetWonGames()
    {
        WhoAmIGamesWined.text = SaveManager.PlayerPrefs.LoadInt("Game1Won", 0).ToString();
        AssociationsGamesWined.text = SaveManager.PlayerPrefs.LoadInt("Game2Won", 0).ToString();
        ImageGamesWined.text = SaveManager.PlayerPrefs.LoadInt("Game3Won", 0).ToString();
    }
    private void SetLosesGames()
    {
        WhoAmIGamesLosed.text = SaveManager.PlayerPrefs.LoadInt("Game1Lossed", 0).ToString();
        AssociationsGamesLosed.text = SaveManager.PlayerPrefs.LoadInt("Game2Lossed", 0).ToString();
        ImageGamesLosed.text = SaveManager.PlayerPrefs.LoadInt("Game3Lossed", 0).ToString();
    }

    private string GetData()
    {
        const string firstPlayKey = "FirstPlayDate";

        if (!SaveManager.PlayerPrefs.IsSaved(firstPlayKey))
        {
            string currentDate = DateTime.Now.ToString("dd.MM.yyyy");
            SaveManager.PlayerPrefs.SaveString(firstPlayKey, currentDate);
        }

        string firstPlayDate = SaveManager.PlayerPrefs.LoadString(firstPlayKey, DateTime.Now.ToString("dd.MM.yyyy"));
        return " " + firstPlayDate;
    }

    private void Close()
    {
        UIManager.Instance.ShowScreen(ScreenTypes.MainMenu);
    }
}
