using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Entity, IDamageable
{
    public Transform[] wayPoints;


    private NavMeshAgent navMeshAgent;
    
    private int currentWayPointIndex = 0;
    [SerializeField] private int _distanceToFollowPath = 2;
    public Player _player;
    public bool followPlayer = false;
    private void Start()
    {
        Health = 100;
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = stats.baseSpeed;
    }

    private void Update()
    {
        EnemyPath();
        if (followPlayer)
        {
            FollowPlayer(_player.transform);
        }
    }

    public void TakeDamage(int amount)
    {
        Health -= amount;
    }

    public void FollowPlayer(Transform target)
    {
        _player = target.GetComponent<Player>();
        
        if(Vector3.Distance(transform.position, _player.transform.position) <= 10)
        {
            var destination = navMeshAgent.SetDestination(_player.transform.position);
        }
        else if (Vector3.Distance(transform.position, _player.transform.position) > 10)
        {
            followPlayer = false;
        }
    }
    
    private void EnemyPath()
    {
        navMeshAgent.destination = wayPoints[currentWayPointIndex].position;
        
        if (Vector3.Distance(transform.position, wayPoints[currentWayPointIndex].position) <= _distanceToFollowPath)
        {
            if (wayPoints[currentWayPointIndex] != wayPoints[^1])
            {
                currentWayPointIndex++;
            }
            else
            {
                currentWayPointIndex = 0;
            }
        }
    }

    private void Dead()
    {
        Destroy(gameObject);
    }
}
