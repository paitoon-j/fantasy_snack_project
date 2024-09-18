using UnityEngine;

public class Titan : EnemyClass
{
    public Titan()
    {
        health = 0;
        attack = 0;
        defense = 0;
        doubleDamage = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            switch (collision.gameObject.layer)
            {
                case (int)EKeyLayer.WARRIOR:
                    CalculateAttackWithHero(collision);
                    break;
                case (int)EKeyLayer.ROGUE:
                    CalculateAttackDoubleWithHero(collision);
                    break;
                case (int)EKeyLayer.WIZARD:
                    CalculateAttackWithHero(collision);
                    break;
                default:
                    Debug.Log("Hit something else!");
                    break;
            }
        }
    }
}