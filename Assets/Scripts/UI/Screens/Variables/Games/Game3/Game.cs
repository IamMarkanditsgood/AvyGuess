using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UI;
using static Game3Config;
using static TeamScreen;

public class Game : BasicScreen
{
    [SerializeField] private Game3Config gameConfig;
    [SerializeField] private Button back;
    [SerializeField] private Result3Manager resultManager;
    private List<Wrod> originalWords;
    private List<Wrod> remainingWords;
    [SerializeField] private TMP_Text _teamName;
    [SerializeField] private TMP_Text _timer;

    private bool isGamePlaying = false;

    private Coroutine timer;
    public List<Team> teams = new List<Team>();
    public List<bool> results = new List<bool>();
    private int _currentTeamIndex = 0;

    [SerializeField] private TMP_Text[] _wordLetters;
    [SerializeField] private Image[] _wordImages;

    [SerializeField] private TMP_Text[] _awailableLetters;
    [SerializeField] private Image[] _awailableImages;

    [SerializeField] private Image _mainImage;

    [SerializeField] private Sprite usedLatter;
    [SerializeField] private Sprite notUsedLatter;

    [SerializeField] private List<char> currentSelectedLetters = new();

    private Wrod currentWord = new();
    private int currentWordindex;

    private bool canPress = false;

    public int CurrentTeamIndex { get { return _currentTeamIndex; } set { _currentTeamIndex = value; } }

    public override void Subscribe()
    {
        base.Subscribe();
        
        back.onClick.AddListener(Back);

        for(int i = 0; i < _wordImages.Length; i++)
        {
            int index = i; // Capture the current index
            _wordImages[index].GetComponent<Button>().onClick.AddListener(() => LettedDeleted(index));
        }
        for (int i = 0; i < _awailableImages.Length; i++)
        {
            int index = i; // Capture the current index
            _awailableImages[index].GetComponent<Button>().onClick.AddListener(() => LetterPressed(index));
        }
    }
    public override void UnSubscribe()
    {
        base.UnSubscribe();
     
        back.onClick.RemoveListener(Back);
        for (int i = 0; i < _wordImages.Length; i++)
        {
            int index = i; // Capture the current index
            _wordImages[index].GetComponent<Button>().onClick.RemoveListener(() => LettedDeleted(index));
        }
        for (int i = 0; i < _awailableImages.Length; i++)
        {
            int index = i; // Capture the current index
            _awailableImages[index].GetComponent<Button>().onClick.RemoveListener(() => LetterPressed(index));
        }
    }
    public override void ResetScreen()
    {
        resultManager.Unsubscribe();
    }

    public override void SetScreen()
    {
        resultManager.Subscribe();
        originalWords = gameConfig.words;
        remainingWords = new List<Wrod>(originalWords);
        teams = DataManager.Instance.teams;
        StartGame();
    }
    public void StartGame()
    {

        remainingWords = new List<Wrod>(originalWords);
        results.Clear();
        foreach (var word in gameConfig.words)
        {
            results.Add(false);
        }
        _teamName.text = teams[_currentTeamIndex].name;
        
        SetWord();
        isGamePlaying = true;


        _timer.text = "Time: 60";


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

        isGamePlaying = false;
        ShowResults();
    }
    private void SetWord()
    {
        currentWordindex = -1;
        gameConfig.SetChars();
        foreach (var letter in _wordImages)
        {
            letter.gameObject.SetActive(false);
            letter.gameObject.GetComponent<Button>().interactable = false;
        }
        foreach (var letter in _awailableImages)
        {
            letter.gameObject.SetActive(true); 
            letter.gameObject.GetComponent<Button>().interactable = true;
        }
        currentSelectedLetters.Clear();

        currentWord = GetRandomUniqueWord();
        _mainImage.sprite = currentWord.closedImage;

        for (int i = 0; i < currentWord.wordLetters.Count; i++)
        {
            _wordLetters[i].gameObject.SetActive(false);
            _wordImages[i].sprite = notUsedLatter;
            _wordImages[i].gameObject.SetActive(true);
            
            currentSelectedLetters.Add('-');
        }

        for(int i = 0;i < _awailableLetters.Length; i++)
        {
            _awailableLetters[i].gameObject.SetActive(true);
            _awailableImages[i].sprite = usedLatter;
            _awailableLetters[i].text = currentWord.gameLetters[i].ToString();
        }
        canPress = true;
    }

    private void LetterPressed(int index)
    {
        char letter = currentWord.gameLetters[index];

        for(int i = 0; i < currentSelectedLetters.Count; i++)
        {
            if (currentSelectedLetters[i] == '-')
            {
                currentSelectedLetters[i] = letter;

                _wordLetters[i].text = letter.ToString();
                _wordLetters[i].gameObject.SetActive(true);

                _wordImages[i].sprite = usedLatter;
                _wordImages[i].GetComponent<Button>().interactable = true;

                _awailableLetters[index].gameObject.SetActive(false);
                _awailableImages[index].sprite = notUsedLatter;
                _awailableImages[index].GetComponent<Button>().interactable = false;

                if (i == currentSelectedLetters.Count - 1)
                {
                   StartCoroutine( WordDone());
                }
                return;
            }
        }

    }

    private void LettedDeleted(int index)
    {
        for(int i = 0; i < _awailableLetters.Length; i++)
        {
            if (_awailableLetters[i].text == currentSelectedLetters[index].ToString())
            {
                _awailableLetters[i].gameObject.SetActive(true);
                _awailableImages[i].sprite = usedLatter;
                _awailableImages[i].GetComponent<Button>().interactable = true;
            }
        }
        currentSelectedLetters[index] = '-';

        _wordLetters[index].gameObject.SetActive(false);

        _wordImages[index].sprite = notUsedLatter;
        _wordImages[index].GetComponent<Button>().interactable = false;

        

    }

    private IEnumerator WordDone()
    {
        canPress = false;

        if (SetResult(currentWord.word))
        {
            _mainImage.sprite = currentWord.openedImage;
        }

        yield return new WaitForSeconds(1f);
  
        SetWord();

    }

    private bool SetResult(string word)
    {
        string currentWord = "";
        for(int i = 0; i < currentSelectedLetters.Count; i++ )
        {
            currentWord += currentSelectedLetters[i];
        }

        bool result = currentWord == word;
        

        for(int i = 0; i < gameConfig.words.Count; i++)
        {
            if (gameConfig.words[i].word == word)
            {
                results[i] = result;
                break;
            }
        }
        return result;
    }

    private void Back()
    {
        UIManager.Instance.ShowScreen(ScreenTypes.Game3Teams);
    }
    private void ShowResults()
    {
        if (_currentTeamIndex < teams.Count - 1)
        {
            resultManager.ShowResults(_currentTeamIndex, results, gameConfig, this);
            _currentTeamIndex++;
        }
    }
    public Wrod GetRandomUniqueWord()
    {
        if (remainingWords.Count == 0)
        {
            Debug.LogWarning("All words have been used. Reshuffling.");
            StopCoroutine(timer);
            ShowResults();
            isGamePlaying = false;
            return null;
        }

        int index = UnityEngine.Random.Range(0, remainingWords.Count);
        Wrod word = remainingWords[index];

        remainingWords.RemoveAt(index);
        return word;
    }
}
[Serializable]
public class Result3Manager
{
    public GameObject view;
    public TMP_Text round;
    public TMP_Text teamName;
    public TMP_Text correctAnswers;

    public Button nextTeam;
    public Button newGame;

    private Game manager;
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

    public void ShowResults(int currentTeam, List<bool> results, Game3Config game3Config, Game game1Manager)
    {
        Screen.orientation = ScreenOrientation.Portrait;
        view.SetActive(true);
        manager = game1Manager;

        int correctneses = 0;
        for (int i = 0; i < results.Count; i++)
        {
            WordResult panel = UnityEngine.Object.Instantiate(result, container);
            panel.SetResult(results[i], game3Config.words[i].word);
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
        UIManager.Instance.ShowScreen(ScreenTypes.Game3Home);
    }
}