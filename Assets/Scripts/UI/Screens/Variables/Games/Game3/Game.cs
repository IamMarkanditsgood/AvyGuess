using UnityEngine;
using UnityEngine.UI;

public class Game : BasicScreen
{
    [SerializeField] private Button back;

    public override void Subscribe()
    {
        base.Subscribe();
        back.onClick.AddListener(Back);
    }
    public override void UnSubscribe()
    {
        base.UnSubscribe();
        back.onClick.RemoveListener(Back);
    }
    public override void ResetScreen()
    {
    }

    public override void SetScreen()
    {
    }
    private void Back()
    {
        UIManager.Instance.ShowScreen(ScreenTypes.Game3Teams);
    }
}