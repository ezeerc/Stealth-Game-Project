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
            .SetLayerMask(playerMask);


        player.InitPlayer(builder);
    }
}