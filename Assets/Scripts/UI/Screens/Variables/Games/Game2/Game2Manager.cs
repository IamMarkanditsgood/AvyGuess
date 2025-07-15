using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static TeamScreen;

public class Game2Manager : BasicScreen
{
    [SerializeField] private Result2Manager resultManager;

    [SerializeField] private Button backButton;

    [SerializeField] private TMP_Text _teamName;
    [SerializeField] private TMP_Text _timer;
    [SerializeField] private TMP_Text _word;
    [SerializeField] private TMP_Text _correctAnswers;
    [SerializeField] private TMP_Text _wrongAnswers;

    public List<Team> teams = new List<Team>();
    public GameConfig gameConfig;
    public List<bool> results = new List<bool>();

    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;
    private float minSwipeDistance = 50f;
    private bool isSwiping = false;

    private int _correctAnswered = 0;
    private int _wrongAnswered = 0;

    private int _currentWordIndex = 0;
    private int _currentTeamIndex = 0;

    public int CurrentTeamIndex { get { return _currentTeamIndex; } set { _currentTeamIndex = value; } }

    private List<string> originalWords;
    private List<string> remainingWords;

    private Coroutine timer;

    void Update()
    {
        if (!isSwiping)
            return;

#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            startTouchPosition = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0))
        {
            endTouchPosition = Input.mousePosition;
            DetectSwipe();
        }
#else
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                startTouchPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                endTouchPosition = touch.position;
                DetectSwipe();
            }
        }
#endif
    }

    void DetectSwipe()
    {
        Vector2 swipeDelta = endTouchPosition - startTouchPosition;

        if (swipeDelta.magnitude < minSwipeDistance)
            return;

        if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y))
        {
            
                
        }
        else
        {
            if (swipeDelta.y > 0)
                SwipeCorrect();
            else
                 SwipeWrong();
        }
    }

    public override void Subscribe()
    {
        base.Subscribe();
        
        backButton.onClick.AddListener(Back);
    }

    public override void UnSubscribe()
    {
        base.UnSubscribe();
       
        backButton.onClick.RemoveListener(Back);
    }

    public override void ResetScreen()
    {
        resultManager.Unsubscribe();
    }

    public override void SetScreen()
    {
        resultManager.Subscribe();
        gameConfig = DataManager.Instance.gameConfig;
        teams = DataManager.Instance.teams;
        originalWords = DataManager.Instance.gameConfig.words;
        remainingWords = new List<string>(originalWords);
        _timer.text = "Time: 60";
        StartGame();
    }

    private void Back()
    {
        if (timer != null)
        {
            StopCoroutine(timer);
        }
        UIManager.Instance.ShowScreen(ScreenTypes.Game2Home);
    }

    public void StartGame()
    {

        remainingWords = new List<string>(originalWords);
        results.Clear();
        foreach (var word in gameConfig.words)
        {
            results.Add(false);
        }
        SetCurrentTeam();
        SetWord();
        isSwiping = true;

        _correctAnswered = 0;
        _wrongAnswered = 0;

        _correctAnswers.text = _correctAnswered + "Correct";
        _wrongAnswers.text = _wrongAnswered + "Skipped";

        _timer.text = "Time: 60";

        _currentWordIndex = 0;

        SetCurrentTeam();
        if (timer != null)
        {
            StopCoroutine(timer);
        }
        timer = StartCoroutine(GameTimer());
    }

    private IEnumerator GameTimer()
    {
        int timer = 60;
        while (timer > 0)
        {
            yield return new WaitForSeconds(1f);
            timer--;
            _timer.text = "Time: " + timer;
        }

        isSwiping = false;
        ShowResults();
    }

    private void ShowResults()
    {
        if (_currentTeamIndex < teams.Count - 1)
        {
            resultManager.ShowResults(_currentTeamIndex, results, this);
            _currentTeamIndex++;
        }
    }

    private void SetCurrentTeam()
    {
        _teamName.text = teams[_currentTeamIndex].name;
    }

    private void SetWord()
    {
        _word.text = GetRandomUniqueWord();
        _currentWordIndex++;
    }
    public string GetRandomUniqueWord()
    {
        if (remainingWords.Count == 0)
        {
            Debug.LogWarning("All words have been used. Reshuffling.");
            isSwiping = false;
            StopCoroutine(timer);
            ShowResults();
            return "";
        }

        int index = UnityEngine.Random.Range(0, remainingWords.Count);
        string word = remainingWords[index];
        remainingWords.RemoveAt(index);
        return word;
    }

    private void SwipeCorrect()
    {
        _correctAnswered++;
        _correctAnswers.text = _correctAnswered + "Correct";
        results[_currentWordIndex] = true;
        SetWord();
    }

    private void SwipeWrong()
    {
        _wrongAnswered++;
        _wrongAnswers.text = _wrongAnswered + "Skipped";
        results[_currentWordIndex] = false;
        SetWord();
    }
}
[Serializable]
public class Result2Manager
{
    public GameObject view;
    public TMP_Text round;
    public TMP_Text teamName;
    public TMP_Text correctAnswers;

    public Button nextTeam;
    public Button newGame;

    private Game2Manager manager;
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

    public void ShowResults(int currentTeam, List<bool> results, Game2Manager game1Manager)
    {
        Screen.orientation = ScreenOrientation.Portrait;
        view.SetActive(true);
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
        round.text = "Round " + (currentTeam + 1);

        int teams = 0;
        foreach (var team in DataManager.Instance.teams)
        {
            if (team.isSelected)
                teams++;
        }

        if (currentTeam == teams - 1)
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
        UIManager.Instance.ShowScreen(ScreenTypes.Game2Home);
    }
}
