using UnityEngine;

public class EnemyClass : MonoBehaviour
{
    public int health;
    public int attack;
    public int defense;
    public int doubleDamage;

    protected virtual void CalculateAttackWithHero(Collider2D collision)
    {
        PlayerClass hero = collision.GetComponent<PlayerClass>();
        int value = attack - hero.defense <= 0 ? 0 : attack - hero.defense;
        hero.health -= value;
    }

    protected virtual void CalculateAttackDoubleWithHero(Collider2D collision)
    {
        PlayerClass hero = collision.GetComponent<PlayerClass>();
        int value = (attack * doubleDamage) - hero.defense <= 0 ? 0 : (attack * doubleDamage) - hero.defense;
        hero.health -= value;
    }
}