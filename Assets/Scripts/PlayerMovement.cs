using UnityEngine;

public class PlayerMovement : Movement
{
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