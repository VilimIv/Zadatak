using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : NetworkBehaviour
{
    [SerializeField] public CharacterController Controller;
    [SerializeField] private float moveSpeed = 3f;
    [Header("Physics")]
    [SerializeField] private float Gravity = 9.81f;
    [SerializeField] private Transform PhysicsPoint;
    [SerializeField] private LayerMask GroundLayer;
    [SerializeField] private float JumpHight = 3f;
    [SerializeField] Animator CharacterAnimator;
    bool IsGrounded;
    [HideInInspector] public Vector3 Velocity;



    void Start()
    {
        if (Controller == null)
            Controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (IsLocalPlayer)
        {
            IsGrounded = Physics.CheckSphere(PhysicsPoint.position, 0.5f, GroundLayer);

            Vector3 moveDir = Vector3.zero;

            if (Input.GetKey(KeyCode.W)) moveDir += transform.forward;
            if (Input.GetKey(KeyCode.S)) moveDir += -transform.forward;
            if (Input.GetKey(KeyCode.A)) moveDir += -transform.right;
            if (Input.GetKey(KeyCode.D)) moveDir += transform.right;

            moveDir = moveDir.normalized;

            if (moveDir != Vector3.zero)
            {
                CharacterAnimator.SetLayerWeight(1, 1);
            }
            else
            {
                CharacterAnimator.SetLayerWeight(1, 0);
            }

            MovePlayerServerRpc(moveDir);

            if (IsGrounded && Velocity.y < 0)
            {
                Velocity.y = -1f;
            }
            else
            {
                Velocity.y += (-Gravity) * Time.deltaTime;
            }

            if (Input.GetKeyDown(KeyCode.Space) && IsGrounded)
            {
                Velocity.y = Mathf.Sqrt(JumpHight * 2f * Gravity);
            }

            SimulatePhysics(Velocity);
        }
    }

    [ServerRpc]
    private void MovePlayerServerRpc(Vector3 moveDirection)
    {
        MovePlayer(moveDirection);
        MovePlayerClientRpc(moveDirection);
    }

    [ClientRpc]
    private void MovePlayerClientRpc(Vector3 moveDirection)
    {
        MovePlayer(moveDirection);
    }

    private void MovePlayer(Vector3 moveDirection)
    {
        Controller.Move(moveDirection * moveSpeed * Time.deltaTime);
    }
    void SimulatePhysics(Vector3 moveDirection)
    {
        Controller.Move(moveDirection * Time.deltaTime);
    }
}