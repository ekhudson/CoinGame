﻿using UnityEngine;
using System.Collections;

public class Scoreboard : MonoBehaviour 
{
    private static Vector2 sGUICoinPos = Vector2.zero;

    private void Start()
    {
        EventManager.Instance.AddHandler<CoinEvent>(CoinEventHandler);
    }

    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(0, 0, Screen.width, 64f), GUI.skin.box);

        GUILayout.BeginHorizontal();

        GUILayout.FlexibleSpace();

        DrawScores();

        if (GUILayout.Button("Save"))
        {
            PlayerData playerData = PlayerManager.Instance.GetPlayer(0);

            SaveLoadController.Instance.Save<PlayerData>(PlayerManager.Instance.GetPlayer(0));
        }

        if (GUILayout.Button("Load"))
        {
            PlayerManager.Instance.LoadPlayerData(0, SaveLoadController.Instance.Load());
        }

        GUILayout.FlexibleSpace();

        GUILayout.EndHorizontal();

        GUILayout.EndArea();
    }

    private void DrawScores()
    {
        int numPlayers = PlayerManager.Instance.GetPlayerCount();

        for (int i = 0; i < numPlayers; i++)
        {
            DrawPlayerScorecard(i, SessionManager.Instance.CurrentPlayerIndex == i);
        }
    }

    private void DrawPlayerScorecard(int playerIndex, bool isCurrentPlayer)
    {
        PlayerData playerData = PlayerManager.Instance.GetPlayer(playerIndex);

        string tempText = string.Format("{0}: {1}", playerData.PlayerName, playerData.GetCurrentScore().ToString());

        if (isCurrentPlayer)
        {
            GUI.color = Color.cyan;
        }

        GUILayout.BeginHorizontal();

        GUILayout.Label(tempText, GUI.skin.button, GUILayout.Height(64f));

        GUILayout.EndHorizontal();

        GUI.color = Color.white;
    }

    public void CoinEventHandler(object sender, CoinEvent evt)
    {
        if (evt.CoinEventType == CoinEvent.CoinEventTypes.SETTLED_FACE_UP)
        {
            PlayerManager.Instance.GetPlayer(SessionManager.Instance.CurrentPlayerIndex).AddToScore(evt.Coin.Value);
        }
    }
}
