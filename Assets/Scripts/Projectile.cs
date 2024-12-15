using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Transform _target;
    private int _damage;
    private string _type;
    
    [SerializeField] private float speed;

    public float speedModifier;
    public float areaDuration;
    
    private Collider2D _collider;

    public enum DamageType
    {
        Projectile,
        Area,
        Splash
    }
    
    public DamageType damageType;

    private bool _hit;
    private float _areaTimer;

    public void SetTarget(Transform target, int damage, string type) {
        _target = target;
        _damage = damage;
        _type = type;
    }

    public int Damage()
    {
        return _damage;
    }

    public string Type()
    {
        return _type;
    }

    private void Start()
    {
        _hit = false;
        _collider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_target == null && damageType == DamageType.Projectile) {
            Destroy(gameObject);
            return;
        }
        else if (_target == null)
        {
            _hit = true;
        }

        if (!_hit)
        {
            transform.position = Vector3.MoveTowards(transform.position, _target.position, speed * Time.deltaTime);
            var sqrDist = (transform.position - _target.position).sqrMagnitude;
            if (sqrDist < 0.01f)
            {
                _hit = true;
                if (damageType == DamageType.Projectile)
                {
                    _target.gameObject.GetComponent<Mob>().TakeDamage(_damage, Type());
                    Destroy(gameObject);
                }
                else
                {
                    _collider.enabled = true;
                }
            }
        }
        else
        {
            _areaTimer += Time.deltaTime;
            if (_areaTimer >= areaDuration)
            {
                Destroy(gameObject);
            }
        }
       
    }
}
