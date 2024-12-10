using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class RgdollController : MonoBehaviour
{
    public Rigidbody rb;
    public CapsuleCollider capsuleCollider;
    public Animator animator;
    
    public List<Rigidbody> rbs = new List<Rigidbody>();
    public List<Collider> colliders = new List<Collider>();
    
    [SerializeField] private NavMeshAgent navMeshAgent;
    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        rbs = GetComponentsInChildren<Rigidbody>().Skip(1).ToList();
        colliders = GetComponentsInChildren<Collider>().Skip(1).ToList();
        colliders.RemoveAt(7);
        colliders.RemoveAt(colliders.Count - 1);
        DeactivateRagdoll();
    }

    public void SetRigidbodyKinematic(bool kinematic)
    {
        foreach (Rigidbody rigid in rbs)
        {
            rigid.isKinematic = kinematic;
        }
    }
    
    public void SetCollidersEnabled(bool enabled)
    {
        foreach (Collider col in colliders)
        {
            col.enabled = enabled;
        }
    }
    public void ActivateRagdoll()
    {
        navMeshAgent.enabled = false;
        capsuleCollider.enabled = false;
        rb.isKinematic = true;
        animator.enabled = false;
        
        SetCollidersEnabled(true);
        SetRigidbodyKinematic(false);
    }
    
    public void DeactivateRagdoll()
    {
        capsuleCollider.enabled = true;
        rb.isKinematic = true;
        animator.enabled = true;
        
        SetCollidersEnabled(false);
        SetRigidbodyKinematic(true);
    }
    
    public void DeactivateRagdollDead()
    {
        capsuleCollider.enabled = false;
        rb.isKinematic = false;
        animator.enabled = false;
        
        SetCollidersEnabled(false);
        SetRigidbodyKinematic(false);
    }
}
