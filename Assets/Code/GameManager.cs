using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public enum GameStates
    {
        PreInit,
        Initializing,
        FrontEnd,
        Loadout,
        InGame,
        PostGame,
    }

    [SerializeField]
    private GameStates m_CurrentGameState = GameStates.PreInit;

    [SerializeField]
    private Scene m_SceneToLoad;

    private float mCurrentLoadingProgress = 0f;

    public void Update()
    {
        switch(m_CurrentGameState)
        {
            case GameStates.PreInit:

                StartCoroutine(LoadScene(m_SceneToLoad.name));
                m_CurrentGameState = GameStates.Initializing;

            break;

            case GameStates.Initializing:

            break;

            case GameStates.FrontEnd:

            break;

            case GameStates.Loadout:

            break;

            case GameStates.InGame:

            break;

            case GameStates.PostGame:

            break;
        }
    }

    private void SetNewState(GameStates newState)
    {
        if (newState == m_CurrentGameState)
        {
            return;
        }

        switch (m_CurrentGameState)
        {
            case GameStates.PreInit:

            break;

            case GameStates.Initializing:

            break;

            case GameStates.FrontEnd:

            break;

            case GameStates.Loadout:

            break;

            case GameStates.InGame:

            break;

            case GameStates.PostGame:

            break;
        }

        GameStates previousState = m_CurrentGameState;
        m_CurrentGameState = newState;

        EventManager.Instance.Post(new GameStateChangeEvent(this, previousState, newState));
    }

    IEnumerator LoadScene(string scene)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);

        while (!asyncLoad.isDone)
        {
            mCurrentLoadingProgress = asyncLoad.progress;
            yield return null;
        }
    }
}
