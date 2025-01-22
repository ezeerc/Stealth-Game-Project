using System;
using Unity.VisualScripting;
using UnityEngine;

public class DirectionArrow : MonoBehaviour
{
    public Transform target;
    private GameObject arrow;

    public static event Action<DirectionArrow> OnCreatedArrow;
    private void Start()
    {
        OnCreatedArrow?.Invoke(this);
        Target.OnTargetCreated += GetTarget;
        arrow = this.GameObject();
        DeactivateArrow();
    }

    void Update()
    {
        if (!target) return;
        GetDirection();
    }

    void GetTarget(Transform newTarget)
    {
        Debug.Log("llaman al target");
        target = newTarget;
    }

    void GetDirection()
    {
        Vector3 relativePos = target.position - transform.position;
        
        Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
        transform.rotation = rotation;
    }

    public void DeactivateArrow()
    {
        arrow.SetActive(false);
    }

    public void ActivateArrow()
    {
        arrow.SetActive(true);
    }
}
