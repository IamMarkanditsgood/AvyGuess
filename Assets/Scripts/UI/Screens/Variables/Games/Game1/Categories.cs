using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Categories : BasicScreen
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
        for (int i = 0; i < _categoryButtons.Length; i++)
        {
            _categoryButtons[i].GetComponent<Image>().sprite = unSellected[i];
        }
        play.interactable = false;
    }
    private void Back()
    {
        UIManager.Instance.ShowScreen(ScreenTypes.Game1Teams);
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
            DataManager.Instance.SetConfig(gameConfigs[_selectedCategoryIndex]);

            Categoryinfo categoryinfo = (Categoryinfo) UIManager.Instance.GetScreen(ScreenTypes.Game1CategoryInfo);
            categoryinfo.SetCategory(_selectedCategoryIndex);
            //Add sending index of category to the next screen
            UIManager.Instance.ShowScreen(ScreenTypes.Game1CategoryInfo);
        }
    }
}
