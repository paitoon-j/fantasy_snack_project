using UnityEngine;

public class Main : MonoBehaviour
{

    [SerializeField] public ButtonModule _startBtn = null;
    [SerializeField] public ButtonModule _exitBtn = null;

    private void Start()
    {
        AudioManager.Instance.PlayBGM((int)EKeyAudio.BGM_MAIN);
        SubscribeEvent();
    }

    private void SubscribeEvent()
    {
        _startBtn.On(LoadScene);
        _exitBtn.On(ExitGame);
    }

    private void UnSubscribeEvent()
    {
        _startBtn.Off();
        _exitBtn.Off();
    }

    private void LoadScene()
    {
        AudioManager.Instance.PlaySFX((int)EKeyAudio.SFX_CLICK);
        AudioManager.Instance.StopBGM();
        UnSubscribeEvent();
        Helper.LoadSceneGame();
        Debug.Log("Load Game");
    }

    private void ExitGame()
    {
        AudioManager.Instance.PlaySFX((int)EKeyAudio.SFX_CLICK);
        Helper.Exit();
        Debug.Log("Exit Game");
    }
}