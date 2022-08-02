using UnityEngine;

public class PlayerMovement : Movement
{
    private const string HorizontalAxis = "Horizontal";
    private const string VerticalAxis = "Vertical";

 protected override void ChooseDirection()
    {
        bool isJumping = false;

        Vector2 horizontalVelocity = new (Input.GetAxis(HorizontalAxis), 0);
        
        if (Input.GetAxis(VerticalAxis) > 0)
        {
            isJumping = true;
        }

        SetDirection(horizontalVelocity, isJumping);
    }
}