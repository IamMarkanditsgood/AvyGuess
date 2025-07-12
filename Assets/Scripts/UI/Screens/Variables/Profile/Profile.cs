using System;
using TMPro;
using UnityEngine.UI;

public class Profile : BasicScreen
{
    public Button back;
    public Button clearHistory;
    public Button avatar;

    public TMP_InputField _name;

    public TMP_Text _time;
    public TMP_Text _gamesPlayed;
    public TMP_Text _gamesWon;
    public TMP_Text _gamesLost;

    public AvatarManager avatarManager;

    public override void Init()
    {
        base.Init();
        _name.text = SaveManager.PlayerPrefs.LoadString("PlayerName", "Player");
    }
    private void OnApplicationQuit()
    {
        SaveManager.PlayerPrefs.SaveString("PlayerName", _name.text);
    }

    public override void Subscribe()
    {
        base.Subscribe();
        avatar.onClick.AddListener(avatarManager.PickFromGallery);
        back.onClick.AddListener(Back);
        clearHistory.onClick.AddListener(ClearHistory);
    }

    public override void UnSubscribe()
    {
        base.UnSubscribe();
        avatar.onClick.RemoveListener(avatarManager.PickFromGallery);
        back.onClick.RemoveListener(Back);
        clearHistory.onClick.RemoveListener(ClearHistory);
    }

    public override void ResetScreen()
    {

    }

    public override void SetScreen()
    {
        avatarManager.SetSavedPicture();
       
        _gamesPlayed.text = SaveManager.PlayerPrefs.LoadInt("GamesPlayed", 0).ToString();
        _gamesWon.text = SaveManager.PlayerPrefs.LoadInt("GamesWon", 0).ToString();
        _gamesLost.text = SaveManager.PlayerPrefs.LoadInt("GamesLost", 0).ToString();

        string time = "In game since";
        time += GetData();
        _time.text = time;

        SetHistory();
    }

    private void SetHistory()
    {

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

    private void Back()
    {
        UIManager.Instance.ShowScreen(ScreenTypes.MainMenu);
    }

    private void ClearHistory()
    {

    }
}
