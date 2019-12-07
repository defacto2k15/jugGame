using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
using UnityEngine;

public class PlayerControllerOC : ReactingOnPlayerDeath
{
    public Rigidbody Rigidbody;
    public GroundedCheckerOC GroundedChecker;
    public WallContactCheckerOC WallContactChecker;

    [Range(0, 100)] public float MaxAngularVelocity= 100;
    [Range(0, 100)] public float HorizontalMovementSpeed = 1;
    [Range(0, 500)] public float UpJumpPower = 1;
    [Range(0, 500)] public float WallJumpNormalPower = 1;
    [Range(0, 500)] public float WallJumpPerpendicularPower = 1;

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
        float horizontalMovement = Input.GetAxis("Horizontal");
        var horizontalMovementSpeed = Vector3.forward* horizontalMovement * HorizontalMovementSpeed;
        _fixedUpdateActions.Enqueue(() => Rigidbody.AddTorque(horizontalMovementSpeed));

        if (GroundedChecker.IsTouchingGround)
        {
            _doubleJumpAttemptsLeft = Constants.PlayerDoubleJumpAttemptsDefaultCount;
            _lastFloorJumpTime = 0;
        }

        if (Input.GetButtonDown("Jump"))
        {
            if(GroundedChecker.IsNearGround && Mathf.Abs(Time.time - _lastFloorJumpTime ) > Constants.PlayerFloorJumpReloadTime)
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
                    //JumpUp();
                    _doubleJumpAttemptsLeft--;
                }
            }
        }
    }

    void FixedUpdate()
    {
        while (_fixedUpdateActions.Any())
        {
            _fixedUpdateActions.Dequeue().Invoke();
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

        Jump(collisionWallNormal * WallJumpNormalPower);
        Jump(Vector3.up * WallJumpPerpendicularPower);
        Debug.Log("Wall jumping");

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

    public override void PlayerIsDead()
    {
        _fixedUpdateActions.Enqueue(() => Rigidbody.velocity = Vector3.zero);
    }
}
