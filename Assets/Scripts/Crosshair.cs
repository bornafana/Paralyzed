using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using TMPro;

public class Crosshair : MonoBehaviourPun
{
    public LayerMask crosshairLayerMask;
    public float rayCastDistance;
    public Camera cam;
    public Transform crosshair;
    public TextMeshProUGUI objName;
    public CanvasGroup UIAnswerSlider;
    public float UISliderFadeSpeed = 0.5f;
    public float textFadeSpeed = 0.5f;

    private Ray ray;
    private RaycastHit hit;
    private Coroutine phoneSlider;
    private bool textFadedIn, phoneUIFadedIn;
    private float transparent = 0f, fullAlpha = 1f;
    private float interactCooldown = 0.2f;


    void Start()
    {
        
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            ray = cam.ScreenPointToRay(new Vector2(crosshair.position.x, crosshair.position.y));
            Debug.DrawRay(ray.origin, ray.direction * rayCastDistance, Color.red, 0.2f);
            if (Physics.Raycast(ray.origin, ray.direction, out hit, rayCastDistance, crosshairLayerMask))
            {
                //Debug.Log("Hitting : " + hit.transform.name);
                objName.text = hit.transform.name;

                if (!textFadedIn)
                    StartCoroutine(TextFadeIn());

                //if (hit.transform.CompareTag("Phone") && !phoneUIFadedIn)
                //{
                //    StartCoroutine(PhoneSliderFadeIn());
                //}


                if (Input.GetKeyDown(KeyCode.E))
                {
                    //StartCoroutine(BeginInteractCooldown(interactCooldown));
                    if (hit.transform.GetComponent<Interactable>())
                    {
                        hit.transform.GetComponent<Interactable>().DoAction();
                    }
                }
            }
            else
            {
                if (phoneUIFadedIn)
                {
                    StartCoroutine(PhoneSliderFadeOut());
                }

                if (textFadedIn)
                    StartCoroutine(TextFadeOut());

                //objName.text = "";
            }

            //Debug.DrawRay(ray.origin, ray.direction * 10f, Color.red);
        }
    }

    IEnumerator BeginInteractCooldown(float delay)
    {
        var distance = rayCastDistance;
        rayCastDistance = 0.001f;
        yield return new WaitForSeconds(delay);
        rayCastDistance = distance;
    }

    IEnumerator PhoneSliderFadeIn()
    {
        float timeStartedLerping = Time.time;
        float lerpPercComplete = 0f;
        phoneUIFadedIn = true;

        while (lerpPercComplete < 1.0f)
        {
            float timeSinceStarted = Time.time - timeStartedLerping;
            lerpPercComplete = timeSinceStarted / UISliderFadeSpeed;
            UIAnswerSlider.alpha = Mathf.Lerp(transparent, fullAlpha, lerpPercComplete);
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator PhoneSliderFadeOut()
    {
        float timeStartedLerping = Time.time;
        float lerpPercComplete = 0f;
        phoneUIFadedIn = false;

        while (lerpPercComplete < 1.0f)
        {
            float timeSinceStarted = Time.time - timeStartedLerping;
            lerpPercComplete = timeSinceStarted / UISliderFadeSpeed;
            UIAnswerSlider.alpha = Mathf.Lerp(fullAlpha, transparent, lerpPercComplete);

            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator TextFadeIn()
    {
        float timeStartedLerping = Time.time;
        float lerpPercComplete = 0f;
        textFadedIn = true;

        while (lerpPercComplete < 1.0f)
        {
            //Debug.Log("fade in");
            float timeSinceStarted = Time.time - timeStartedLerping;
            lerpPercComplete = timeSinceStarted / textFadeSpeed;
            objName.alpha = Mathf.Lerp(transparent, fullAlpha, lerpPercComplete);
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator TextFadeOut()
    {
        float timeStartedLerping = Time.time;
        float lerpPercComplete = 0f;
        textFadedIn = false;

        while (lerpPercComplete < 1.0f)
        {
            //Debug.Log("fade out");
            float timeSinceStarted = Time.time - timeStartedLerping;
            lerpPercComplete = timeSinceStarted / textFadeSpeed;
            objName.alpha = Mathf.Lerp(fullAlpha, transparent, lerpPercComplete);

            yield return new WaitForEndOfFrame();
        }
    }
}
