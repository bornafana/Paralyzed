using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;
using TMPro;
using UnityEngine.Rendering.PostProcessing;

/// <summary>
/// INSPECTION POINT NEEDS TRIGGER THAT IS TAGGED AS "Inspection" TO WORK PROPERLY. OTHER WISE YOU CANNOT PUT OBJECT BACK FROM INSPECT MODE
/// </summary>

public class Inspect : Interactable
{
    InspectBase inspectBase;
    FirstPersonController player;
    public float sizeMultiplier;
    public string itemInfoText;
    private LayerMask defaultLayerMask;


    // public CamZoom camZoom;
    // public float lerpSpeed = 0.5f;

    // [Header("Player Settings")]
    // public float moveSpeedDuringInspect;
    // public float runMultiplierDuringInspect;
    // public float mouseSensitivityDuringInspect;
    // //public CamZoom camZoom;
    // public Transform inspectionPoint;
    // public int maxZoom;
    // public int minZoom;

    // [Header("UI")]
    // public RawImage crosshair;
    // public TextMeshProUGUI descriptionTextUI;
    // public TextMeshProUGUI objectName;
    // public TextMeshProUGUI eToExitUI;
    // public Color startColor;

    // [Header("Audio")]
    // public AudioClip bgSFX;

    // [Header("Other")]
    // public float inspectBlurAmount;
    // public float timeToBlur = 0.5f;

    // public float rotateSpeed = 0.5f;

    // public string itemInfoText;
    // public Color colorWhenInspecting;

    float startForwardMoveSpeed, startBackMoveSpeed, startStrafeMoveSpeed, startRunMultiplier, starXtSensitivity, startYsensitivity;
    float startFieldOfView;

    bool canRotate = false;
    bool lerpDone = false;
    [HideInInspector] public bool inspecting = false;
    Collider col;

    Vector3 startPos, startSize, sizeWhenInspecting;
    Quaternion startRotation;



    public void Start()
    {
        defaultLayerMask = gameObject.layer;
        StartCoroutine(GetInspectionBase(0.2f, InitializeInspectionBase));
    }

    public override void DoAction()
    {
        Vector3 toTarget = (inspectBase.inspectionPoint.position - transform.position).normalized;

        // if (Vector3.Dot(toTarget, transform.forward) <= 0)
        // {
        //     Debug.Log("Inspection point is behind this game object.");
        //     return;
        // }

        //inspectBase.camZoom.Blur(inspectBase.camZoom.cameraEffects.depthOfField.settings.focusDistance, inspectBase.inspectBlurAmount, inspectBase.timeToBlur, true);

        //if (inspectBase.bgSFX != null)
        //    AudioManager.instance.PlayClip(inspectBase.bgSFX, 0f);

        inspecting = true;
        //camZoom.enabled = false;
        inspectBase.eToExitUI.enabled = true;
        inspectBase.crosshairImage.enabled = false;




        StartCoroutine(LerpToInspectionPoint());
        //transform.LookAt(player.transform);
        inspectBase.descriptionTextUI.text = itemInfoText;

        player.m_WalkSpeed = inspectBase.moveSpeedDuringInspect;
        player.m_RunSpeed = inspectBase.runMultiplierDuringInspect;
        player.m_MouseLook.XSensitivity = inspectBase.mouseSensitivityDuringInspect;
        player.m_MouseLook.YSensitivity = inspectBase.mouseSensitivityDuringInspect;
    }

    void Update()
    {
        // Debug.Log("Inspecting: " + inspecting);
        // Debug.Log("canRotate: " + canRotate);
        // Debug.Log("animDone: " + lerpDone);

        //Debug.Log("INSPECTING: " + inspecting);

        if (inspecting && lerpDone)
        {
            if (canRotate)
            {
                if (Input.GetAxis("Mouse X") > 0f)
                {
                    transform.Rotate(new Vector3(0f, -inspectBase.inspectionRotateSpeed * Time.deltaTime, 0f), Space.Self);
                }

                if (Input.GetAxis("Mouse X") < 0f)
                {
                    transform.Rotate(new Vector3(0f, inspectBase.inspectionRotateSpeed * Time.deltaTime, 0f), Space.Self);
                }

                if (Input.GetAxis("Mouse ScrollWheel") > 0f)
                {
                    if (inspectBase.cam.fieldOfView > inspectBase.minZoom)
                        inspectBase.cam.fieldOfView--;
                }

                if (Input.GetAxis("Mouse ScrollWheel") < 0f)
                {
                    if (inspectBase.cam.fieldOfView < inspectBase.maxZoom)
                        inspectBase.cam.fieldOfView++;
                }
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                canRotate = false;
                inspectBase.eToExitUI.enabled = false;
                //camZoom.enabled = true;
                col.isTrigger = false;
                inspectBase.crosshairImage.enabled = true;
                player.enabled = true;
                StartCoroutine(LerpToOriginalPoint(transform.rotation, inspectBase.cam.fieldOfView));
                //inspectBase.camZoom.Blur(inspectBase.inspectBlurAmount, inspectBase.camZoom.zoomOutBlur, inspectBase.timeToBlur, false);

                player.m_WalkSpeed = startForwardMoveSpeed;
                player.m_RunSpeed = startRunMultiplier;
                player.m_MouseLook.XSensitivity = starXtSensitivity;
                player.m_MouseLook.YSensitivity = startYsensitivity;
            }

        }
    }

    IEnumerator LerpToInspectionPoint()
    {
        // Debug.Log("Start: " + startPos);
        // Debug.Log("END: " + inspectionPoint.position);
        float timeStartedLerping = Time.time;
        float lerpPercComplete = 0f;
        Quaternion toRotation = Quaternion.LookRotation(player.transform.position - transform.position);
        //toRotation.eulerAngles = new Vector3(0f, toRotation.eulerAngles.y, toRotation.eulerAngles.z);

        while (lerpPercComplete < 1.0f)
        {
            float timeSinceStarted = Time.time - timeStartedLerping;
            lerpPercComplete = timeSinceStarted / inspectBase.lerpSpeed;
            transform.parent.position = Vector3.Lerp(startPos, inspectBase.inspectionPoint.position, lerpPercComplete);
            transform.parent.localScale = Vector3.Lerp(startSize, sizeWhenInspecting, lerpPercComplete);
            transform.rotation = Quaternion.Lerp(startRotation, toRotation, lerpPercComplete);
            inspectBase.descriptionTextUI.color = Color.Lerp(inspectBase.startColor, inspectBase.colorWhenInspecting, lerpPercComplete);

            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator LerpToOriginalPoint(Quaternion currPoint, float currFieldOfView)
    {
        float timeStartedLerping = Time.time;
        float lerpPercComplete = 0f;

        while (lerpPercComplete < 1.0f)
        {
            float timeSinceStarted = Time.time - timeStartedLerping;
            lerpPercComplete = timeSinceStarted / inspectBase.lerpSpeed;
            transform.parent.position = Vector3.Lerp(inspectBase.inspectionPoint.position, startPos, lerpPercComplete);
            transform.parent.localScale = Vector3.Lerp(sizeWhenInspecting, startSize, lerpPercComplete);
            transform.rotation = Quaternion.Lerp(currPoint, startRotation, lerpPercComplete);
            inspectBase.descriptionTextUI.color = Color.Lerp(inspectBase.colorWhenInspecting, inspectBase.startColor, lerpPercComplete);
            inspectBase.cam.fieldOfView = Mathf.Lerp(currFieldOfView, startFieldOfView, lerpPercComplete);

            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator GetInspectionBase(float delay, Action initialize)
    {
        yield return new WaitForSeconds(0.5f);

        List<FirstPersonController> players = FindObjectsOfType<FirstPersonController>().ToList();

        foreach (FirstPersonController child in players)
        {
            if (child.isMine)
            {
                inspectBase = FindObjectOfType<InspectBase>();
            }
        }

        initialize();
    }

    void InitializeInspectionBase()
    {
        player = inspectBase.player;

        startPos = transform.parent.transform.position;
        startSize = transform.parent.localScale;
        startRotation = transform.rotation;
        startForwardMoveSpeed = inspectBase.player.m_WalkSpeed;

        startRunMultiplier = inspectBase.player.m_RunSpeed;
        starXtSensitivity = inspectBase.player.m_MouseLook.XSensitivity;
        startYsensitivity = inspectBase.player.m_MouseLook.YSensitivity;


        startFieldOfView = inspectBase.cam.fieldOfView;
        sizeWhenInspecting = new Vector3(transform.localScale.x * sizeMultiplier, transform.localScale.y * sizeMultiplier, transform.localScale.z * sizeMultiplier);

        col = transform.GetComponent<Collider>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Inspection"))
        {
            //Debug.Log("ENTERED");
            canRotate = lerpDone = true;
            inspectBase.objectName.enabled = false;
            inspectBase.crosshairScript.enabled = false;
            gameObject.layer = 0;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Inspection"))
        {
            //Debug.Log("EXITED");
            canRotate = lerpDone = inspecting = false;
            inspectBase.objectName.enabled = true;
            inspectBase.crosshairScript.enabled = true;
            gameObject.layer = defaultLayerMask;
        }
    }
}