using UnityEngine;

public class AttackPotion : MonoBehaviour
{
    public int increaseAttack = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        RecoveryHealth(collision);
    }

    private void RecoveryHealth(Collider2D collision)
    {
        switch (collision.gameObject.layer)
        {
            case (int)EKeyLayer.WIZARD:
            case (int)EKeyLayer.ROGUE:
            case (int)EKeyLayer.WARRIOR:
                RecoveryWithWizard(collision);
                break;
            default:
                Debug.Log("Hit something else!");
                break;
        }
    }

    private void RecoveryWithWizard(Collider2D collision)
    {
        PlayerClass player = collision.GetComponent<PlayerClass>();
        player.attack += increaseAttack;
        Destroy(gameObject);
    }
}