using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class PlayerControllerOC : ReactingOnPlayerDeath
{
    public Rigidbody Rigidbody;
    public GroundedCheckerOC GroundedChecker;
    public WallContactCheckerOC WallContactChecker;

    [Range(0, 10)] public float HorizontalMovementSpeed = 1;
    [Range(0, 500)] public float UpJumpPower = 1;
    [Range(0, 500)] public float WallJumpNormalPower = 1;
    [Range(0, 500)] public float WallJumpPerpendicularPower = 1;

    private int _doubleJumpAttemptsLeft;
    private float _lastFloorJumpTime;

    public void Start()
    {
        _doubleJumpAttemptsLeft = 0;
    }

    void FixedUpdate()
    {
        float horizontalMovement = Input.GetAxis("Horizontal");
        Rigidbody.AddTorque(Vector3.back * horizontalMovement * HorizontalMovementSpeed);


        if (GroundedChecker.IsTouchingGround)
        {
            _doubleJumpAttemptsLeft = Constants.PlayerDoubleJumpAttemptsDefaultCount;
            _lastFloorJumpTime = 0;
        }

        if (Input.GetButtonDown("Jump"))
        {
            if(GroundedChecker.IsNearGround && Mathf.Abs(_lastFloorJumpTime - Time.time) > Constants.PlayerFloorJumpReloadTime)
            {
                if (_lastFloorJumpTime > 0.0001)
                {
                    Debug.Log("Delta is "+Mathf.Abs(_lastFloorJumpTime - Time.time) );
                }
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

    private void WallJump()
    {
        var collisionWallNormal = WallContactChecker.RetriveAndClearContactNormal().XYComponent();
        var perpVector = Vector2.Perpendicular(collisionWallNormal);
        var velocityOnNormalComponent = VectorUtils.Project(Rigidbody.velocity, collisionWallNormal);
        var velocityOnPerpendicularComponent = VectorUtils.Project(Rigidbody.velocity, perpVector);

        var finalFlatVelocity = perpVector * velocityOnPerpendicularComponent;
        Rigidbody.velocity = new Vector3(finalFlatVelocity.x, finalFlatVelocity.y, 0);

        Jump(collisionWallNormal * WallJumpNormalPower);
        Jump(Vector3.up * WallJumpPerpendicularPower);
        Debug.Log("Wall jumping");

    }

    private void FloorJump()
    {
        Rigidbody.velocity = new Vector3(Rigidbody.velocity.x, 0, Rigidbody.velocity.z);
        _lastFloorJumpTime = Time.time;
        Jump(Vector3.up * UpJumpPower);
    }

    private void Jump(Vector3 vec)
    {
        Rigidbody.AddForce(vec);
    }

    public override void PlayerIsDead()
    {
        Rigidbody.velocity = Vector3.zero;
    }
}
