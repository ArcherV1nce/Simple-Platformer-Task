using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Coin : MonoBehaviour
{
    private const int MinValue = 1;

    [SerializeField, Range (1, 50)] private int _value;

    public int Value => _value;

    private void OnValidate()
    {
        if (_value <= 0)
        {
            _value = MinValue;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        InteractWithPlayer(collision);
    }

    private void InteractWithPlayer(Collider2D collision)
    {
        if (collision.GetComponent<Player>())
        {
            collision.GetComponent<Player>().AddCoins(this);
        }

        Destroy(this.gameObject);
    }
}