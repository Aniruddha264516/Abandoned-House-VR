using UnityEngine;

public class MouseLook : MonoBehaviour
{
    private float x;
    private float y;
    private Vector3 rotate;
    public float sensitivity;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Update()
    {
        x = Input.GetAxis("Mouse X");
        y = Input.GetAxis("Mouse Y");

        rotate = new Vector3(-y * sensitivity, x * sensitivity, 0);
        transform.eulerAngles = transform.eulerAngles + rotate;
    }
}
