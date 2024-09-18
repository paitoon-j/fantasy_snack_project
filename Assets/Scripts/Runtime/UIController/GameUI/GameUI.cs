using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField] private ButtonModule _pauseBtn;
    [SerializeField] private Image _frame;
    [SerializeField] private Sprite _warriorSprite;
    [SerializeField] private Sprite _rogueSprite;
    [SerializeField] private Sprite _wizardSprite;
    [SerializeField] private TextMeshProUGUI _hp;
    [SerializeField] private TextMeshProUGUI _atk;
    [SerializeField] private TextMeshProUGUI _def;
    [SerializeField] private TextMeshProUGUI _levelStage;

    public int levelCurrent { get; set; }
    private int _killEnemyCurrent = 0; // Current kill enemy
    private int _killEnemyMax = 5; // Every 5 kill enemy is increase stage level

    ////////////////////////////////////////////////

    public void Init(Action action)
    {
        _pauseBtn.On(action);
        levelCurrent = 1;
    }

    public void UpdateLevelHardGame(Action action)
    {
        _killEnemyCurrent++;
        if (_killEnemyCurrent >= _killEnemyMax)
        {
            _killEnemyCurrent = 0;
            levelCurrent++;
            _levelStage.text = "Level Stage : " + levelCurrent;
            AudioManager.Instance.PlaySFX((int)EKeyAudio.SFX_LEVEL_STAGE_UP);
            action.Invoke();
        }
    }

    public void SetFrame(int layer)
    {
        switch (layer)
        {
            case (int)EKeyLayer.WARRIOR:
                _frame.sprite = _warriorSprite;
                break;
            case (int)EKeyLayer.ROGUE:
                _frame.sprite = _rogueSprite;
                break;
            case (int)EKeyLayer.WIZARD:
                _frame.sprite = _wizardSprite;
                break;
            default:
                Debug.Log("not found class layer");
                break;
        }
    }

    public void SetStatus(int hp, int atk, int def)
    {
        _hp.text = "HP : " + hp.ToString();
        _atk.text = "ATK : " + atk.ToString();
        _def.text = "DEF : " + def.ToString();
    }
}
