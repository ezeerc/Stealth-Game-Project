using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class DirectionArrow : MonoBehaviour
{
    public Transform target;
    private GameObject arrow;

    public static event Action<DirectionArrow> OnCreatedArrow;
    private void Start()
    {
        Target.OnTargetCreated += GetTarget;
        arrow = this.GameObject();
        OnCreatedArrow?.Invoke(this);
        DeactivateArrow();
    }

    void Update()
    {
        if (!target) return;
        GetDirection();
    }

    void GetTarget(Transform newTarget)
    {
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

    public void SetArrow(float seconds)
    {
        StartCoroutine(WaitToSet(seconds));
    }

    IEnumerator WaitToSet(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        OnCreatedArrow?.Invoke(this);
        Debug.Log("Arrow activated");
        DeactivateArrow();
    }
}
