using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Damage : MonoBehaviour
{
    [SerializeField] private Collider2D _area;
    [SerializeField] private int _amount;

    public int Amount => _amount;

    private void Awake()
    {
        SetUp();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Character character))
        {
            character.ReceiveDamage(this);
        }
    }

    private void SetUp()
    {
        _area = GetComponent<Collider2D>();
        _area.isTrigger = true;
    }
}