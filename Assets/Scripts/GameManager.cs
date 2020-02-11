using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Realtime;
using UnityEngine.UI;
using Photon.Pun;
using UnityStandardAssets.Characters.FirstPerson;

#pragma warning disable 649

/// <summary>
/// Game manager.
/// Connects and watch Photon Status, Instantiate Player
/// Deals with quiting the room and the game
/// Deals with level loading (outside the in room synchronization)
/// </summary>
public class GameManager : MonoBehaviourPunCallbacks
{
    #region Public Fields

    public static GameManager Instance;
    public LifeCounter lifeCounter;

    public GameObject localPlayer;
    public float playerSpawnDelay;
    public Text pingText;
    private Transform playerRespawnPoint;

    #endregion

    #region Private Fields

    private GameObject instance;

    [Tooltip("The prefab to use for representing the player")]
    [SerializeField]
    private GameObject playerPrefab;

    #endregion

    #region MonoBehaviour CallBacks


    /// <summary>
    /// MonoBehaviour method called on GameObject by Unity during initialization phase.
    /// </summary>
    void Start()
    {
        playerRespawnPoint = transform;
        Instance = this;

        // in case we started this demo with the wrong scene being active, simply load the menu scene
        if (!PhotonNetwork.IsConnected)
        {
            SceneManager.LoadScene(0);

            return;
        }

        StartCoroutine(SpawnPlayerWithDelay());
        //if (FirstPersonController.LocalPlayerInstance == null)
        //{
        //    Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);
        //    localPlayer = PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(0f, 2f, 0f), Quaternion.identity, 0);

        //    // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
        //    //if(PhotonNetwork.NickName.ToLower().Trim().Contains("blind"))
        //    //    PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(0f, 2f, 0f), Quaternion.identity, 1);
        //    //else if (PhotonNetwork.NickName.ToLower().Trim().Contains("deaf"))
        //    //    PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(5f, 2f, 0f), Quaternion.identity, 2);
        //    //else if (PhotonNetwork.NickName.ToLower().Trim().Contains("mute"))
        //    //    PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(10f, 5f, 0f), Quaternion.identity, 3);
        //    //else
        //    //    PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(10f, 5f, 0f), Quaternion.identity, 4);

        //    //Debug.Log("Spawning");
        //}
        //else
        //{

        //    Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
        //}
    }

    IEnumerator SpawnPlayerWithDelay()
    {
        yield return new WaitForSeconds(playerSpawnDelay);

        if (RigidbodyFirstPersonController.LocalPlayerInstance == null)
        {
            Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);
            localPlayer = PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(0f, 2f, 0f), Quaternion.identity, 0);
        }
        else
        {

            Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
        }
    }



    /// <summary>
    /// MonoBehaviour method called on GameObject by Unity on every frame.
    /// </summary>
    void Update()
    {
        pingText.text = "Ping: " + PhotonNetwork.GetPing();
        // "back" button of phone equals "Escape". quit app if that's pressed
        if (Input.GetKeyDown(KeyCode.Tilde))
        {
            PhotonNetwork.LoadLevel(0);
        }

        CheckForGameOver();
    }

    public void RespawnPlayer()
    {
        StartCoroutine(PlayerControlsCooldown(0.5f));
        lifeCounter.RemoveLife();
        localPlayer.transform.position = playerRespawnPoint.position;
    }

    private IEnumerator PlayerControlsCooldown(float delay)
    {
        if (localPlayer.GetComponent<RigidbodyFirstPersonController>() != null)
        {
            var playerControls = localPlayer.GetComponent<RigidbodyFirstPersonController>();
            playerControls.GetComponent<Rigidbody>().velocity = Vector3.zero;
            playerControls.enabled = false;
            yield return new WaitForSeconds(delay);
            playerControls.enabled = true;
        }
        else
        {
            var playerControls = localPlayer.GetComponent<FirstPersonController>();
            playerControls.enabled = false;
            yield return new WaitForSeconds(delay);
            playerControls.enabled = true;
        }
    }

    void CheckForGameOver()
    {
        if (lifeCounter.livesRemaining <= 0)
        {
            LeaveRoom();
            PhotonNetwork.LoadLevel(0);
        }
    }

    #endregion

    #region Photon Callbacks

    /// <summary>
    /// Called when a Photon Player got connected. We need to then load a bigger scene.
    /// </summary>
    /// <param name="other">Other.</param>
    public override void OnPlayerEnteredRoom(Player other)
    {
        Debug.Log("OnPlayerEnteredRoom() " + other.NickName); // not seen if you're the player connecting

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom
        }
    }

    /// <summary>
    /// Called when a Photon Player got disconnected. We need to load a smaller scene.
    /// </summary>
    /// <param name="other">Other.</param>
    public override void OnPlayerLeftRoom(Player other)
    {
        Debug.Log("OnPlayerLeftRoom() " + other.NickName); // seen when other disconnects

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom
        }
    }

    /// <summary>
    /// Called when the local player left the room. We need to load the launcher scene.
    /// </summary>
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }

    #endregion

    #region Public Methods

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void QuitApplication()
    {
        Application.Quit();
    }

    #endregion

}
