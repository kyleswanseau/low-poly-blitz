using UnityEngine;

public class Tetra : Unit
{
    private static float MOVE_SPEED = 3f;

    new protected void Start()
    {
        _moveSpeed = MOVE_SPEED;
        base.Start();
    }

    new private void Update()
    {
        base.Update();
    }
}
