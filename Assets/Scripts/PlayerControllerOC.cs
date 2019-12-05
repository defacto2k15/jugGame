using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class PlayerControllerOC : MonoBehaviour
{
    public Rigidbody Rigidbody;
    public GroundedCheckerOC GroundedChecker;
    public WallContactCheckerOC WallContactChecker;
    [Range(0,10)]
    public float HorizontalMovementSpeed =1;
    [Range(0,500)]
    public float JumpPower =1;

    private int _doubleJumpAttemptsLeft;

    public void Start()
    {
        _doubleJumpAttemptsLeft = 0;
    }

    void Update()
    {
        float horizontalMovement = Input.GetAxis("Horizontal");
        Rigidbody.AddTorque(Vector3.back* horizontalMovement * HorizontalMovementSpeed);


        if (GroundedChecker.IsGrounded)
        {
            _doubleJumpAttemptsLeft = Constants.PlayerDoubleJumpAttemptsDefaultCount;
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (GroundedChecker.IsGrounded)
            {
                JumpUp();
            }
            else
            {
                if (WallContactChecker.HasActiveContact)
                {
                    WallJump();
                }else if (_doubleJumpAttemptsLeft > 0)
                {
                    //JumpUp();
                    _doubleJumpAttemptsLeft--;
                }
            }
        }
    }

    private void WallJump()
    {
        var collisionWallNormal = WallContactChecker.RetriveAndClearContactNormal();
        Jump(collisionWallNormal);
        JumpUp();
        Debug.Log("Wall jumping");

    }

    private void JumpUp()
    {
        Jump(Vector3.up);
    }

    private void Jump(Vector3 direction)
    {
        Rigidbody.AddForce(direction * JumpPower);
    }
}
