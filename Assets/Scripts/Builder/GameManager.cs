using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Player playerPrefab;
    [SerializeField] private Controller moveController;
    [SerializeField] private Controller aimMoveController;
    [SerializeField] private Vector3 initialPosition;
    [SerializeField] private int speed;
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private SneakSkill sneakSkill;

    public DetectionState detectionState = DetectionState.Hidden; //////// TOMI //////////////////////////////////

    private void Awake()
    {
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
}