using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class BasicPopup : MonoBehaviour
{
    [SerializeField] private GameObject _view;
    [SerializeField] private PopupTypes _popupType;
    [SerializeField] private Button _closeButton;
    [SerializeField] protected GameObject _firstSelectedButton;

    public bool isActive;

    public PopupTypes PopupType => _popupType;

    private void Start()
    {
        Subscribe();
    }

    private void OnDestroy()
    {
        Unsubscribe();
    }

    public virtual void Subscribe()
    {
        if(_closeButton != null)
        {
            _closeButton.onClick.AddListener(ClosePressed);
        }
    }

    public virtual void Unsubscribe()
    {
        if (_closeButton != null)
        {
            _closeButton.onClick.RemoveAllListeners();
        }
    }

    public virtual void Show()
    {
        isActive = true;
        _view.SetActive(true);
        if (_firstSelectedButton != null)
        {
            EventSystem.current.SetSelectedGameObject(_firstSelectedButton);
        }
        SetPopup();
       
    }


    public virtual void Hide()
    {
        isActive = false;
        EventSystem.current.SetSelectedGameObject(null);

        ResetPopup();
        _view.SetActive(false);
    }

    public virtual void ClosePressed()
    {
        Hide();
    }

    public abstract void SetPopup();

    public abstract void ResetPopup();

    public void SetFirstSelectedButton(GameObject button)
    {
        if (button != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(button);
        }
    }
}
