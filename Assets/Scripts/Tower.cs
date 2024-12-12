using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public int cost;
    public int damage;
    public float fireRate;
    public float range;
    public GameObject projectile;
    
    private bool _isPlaced;
    private Transform _target;
    private bool _hasTarget;

    private float _timer;
    
    //TODO: Add type and upgrades
    
    void Start()
    {
        _isPlaced = false;
    }
    
    void Update()
    {
        if (!_hasTarget)
        {
            ChooseTarget();
        }
        else
        {
            Fire();
            TrackTarget();
        }
    }

    void ChooseTarget()
    {
        GameObject[] mobs = GameObject.FindGameObjectsWithTag("Mob");

        var closestDistanceSqr = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach (GameObject mob in mobs)
        {
            float distSqr = (transform.position - mob.transform.position).sqrMagnitude;

            if (distSqr < closestDistanceSqr && distSqr <= range * range)
            {
                closestDistanceSqr = distSqr;
                closestEnemy = mob.transform;
                _hasTarget = true;
                Debug.Log("Found target");
            }
        }

        _target = closestEnemy;
    }

    void TrackTarget()
    {
        if (_target == null)
        {
            return;
        }
        float distSqr = (transform.position - _target.position).sqrMagnitude;
        if (distSqr > range * range)
        {
            _target = null;
            _hasTarget = false;
            _timer = 0f;
            Debug.Log("Lost target");
        }
    }

    void Fire()
    {
        _timer += Time.deltaTime;
        if (_timer < fireRate)
        {
            return;
        }
        _timer = 0f;
        var go = Instantiate(projectile, transform.position, Quaternion.identity);
        go.GetComponent<Projectile>().SetTarget(_target);
    }
}
