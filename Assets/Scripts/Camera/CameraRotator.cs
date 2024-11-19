using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraRotator : MonoBehaviour
{

    private float _currentAngle = -45;
    private float _isometricAngle = 30;
    private bool _isRotating = false;
    public static event Action OnRotate;
    

    [SerializeField] float rotationSpeed = 5f;
    
    public void RotateRight()
    {
        if (!_isRotating)
        {
            _currentAngle -= 45f;
            StartCoroutine(SmoothRotate());
            OnRotate?.Invoke();
        }
    }

    public void RotateLeft()
    {
        if (!_isRotating)
        {
            _currentAngle += 45f;
            StartCoroutine(SmoothRotate());
            OnRotate?.Invoke();
        }
    }

    private IEnumerator SmoothRotate()
    {
        _isRotating = true;
        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(_isometricAngle, _currentAngle, 0);
        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime * rotationSpeed;
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);
            yield return null;
        }

        transform.rotation = targetRotation;
        _isRotating = false;
    }

    public int GetAngle()
    {
        return ((int)_currentAngle);
    }
}
