using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Mob : MonoBehaviour
{
    [SerializeField] private List<int> path;
    private List<Transform> pathObjects;

    [SerializeField] private int health;
    [SerializeField] private float speed;
    [SerializeField] private int damage;
    [SerializeField] private string type;
    [SerializeField] private int reward;
    [SerializeField] private int scoreIncrease;
    [SerializeField] private IntEvent playerDamageEvent;
    [SerializeField] private IntEvent scoreGainEvent;
    [SerializeField] private IntEvent moneyGainEvent;
    
    //TODO: Add strengths and weaknesses
    
    private int _currentPathIndex = 0;
    private float _startSpeed;

    private void Awake()
    {
        _startSpeed = speed;
        var assistant = FindObjectOfType<PathAssistant>(); //TODO: Get Node transforms from PathAssistant as well
        path = assistant.ChooseRandomPath();
        pathObjects = new List<Transform>();
        var parent = GameObject.Find("Nodes");
        foreach (Transform child in parent.transform)
        {
            pathObjects.Add(child);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        transform.position = pathObjects[_currentPathIndex].position;
        _currentPathIndex++;
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, pathObjects[path[_currentPathIndex]].position, speed * Time.deltaTime);
        Navigate();
    }

    protected void Navigate()
    {
        if (Vector3.Distance(transform.position, pathObjects[path[_currentPathIndex]].position) < 0.01f)
        {
            _currentPathIndex++;
            if (_currentPathIndex >= path.Count)
            {
                playerDamageEvent.Raise(damage);
                Destroy(gameObject); //Todo: Object pooling
            }
        }
    }

    public void TakeDamage(int _damage, string projectileType)
    {
        if (string.Equals(projectileType, "burger", System.StringComparison.OrdinalIgnoreCase) &&
            string.Equals(type, "vegan", System.StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        if ((string.Equals(projectileType, "soup", System.StringComparison.OrdinalIgnoreCase) || 
             string.Equals(projectileType, "cake", System.StringComparison.OrdinalIgnoreCase)) && 
            string.Equals(type, "lactose intolerant", System.StringComparison.OrdinalIgnoreCase))
        {
            return;
        }
        health -= _damage;
        if (health <= 0)
        {
            moneyGainEvent.Raise(reward);
            scoreGainEvent.Raise(scoreIncrease);
            Destroy(gameObject);
        }
    }

    public void ModifySpeed(float modifier)
    {
        speed = _startSpeed * modifier;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var projectile = other.GetComponent<Projectile>();
        if (projectile == null)
        {
            return;
        }
        if (projectile.damageType == Projectile.DamageType.Area)
        {
            ModifySpeed(projectile.speedModifier);
        }
        else if (projectile.damageType == Projectile.DamageType.Splash)
        {
            TakeDamage(projectile.Damage(), projectile.Type());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        ModifySpeed(1f);
    }
}
