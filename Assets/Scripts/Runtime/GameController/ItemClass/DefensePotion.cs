using UnityEngine;

public class DefensePotion : MonoBehaviour
{
    public int increaseDefense = 0;

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
        player.defense += increaseDefense;
        Destroy(gameObject);
    }
}