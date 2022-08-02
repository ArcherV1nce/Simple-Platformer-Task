using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class Collectible : MonoBehaviour
{
    private const int MinValue = 1;

    [SerializeField, Range (1, 50)] private int _value;
    [SerializeField] private bool _isPickedUp;
    [SerializeField] private UnityEvent _onPickUp;

    public int Value => _value;
    public bool IsPickedUp => _isPickedUp;

    private void Awake()
    {
        SetUp();
    }

    private void OnValidate()
    {
        ValidateValue();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        InteractWithPlayer(collision);
    }

    private void InteractWithPlayer(Collider2D collision)
    {
        if (collision.GetComponent<Player>() && _isPickedUp == false)
        {
            collision.GetComponent<Player>().AddCoins(this);
            _isPickedUp = true;
            _onPickUp.Invoke();
        }
    }

    private void SetUp()
    {
        _isPickedUp = false;
    }

    private void ValidateValue()
    {
        if (_value <= 0)
        {
            _value = MinValue;
        }
    }

    private void DestoyObject ()
    {
        Destroy(this.gameObject);
    }
}