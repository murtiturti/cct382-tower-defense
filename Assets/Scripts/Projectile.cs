using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Transform _target;
    [SerializeField] private float speed;
    
    public void SetTarget(Transform target) {
        _target = target;
    }
    
    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, _target.position, speed * Time.deltaTime);
        var sqrDist = (transform.position - _target.position).sqrMagnitude;
        if (sqrDist < 0.01f) {
            Destroy(gameObject);
            //hurt mob
            return;
        }
    }
}
