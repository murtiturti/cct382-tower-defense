using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Playables;

public class Tower : MonoBehaviour, IPointerClickHandler
{
    public int[] cost;
    public int damage;
    public int level = 1;
    public float fireRate;
    public float range;
    public string type;
    public GameObject projectile;

    private Transform _target;
    private bool _hasTarget;

    private float _timer;

    private GameObject _towerRange;

    // DO NOT ASSIGN THESE VALUES
    public int x;
    public int y;
    public int z;

    private void Start()
    {
        _towerRange = transform.GetChild(0).gameObject;
        SpriteRenderer spriteRenderer = _towerRange.GetComponent<SpriteRenderer>();
        
        if (spriteRenderer != null)
        {
            // Get the size of the sprite in world units
            float spriteWorldUnitSize = spriteRenderer.bounds.size.x / _towerRange.transform.localScale.x;

            // Calculate the scale factor to make the sprite's radius match the range
            float scaleFactor = (range * 2) / spriteWorldUnitSize; // Diameter = range * 2

            // Apply the correct scale
            _towerRange.transform.localScale = new Vector3(scaleFactor, scaleFactor, 1);
        }
        _towerRange.SetActive(false);
    }

    private void Update()
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

    public void OnPointerClick(PointerEventData eventData)
    {
        TowerManager.instance.clickOnTower(this.gameObject);
    }

    public int getRefund()
    {
        int refund = 0;
        for (int i = 0; i < level;  i++)
        {
            refund += cost[i];
        }

        return (int)(0.7 * refund);
    }

    private void ChooseTarget()
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
            }
        }

        _target = closestEnemy;
    }

    private void TrackTarget()
    {
        if (_target == null)
        {
            _hasTarget = false;
            return;
        }
        float distSqr = (transform.position - _target.position).sqrMagnitude;
        if (distSqr > range * range)
        {
            _target = null;
            _hasTarget = false;
            _timer = 0f;
        }
    }

    private void Fire()
    {
        _timer += Time.deltaTime;
        if (_timer < fireRate)
        {
            return;
        }
        _timer = 0f;
        var go = Instantiate(projectile, transform.position, Quaternion.identity);
        go.GetComponent<Projectile>().SetTarget(_target, damage * level, type);
    }

    public void SetTilePosition(Vector3Int tilePosition)
    {
        x = tilePosition.x;
        y = tilePosition.y;
        z = tilePosition.z;
    }

    public void ToggleRange(bool toggle)
    {
        _towerRange.SetActive(toggle);
    }
}
