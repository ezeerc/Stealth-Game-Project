using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotator : MonoBehaviour
{

    private float _currentAngle = -45;
    private float _isometricAngle = 30;
    
    public void RotateRight()
    {
        _currentAngle -= 90f;
        RotateCamera();
    }

    public void RotateLeft()
    {
        _currentAngle += 90f;
        RotateCamera();
    }

    private void RotateCamera()
    {
        Quaternion newRotation = Quaternion.Euler(_isometricAngle, _currentAngle, 0);
        transform.rotation = newRotation;
    }
}
