using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Joystick : Controller, IDragHandler, IEndDragHandler
{
    private Vector3 _initialPosition;
    private Transform _camera;
    [SerializeField] private float maxMagnitude = 75;

    private void Start()
    {
        _initialPosition = transform.position;
        if (Camera.main != null) _camera = Camera.main.transform;
    }

    public override Vector3 GetMovementInput()
    {
        Transform iso = new GameObject ().transform;
        iso.rotation = Quaternion.Euler (0, 0, 45); // establece el ángulo de rotación de la cámara en función de la configuración
        var modifiedDir = new Vector3(_moveDir.x, _moveDir.y, 0);
        modifiedDir = iso.TransformDirection(modifiedDir);
        modifiedDir /= maxMagnitude;
        return new Vector3(modifiedDir.x, 0, modifiedDir.y).normalized;
    }

    public void OnDrag(PointerEventData eventData)
    {
        _moveDir = Vector3.ClampMagnitude((Vector3)eventData.position - _initialPosition, maxMagnitude);
        transform.position = _initialPosition + _moveDir;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.position = _initialPosition;
        _moveDir = Vector3.zero;
    }
}
