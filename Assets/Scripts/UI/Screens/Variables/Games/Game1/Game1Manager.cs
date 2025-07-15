using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static TeamScreen;

public class Game1Manager : BasicScreen
{
    [SerializeField] private ResultManager resultManager;
    [SerializeField] private GameObject screen1;
    [SerializeField] private Button continueButton;
    [SerializeField] private GameObject screen2;
    [SerializeField] private Button startButton;
    [SerializeField] private GameObject screen3;

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
            if (swipeDelta.x > 0)
                SwipeWrong();

            else
                SwipeCorrect();
        }
        else
        {
            if (swipeDelta.y > 0)
                Debug.Log("Swipe Up");
            else
                Debug.Log("Swipe Down");
        }
    }

    public override void Subscribe()
    {
        base.Subscribe();
        
        backButton.onClick.AddListener(Back);
        continueButton.onClick.AddListener(Continue);
        startButton.onClick.AddListener(StartGame);
    }

    public override void UnSubscribe()
    {
        base.UnSubscribe();
        resultManager.Unsubscribe();
        backButton.onClick.RemoveListener(Back);
        continueButton.onClick.RemoveListener(Continue);
        startButton.onClick.RemoveListener(StartGame);
    }

    public override void ResetScreen()
    {
        resultManager.Unsubscribe();
        screen1.SetActive(false);
        screen2.SetActive(false);
        screen3.SetActive(false);
    }

    public override void SetScreen()
    {
        resultManager.Subscribe();
        gameConfig = DataManager.Instance.gameConfig;
        teams = DataManager.Instance.teams;
        originalWords = DataManager.Instance.gameConfig.words;
        remainingWords = new List<string>(originalWords);
        screen1.SetActive(true);
        screen2.SetActive(false);
        screen3.SetActive(false);
        _timer.text = "Time: 60";
    }

    private void Back()
    {
        if (timer != null)
        {
            StopCoroutine(timer);
        }
        UIManager.Instance.ShowPopup(PopupTypes.Game1Exit);
    }

    private void Continue()
    {
        screen1.SetActive(false);
        screen2.SetActive(true);
        screen3.SetActive(false);
    }
    public void StartGame()
    {

        remainingWords = new List<string>(originalWords);
        results.Clear();
        foreach (var word in gameConfig.words)
        {
            results.Add(false);
        }
        screen1.SetActive(false);
        screen2.SetActive(false);
        screen3.SetActive(true);
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
        if(_currentTeamIndex < teams.Count -1)
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
