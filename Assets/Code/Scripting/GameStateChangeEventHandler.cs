﻿using UnityEngine;
using UnityEngine.Events;

public class GameStateChangeEventHandler : SimpleScript.ActionBase
{
    [SerializeField]
    private GameManager.GameStates m_GameState;

    [SerializeField]
    private UnityEvent m_OnStateChanged;

    private void Start()
    {
        EventManager.Instance.AddHandler<GameStateChangeEvent>(GameStateChangedHandler);
    }

    private void OnDestroy()
    {
        EventManager.Instance.RemoveHandler<GameStateChangeEvent>(GameStateChangedHandler);
    }

    private void GameStateChangedHandler(object sender, GameStateChangeEvent gameStateChangedEvent)
    {
        if (gameStateChangedEvent.NewState == m_GameState)
        {

        }
    }
}
