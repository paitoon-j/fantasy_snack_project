using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class GameController : MonoBehaviour
{
    [SerializeField] public Tilemap backgroundTilemap;
    [SerializeField] public Tilemap obstacleTilemap;
    [SerializeField] public Tilemap trapTilemap;
    [SerializeField] public GameObject collectGroup;
    [SerializeField] public GameObject enemyGroup;

    private float _moveSpeedDuration = 0.2f;
    private int _moveDistance = 1;
    private bool _isMoving = false;
    private List<Vector2> _posList = new List<Vector2>();

    public PlayerClass player { get; set; }

    #region get data

    private bool IsBlocked(Vector2 targetPosition)
    {
        return obstacleTilemap.GetTile(new Vector3Int((int)targetPosition.x, (int)targetPosition.y, 0)) != null;
    }

    private bool IsTraped(Vector2 targetPosition)
    {
        return trapTilemap.GetTile(new Vector3Int((int)targetPosition.x, (int)targetPosition.y, 0)) != null;
    }

    private bool IsReturningToPreviousPosition(Vector2 targetPosition)
    {
        return collectGroup.transform.childCount > 1 && targetPosition == _posList[1];
    }

    private List<Vector2> GetCurrentPositions()
    {
        List<Vector2> positions = new List<Vector2>();
        foreach (Transform child in collectGroup.transform)
        {
            positions.Add(child.position);
        }
        return positions;
    }

    private Vector2 GetTargetPosition(Vector2 moveInput)
    {
        float targetX = Mathf.FloorToInt(player.transform.position.x + moveInput.x * _moveDistance);
        float targetY = Mathf.FloorToInt(player.transform.position.y + moveInput.y * _moveDistance);
        return new Vector2(targetX, targetY);
    }

    private Vector2 GetRandomValidPosition(Vector3 enemyPosition)
    {
        int targetX = Mathf.FloorToInt(enemyPosition.x * _moveDistance);
        int targetY = Mathf.FloorToInt(enemyPosition.y * _moveDistance);
        int randomDirection = Helper.GetRandomRang(0, 1);
        int randomX = randomDirection == 0 ? Helper.GetRandomRang(targetX - 1, targetX + 1) : targetX;
        int randomY = randomDirection == 1 ? Helper.GetRandomRang(targetY - 1, targetY + 1) : targetY;
        return new Vector2(randomX, randomY);
    }

    public bool HandleHitTrap()
    {
        Vector3Int cellPosition = trapTilemap.WorldToCell(player.transform.position);
        TileBase tile = trapTilemap.GetTile(cellPosition);
        if (tile != null)
        {
            return true;
        }
        return false;
    }

    public bool IsPlayerDead()
    {
        return player.health <= 0 ? true : false;
    }

    public bool GameOver()
    {
        if (HasDuplicates(_posList) || collectGroup.transform.childCount == 0)
        {
            return true;
        }
        return false;
    }

    private bool HasDuplicates(List<Vector2> list)
    {
        HashSet<Vector2> seen = new HashSet<Vector2>();
        foreach (Vector2 pos in list)
        {
            if (!seen.Add(pos)) return true;
        }
        return false;
    }

    #endregion

    //////////////////////////////////////////////////////////////////////

    public void Init()
    {
        SetPlayerControl(0);
        for (int i = 0; i < collectGroup.transform.childCount; i++)
        {
            _posList.Add(collectGroup.transform.GetChild(i).position);
        }
    }

    private void SetPlayerControl(int index)
    {
        if (index < collectGroup.transform.childCount)
        {
            GameObject playerClass = collectGroup.transform.GetChild(index).gameObject;
            player = playerClass.GetComponent<PlayerClass>();
            EnablePlayerControl(true);
        }
    }

    public void EnablePlayerControl(bool isActive)
    {
        player.GetComponent<BoxCollider2D>().enabled = isActive;
        player.enabled = isActive;
        player.tag = isActive == true ? "Player" : "Untagged";
    }

    public async UniTask SwitchCharacterAsync(bool isMoveForward)
    {
        if (!_isMoving && collectGroup.transform.childCount > 1)
        {
            _isMoving = true;
            player.GetComponent<BoxCollider2D>().enabled = false;
            List<Vector2> newPosList = GetCurrentPositions();
            SetSiblingCollectGroup(isMoveForward);
            AnimateCharacterSwitch(newPosList);
            await Helper.DelayAsync(_moveSpeedDuration);
            SetPlayerControl(0);
            _isMoving = false;
        }
    }

    public void MovePlayer(InputAction.CallbackContext ctx, Action action)
    {
        Vector2 moveInput = ctx.ReadValue<Vector2>();
        Vector2 targetPosition = GetTargetPosition(moveInput);

        if (IsBlocked(targetPosition) || IsReturningToPreviousPosition(targetPosition))
        {
            return;
        }

        if (!_isMoving)
        {
            _isMoving = true;
            _posList.Insert(0, targetPosition);
            AnimatePlayerMove(targetPosition, action);
            AnimateGroupMove();
        }
    }

    private void AnimatePlayerMove(Vector2 targetPosition, Action complete)
    {
        player.transform
            .DOMove(targetPosition, _moveSpeedDuration)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                complete.Invoke();
            });
    }

    private void AnimateGroupMove()
    {
        int startIndex = 1;
        for (int i = startIndex; i < collectGroup.transform.childCount; i++)
        {
            collectGroup.transform.GetChild(i)
                .DOMove(_posList[i], _moveSpeedDuration)
                .SetEase(Ease.Linear);
        }
    }

    //////////////////////////////////////////////////////////////////////

    public void DestroyPlayer()
    {
        Destroy(player.gameObject);
        SetPlayerControl(1);
        _posList.RemoveAt(0);
    }

    //////////////////////////////////////////////////////////////////////

    public void EnemyMove(Action complete)
    {
        ClearOldPositions();
        OnEnemyMoveAsync(complete);
    }

    private void ClearOldPositions()
    {
        int max = collectGroup.transform.childCount + 1;
        if (_posList.Count >= max)
        {
            _posList.RemoveAt(_posList.Count - 1);
        }
    }

    private async void OnEnemyMoveAsync(Action complete)
    {
        if (enemyGroup.transform.childCount > 0)
        {
            MoveAllEnemies();
            await Helper.DelayAsync(0.1f);
            complete.Invoke();
            _isMoving = false;
        }
    }

    //////////////////////////////////////////////////////////////////////

    private void SetSiblingCollectGroup(bool isMoveForward)
    {
        Transform collect = collectGroup.transform;
        int childCount = collect.childCount;

        if (isMoveForward)
        {
            Transform lastChild = collect.GetChild(childCount - 1);
            lastChild.SetSiblingIndex(0);
        }
        else
        {
            Transform firstChild = collect.GetChild(0);
            firstChild.SetSiblingIndex(childCount - 1);
        }
    }

    private void AnimateCharacterSwitch(List<Vector2> positions)
    {
        for (int i = 0; i < collectGroup.transform.childCount; i++)
        {
            collectGroup.transform.GetChild(i)
                .DOMove(positions[i], _moveSpeedDuration)
                .SetEase(Ease.Linear);
        }
    }

    private void MoveAllEnemies()
    {
        Transform enemy = enemyGroup.transform;

        for (int i = 0; i < enemy.childCount; i++)
        {
            Transform child = enemy.GetChild(i);
            Vector2 moveTarget = GetRandomValidPosition(child.position);

            if (IsBlocked(moveTarget) || IsTraped(moveTarget))
            {
                Debug.Log("Enemy can't move to position");
                continue;
            }

            MoveEnemy(child, moveTarget);
        }
    }

    private void MoveEnemy(Transform enemy, Vector2 targetPosition)
    {
        enemy.DOMove(targetPosition, _moveSpeedDuration).SetEase(Ease.Linear);
    }
}