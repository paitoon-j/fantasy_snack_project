using UnityEngine;

public class PlayerClass : MonoBehaviour
{
    public int health;
    public int attack;
    public int defense;
    public int doubleDamage;

    public PlayerControls controls;

    public void OnEnable()
    {
        controls = new PlayerControls();
        controls.Player.Enable();
    }

    public void OnDisable()
    {
        controls.Player.Disable();
    }

    protected virtual void SetCollectHero(Collider2D collision)
    {
        switch (collision.gameObject.layer)
        {
            case (int)EKeyLayer.WIZARD:
            case (int)EKeyLayer.ROGUE:
            case (int)EKeyLayer.WARRIOR:
                AudioManager.Instance.PlaySFX((int)EKeyAudio.SFX_HIT_COLLECT_HERO);
                SetHitCollision(collision);
                break;
            default:
                Debug.Log("Hit something else!");
                break;
        }
    }

    protected virtual void SetHitCollision(Collider2D collision)
    {
        collision.transform.SetParent(transform.parent);
        collision.GetComponent<Collider2D>().enabled = false;
    }

    protected virtual void CalculateAttackWithEnemy(Collider2D collision)
    {
        EnemyClass enemy = collision.GetComponent<EnemyClass>();
        int value = attack - enemy.defense <= 0 ? 0 : attack - enemy.defense;
        enemy.health -= value;
        OnDeath(enemy);
    }

    protected virtual void CalculateAttackDoubleWithEnemy(Collider2D collision)
    {
        EnemyClass enemy = collision.GetComponent<EnemyClass>();
        int value = (attack * doubleDamage) - enemy.defense <= 0 ? 0 : (attack * doubleDamage) - enemy.defense;
        enemy.health -= value;
        OnDeath(enemy);
    }

    protected virtual void OnDeath(EnemyClass enemy)
    {
        if (enemy.health <= 0)
        {
            Destroy(enemy.gameObject);
        }
    }
}