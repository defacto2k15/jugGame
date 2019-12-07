using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
using UnityEngine;

public class PlayerControllerOC : ReactingOnPlayerReset
{
    public Rigidbody Rigidbody;
    public GroundedCheckerOC GroundedChecker;
    public WallContactCheckerOC WallContactChecker;

    [Range(0, 100)] public float MaxAngularVelocity= 100;
    [Range(0, 100)] public float HorizontalMovementSpeed = 1;
    [Range(0, 500)] public float UpJumpPower = 1;
    [Range(0, 500)] public float WallJumpNormalPower = 1;
    [Range(0, 1000)] public float WallJumpPerpendicularPower = 1;

    private int _doubleJumpAttemptsLeft;
    private float _lastFloorJumpTime;
    private Queue<Action> _fixedUpdateActions;

    public void Start()
    {
        _doubleJumpAttemptsLeft = 0;
        _fixedUpdateActions = new Queue<Action>();
        _fixedUpdateActions.Enqueue(() => Rigidbody.maxAngularVelocity = MaxAngularVelocity);
    }

    void Update()
    {
        HandleHorizontalMovement();

        HandleJumping();
    }

    void FixedUpdate()
    {
        while (_fixedUpdateActions.Any())
        {
            _fixedUpdateActions.Dequeue().Invoke();
        }
    }

    private void HandleHorizontalMovement()
    {
        float horizontalMovement = Input.GetAxis("Horizontal");
        var horizontalMovementSpeed = Vector3.back * horizontalMovement * HorizontalMovementSpeed;
        _fixedUpdateActions.Enqueue(() => Rigidbody.AddTorque(horizontalMovementSpeed));
    }

    private void HandleJumping()
    {
        if (GroundedChecker.IsTouchingGround)
        {
            _doubleJumpAttemptsLeft = Constants.PlayerDoubleJumpAttemptsDefaultCount;
            _lastFloorJumpTime = 0;
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (GroundedChecker.IsNearGround && Mathf.Abs(Time.time - _lastFloorJumpTime) > Constants.PlayerFloorJumpReloadTime)
            {
                FloorJump();
            }
            else
            {
                if (WallContactChecker.HasActiveContact)
                {
                    WallJump();
                }
                else if (_doubleJumpAttemptsLeft > 0)
                {
                    FloorJump();
                    _doubleJumpAttemptsLeft--;
                }
            }
        }
    }

    private void WallJump()
    {
        var collisionWallNormal = WallContactChecker.RetriveAndClearContactNormal().XZComponent();
        var perpVector = Vector2.Perpendicular(collisionWallNormal);
        var velocityOnNormalComponent = VectorUtils.Project(Rigidbody.velocity, collisionWallNormal);
        var velocityOnPerpendicularComponent = VectorUtils.Project(Rigidbody.velocity, perpVector);

        var finalFlatVelocity = perpVector * velocityOnPerpendicularComponent;
        _fixedUpdateActions.Enqueue(() => Rigidbody.velocity = new Vector3(finalFlatVelocity.x, finalFlatVelocity.y, 0));

        Jump(new Vector3(collisionWallNormal.x, collisionWallNormal.y, 0) * WallJumpNormalPower);
        Jump(Vector3.up * WallJumpPerpendicularPower);
    }

    private void FloorJump()
    {
        _fixedUpdateActions.Enqueue(() => Rigidbody.velocity = new Vector3(Rigidbody.velocity.x, 0, Rigidbody.velocity.z));
        _lastFloorJumpTime = Time.time;
        Jump(Vector3.up * UpJumpPower);
    }

    private void Jump(Vector3 vec)
    {
        _fixedUpdateActions.Enqueue(() => Rigidbody.AddForce(vec));
    }

    public override void PlayerIsReset()
    {
        _fixedUpdateActions.Enqueue(() => Rigidbody.velocity = Vector3.zero);
    }
}
