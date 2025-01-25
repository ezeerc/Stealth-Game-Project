using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Joystick : Controller, IDragHandler, IEndDragHandler, IPointerDownHandler
{
    private Vector3 _initialPosition;
    [SerializeField] private float maxMagnitude = 200;//regula la velocidad de movimiento conforme se aleja la palanca -mayor, más lento se mueve hasta que se aleja-
    private Transform _iso;
    public int rotationAngle = 45;

    private float _speedFactor; //extra para regular la velocidad del movimiento


    [SerializeField] private RectTransform joystickBackground; // Fondo del joystick
    [SerializeField] private RectTransform joystickHandle; // palanca del joystick
    [SerializeField] private Canvas canvas; // Canvas donde está el objeto
    [SerializeField] private float maxDistance = 100f; // distancia máxima desde el fondo a la palanca (cuánto se aleja).

    private Vector2 _initialPositionBackground;
    private Vector2 _input;
    private bool _isTouching;
    [SerializeField] private bool _dynamicJoystick = false;

    private void Start()
    {
        _iso = new GameObject().transform;
        _initialPosition = transform.position;

        if (joystickBackground != null)
            _initialPositionBackground = joystickBackground.anchoredPosition;

        ResetJoystick();
    }

    private void ResetJoystick()
    {
        if (!_dynamicJoystick) return;
        if (!_isTouching) return;

        joystickBackground.position = _initialPositionBackground;
        joystickHandle.anchoredPosition = Vector2.zero;
        _input = Vector2.zero;
        _isTouching = false;
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
            return new Vector3(modifiedDir.x, 0, modifiedDir.y).normalized * _speedFactor;
        }

        return new Vector3(0, 0, 0);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!_dynamicJoystick) return;
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            
            if (touch.position.x < Screen.width / 2)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    _isTouching = true;
                    Vector2 touchPosition = touch.position;
                    
                    RectTransformUtility.ScreenPointToWorldPointInRectangle(
                        canvas.transform as RectTransform,
                        touchPosition,
                        canvas.worldCamera,
                        out Vector3 localPoint
                    );
                    
                    joystickBackground.position = localPoint;
                    
                    joystickHandle.anchoredPosition = Vector2.zero;
                    
                    _initialPosition = localPoint;
                }

                if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
                {
                    Vector2 touchPosition = touch.position;
                    
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(
                        joystickBackground,
                        touchPosition,
                        canvas.worldCamera,
                        out Vector2 localPoint
                    );
                    
                    _input = Vector2.ClampMagnitude(localPoint, maxDistance);
                    
                    joystickHandle.anchoredPosition = _input;
                }
            }
        }
        else
        {
            ResetJoystick();
        }
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        _moveDir = Vector3.ClampMagnitude((Vector3)eventData.position - _initialPosition, maxMagnitude);
        transform.position = _initialPosition + _moveDir;
        _speedFactor = _moveDir.magnitude / maxMagnitude;
        MovingStick = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (_dynamicJoystick)
        {
            _moveDir = Vector3.zero;
            _speedFactor = 0;
            MovingStick = false;
            joystickHandle.anchoredPosition = Vector2.zero;
            ResetJoystick();
        }
        else
        {
            transform.position = _initialPosition;
            _moveDir = Vector3.zero;
            _speedFactor = 0;
            MovingStick = false;
        }
    }

    public void ChangeRotationAngle(int angle)
    {
        rotationAngle = (angle) * -1;
    }

    public void SetCenter()
    {
        transform.position = _initialPosition;
        _moveDir = Vector3.zero;
        _speedFactor = 0;
        MovingStick = false;
    }

    private void OnDisable()
    {
        ResetJoystick();
    }
}