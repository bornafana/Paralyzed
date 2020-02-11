using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class LifeCounter : MonoBehaviourPun
{
    //public static LifeCounter lives;
    public int livesRemaining;
    public int totalLives;


    void Awake()
    {
    //    if (lives == null)
    //    {
    //        lives = this;
    //    }
    //    else
    //    {
    //        if (lives != this)
    //        {
    //            Destroy(lives.gameObject);
    //            lives = this;
    //        }
    //    }

    //    DontDestroyOnLoad(lives.gameObject);
    }

    void Start()
    {
        livesRemaining = totalLives;
    }

    void Update()
    {
    }

    public void AddLife()
    {
        photonView.RPC("RPC_SetRemainingLives", RpcTarget.All, ++livesRemaining);
    }

    public void RemoveLife()
    {
        //SetRemainingLives(--livesRemaining);
        photonView.RPC("RPC_SetRemainingLives", RpcTarget.All, --livesRemaining);
    }

    public void SetTotalLives(int newValue)
    {
        livesRemaining = newValue;
    }

    [PunRPC]
    public void RPC_SetTotalLives(int newValue)
    {
        livesRemaining = newValue;
        photonView.RPC("RPC_Whistle", RpcTarget.All, "whistle");
    }

    [PunRPC]
    public void RPC_SetRemainingLives(int newValue)
    {
        livesRemaining = newValue;
        livesRemaining = Mathf.Clamp(livesRemaining, 0, totalLives);
    }


   

}
