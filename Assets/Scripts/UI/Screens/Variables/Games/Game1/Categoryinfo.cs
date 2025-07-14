using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Categoryinfo : BasicScreen
{
    [SerializeField] private Button back;

    [SerializeField] private Button play;

    [SerializeField] private Image categoryImage;
    [SerializeField] private TMP_Text categoryNameText;
    [SerializeField] private TMP_Text categoryDescriptionText;

    [Serializable]
    public class CategoryData
    {
        public Sprite categoryImage;
        public string categoryName;
        public string categoryDescription;
    }

    [SerializeField] private CategoryData[] categories;

    private int _selectedCategoryIndex = -1;

    public void SetCategory(int index)
    {
        _selectedCategoryIndex = index;
    }
    public override void Subscribe()
    {
        base.Subscribe();
        back.onClick.AddListener(Back);

        play.onClick.AddListener(OnPlayButtonClick);
    }
    public override void UnSubscribe()
    {
        base.UnSubscribe();
        back.onClick.RemoveListener(Back);
        play.onClick.RemoveListener(OnPlayButtonClick);

    }
    public override void ResetScreen()
    {
    }

    public override void SetScreen()
    {
        categoryImage.sprite = categories[_selectedCategoryIndex].categoryImage;
        categoryNameText.text = categories[_selectedCategoryIndex].categoryName;
        categoryDescriptionText.text = categories[_selectedCategoryIndex].categoryDescription;
    }
    private void Back()
    {
        UIManager.Instance.ShowScreen(ScreenTypes.Game1Teams);
    }
    public void OnPlayButtonClick()
    {
        UIManager.Instance.ShowScreen(ScreenTypes.Game1Main);
    }
}
