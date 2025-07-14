using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Levels : BasicScreen
{
    [SerializeField] private Button back;

    [SerializeField] private Button[] _categoryButtons;
    [SerializeField] private Sprite[] sellected;
    [SerializeField] private Sprite[] unSellected;

    [SerializeField] private GameConfig[] gameConfigs;

    [SerializeField] private Button play;

    private int _selectedCategoryIndex = -1;

    public override void Subscribe()
    {
        base.Subscribe();
        back.onClick.AddListener(Back);

        play.onClick.AddListener(OnPlayButtonClick);
        for (int i = 0; i < _categoryButtons.Length; i++)
        {
            int index = i;
            _categoryButtons[i].onClick.AddListener(() => OnCategoryButtonClick(index));
        }
    }
    public override void UnSubscribe()
    {
        base.UnSubscribe();
        back.onClick.RemoveListener(Back);
        play.onClick.RemoveListener(OnPlayButtonClick);
        for (int i = 0; i < _categoryButtons.Length; i++)
        {
            _categoryButtons[i].onClick.RemoveAllListeners();
        }
    }
    public override void ResetScreen()
    {
    }

    public override void SetScreen()
    {
        play.interactable = false;
    }
    private void Back()
    {
        UIManager.Instance.ShowScreen(ScreenTypes.Game2Teams);
    }
    public void OnCategoryButtonClick(int index)
    {
        if (_selectedCategoryIndex == index)
        {
            _selectedCategoryIndex = -1;
            play.interactable = false;
        }
        else
        {
            _selectedCategoryIndex = index;
            play.interactable = true;
        }
        UpdateCategoryButtons();
    }
    private void UpdateCategoryButtons()
    {
        for (int i = 0; i < _categoryButtons.Length; i++)
        {
            if (i == _selectedCategoryIndex)
            {
                _categoryButtons[i].GetComponent<Image>().sprite = sellected[i];
            }
            else
            {
                _categoryButtons[i].GetComponent<Image>().sprite = unSellected[i];
            }
        }
    }
    public void OnPlayButtonClick()
    {
        if (_selectedCategoryIndex >= 0 && _selectedCategoryIndex < gameConfigs.Length)
        {

            UIManager.Instance.ShowScreen(ScreenTypes.Game2Main);
        }
    }
}
