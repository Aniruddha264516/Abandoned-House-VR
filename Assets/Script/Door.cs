using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public float raycastDistance = 5f; 
    public LayerMask doorLayer;       
    private Animator currentDoorAnimator;
    private bool isDoorOpen = false;

    public AudioClip openDoorSound;  
    public AudioClip closeDoorSound;
    public AudioClip locksound;
    private AudioSource audioSource;

    public Transform vrCameraTransform; 
    private SelectionManager selectionManager;
    public GameObject lockdoortext;
    private bool doortext = false;

    private void Start()
    {
        
        audioSource = gameObject.AddComponent<AudioSource>();
        selectionManager = FindAnyObjectByType<SelectionManager>();
       
        if (vrCameraTransform == null)
        {
            Camera mainCamera = Camera.main;
            if (mainCamera != null)
            {
                vrCameraTransform = mainCamera.transform;
            }
        }
    }

    private void Update()
    {
        
        if (IsTouchInput() || Input.GetKeyDown(KeyCode.F))
        {
            PerformRaycast();
        }
    }

    private void PerformRaycast()
    {
        if (vrCameraTransform == null) return;

        
        Ray ray = new Ray(vrCameraTransform.position, vrCameraTransform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, raycastDistance, doorLayer))
        {
            Animator doorAnimator = hit.collider.GetComponent<Animator>();

            if (doorAnimator != null)
            {
                currentDoorAnimator = doorAnimator;

                if (!isDoorOpen)
                {
                    if(selectionManager.KeyFound == true)
                    {
                        OpenDoor();
                        isDoorOpen = true;
                        lockdoortext.SetActive(false);
                    }
                    
                    else
                    {
                        LockDoor();
                    }
                }

                else
                {
                    CloseDoor();
                    isDoorOpen = false;
                }
            }
        }
     
    }

    private void OpenDoor()
    {
        if (currentDoorAnimator == null) return;
        currentDoorAnimator.SetBool("Open", true);
        PlayAudio(openDoorSound);
    }

    private void CloseDoor()
    {
        if (currentDoorAnimator == null) return;
        currentDoorAnimator.SetBool("Open", false);
        PlayAudio(closeDoorSound); 
    }

    private void LockDoor()
    {
        if (currentDoorAnimator == null) return;
        currentDoorAnimator.SetTrigger("Lock");
        lockdoortext.SetActive (true);
        StartCoroutine(HideLockTextAfterDelay());
        PlayAudio(locksound);
    }

    private IEnumerator HideLockTextAfterDelay()
    {
        yield return new WaitForSeconds(0.1f);
        lockdoortext.SetActive(false);
    }

    private void PlayAudio(AudioClip clip)
    {
        if (clip == null || audioSource == null) return;
        audioSource.PlayOneShot(clip); 
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
