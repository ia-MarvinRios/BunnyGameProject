using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody), typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    // --- HIDDEN VARIABLES ---
    Rigidbody _rb;
    PlayerInput _input;
    Vector3 _impulseForce;
    bool isHoldingJump = false;
    float _holdStartTime = 0f;
    float _holdDuration = 0f;

    // JUMP DIRECTION
    bool _isJumpingLeft = false;
    Vector3 JUMPDIR_RIGHT = new Vector3(1, 1, 0);
    Vector3 JUMPDIR_LEFT = new Vector3(-1, 1, 0);


    // --- INSPECTOR VARIABLES ---
    [Header("Controller Settings")]
    [Space(5)]
    [SerializeField] float _jumpMultiplier = 1f;
    [SerializeField] float _minHoldTime = 0;
    [SerializeField] float _maxHoldTime = 2f;
    [SerializeField] LayerMask _groundLayer;
    [SerializeField] LineRenderer _lineRenderer;
    [SerializeField, Range(2, 30)] int _resolution = 30;
    [SerializeField, Range(1, 5)] float _simulationTime = 5f;
    [Space(10)]
    [SerializeField] float _raycastDistance = 0.05f;
    [SerializeField] Vector3 _raycastOffset = new Vector3(0, 0, 0);
    [Header("Animations")]
    [SerializeField] Animator _animator;
    [SerializeField] GameObject _modelObject;

    private void Awake()
    {
        _input = GetComponent<PlayerInput>();
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        CalcTraj();
        CheckGrounded();
    }

    

    public void Jump(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            Vector2 pos = Touchscreen.current.primaryTouch.position.ReadValue();
            _isJumpingLeft = pos.x < Screen.width * 0.5f;
            _modelObject.transform.rotation = _isJumpingLeft ? Quaternion.Euler(0, -90, 0) : Quaternion.Euler(0, 90, 0);

            isHoldingJump = true;
            _holdStartTime = Time.time;

            StopCoroutine("DisableLineWithWaitTime");

            // Animator
            if (_animator != null)
            {
                _animator.SetTrigger("JumpCharge");
            }
        }
        if (ctx.canceled)
        {
            isHoldingJump = false;
            _holdDuration = Time.time - _holdStartTime;

            // Jump
            _rb.AddForce(_impulseForce, ForceMode.Impulse);

            _holdDuration = 0f;
            StartCoroutine(DisableLineWithWaitTime(_simulationTime));

            // Animator
            if (_animator != null)
            {
                _animator.SetTrigger("Jump");
            }
        }
    }

    void CalcTraj()
    {
        if (isHoldingJump)
        {
            _holdDuration = Mathf.Clamp(isHoldingJump ? Time.time - _holdStartTime : _holdDuration, _minHoldTime, _maxHoldTime);
            _impulseForce = _isJumpingLeft ? JUMPDIR_LEFT * _jumpMultiplier * _holdDuration : JUMPDIR_RIGHT * _jumpMultiplier * _holdDuration;


            _lineRenderer.positionCount = 0;
            _lineRenderer.positionCount = _resolution;

            float step = _simulationTime / _resolution;

            for (int i = 0; i < _resolution; i++)
            {
                float t = i * step;

                // p(t) = p0 + v0*t + 0.5*g*t^2
                Vector3 pos = transform.position
                            + _impulseForce * t
                            + 0.5f * Physics.gravity * t * t;

                Vector3 nextPos = transform.position
                                + _impulseForce * (t + step)
                                + 0.5f * Physics.gravity * (t + step) * (t + step);

                // Draw line for this segment
                Debug.DrawLine(pos, nextPos, Color.red, 0.1f);
                _lineRenderer.SetPosition(i, pos);

                // Check for ground collision
                if (Physics.Linecast(pos, nextPos, out RaycastHit hit, _groundLayer))
                {
                    Debug.Log("Hit Ground at " + hit.point);
                    _lineRenderer.positionCount = i + 1;
                    break;
                }
            }
        }
    }

    void CheckGrounded()
    {
        if (Physics.Raycast(transform.position + _raycastOffset, Vector3.down, _raycastDistance, _groundLayer))
        {
            Debug.Log("Grounded");

            // Animator
            if (_animator != null)
            {
                _animator.SetBool("IsGrounded", true);
            }
        }
        else
        {
            // Animator
            if (_animator != null)
            {
                _animator.SetBool("IsGrounded", false);
            }
        }
    }

    IEnumerator DisableLineWithWaitTime(float t)
    {
        yield return new WaitForSeconds(t);
        _lineRenderer.positionCount = 0;
    }
}
