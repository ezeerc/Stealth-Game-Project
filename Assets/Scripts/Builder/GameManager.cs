using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Player playerPrefab;
    [SerializeField] private Joystick moveController;
    [SerializeField] private Joystick aimMoveController;
    public Vector3 initialPosition;
    public int speed;
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private SneakSkill sneakSkill;
    [SerializeField] private GameObject[] pickUpPrefabs;
    public int currenCameraAngle = -45;
    [SerializeField] private CameraRotator cameraRotator;

    public DetectionState detectionState = DetectionState.Hidden; //////// TOMI //////////////////////////////////
    public static GameManager Instance { get; private set; }

    public GameObject winMenu;
    public GameObject loseMenu;

    private void Awake()
    {
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        } 
        
        Player player = Instantiate(playerPrefab);

        PlayerBuilder builder = new PlayerBuilder()
            .SetMoveController(moveController)
            .SetAimMoveController(aimMoveController)
            .SetMinHealth(0)
            .SetMaxHealth(100)
            .SetPosition(initialPosition)
            .SetSpeed(speed)
            .SetLayerMask(playerMask)
            .SetSneakSkill(sneakSkill);


        player.InitPlayer(builder);
    }

    private void Start()
    {
        StartCoroutine(ResetSuscriptionCoroutine(1));
    }
    
    IEnumerator ResetSuscriptionCoroutine(int time)
    {
        yield return new WaitForSeconds(time);
        
        Target.OnTargetDeath -= WonMenu;
        Target.TargetWon -= LoseMenu;
        Player.OnDeath -= LoseMenu;
        CameraRotator.OnRotate -= RotateJoystickAngle;

        Target.OnTargetDeath += WonMenu;
        Target.TargetWon += LoseMenu;
        Player.OnDeath += LoseMenu;
        CameraRotator.OnRotate += RotateJoystickAngle;
    }

    private void RotateJoystickAngle()
    {
        currenCameraAngle = cameraRotator.GetAngle();
        moveController.ChangeRotationAngle(currenCameraAngle);
        aimMoveController.ChangeRotationAngle(currenCameraAngle);
    }

    public void ChangeDetectionState(int detecctionNumber) // hacer switch
    {
        if (detecctionNumber == 0)
        {
            detectionState = DetectionState.Hidden;
        }

        else if (detecctionNumber == 1)
        {
            detectionState = DetectionState.Alerted;
        }

        else
        {
            detectionState = DetectionState.Detected;
        }
    }

    public enum DetectionState //////////////////////////////// TOMI //////////////////////////////////
    {
        Hidden,
        Alerted,
        Detected
    }

    public void InstantietePrefab(Vector3 position)
    {
        Instantiate(pickUpPrefabs[Random.Range(0, pickUpPrefabs.Length)], position, Quaternion.identity);
    }

    private void WonMenu()
    {
        winMenu.SetActive(true);

    }

    private void LoseMenu()
    {
        loseMenu.SetActive(true);
    }
    
    private void OnDestroy()
    {
        Target.OnTargetDeath -= WonMenu;
        Target.TargetWon -= LoseMenu;
        Player.OnDeath -= LoseMenu;
        CameraRotator.OnRotate -= RotateJoystickAngle;
    }
}