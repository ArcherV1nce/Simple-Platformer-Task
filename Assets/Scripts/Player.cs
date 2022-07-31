using UnityEngine;

public class Player : Character
{
    private int _coins;

    public int Coins => _coins;

    public void AddCoins (Coin coin)
    {
        _coins += coin.Value;
        Debug.Log($"Player has {Coins} coins.");
    }
}
