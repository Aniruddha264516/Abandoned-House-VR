

using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
public class SelectionManager : MonoBehaviour
{
    [SerializeField] private Material selectionMaterial;
    [SerializeField] private string selectwithtag = "SelectionObject";
    [SerializeField] private Material DeselectMaterial;
    [SerializeField] private Transform pointerTransform;
    [SerializeField] private Transform grabPoint;
    [SerializeField] private GameObject GeneratorUI;
    [SerializeField] private GameObject KeyUI;
    [SerializeField] private GameObject GeneratorCompleteImg;
    [SerializeField] private GameObject KeyCompleteImg;
    [SerializeField] private AudioClip GeneratorPickUP;
    [SerializeField] private AudioClip KeyPickUp;
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject Keytext;
    [SerializeField] private GameObject Generatortext;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject FuseCompleteImg;
    [SerializeField] private GameObject FuseUI;
    [SerializeField] private GameObject FuseText;
    [SerializeField] private AudioClip FusePickUP;

    private bool IsPickupFuse = false;
    private bool IsFusefound = false;
    private bool IsGeneratorfound = false;
    private bool Iskeyfound = false;
    private bool IsPickUPGenerator = false;
    private bool IsPickUPKey = false;
    private bool isHoldingObject = false;
    private Transform grabbedObject;
    private Transform _selection;
    private AudioSource _audioSource;
    internal static SelectionManager instance;
    public bool KeyFound
    {
        get { return Iskeyfound; }
    }

    public bool GeneratorFound
    {
        get { return IsGeneratorfound; }
    }

    public bool FuseFound
    {
        get { return IsFusefound; }
    }

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        instance = gameObject.GetComponent<SelectionManager>();
    }

    void Update()
    {
        if (_selection != null)
        {
            var selectionrenderer = _selection.GetComponent<Renderer>();
            selectionrenderer.material = DeselectMaterial;
            _selection = null;
        }

        Ray ray = new Ray(pointerTransform.position, pointerTransform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            var selection = hit.transform;
            var selectionRenderer = selection.GetComponent<Renderer>();

            if (selection.CompareTag(selectwithtag))
            {
                if (selectionRenderer != null)
                {
                    selectionRenderer.material = selectionMaterial;

                    if (Input.GetKeyDown(KeyCode.E) || IsTouchInput())
                    {
                        if (selection.name.Equals("Generator"))
                        {
                            Destroy(selection.gameObject);
                            if (GeneratorUI != null && GeneratorCompleteImg != null)
                            {
                                GeneratorUI.SetActive(false);
                                GeneratorCompleteImg.SetActive(true);
                                _audioSource.PlayOneShot(GeneratorPickUP);
                                IsPickUPGenerator = true;
                                IsGeneratorfound = true;
                                Generatortext.SetActive(false);
                            }
                        }
                        else if (selection.name.Equals("Key"))
                        {
                            Destroy(selection.gameObject);
                            if (KeyUI != null && KeyCompleteImg != null)
                            {
                                KeyUI.SetActive(false);
                                KeyCompleteImg.SetActive(true);
                                _audioSource.PlayOneShot(KeyPickUp);
                                IsPickUPKey = true;
                                Iskeyfound = true;
                                Keytext.SetActive(false);
                            }
                        }

                        else if (selection.name.Equals("Fuse"))
                        {
                            Destroy(selection.gameObject);
                            if (FuseUI != null && FuseCompleteImg != null)
                            {
                               FuseUI.SetActive(false);
                                FuseCompleteImg.SetActive (true);
                                _audioSource.PlayOneShot (FusePickUP);
                                IsFusefound = true;
                                IsPickupFuse = true;
                                FuseText.SetActive(false);
                            }
                        }

                      

                        if (IsPickUPGenerator && IsPickUPKey && IsPickupFuse == true)
                        {
                            StartCoroutine(DestroyCanvasWithDelay());
                        }
                    }
                    
                }

                if (selection.CompareTag("GroundPlane") || selection.CompareTag("Door") || selection.CompareTag("Van") || selection.CompareTag("House"))
                {
                    return;
                }

                _selection = selection;
            }
        }
    }

    private IEnumerator DestroyCanvasWithDelay()
    {
        yield return new WaitForSeconds(2f); 
        if (canvas != null)
        {
            Destroy(canvas);
            animator.SetBool("IsCollected", true);
        }
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




