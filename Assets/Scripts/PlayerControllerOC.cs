using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
using UnityEngine;

public class PlayerControllerOC : ReactingOnPlayerReset
{
    public Rigidbody Rigidbody;
    public SphereCollider SphereCollider;
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
                    DoubleJump();
                    _doubleJumpAttemptsLeft--;
                }
            }
        }
    }

    private void DoubleJump()
    {
        var yVelocity = Rigidbody.velocity.y;
        yVelocity = Mathf.Max(0, yVelocity);
        _fixedUpdateActions.Enqueue(() => Rigidbody.velocity = new Vector3(Rigidbody.velocity.x, yVelocity, Rigidbody.velocity.z));
        Jump(Vector3.up*UpJumpPower);
    }

    private void WallJump()
    {
        var collisionNormal = WallContactChecker.RetriveAndClearContactNormal().XYComponent();
        var perpVector = Vector2.Perpendicular(collisionNormal);
        if (perpVector.y < 0)
        {
            perpVector.y *= -1;
        }

        ResolveCollisionJump(collisionNormal, perpVector, WallJumpNormalPower, WallJumpPerpendicularPower);
    }

    private void FloorJump()
    {
        var collisionNormal = new Vector2(0,1);
        var perpVector = Vector2.Perpendicular(collisionNormal);
        ResolveCollisionJump(collisionNormal, perpVector, UpJumpPower,0);

        _lastFloorJumpTime = Time.time;
    }

    private void ResolveCollisionJump(Vector2 collisionNormal, Vector2 perpVector, float normalJumpPower, float perpJumpPower)
    {
        var flatVelocity = Rigidbody.velocity.XYComponent();
        var collisionAngle = Vector2.Dot(collisionNormal, flatVelocity.normalized);

        if (flatVelocity.magnitude < 0.0001f || collisionAngle > 0)
        {
            //nothing to do, after collision, just add force
        }
        else
        {
            // before jumping, we simulate collision
            var normalVelocity = VectorUtils.Project(flatVelocity, collisionNormal);
            var perpendicularVelocity = VectorUtils.Project(flatVelocity, perpVector);

            var bounciness = SphereCollider.material.bounciness * 0.5f; // 0.5 simulates unity calculations ok
            var oldEnergy = Rigidbody.mass * Mathf.Pow(normalVelocity, 2);
            var newEnergy = oldEnergy * bounciness;
            var newNormalVelocity = Mathf.Pow(newEnergy / Rigidbody.mass, 0.2f); //0.2 makes up-collisions less powerful

            var newFlatVelocity = collisionNormal * newNormalVelocity + perpVector * perpendicularVelocity;
            _fixedUpdateActions.Enqueue(() => Rigidbody.velocity = new Vector3(newFlatVelocity.x, newFlatVelocity.y, 0));
        }

        Jump(collisionNormal * normalJumpPower);
        Jump(perpVector * perpJumpPower);
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
