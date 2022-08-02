using UnityEngine;

[RequireComponent(typeof(Collectible), typeof(Animator))]
public class CollectiblesAnimationControl : MonoBehaviour
{
    private const string PickedUpParameter = "IsPickedUp";

    private Collectible _collectible;
    private Animator _animator;
    private bool _pickedUp;

    private void Awake()
    {
        SetUp();
    }

    public void SetParameters ()
    {
        _animator.SetBool(PickedUpParameter, _collectible.IsPickedUp);
    }

    private void SetUp()
    {
        _collectible = GetComponent<Collectible>();
        _animator = GetComponent<Animator>();
    }
}
