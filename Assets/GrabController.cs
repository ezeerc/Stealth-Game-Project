using UnityEngine;

public class PlayerGrab : MonoBehaviour
{
    public float grabRadius = 2.0f;           // Radio de detección del OverlapSphere
    public LayerMask grabLayer;               // Capa de detección de puntos de agarre
    public Transform playerHand;              // La mano del jugador donde se conectará el Character Joint
    private CharacterJoint _characterJoint;    // El Character Joint que se creará en tiempo de ejecución
    private Transform _nearestGrabPoint;       // El punto de agarre más cercano
    public RagdollController _ragdollController;
    private Rigidbody npcRigidbody;
    private FixedJoint fixedJoint;
    void Update()
    {
        // Detecta si el jugador presiona la tecla de agarre
        if (Input.GetKeyDown(KeyCode.E))
        {
            FindAndGrabNearestJoint();
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            ReleaseGrab();
        }
    }

    void FindAndGrabNearestJoint()
    {
        // Usa OverlapSphere para encontrar todos los colliders en el radio de agarre
        Collider[] colliders = Physics.OverlapSphere(transform.position, grabRadius, grabLayer);

        float nearestDistance = float.MaxValue;
        _nearestGrabPoint = null;

        // Recorre los colliders encontrados para encontrar el más cercano
        foreach (Collider collider in colliders)
        {
            float distance = Vector3.Distance(transform.position, collider.transform.position);

            // Encuentra el collider más cercano que actúe como punto de agarre
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                _nearestGrabPoint = collider.transform;
                npcRigidbody = collider.GetComponent<Rigidbody>();
                _ragdollController = collider.GetComponentInParent<RagdollController>();
            }
        }

        // Si encontró un punto de agarre cercano, crea el Character Joint
        if (_nearestGrabPoint != null)
        {
            GrabNPC(_nearestGrabPoint);
        }
    }

    void GrabNPC(Transform grabPoint)
    {
        // Asegura que el Character Joint no exista ya
        if (_characterJoint != null) return;

        // Crea el Character Joint en la mano del jugador y lo conecta al punto de agarre
        fixedJoint = playerHand.gameObject.AddComponent<FixedJoint>();
        _ragdollController.DeactivateRagdollDead();
        fixedJoint.connectedBody = npcRigidbody;

        // Opcional: Configura propiedades del FixedJoint para ajustar rigidez y amortiguación
        fixedJoint.breakForce = 1000f; // Ajusta la fuerza de ruptura para soltar el NPC
        fixedJoint.breakTorque = 1000f;
        
        Debug.Log("Agarrando al NPC en el punto de agarre más cercano.");
    }

    void ReleaseGrab()
    {
        // Destruye el Character Joint para soltar el NPC
        if (_characterJoint != null)
        {
            _ragdollController.ActivateRagdoll();
            Destroy(fixedJoint);
            fixedJoint = null;
            Debug.Log("NPC liberado.");
        }
    }

    void OnDrawGizmosSelected()
    {
        // Dibuja la esfera de agarre en la escena para visualizar el alcance del OverlapSphere
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, grabRadius);
    }
}
