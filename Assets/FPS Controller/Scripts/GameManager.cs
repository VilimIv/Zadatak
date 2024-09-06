using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int KillsToWin = 10;
    public static int S_KillsToWin;
    public List<Transform> RespawnPoints;
    public static List<Transform> S_RespawnPoints;

    public FPSCharacterManager[] AllPlayers;

    public GameObject WinPanel;
    public GameObject YouLosePanel;

    public static GameObject S_WinPanel;
    public static GameObject S_YouLosePanel;

    public static bool MatchEnded;

    void Awake()
    {
        S_RespawnPoints = RespawnPoints;
        S_KillsToWin = KillsToWin;
        S_WinPanel = WinPanel;
        S_YouLosePanel = YouLosePanel;
    }

    private void Update()
    {
        if (MatchEnded)
            return;

        AllPlayers = FindObjectsOfType<FPSCharacterManager>();

        for (int i = 0; i < AllPlayers.Length; i++)
        {
            if (AllPlayers[i].Eliminations.Value >= KillsToWin)
            {
                if (AllPlayers[i].IsOwner)
                {
                    WinPanel.SetActive(true);
                    AllPlayers[i].gameObject.GetComponent<Health>().CurrentHealth.Value = 100;
                    AllPlayers[i].gameObject.GetComponent<ReSpawnHandler>().CountdownTimer = AllPlayers[i].gameObject.GetComponent<ReSpawnHandler>().RestartTime;
                    AllPlayers[i].gameObject.GetComponent<ReSpawnHandler>().Restart = true;
                    MatchEnded = true;
                }
                else
                {
                    S_YouLosePanel.SetActive(true);
                    AllPlayers[i].gameObject.GetComponent<Health>().CurrentHealth.Value = 100;
                    AllPlayers[i].gameObject.GetComponent<ReSpawnHandler>().CountdownTimer = AllPlayers[i].gameObject.GetComponent<ReSpawnHandler>().RestartTime;
                    AllPlayers[i].gameObject.GetComponent<ReSpawnHandler>().Restart = true;
                    MatchEnded = true;
                }
            }
        }
    }
}