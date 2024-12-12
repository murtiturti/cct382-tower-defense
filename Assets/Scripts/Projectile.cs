using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Transform _target;
    private int _damage;
    [SerializeField] private float speed;
    
    public void SetTarget(Transform target, int damage) {
        _target = target;
        _damage = damage;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (_target == null) {
            Destroy(gameObject);
            return;
        }
        transform.position = Vector3.MoveTowards(transform.position, _target.position, speed * Time.deltaTime);
        var sqrDist = (transform.position - _target.position).sqrMagnitude;
        if (sqrDist < 0.01f) {
            _target.gameObject.GetComponent<Mob>().TakeDamage(_damage);
            Destroy(gameObject);
            //hurt mob
            return;
        }
    }
}
