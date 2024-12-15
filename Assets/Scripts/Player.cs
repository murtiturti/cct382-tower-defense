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

    [SerializeField] private GameObject specialAttack;
    [SerializeField] private float specialCooldown;
    private float _specialTimer;
    private Camera mainCam;

    private void Start()
    {
        takeDamage.RegisterListener(OnDamageTaken);
        winMoney.RegisterListener(OnMoneyGain);
        playerMoney.Value = money;
        mainCam = Camera.main;
    }

    private void Update()
    {
        _specialTimer += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Alpha1) && _specialTimer >= specialCooldown)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 1f;
            Vector3 worldPos = mainCam.ScreenToWorldPoint(mousePos);
            Instantiate(specialAttack, worldPos, Quaternion.identity);
            _specialTimer = 0f;
        }
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
