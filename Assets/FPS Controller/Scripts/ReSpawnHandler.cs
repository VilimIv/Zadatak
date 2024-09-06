using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Multiplayer.Samples.Utilities.ClientAuthority;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(FPSCharacterManager)), RequireComponent(typeof(Health))]
public class ReSpawnHandler : MonoBehaviour
{
    public bool Respawn = true;
    public float RespwanTime = 5f;
    public float RestartTime = 30f;
    [Header("Other Refrences")]

    public TextMeshProUGUI CountdownText;

    [HideInInspector] public bool RespawnInProcess;
    [HideInInspector] public bool Restart;
    FPSCharacterManager characterManager;
    public float CountdownTimer;
    // Start is called before the first frame update
    void Start()
    {
        characterManager = GetComponent<FPSCharacterManager>();
        if (CountdownText != null)
            CountdownText.gameObject.SetActive(false);

        CountdownTimer = RespwanTime;
    }

    // Update is called once per frame
    void Update()
    {
        #region Respawn
        if (Respawn)
        {
            if (RespawnInProcess)
            {
                if (GameManager.MatchEnded)
                    return;

                if (CountdownText != null)
                    CountdownText.gameObject.SetActive(true);

                CountdownTimer -= Time.deltaTime;

                if (CountdownText != null)
                    CountdownText.text = CountdownTimer.ToString("f0");

                if (CountdownTimer <= 0)
                {
                    ReSpawn();
                    RespawnInProcess = false;

                    GameManager.MatchEnded = false;

                    if (CountdownText != null)
                        CountdownText.gameObject.SetActive(false);

                    CountdownTimer = RespwanTime;
                }
            }
        }
        else
        {
            if (RespawnInProcess)
            {
                RespawnInProcess = false;
            }
        }

        #endregion

        #region Restart
        if (Restart)
        {
            if (CountdownText != null)
                CountdownText.gameObject.SetActive(true);

            CountdownTimer -= Time.deltaTime;

            if (CountdownText != null)
                CountdownText.text = CountdownTimer.ToString("f0");

            if (CountdownTimer <= 0)
            {
                RestartMatch();

                if (GetComponent<FPSCharacterManager>().IsOwner)
                {
                    GetComponent<FPSCharacterManager>().Eliminations.Value = 0;
                }

                GameManager.MatchEnded = false;

                if (CountdownText != null)
                    CountdownText.gameObject.SetActive(false);

                CountdownTimer = RespwanTime;
                Restart = false;
            }
        }
        #endregion
    }

    void ReSpawn()
    {
        GameManager.MatchEnded = false;
        GameManager.S_WinPanel.gameObject.SetActive(false);
        GameManager.S_YouLosePanel.gameObject.SetActive(false);
        int Rand = Random.Range(0, GameManager.S_RespawnPoints.Count);
        transform.position = GameManager.S_RespawnPoints[Rand].position;
        transform.rotation = GameManager.S_RespawnPoints[Rand].rotation;

        GetComponent<Health>().CurrentHealth.Value = 100;
        GetComponent<Health>().isDead = false;
        GetComponent<Health>().isDeadCounted = false;

        characterManager.enabled = true;
        characterManager.Refrences.CharcaterAniamtor.SetBool("Death_b", false);
        characterManager.GrabWeapon(0);
        GetComponent<Health>().ShowCharacterServerRPC();

        ResetPlayerPositionServerRPC();
        characterManager.Refrences.CharcaterAniamtor.transform.localPosition = Vector3.zero;

        if (GetComponent<CameraMovement>() != null)
            GetComponent<CameraMovement>().enabled = true;

        if (GetComponent<PlayerController>() != null)
        {
            var playerController = GetComponent<PlayerController>();
            playerController.enabled = true;

            playerController.Velocity = Vector3.zero;

            CharacterController controller = playerController.Controller;
            if (controller != null)
            {
                controller.enabled = false;
                controller.transform.position = transform.position;
                controller.enabled = true;
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    void ResetPlayerPositionServerRPC()
    {
        Debug.Log("POsition Rested Once again :(");
        GetComponent<FPSCharacterManager>().Refrences.CharcaterAniamtor.transform.localPosition = Vector3.zero;
    }

    void RestartMatch()
    {
        GameManager.MatchEnded = false;
        GameManager.S_WinPanel.gameObject.SetActive(false);
        GameManager.S_YouLosePanel.gameObject.SetActive(false);
        int Rand = Random.Range(0, GameManager.S_RespawnPoints.Count);
        transform.position = GameManager.S_RespawnPoints[Rand].position;
        transform.rotation = GameManager.S_RespawnPoints[Rand].rotation;

        GetComponent<Health>().CurrentHealth.Value = 100;
        GetComponent<Health>().isDead = false;
        GetComponent<Health>().isDeadCounted = false;

        characterManager.enabled = true;
        characterManager.Refrences.CharcaterAniamtor.SetBool("Death_b", false);
        characterManager.GrabWeapon(0);
        GetComponent<Health>().ShowCharacterServerRPC();

        ResetPlayerPositionServerRPC();
        characterManager.Refrences.CharcaterAniamtor.transform.localPosition = Vector3.zero;

        if (GetComponent<CameraMovement>() != null)
            GetComponent<CameraMovement>().enabled = true;

        if (GetComponent<PlayerController>() != null)
        {
            var playerController = GetComponent<PlayerController>();
            playerController.enabled = true;

            playerController.Velocity = Vector3.zero;

            CharacterController controller = playerController.Controller;
            if (controller != null)
            {
                controller.enabled = false;
                controller.transform.position = transform.position;
                controller.enabled = true;
            }
        }
    }
}