using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialAttack : MonoBehaviour
{
    public float duration;
    private float _timer;

    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= duration)
        {
            Destroy(gameObject);
        }
    }
}
