using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [Header("Player settings")] 
    [SerializeField] private Player playerPrefab;

    [SerializeField] private Joystick moveController;
    [SerializeField] private Joystick aimMoveController;
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private GameObject[] pickUpPrefabs;
    public Vector3 initialPosition;
    public int speed;

    [Header("Camera rotation settings")] 
    [SerializeField] private CameraRotator cameraRotator;

    public int currenCameraAngle = -45;

    [Header("Detection state")]
    public DetectionState detectionState = DetectionState.Hidden;

    public static GameManager Instance { get; private set; }
    public static Action FullActivity;
    public static Action NormalActivity;

    [Header("Menus settings")] 
    public GameObject winMenu;
    public GameObject loseMenu;

    [Header("Checkpoint settings")] 
    private Player playerCheckpoint;
    [SerializeField] private List<Enemy> enemiesCheckpoint;
    private CheckpointManager _checkpointManager;
    private bool _loseMenu = false;

    private DetectionStateLUT _detectionStateLut;
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
            .SetLayerMask(playerMask);


        player.InitPlayer(builder);
    }

    private void Start()
    {
        StartCoroutine(ResetSuscriptionCoroutine(2));
        _checkpointManager = new CheckpointManager();
        playerCheckpoint = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }
    

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5)) // Guarda checkpoint
        {
            _checkpointManager.SaveCheckpoint(playerCheckpoint, enemiesCheckpoint);
        }
        else if (Input.GetKeyDown(KeyCode.F9)) // Carga checkpoint
        {
            _checkpointManager.LoadCheckpoint(playerCheckpoint, enemiesCheckpoint);
            ResetLoseMenu();
        }
    }

    IEnumerator ResetSuscriptionCoroutine(int time)
    {
        _detectionStateLut = null;
        yield return new WaitForSeconds(time);
        _loseMenu = false;

        Target.OnTargetDeath -= WonMenu;
        Target.TargetWon -= LoseMenu;
        Player.OnDeath -= LoseMenu;
        CameraRotator.OnRotate -= RotateJoystickAngle;

        Target.OnTargetDeath += WonMenu;
        Target.TargetWon += LoseMenu;
        Player.OnDeath += LoseMenu;
        CameraRotator.OnRotate += RotateJoystickAngle;
        _detectionStateLut = new DetectionStateLUT(NormalActivity, FullActivity);
    }

    private void RotateJoystickAngle()
    {
        currenCameraAngle = cameraRotator.GetAngle();
        moveController.ChangeRotationAngle(currenCameraAngle);
        aimMoveController.ChangeRotationAngle(currenCameraAngle);
    }

    public void ChangeDetectionState(int detecctionNumber) //LUT
    {
        if (Enum.IsDefined(typeof(DetectionState), detecctionNumber))
        {
            detectionState = (DetectionState)detecctionNumber;
            _detectionStateLut.ExecuteAction(detectionState);
        }
        else
        {
            print("Invalid detection state number");
        }
    }

    public enum DetectionState
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
        _loseMenu = true;
    }

    private void ResetLoseMenu()
    {
        if (_loseMenu)
        {
            loseMenu.SetActive(false);
        }
    }


    private void OnDestroy()
    {
        Target.OnTargetDeath -= WonMenu;
        Target.TargetWon -= LoseMenu;
        Player.OnDeath -= LoseMenu;
        CameraRotator.OnRotate -= RotateJoystickAngle;
    }
}