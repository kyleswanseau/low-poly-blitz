using UnityEngine;

public class Cube : Unit
{
    private static float MOVE_SPEED = 5f;

    new protected void Start()
    {
        moveSpeed = MOVE_SPEED;
        base.Start();
    }

    new private void Update()
    {
        base.Update();
    }
}
