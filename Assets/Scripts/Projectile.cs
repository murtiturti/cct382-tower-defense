using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Transform _target;
    private int _damage;
    [SerializeField] private float speed;

    private enum DamageType
    {
        Projectile,
        AreaDamage,
        AreaEffect
    }
    [SerializeField] private DamageType damageType;
    
    [SerializeField] private float slowDownMultiplier;
    [SerializeField] private float areaRadius;
    [SerializeField] private float areaDuration;
    [SerializeField] private float areaDamageRate;

    private float _areaDurationTimer = 0f;
    private float _areaDamageTimer = 0f;
    private bool _startTimer = false;
    
    private List<Mob> _mobsInRange = new List<Mob>();

    public void SetTarget(Transform target, int damage) {
        _target = target;
        _damage = damage;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (_target == null && damageType == DamageType.Projectile) {
            Destroy(gameObject);
            return;
        }
        else if (_target == null && !_startTimer)
        {
            Destroy(gameObject);
            return;
        }

        if (!_startTimer)
        {
            transform.position = Vector3.MoveTowards(transform.position, _target.position, speed * Time.deltaTime);
            var sqrDist = (transform.position - _target.position).sqrMagnitude;
            if (sqrDist < 0.01f)
            {
                if (damageType == DamageType.Projectile)
                {
                    _target.gameObject.GetComponent<Mob>().TakeDamage(_damage);
                    Destroy(gameObject);
                }
                else
                {
                    FindMobsInRange();
                    _target = null;
                    _startTimer = true;
                
                    foreach (var mob in _mobsInRange)
                    {
                        if (damageType == DamageType.AreaEffect)
                        {
                            mob.AdjustSpeed(slowDownMultiplier);
                        }
                        mob.TakeDamage(_damage);
                    }
                }
            }
        }

        if (_startTimer)
        {
            _areaDurationTimer += Time.deltaTime;
            _areaDamageTimer += Time.deltaTime;
            if (_areaDamageTimer >= areaDamageRate)
            {
                _areaDamageTimer = 0f;
                FindMobsInRange();
                foreach (var mob in _mobsInRange)
                {
                    if (damageType == DamageType.AreaDamage)
                    {
                        mob.AdjustSpeed(slowDownMultiplier);
                    }
                    mob.TakeDamage(_damage);
                }
            }
            if (_areaDurationTimer >= areaDuration)
            {
                Debug.Log("Destroyed");
                Destroy(gameObject);
            }
        }
    }

    private void FindMobsInRange()
    {
        foreach (var mob in _mobsInRange)
        {
            mob.AdjustSpeed(1f);
        }
        _mobsInRange.Clear();
        var allMobs = FindObjectsOfType<Mob>().ToList();
        foreach (var mob in allMobs)
        {
            if (Vector3.Distance(transform.position, mob.transform.position) > areaRadius)
            {
                _mobsInRange.Add(mob);
            }
        }
    }
}
