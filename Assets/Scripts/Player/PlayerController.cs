using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // input
    private PlayerControls _playerControlsAsset;
    private InputAction _moveAction, _exitAction;

    // movement
    private Rigidbody rb;
    [SerializeField] private float _movementForce = 1f;
    [SerializeField] private float _jumpForce = 5f;
    [SerializeField] private float _maxSpeed = 5f;
    private Vector3 forceDirection = Vector3.zero;
    [SerializeField] private LayerMask _layerMask;

    [SerializeField] private Camera _playerCamera;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        _playerControlsAsset = new PlayerControls();        
    }

    private void OnEnable()
    {
        _playerControlsAsset.Player.Jump.started += DoJump;
        _moveAction = _playerControlsAsset.Player.Move;
        _exitAction = _playerControlsAsset.Player.Exit;
        _playerControlsAsset.Player.Enable();
    }

    private void OnDisable()
    {
        _playerControlsAsset.Player.Jump.started -= DoJump;
        _playerControlsAsset.Player.Disable();
    }

    private void Update()
    {
        if (_exitAction.IsPressed())
        {
            GameManager.instance.PauseGame();
        }
    }
    
    private void FixedUpdate()
    {
        forceDirection += _moveAction.ReadValue<Vector2>().x * GetCameraRight(_playerCamera) * _movementForce;
        forceDirection += _moveAction.ReadValue<Vector2>().y * GetCameraForward(_playerCamera) * _movementForce;

        rb.AddForce(forceDirection, ForceMode.Impulse);
        forceDirection = Vector3.zero;

        /*if (rb.velocity.y < 0f)
            rb.velocity += Vector3.down * Physics.gravity.y * Time.fixedDeltaTime;*/

        Vector3 horizontalVelocity = rb.velocity;
        horizontalVelocity.y = 0f;
        if (horizontalVelocity.sqrMagnitude > _maxSpeed * _maxSpeed)
            rb.velocity = horizontalVelocity.normalized * _maxSpeed + Vector3.up * rb.velocity.y;

        LookAt();
    }

    private Vector3 GetCameraForward(Camera cam)
    {
        Vector3 forward = cam.transform.forward;
        forward.y = 0f;
        return forward.normalized;
    }

    private Vector3 GetCameraRight(Camera cam)
    {
        Vector3 right = cam.transform.right;
        right.y = 0f;
        return right.normalized;
    }

    private void LookAt()
    {
        Vector3 direction = rb.velocity;
        direction.y = 0f;

        if (direction.sqrMagnitude > 0.1f && _moveAction.ReadValue<Vector2>().sqrMagnitude > 0.1f)
            rb.rotation = Quaternion.LookRotation(direction, Vector3.up);
        else
            rb.angularVelocity = Vector3.zero;
    }

    private void DoJump(InputAction.CallbackContext obj)
    {
        if (IsGround())
        {
            forceDirection += Vector3.up * _jumpForce;
        }
    }

    private bool IsGround()
    {
        Ray ray = new Ray(transform.position + Vector3.up * 0.25f, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, 0.55f, _layerMask)) return true;
        else return false;
    }

    public void SpringBouncer(float jumpForce)
    {
        forceDirection += Vector3.up * jumpForce;
    }
}
