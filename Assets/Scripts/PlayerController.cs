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

    // TEMPORARY CONST FOR JUMP DIRECTION
    Vector3 JUMPDIR = new Vector3(1, 1, 0);


    // --- INSPECTOR VARIABLES ---
    [Header("Controller Settings")]
    [Space(5)]
    [SerializeField] float _jumpMultiplier = 1f;
    [SerializeField] LayerMask _groundLayer;
    [SerializeField] LineRenderer _lineRenderer;
    [SerializeField, Range(2, 30)] int _resolution = 30;
    [SerializeField, Range(1, 5)] float _simulationTime = 5f;

    private void Awake()
    {
        _input = GetComponent<PlayerInput>();
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        CalcTraj();
    }

    public void Jump(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            isHoldingJump = true;
            _holdStartTime = Time.time;

            StopCoroutine("DisableLineWithWaitTime");
        }
        if (ctx.canceled)
        {
            isHoldingJump = false;
            _holdDuration = Time.time - _holdStartTime;

            // Jump
            _rb.AddForce(_impulseForce, ForceMode.Impulse);

            _holdDuration = 0f;
            StartCoroutine(DisableLineWithWaitTime(_simulationTime));
        }
    }
    public void StartJumpHold()
    {
        isHoldingJump = true;
        _holdStartTime = Time.time;

        StopCoroutine("DisableLineWithWaitTime");
    }

    public void EndJumpHold()
    {
        isHoldingJump = false;
        _holdDuration = Time.time - _holdStartTime;

        // Jump
        _rb.AddForce(_impulseForce, ForceMode.Impulse);

        _holdDuration = 0f;
        StartCoroutine(DisableLineWithWaitTime(_simulationTime));
    }

    void CalcTraj()
    {
        if (isHoldingJump)
        {
            _holdDuration = isHoldingJump ? Time.time - _holdStartTime : _holdDuration;
            _impulseForce = JUMPDIR * _jumpMultiplier * _holdDuration;

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

    IEnumerator DisableLineWithWaitTime(float t)
    {
        yield return new WaitForSeconds(t);
        _lineRenderer.positionCount = 0;
    }
}
