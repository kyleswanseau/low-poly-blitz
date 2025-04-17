using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Rigidbody rb;
    private static float zoomSpeed = 10f;
    private static float moveSpeed = 50f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        MoveCamera();
    }

    private void MoveCamera()
    {
        float height = 0f;
        float scroll = Input.mouseScrollDelta.y;
        if ((scroll == 1f && transform.position.y > 20f) || (scroll == -1f && transform.position.y < 80f))
        {
            height = zoomSpeed * -scroll;
        }
        transform.position += new Vector3(0f, height, 0f);

        Vector3 accel = new Vector3();
        if (Input.GetKey(KeyCode.UpArrow))
        {
            accel += Vector3.forward;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            accel += Vector3.back;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            accel += Vector3.left;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            accel += Vector3.right;
        }
        rb.AddForce(accel.normalized * moveSpeed * transform.position.y/10);
    }
}
