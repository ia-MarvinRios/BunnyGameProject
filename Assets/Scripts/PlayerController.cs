using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody), typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    Rigidbody _rb;
    PlayerInput _input;

    // TEMPORARY CONST FOR JUMP DIRECTION
    Vector3 JUMPDIR = new Vector3(1, 1, 0);

    [Header("Controller Settings")]
    [SerializeField] float _jumpForce = 2f;

    private void Awake()
    {
        _input = GetComponent<PlayerInput>();
        _rb = GetComponent<Rigidbody>();
    }

    public void Jump(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            _rb.AddForce(JUMPDIR * _jumpForce, ForceMode.Impulse);
        }
    }

}
