using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public static class SteeringBehaviors
{
    public static void Seek(NavMeshAgent agent, Vector3 location)
    {
        agent.SetDestination(location);
    }

    public static void Flee(NavMeshAgent agent, Vector3 location)
    {
        Vector3 fleeVector = location - agent.transform.position;
        agent.SetDestination(agent.transform.position - fleeVector);
    }
    
    public static void FaceBehavior(NavMeshAgent agent, Player player, int angularSpeed)
    {
        var rb = agent.GetComponent<Rigidbody>();
        Vector3 direction = player.transform.position - agent.transform.position;
        float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        rb.rotation = Quaternion.Euler(agent.transform.rotation.eulerAngles.y, angle, angularSpeed * Time.deltaTime);
    }

    public static void Evade(NavMeshAgent agent, Rigidbody agentRigidbody,Player player)
    {
        Vector3 targetDir = player.transform.position - agent.transform.position;
        float lookAhead = targetDir.magnitude/(agent.speed + agentRigidbody.velocity.magnitude);
        Flee(agent, player.transform.position + player.transform.forward * lookAhead);
    }
    
    
}