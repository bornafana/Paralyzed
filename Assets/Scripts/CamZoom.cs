using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;



public class CamZoom : MonoBehaviour
{
    public Camera mainCam;
    Animator camController;
    //public PostProcessingBehaviour postProcessing;
    public Crosshair crosshair;

    [Header("Zoom Settings")]
    float zoomCooldown = 2f; // DOES NOT WORK

    public float zoomInFOV;
    public float zoomOutFOV;
    public float zoomInTime;
    public float zoomOutTime;

    float camRemainingTime;
    float blurRemainingTime;
    float vignetteRemainingTime;
    float shakeRemainingTime;
    AudioSource audioSource;
    //[HideInInspector] public PostProcessingProfile cameraEffects;

    //DepthOfFieldModel.Settings depthOfFieldSettings;
    //VignetteModel.Settings vignetteSettings;

    bool isZoomed = false; // Is the camera zoomed?
    bool canZoom = true; // Used in case the player's zoom is on cooldown
    bool canExtraZoom = true; // Should the painting zoom animations happen?
    bool lookedAtPainting = false; // Used to zoom out when player looks at the painting and then looks away to be able to zoom out cleanly
    Ray ray;
    RaycastHit hit;
    LayerMask layerMask;

    void Start()
    {
        layerMask = LayerMask.GetMask("Interactable");

        camController = gameObject.GetComponent<Animator>();

        //cameraEffects = postProcessing.profile;
        audioSource = GetComponent<AudioSource>();

        // depthOfFieldSettings = cameraEffects.depthOfField.settings;
        // vignetteSettings = cameraEffects.vignette.settings;

        // vignetteSettings.intensity = defaultVignette;
        // cameraEffects.vignette.settings = vignetteSettings;
        // cameraEffects.depthOfField.settings = depthOfFieldSettings;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("CAN ZOOM: " + canZoom);
        if (!canZoom)
            return;

        if (Input.GetMouseButtonDown(1))
        {
            ZoomIn();
        }
        if (Input.GetMouseButtonUp(1))
        {
            ZoomOut();
        }

        if (isZoomed && canExtraZoom) // Looks for painting and zooms extra + effects if detected
        {
            ray = crosshair.cam.ScreenPointToRay(new Vector2(crosshair.crosshair.position.x, crosshair.crosshair.position.y));
            if (Physics.Raycast(ray.origin, ray.direction, out hit, crosshair.rayCastDistance, layerMask))
            {
                Debug.Log("OUT: " + hit.transform);
                //if (hit.transform.CompareTag(""))
                //{
                //    // Add camera effects here if whatever tag is detected
                //    camController.SetBool("shake", true);
                //    canExtraZoom = false;
                //    lookedAtPainting = true;
                //}
            }
            // else
            // {
            //     ToggleCanZoom();
            //     ZoomOut();
            // }
        }
    }

    void ZoomIn()
    {
        //StartCoroutine(ZoomBlurCor(cameraEffects.depthOfField.settings.focusDistance, zoomInBlur, blurTime, true));
        isZoomed = true;
        StartCoroutine(CameraZoom(mainCam.fieldOfView, zoomInFOV, zoomInTime));
    }

    void ZoomOut()
    {
        StartCoroutine(ToggleCanZoom(false));
        StartCoroutine(ToggleCanZoom(true, zoomCooldown));

        StopAllCoroutines();

        isZoomed = false;
        canExtraZoom = true;
        //camController.SetBool("shake", false);

        StartCoroutine(CameraZoom(mainCam.fieldOfView, zoomOutFOV, zoomOutTime));
    }

    IEnumerator CameraZoom(float startValue, float endValue, float seconds)
    {
        float timeStartedLerping = Time.time;
        float camZoomPercComplete = 0f;

        while (camZoomPercComplete < 1.0f)
        {
            float timeSinceStarted = Time.time - timeStartedLerping;
            camZoomPercComplete = timeSinceStarted / seconds;
            camRemainingTime = timeSinceStarted;
            mainCam.fieldOfView = Mathf.Lerp(startValue, endValue, camZoomPercComplete);
            yield return new WaitForEndOfFrame();
        }
    }

    //public void Blur(float startValue, float endValue, float seconds, bool blurStart)
    //{
    //    StartCoroutine(ZoomBlurCor(startValue, endValue, seconds, blurStart));
    //}

    //IEnumerator ZoomBlurCor(float startValue, float endValue, float seconds, bool blurStart)
    //{
    //    if (!postProcessing.profile.depthOfField.enabled && blurStart)
    //    {
    //        postProcessing.profile.depthOfField.enabled = blurStart;
    //    }

    //    float timeStartedLerping = Time.time;
    //    float blurPercentageComplete = 0f;

    //    while (blurPercentageComplete < 1.0f)
    //    {
    //        //Debug.Log("Depth: " + depthOfFieldSettings.focusDistance);
    //        float timeSinceStarted = Time.time - timeStartedLerping;
    //        blurPercentageComplete = timeSinceStarted / seconds;
    //        blurRemainingTime = timeSinceStarted;
    //        depthOfFieldSettings.focusDistance = Mathf.Lerp(startValue, endValue, blurPercentageComplete);
    //        cameraEffects.depthOfField.settings = depthOfFieldSettings;
    //        yield return new WaitForEndOfFrame();
    //    }

    //    if (postProcessing.profile.depthOfField.enabled && !blurStart)
    //    {
    //        postProcessing.profile.depthOfField.enabled = blurStart;
    //    }
    //}

    //IEnumerator ZoomVignette(float startValue, float endValue, float seconds)
    //{
    //    float timeStartedLerping = Time.time;
    //    float vignettePercentageComplete = 0f;

    //    while (vignettePercentageComplete < 1.0f)
    //    {
    //        float timeSinceStarted = Time.time - timeStartedLerping;
    //        vignettePercentageComplete = timeSinceStarted / seconds;
    //        vignetteRemainingTime = timeSinceStarted;
    //        vignetteSettings.intensity = Mathf.Lerp(startValue, endValue, vignettePercentageComplete);
    //        cameraEffects.vignette.settings = vignetteSettings;
    //        yield return new WaitForEndOfFrame();
    //    }
    //}

    //IEnumerator ZoomShake(float startValue, float endValue, float seconds)
    //{
    //    float timeStartedLerping = Time.time;
    //    float shakePercentageComplete = 0f;

    //    while (shakePercentageComplete < 1.0f)
    //    {
    //        float timeSinceStarted = Time.time - timeStartedLerping;
    //        shakePercentageComplete = timeSinceStarted / seconds;
    //        shakeRemainingTime = timeSinceStarted;
    //        camController.speed = Mathf.Lerp(startValue, endValue, shakePercentageComplete);
    //        yield return new WaitForEndOfFrame();
    //    }

    //}

    IEnumerator ToggleCanZoom(bool toggle, float seconds = 0f)
    {
        yield return new WaitForSeconds(seconds);

        canZoom = toggle;
        Debug.Log("CAN ZOOM cor: " + canZoom);
    }
}


