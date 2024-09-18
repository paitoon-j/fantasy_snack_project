using UnityEngine;

public class PopupPause : PopupModule
{
    [SerializeField] private ButtonModule _resumeBtn;
    [SerializeField] private ButtonModule _quitBtn;

    public void Show()
    {
        Debug.Log("Show popup pause");
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
        SetResumeCallback();
        SetExitCallback();
    }

    private void UnSubscribeEvent()
    {
        _resumeBtn.Off();
        _quitBtn.Off();
    }

    private void SetResumeCallback()
    {
        _resumeBtn.On(() =>
        {
            AudioManager.Instance.PlaySFX((int)EKeyAudio.SFX_CLICK);
            Debug.Log("Close popup pause");
            Hide();
        });
    }

    private void SetExitCallback()
    {
        _quitBtn.On(() =>
        {
            AudioManager.Instance.PlaySFX((int)EKeyAudio.SFX_CLICK);
            Debug.Log("Exit Game");
            Helper.Exit();
        });
    }
}
