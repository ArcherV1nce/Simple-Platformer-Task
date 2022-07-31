using UnityEngine;

public class PlayerMovement : Movement
{
    protected override void Update()
    {
        base.Update();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void ChooseDirection()
    {
        bool isJumping = false;

        Vector2 horizontalVelocity = new (Input.GetAxis("Horizontal"), 0);
        
        if (Input.GetKey(KeyCode.W))
        {
            isJumping = true;
        }

        SetDirection(horizontalVelocity, isJumping);
    }
}