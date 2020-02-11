using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
    private LifeCounter lifeCounter;
    public TextMeshProUGUI livesText;

    void Start()
    {
        lifeCounter = FindObjectOfType<LifeCounter>();
    }

    void Update()
    {
        if (livesText != null)
        {
            livesText.text = "Lives Remaining: " + lifeCounter.livesRemaining;
        }
        else
        {
            livesText.text = "Life counter not found";
        }
    }
}
