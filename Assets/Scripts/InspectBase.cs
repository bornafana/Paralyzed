﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityStandardAssets.Characters.FirstPerson;

/// <summary>
/// ATTACH THIS SCRIPT TO AN EMPTY, AND CHILD IT TO THE PLAYER. 
/// </summary>
///

public class InspectBase : MonoBehaviour
{
    public float lerpSpeed = 0.5f;

    [Header("UI")]
    public Image crosshairImage;
    public TextMeshProUGUI descriptionTextUI;
    public TextMeshProUGUI objectName;
    public TextMeshProUGUI eToExitUI;
    public Color startColor;
    public Color colorWhenInspecting;


    [Header("Player Settings")]
    public Crosshair crosshairScript;
    public FirstPersonController player;
    public Camera cam;
    public float moveSpeedDuringInspect;
    public float runMultiplierDuringInspect;
    public float mouseSensitivityDuringInspect;
    public CamZoom camZoom;
    public Transform inspectionPoint;
    public int maxZoom;
    public int minZoom;
    public float inspectionRotateSpeed;
    //public float inspectBlurAmount;
    //public float timeToBlur;

    [Header("Audio")]
    public AudioClip bgSFX;

    protected float startForwardMoveSpeed, startBackMoveSpeed, startStrafeMoveSpeed, startRunMultiplier, starXtSensitivity, startYsensitivity;
    protected float startFieldOfView;

    protected Vector3 startPos, startSize, sizeWhenInspecting;
    protected Quaternion startRotation;


    public void Start()
    {
        startPos = transform.position;
        startSize = transform.localScale;
        startRotation = transform.rotation;

        startForwardMoveSpeed = player.m_WalkSpeed;
        startRunMultiplier = player.m_RunSpeed;
        starXtSensitivity = player.m_MouseLook.XSensitivity;
        startYsensitivity = player.m_MouseLook.YSensitivity;

        startFieldOfView = cam.fieldOfView;
    }
}