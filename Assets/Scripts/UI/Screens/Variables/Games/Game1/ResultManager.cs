using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable] 
public class ResultManager
{
    public GameObject view;
    public TMP_Text round;
    public TMP_Text teamName;
    public TMP_Text correctAnswers;

    public Button nextTeam;
    public Button newGame;

    private Game1Manager manager;
    public Transform container;
    public WordResult result;

    public void Subscribe()
    {
        nextTeam.onClick.AddListener(NextTeam);
        newGame.onClick.AddListener(NewGame);
    }
    public void Unsubscribe()
    {
        nextTeam.onClick.RemoveListener(NextTeam);
        newGame.onClick.RemoveListener(NewGame);
    }

    public void ShowResults(int currentTeam, List<bool> results, Game1Manager game1Manager)
    {
        Screen.orientation = ScreenOrientation.Portrait;
        view .SetActive(true);
        manager = game1Manager;

        int correctneses = 0;
        for (int i = 0; i < DataManager.Instance.gameConfig.words.Count; i++)
        {
            WordResult panel = UnityEngine.Object.Instantiate(result, container);
            panel.SetResult(results[i], DataManager.Instance.gameConfig.words[i]);
            if (results[i])
                correctneses++;
        }

        correctAnswers.text = correctneses.ToString();
        teamName.text = DataManager.Instance.teams[currentTeam].name;
        round.text = "Round " +( currentTeam + 1);

        int teams = 0;
        foreach (var team in DataManager.Instance.teams)
        {
            if(team.isSelected)
                teams++;
        }

        if(currentTeam == teams - 1)
        {
            nextTeam.gameObject.SetActive(false);
            newGame.gameObject.SetActive(true);
        }
        else
        {
            nextTeam.gameObject.SetActive(true);
            newGame.gameObject.SetActive(false);

        }
    }

    private void NextTeam()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        manager.SetScreen();
        view.SetActive(false);
    }

    private void NewGame()
    {
        manager.CurrentTeamIndex = 0;
        view.SetActive(false);
        UIManager.Instance.ShowScreen(ScreenTypes.Game1Home);
    }
}
