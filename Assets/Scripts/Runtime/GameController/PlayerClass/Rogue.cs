using UnityEngine;

public class Rogue : PlayerClass
{
    public Rogue()
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
                    CalculateAttackDoubleWithEnemy(collision);
                    Debug.Log("Attack Spider");
                    break;
                case (int)EKeyLayer.GHOST:
                    CalculateAttackWithEnemy(collision);
                    Debug.Log("Attack Ghost");
                    break;
                case (int)EKeyLayer.TITAN:
                    CalculateAttackWithEnemy(collision);
                    Debug.Log("Attack Titan");
                    break;
                default:
                    break;
            }
        }
    }
}