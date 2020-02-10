using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Dropdown))]
public class PlayerSelection : MonoBehaviour
{
    public static string playerSelection;
    private TMP_Dropdown dropdown;

    public static string playerTypePrefKey = "PlayerType";

    void Start()
    {
        dropdown = GetComponent<TMP_Dropdown>();
        SetPlayerType();
    }

    void Update()
    {

    }

    public void SetPlayerType()
    {
        string type = dropdown.options[dropdown.value].text;

        if (type == null || string.IsNullOrEmpty(type))
        {
            Debug.LogError("Player Type is null or empty");
            return;
        }

        PhotonNetwork.NickName = type;
        PlayerPrefs.SetString(playerTypePrefKey, type);
    }
}
