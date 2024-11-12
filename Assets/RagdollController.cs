using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RagdollController : MonoBehaviour
{
    public Rigidbody rb;
    public CapsuleCollider capsuleCollider;
    public Animator animator;
    
    public List<Rigidbody> rbs = new List<Rigidbody>();
    public List<Collider> colliders = new List<Collider>();
    //public Rigidbody[] _rbs;
    //public Collider[]  _colliders;
    private void Awake()
    {
        //capsuleCollider.isTrigger = true;
        rbs = GetComponentsInChildren<Rigidbody>().Skip(1).ToList();
        //_rbs = GetComponentsInChildren<Rigidbody>().Skip(1).ToArray();
        //_colliders = GetComponentsInChildren<Collider>().Skip(1).ToArray();
        colliders = GetComponentsInChildren<Collider>().Skip(1).ToList();
        colliders.RemoveAt(colliders.Count - 1);
        DeactivateRagdoll();
    }

    /*private void SetCollidersEnabled(bool enabled)
    {
        foreach (Collider col in _colliders)
        {
            col.enabled = enabled;
        }
    }*/

    /*private void SetRigidbodyKinematic(bool kinematic)
    {
        foreach (Rigidbody rigid in _rbs)
        {
            rigid.isKinematic = kinematic;
        }
    }*/

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
        capsuleCollider.enabled = false;
        rb.isKinematic = true;
        animator.enabled = false;
        
        SetCollidersEnabled(true);
        SetRigidbodyKinematic(false);
    }
    
    public void DeactivateRagdoll()
    {
        capsuleCollider.enabled = true;
        rb.isKinematic = false;
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
