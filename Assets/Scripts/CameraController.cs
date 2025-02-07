using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class CameraController : MonoBehaviour
{
    Mouse mouse;
    public GameObject obj;
    private Rigidbody rb;
    private Vector2? startPos;
    private Vector2? endPos;
    private static float zoomSpeed = 10f;
    private static float moveSpeed = 50f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mouse = Mouse.current;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveCamera();
        SelectAssets();
    }

    private void OnGUI()
    {
        if (startPos != null && endPos != null)
        {
            Rect selection = SomeRect(startPos.Value, endPos.Value);
            selection.yMin = Screen.height - selection.yMin;
            selection.yMax = Screen.height - selection.yMax;
            GUI.DrawTexture(selection, Texture2D.grayTexture);
        }
    }

    void MoveCamera()
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

    void SelectAssets()
    {
        Vector2? pos = mouse.position.value;
        if (mouse.leftButton.wasReleasedThisFrame)
        {
            endPos = pos;
            Rect selection = SomeRect(startPos.Value, endPos.Value);
            Vector2 objPos = obj.GetComponent<Asset>().getPositionInCam();
            obj.GetComponent<Asset>().setSelected(selection.Contains(objPos));
            startPos = endPos = null;
        }
        else if (mouse.leftButton.wasPressedThisFrame)
        {
            obj.GetComponent<Asset>()
                .setSelected(false);
            startPos = pos;
        }
        if (mouse.leftButton.isPressed)
        {
            endPos = pos;
            Rect selection = SomeRect(startPos.Value, endPos.Value);
            Vector2 objPos = obj.GetComponent<Asset>().getPositionInCam();
            obj.GetComponent<Asset>().setHovered(selection.Contains(objPos));
        }
    }

    Rect SomeRect(Vector2 startPos, Vector2 endPos)
    {
        float xMin = Mathf.Min(startPos.x, endPos.x);
        float yMin = Mathf.Min(startPos.y, endPos.y);
        float xMax = Mathf.Max(startPos.x, endPos.x);
        float yMax = Mathf.Max(startPos.y, endPos.y);
        return Rect.MinMaxRect(xMin, yMin, xMax, yMax);
    }
}
