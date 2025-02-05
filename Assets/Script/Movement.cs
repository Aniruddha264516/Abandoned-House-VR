using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class VRMovement : MonoBehaviour
{
    RaycastHit hit;
    bool IsGroundHit = false;
    public GameObject pointer;
    private Vector3 targetPosition;
    private bool isMoving = false;

    private Renderer groundRenderer;
    public float movementSpeed;

    public Transform vrCameraTransform;
    internal static VRMovement instance;

    public string CurrentSurface { get; private set; }

    public bool IsMoving
    {
        get { return isMoving; }
    }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
   
        if (vrCameraTransform == null)
        {
            Camera mainCamera = Camera.main;
            if (mainCamera != null)
            {
                vrCameraTransform = mainCamera.transform;
            }
        }
    }

    private void FixedUpdate()
    {
        if (vrCameraTransform == null) return;

        Vector3 rayOrigin = vrCameraTransform.position;
        Vector3 rayDirection = vrCameraTransform.forward;

        if (Physics.Raycast(rayOrigin, rayDirection, out hit))
        {
            if (hit.transform.gameObject.CompareTag("GroundPlane") || hit.transform.gameObject.CompareTag("Stairs") || hit.transform.gameObject.CompareTag("UpGround"))
            {
                IsGroundHit = true;
                pointer.SetActive(true);
                pointer.transform.position = hit.point;
                groundRenderer = hit.transform.GetComponent<Renderer>();
            }

            else
            {
                IsGroundHit = false;
                pointer.SetActive(false);
                CurrentSurface = null;
            }
        }
        else
        {
            IsGroundHit = false;
            pointer.SetActive(false);
            CurrentSurface = null;
        }
    }

    public void Update()
    {
        if (IsGroundHit && (Input.GetMouseButtonDown(0) || IsTouchInput()))
        {
            if (PlaneBounds(hit.point))
            {
                Debug.Log("Moveable area");
                targetPosition = hit.point + new Vector3(0, 1.775f, 0);
                isMoving = true;
            }
        }

        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, movementSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                isMoving = false;
            }
        }
    }

    private bool PlaneBounds(Vector3 point)
    {
        if (groundRenderer == null) return false;
        Bounds planeBounds = groundRenderer.bounds;
        return planeBounds.Contains(hit.point);
    }

    private bool IsTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                return true;
            }
        }
        return false;
    }
}
