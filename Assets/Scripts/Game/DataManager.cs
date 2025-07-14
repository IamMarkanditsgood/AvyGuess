using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static TeamScreen;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; }

    public List<Team> teams = new List<Team>();

    public GameConfig gameConfig;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void SetConfig(GameConfig newGameConfig)
    {
        gameConfig = newGameConfig;
    }
    public void SetTeams(List<Team> newTeams)
    {
        teams.Clear();

        foreach (var team in newTeams)
        {
            teams.Add(team.Clone());
        }
    }

}
