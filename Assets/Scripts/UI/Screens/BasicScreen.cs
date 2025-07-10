using UnityEngine;
using UnityEngine.EventSystems;

public abstract class BasicScreen : MonoBehaviour
{
    [SerializeField] private GameObject _view;
    [SerializeField] protected GameObject _firstSelectedButton;
    [SerializeField] private ScreenTypes _screenType;
    [SerializeField] private bool _isLandscape; 

    [Header("ForTest")]
    [SerializeField] protected bool _isActive;

    public ScreenTypes ScreenType => _screenType;

    public virtual void Init()
    {
    }

    public virtual void Subscribe()
    {
    }

    public virtual void UnSubscribe()
    {
    }

    public virtual void Show()
    {
        if (_isLandscape)
        {
            SetLandscape();
        }
        else
        {
            SetPortrait();
        }

        _view.SetActive(true);
        _isActive = true;

        SetFirstSelectedButton(_firstSelectedButton);

        SetScreen();
    }
    public virtual void Hide() 
    {
        ResetScreen();
        EventSystem.current.SetSelectedGameObject(null);
        _view.SetActive(false);
        _isActive = false;
    }

    public abstract void SetScreen();

    public abstract void ResetScreen();

    public void SetFirstSelectedButton(GameObject button)
    {
        if (button != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(button);
        }
    }
    public void SetLandscape()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
    }

    public void SetPortrait()
    {
        Screen.orientation = ScreenOrientation.Portrait;
    }
}