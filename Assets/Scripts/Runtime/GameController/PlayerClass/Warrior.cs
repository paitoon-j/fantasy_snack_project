using UnityEngine;

public class Warrior : PlayerClass
{
    public Warrior()
    {
        health = 0;
        attack = 0;
        defense = 0;
        doubleDamage = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (this.enabled)
        {
            SetCollectHero(collision);
            switch (collision.gameObject.layer)
            {
                case (int)EKeyLayer.SPIDER:
                    CalculateAttackWithEnemy(collision);
                    Debug.Log("Attack Spider");
                    break;
                case (int)EKeyLayer.GHOST:
                    CalculateAttackWithEnemy(collision);
                    Debug.Log("Attack Ghost");
                    break;
                case (int)EKeyLayer.TITAN:
                    CalculateAttackDoubleWithEnemy(collision);
                    Debug.Log("Attack Titan");
                    break;
                default:
                    break;
            }
        }
    }
}