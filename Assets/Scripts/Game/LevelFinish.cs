using UnityEngine.Events;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class LevelFinish : MonoBehaviour
{
    [SerializeField] private UnityEvent _onLevelFinish;

    private Collider2D _trigger;

    private void Awake()
    {
        SetUp();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        TryFinishScene(collision);
    }

    private void SetUp()
    {
        _trigger = GetComponent<Collider2D>();
        _trigger.isTrigger = true;
    }

    private bool TryFinishScene(Collider2D collider)
    {
        bool isPlayer = false;

        if (collider.TryGetComponent(out Player player))
        {
            isPlayer = true;
            _onLevelFinish.Invoke();
        }

        return isPlayer;
    }
}
