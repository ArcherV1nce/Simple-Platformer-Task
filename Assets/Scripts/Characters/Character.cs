using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Character : MonoBehaviour
{
    [SerializeField] protected int _health = 50;

    public virtual void ReceiveDamage(Damage damage)
    {
        _health -= damage.Amount;

        if (_health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy (gameObject);
    }
}
