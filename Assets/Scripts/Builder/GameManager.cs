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
    private void Start()
    {
        Player player = Instantiate(playerPrefab);
        
        PlayerBuilder builder = new PlayerBuilder()
            .SetMoveController(moveController)
            .SetAimMoveController(aimMoveController)
            .SetMinHealth(0)
            .SetMaxHealth(100)
            .SetPosition(initialPosition)
            .SetSpeed(speed);
        
        player.InitPlayer(builder);
    }
}