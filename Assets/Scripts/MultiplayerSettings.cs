using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MultiplayerSettings : MonoBehaviour
{
    public static MultiplayerSettings multiplayerSettings;
    public bool delayStart;
    public int maxPlayers = 3;
    public int menuScene;
    public int multiplayerScene;

    void Awake()
    {
        if (multiplayerSettings == null)
        {
            multiplayerSettings = this;
        }
        else
        {
            if (multiplayerSettings != this)
            {
                Destroy(gameObject);
            }
        }
        
        DontDestroyOnLoad(gameObject);
    }


    void Start()
    {
    }
}
