using UnityEngine;

public class PopupGameOver : PopupModule
{
    [SerializeField] private ButtonModule _restartBtn;

    public void Show()
    {
        Debug.Log("Show popup game over");
        base.Open();
        SubscribeEvent();
    }

    public void Hide()
    {
        UnSubscribeEvent();
        base.Close();
    }

    private void SubscribeEvent()
    {
        SetRestartCallback();
    }

    private void UnSubscribeEvent()
    {
        _restartBtn.Off();
    }

    private void SetRestartCallback()
    {
        _restartBtn.On(() =>
        {
            AudioManager.Instance.PlaySFX((int)EKeyAudio.SFX_CLICK);
            Helper.LoadSceneGame();
            Hide();
        });
    }
}