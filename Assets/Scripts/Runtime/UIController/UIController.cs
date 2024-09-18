using UnityEngine;

public class UIController : MonoBehaviour
{
    public GameUI Game { get => _gameUI; }
    [SerializeField] private GameUI _gameUI;

    public PopupUI Popup { get => _popupUI; }
    [SerializeField] private PopupUI _popupUI;

    public void Init()
    {
        _gameUI.Init(OpenPausePopup);
    }

    public void OpenPausePopup()
    {
        AudioManager.Instance.PlaySFX((int)EKeyAudio.SFX_CLICK);
        _popupUI.ShowPausePopup();
    }

    public void OpenGameOverPopup()
    {
        _popupUI.ShowGameOverPopup();
    }
}