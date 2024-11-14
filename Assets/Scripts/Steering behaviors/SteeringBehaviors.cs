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
}
