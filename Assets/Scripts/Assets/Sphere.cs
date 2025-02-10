using UnityEngine;

public class Sphere : Unit
{
    private static float MOVE_SPEED = 5f;

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
