using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeamScreen : BasicScreen
{
    [SerializeField] private Button readTeams;
    [SerializeField] private GameObject teamList;
    [SerializeField] private Button next;
    [SerializeField] private ScreenTypes _nextloadedScreen;

    [SerializeField] private Button[] teamButtons;
    [SerializeField] private GameObject[] teamChooseSign;
    [SerializeField] private Button closeList;
    [SerializeField] private Button saveList;
    [SerializeField] private Button back;
    [SerializeField] private ScreenTypes _mainMenuScreen;

    [Serializable]
    public class Team
    {
        public string name;
        public GameObject teamImage;
        public bool isSelected;
        public Team Clone()
        {
            return new Team
            {
                name = this.name,
                teamImage = this.teamImage, 
                isSelected = this.isSelected
            };
        }
    }

    [SerializeField] private List<Team> teams;

    private int[] selectedTeams = new int[6];
    private bool save;

    public override void Subscribe()
    {
        base.Subscribe();
        readTeams.onClick.AddListener(ReadTeams);
        next.onClick.AddListener(Next);
        closeList.onClick.AddListener(CloseList);
        saveList.onClick.AddListener(Save);
        back.onClick.AddListener(Back);
        for (int i = 0; i < teamButtons.Length; i++)
        {
            int index = i; // Capture the current index
            teamButtons[i].onClick.AddListener(() => TeamPressed(index));
        }
    }

    public override void UnSubscribe()
    {
        base.UnSubscribe();
        readTeams.onClick.RemoveListener(ReadTeams);
        next.onClick.RemoveListener(Next);
        closeList.onClick.RemoveListener(CloseList);
        saveList.onClick.RemoveListener(Save);
        back.onClick.RemoveListener(Back);
        for (int i = 0; i < teamButtons.Length; i++)
        {
            int index = i; // Capture the current index
            teamButtons[i].onClick.RemoveListener(() => TeamPressed(index));
        }
    }

    public override void ResetScreen()
    {
        teamList.SetActive(false);
        foreach (var team in teams)
        {
            team.teamImage.SetActive(false);
            team.isSelected = false;
        }
        foreach (var team in teamChooseSign)
        {
            team.SetActive(false);
        }
        for (int i = 0; i < selectedTeams.Length; i++)
        {
            selectedTeams[i] = 0;
        }
    }

    public override void SetScreen()
    {
        for (int i = 0; i < selectedTeams.Length; i++)
        {
            selectedTeams[i] = 0;
        }

        SetTeamImages();
    }

    private void SetTeamImages()
    {
        next.interactable = false;
        foreach (var team in teams)
        {
            if (team.isSelected)
            {
                next.interactable = true;
                team.teamImage.SetActive(true);
            }
            else
            {
                team.teamImage.SetActive(false);
            }
        }
    }

    private void Next()
    {
        if (next.interactable)
        {
            DataManager.Instance.SetTeams(teams);
            UIManager.Instance.ShowScreen(_nextloadedScreen);
        }
    }
    private void ReadTeams()
    {
        teamList.SetActive(true);
        SetTeamList();
    }

    private void SetTeamList()
    {
        for (int i = 0; i < selectedTeams.Length; i++)
        {
            if (selectedTeams[i] == 1)
            {
                teamChooseSign[i].SetActive(true);
            }
            else
            {
                teamChooseSign[i].SetActive(false);
            }
        }
    }

    private void TeamPressed(int index)
    {
        if (selectedTeams[index] == 0)
        {
            teamChooseSign[index].SetActive(true);
            selectedTeams[index] = 1;
        }
        else
        {
            selectedTeams[index] = 0;
            teamChooseSign[index].SetActive(false);
        }
           
    }

    private void Save()
    {
        save = true;
        teamList.SetActive(false);
        for (int i = 0; i < selectedTeams.Length; i++)
        {
            teams[i].isSelected = selectedTeams[i] == 1;
        }
        SetTeamImages();
    }

    private void CloseList()
    {
        if (save)
        {
            for (int i = 0; i < selectedTeams.Length; i++)
            {
                if (teams[i].isSelected)
                {
                    selectedTeams[i] = 1;
                }
                else
                {
                    selectedTeams[i] = 0;
                }
            }
        }
        else
        {
            for (int i = 0; i < selectedTeams.Length; i++)
            {
                selectedTeams[i] = 0;
            }
        }
        teamList.SetActive(false);

        SetTeamList();
    }
    private void Back()
    {
        UIManager.Instance.ShowScreen(_mainMenuScreen);
        
    }
}
