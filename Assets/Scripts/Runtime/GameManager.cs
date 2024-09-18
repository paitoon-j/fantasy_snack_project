using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameController _controller;
    [SerializeField] private SpawnController _spawn;
    [SerializeField] private UIController _ui;

    private int _moveCurrentTimes = 0;
    private int _moveTimesMax = 10; // Every move 10 Times is Spawn Potion
    private int _collectCount = 0;
    private int _enemyCount = 0;

    private void Start()
    {
        AudioManager.Instance.PlayBGM((int)EKeyAudio.BGM_GAME);
        Init();
        SetValue();
        BindPlayerControls();
        UpdatePlayerUI();
        UpdateSpawnHero();
        UpdateSpawnEnemy();
        UpdateSpawnPotion();
    }

    private void Init()
    {
        _controller.Init();
        _spawn.Init(_controller.obstacleTilemap, _controller.trapTilemap);
        _ui.Init();
    }

    private void SetValue()
    {
        _enemyCount = _controller.enemyGroup.transform.childCount;
        _collectCount = _controller.collectGroup.transform.childCount;
    }

    private void BindPlayerControls()
    {
        var playerControls = _controller.player.controls.Player;
        playerControls.SwitchQ.started += ctx => SetSwitchCharacterAsync(true);
        playerControls.SwitchE.started += ctx => SetSwitchCharacterAsync(false);
        playerControls.Movement.started += OnMovePerformed;
    }

    private async void SetSwitchCharacterAsync(bool isMoveForward)
    {
        if (_ui.Popup.IsShowPopup) return;
        await _controller.SwitchCharacterAsync(isMoveForward);
        AudioManager.Instance.PlaySFX((int)EKeyAudio.SFX_SWITCH_HERO);
        UpdatePlayerUI();
    }

    private void OnMovePerformed(InputAction.CallbackContext ctx)
    {
        if (_ui.Popup.IsShowPopup) return;
        _controller.MovePlayer(ctx, OnPlayerMoveComplete);
    }

    private void OnPlayerMoveComplete()
    {
        UpdateMovementTimes();
        OnDeath();
        UpdatePlayerUI();
        UpdateSpawnEnemy();
        UpdateSpawnPotion();
        _controller.EnemyMove(OnEnemyMoveCompleteAsync);
    }

    private async void OnEnemyMoveCompleteAsync()
    {
        if (_controller.IsPlayerDead())
        {
            if (_controller.player != null)
            {
                OnDeath();
                await Helper.DelayAsync(0.1f);
            }
        }
        GameOver();
    }

    private void UpdateMovementTimes()
    {
        _moveCurrentTimes++;
    }

    private void UpdatePlayerUI()
    {
        PlayerClass player = _controller.player;
        _ui.Game.SetFrame(player.gameObject.layer);
        _ui.Game.SetStatus(player.health, player.attack, player.defense);
    }

    private void UpdateSpawnHero()
    {
        GameObject collectGroup = _controller.collectGroup;
        Tilemap obstacleTilemap = _controller.obstacleTilemap;
        Tilemap trapTilemap = _controller.trapTilemap;

        if (_collectCount != collectGroup.transform.childCount)
        {
            _spawn.SetSpawnHero(obstacleTilemap, trapTilemap);
            _collectCount = collectGroup.transform.childCount;
            Debug.Log("Spawn Hero");
        }
    }

    private void UpdateSpawnEnemy()
    {
        GameObject enemyGroup = _controller.enemyGroup;
        Tilemap obstacleTilemap = _controller.obstacleTilemap;
        Tilemap trapTilemap = _controller.trapTilemap;

        if (_enemyCount != enemyGroup.transform.childCount)
        {
            _spawn.SetSpawnEnemy(obstacleTilemap, trapTilemap);
            _enemyCount = enemyGroup.transform.childCount;
            _ui.Game.UpdateLevelHardGame(() => _spawn.EnemyLevelUp(_ui.Game.levelCurrent));
            Debug.Log("Spawn Enemy");
        }
    }

    private void UpdateSpawnPotion()
    {
        Tilemap obstacleTilemap = _controller.obstacleTilemap;
        Tilemap trapTilemap = _controller.trapTilemap;

        if (_moveCurrentTimes >= _moveTimesMax)
        {
            _spawn.SetSpawnPotion(obstacleTilemap, trapTilemap);
            _moveCurrentTimes = 0;
            Debug.Log("Spawn Potion");
        }
    }

    private void OnDeath()
    {
        if (_controller.HandleHitTrap() || _controller.IsPlayerDead())
        {
            _controller.DestroyPlayer();
            BindPlayerControls();
            _collectCount -= 1;
        }
        else
        {
            UpdateSpawnHero();
        }
    }

    private void GameOver()
    {
        if (_controller.GameOver())
        {
            _ui.Popup.ShowGameOverPopup();
        }
    }
}