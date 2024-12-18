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
    public event Action OnRestart;

    [Header("Menus settings")] 
    public GameObject winMenu;
    public GameObject loseMenu;

    [Header("Checkpoint settings")] 
    private Player playerCheckpoint;
    [SerializeField] private List<Enemy> enemiesCheckpoint;
    private CheckpointManager _checkpointManager;
    private bool _loseMenu = false;
    
    [Header("Audio settings")] 
    private AudioSource _source;
    [SerializeField] private AudioClip _getClipWin;
    [SerializeField] private AudioClip _getClipLose;
    
    
    private DetectionStateLUT _detectionStateLut;
    private bool hasAddedMoney = false;
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
        MusicManager.Instance.StopAudio();
        StartCoroutine(ResetSuscriptionCoroutine(2));
        _checkpointManager = new CheckpointManager();
        playerCheckpoint = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        _source = GetComponent<AudioSource>();
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

    public void SaveGame()
    {
        StartCoroutine(WaitTimeCoroutine());
        _checkpointManager.SaveCheckpoint(playerCheckpoint, enemiesCheckpoint);
    }

    IEnumerator WaitTimeCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        GetActiveEnemies();
    }

    public void LoadGame()
    {
        _oneTimeEnemies = false;
        ResetLoseMenu();
        OnRestart?.Invoke();
        StartCoroutine(ResetSuscriptionCoroutine(0.1f));
        _checkpointManager.LoadCheckpoint(playerCheckpoint, enemiesCheckpoint);
    }
    
    IEnumerator ResetSuscriptionCoroutine(float time)
    {
        _detectionStateLut = null;
        yield return new WaitForSeconds(time);
        _loseMenu = false;
        hasAddedMoney = false;
        MusicManager.Instance.PlaySameAudio();

        Target.OnTargetDeath -= WonMenu;
        Target.TargetWon -= LoseMenu;
        Player.OnDeath -= LoseMenu;
        CameraRotator.OnRotate -= RotateJoystickAngle;

        Target.OnTargetDeath += WonMenu;
        Target.TargetWon += LoseMenu;
        Player.OnDeath += LoseMenu;
        CameraRotator.OnRotate += RotateJoystickAngle;
        _detectionStateLut = new DetectionStateLUT(NormalActivity, FullActivity);
        ScreenManager.instance.ShowScreen("WelcomeMessage");
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

    /*public void InstantietePrefab(Vector3 position)
    {
        Instantiate(pickUpPrefabs[Random.Range(0, pickUpPrefabs.Length)], position, Quaternion.identity);
    }*/

    private void WonMenu()
    {
        ScreenManager.instance.ShowScreen("WinScreen");
        if (!hasAddedMoney)
        {
            GameManager.Instance.StartCoroutine(WinLoseMusic(_getClipWin));
            //MusicManager.Instance.StopAudio();
            //GetSfx(_getClipWin);
            CurrencyManager.Instance.AddMoney(10);
            hasAddedMoney = true;
            
        }
    }

    public void LoseMenu()
    {
        GameManager.Instance.StartCoroutine(WinLoseMusic(_getClipLose));
        ScreenManager.instance.ShowScreen("GameOverScreen");
        _loseMenu = true;
    }

    private void ResetLoseMenu()
    {
        if (_loseMenu)
        {
            ScreenManager.instance.HideScreen("GameOverScreen");
        }
    }


    private void OnDestroy()
    {
        Target.OnTargetDeath -= WonMenu;
        Target.TargetWon -= LoseMenu;
        Player.OnDeath -= LoseMenu;
        CameraRotator.OnRotate -= RotateJoystickAngle;
    }
    
    public void GetSfx(AudioClip customClip = null)
    {
        AudioClip clipToPlay = customClip;

        if (clipToPlay == null)
        {
            Debug.LogWarning("No se asignó audio para reproducir");
            return;
        }
        
        if (_source.clip != clipToPlay || !_source.isPlaying)
        {
            _source.clip = clipToPlay;
            _source.Play();
        }
    }

    IEnumerator WinLoseMusic(AudioClip clipToPlay)
    {
        MusicManager.Instance.StopAudio();
        GetSfx(clipToPlay);
        float startTime = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup - startTime < clipToPlay.length)
        {
            yield return null;
        }
        _source.Stop();
    }

    public void RestartGameOver()
    {
        OnRestart?.Invoke();
    }
    
    private bool _oneTimeEnemies = false;
    public void GetActiveEnemies()
    {
        if (!_oneTimeEnemies)
        {
            enemiesCheckpoint.Clear();
            
            Enemy[] allEnemies = FindObjectsOfType<Enemy>();
            
            foreach (var enemy in allEnemies)
            {
                if (enemy.gameObject.activeSelf)
                {
                    enemiesCheckpoint.Add(enemy);
                }
            }
            _oneTimeEnemies = true;
        }
    }
}