using System.Collections.Generic;
using UnityEngine;

public class CoinSpawn : MonoBehaviour
{
    [SerializeField] private Collectible _coin;
    [SerializeField] private Transform _parentObject;
    [SerializeField] List<Transform> _positions;

    private void Awake()
    {
        SpawnCoins();
    }

    private void SpawnCoins()
    {
        foreach (Transform transform in _positions)
        {
            Instantiate(_coin, transform.position, Quaternion.identity, _parentObject);
        }
    }
}