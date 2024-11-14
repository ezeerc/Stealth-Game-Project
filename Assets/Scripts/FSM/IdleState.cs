using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    public ChaseState chaseState;
    public bool canSeeThePlayer;
    public LayerMask playerLayer;  // La capa asignada al jugador
    public float detectionRadius = 5f;  // Radio de la esfera de detección
    private Transform playerTransform;
    public override State RunCurrentState()
    {
        //StartCoroutine(DetectPlayerCoroutine());
        DetectPlayer();
        return this;
        
        /*if (canSeeThePlayer)
        {
            return chaseState;
        }
        else
        {
            return this;
        }*/
    }

    /*private IEnumerator DetectPlayerCoroutine()
    {
        yield return new WaitForSeconds(2f);
        DetectPlayer();
    }*/
    void DetectPlayer()
    {
        // Define el punto central de la esfera en la posición del objeto actual
        Vector3 spherePosition = transform.position;

        // Usa Physics.OverlapSphere para detectar colisiones en la esfera
        Collider[] colliders = Physics.OverlapSphere(spherePosition, detectionRadius, playerLayer);

        // Si hay colisionadores en la capa de jugador dentro del radio, significa que el jugador está en la esfera
        if (colliders.Length > 0)
        {
            foreach (Collider collider in colliders)
            {
                if (collider is not null)
                {
                    canSeeThePlayer = true;
                    //print("veo al jugador");
                }
            }
        }
        else
        {
            canSeeThePlayer = false;
            //print("no veo al jugador");
        }
    }
    
    
    
    /*void OnDrawGizmos()
    {
        // Dibuja la esfera en la vista de escena para que sea visible mientras editas
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }*/
}
