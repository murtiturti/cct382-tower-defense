using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private int health;
    [SerializeField] private int money;

    [SerializeField] private IntEvent takeDamage;
    [SerializeField] private IntEvent winMoney;

    [SerializeField] private IntVariable playerMoney;

    //TODO: Add special moves

    private void Start()
    {
        takeDamage.RegisterListener(OnDamageTaken);
        winMoney.RegisterListener(OnMoneyGain);
        playerMoney.Value = money;
    }

    public int Health()
    {
        return health;
    }

    private void OnMoneyGain(int moneyGain)
    {
        money += moneyGain;
        playerMoney.Value = money;
    }

    private void OnDamageTaken(int damage)
    {
        health -= damage;
    }

    private void OnDestroy()
    {
        takeDamage.UnregisterListener(OnDamageTaken);
    }
}
