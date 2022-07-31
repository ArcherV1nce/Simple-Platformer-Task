using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Character : MonoBehaviour
{
    [SerializeField] protected int _health = 50;

    public virtual void ReceiveDamage (int damage)
    {
        _health -= damage;
    }
}
