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
    private Transform _iso;
    public int rotationAngle = 45;

    private void Start()
    {
        _iso = new GameObject().transform;
        _initialPosition = transform.position;
        if (Camera.main != null) _camera = Camera.main.transform;
    }

    public override Vector3 GetMovementInput()
    {
        if (_iso)
        {
            _iso.rotation =
                Quaternion.Euler(0, 0,
                    rotationAngle); // modifica el ángulo de rotación de los inputs en función de la cámara (45°)
            var modifiedDir = new Vector3(_moveDir.x, _moveDir.y, 0);
            modifiedDir = _iso.TransformDirection(modifiedDir);
            modifiedDir /= maxMagnitude;
            return new Vector3(modifiedDir.x, 0, modifiedDir.y).normalized;
            
        }

        return new Vector3(0, 0, 0);
    }

    public void OnDrag(PointerEventData eventData)
    {
        _moveDir = Vector3.ClampMagnitude((Vector3)eventData.position - _initialPosition, maxMagnitude);
        transform.position = _initialPosition + _moveDir;
        MovingStick = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.position = _initialPosition;
        _moveDir = Vector3.zero;
        MovingStick = false;
    }

    public void ChangeRotationAngle(int angle)
    {
        rotationAngle = (angle)*-1;
    }

    public void SetCenter()
    {
        transform.position = _initialPosition;
        _moveDir = Vector3.zero;
        MovingStick = false;
    }
}