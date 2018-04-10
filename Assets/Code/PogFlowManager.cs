using UnityEngine;
using System.Collections.Generic;

public class PogFlowManager : MonoBehaviour
{
    public int m_InitialScreen = 0;
    public List<GameObject> m_Screens;

    public void Start()
    {
        SetActiveScreen(m_InitialScreen);
    }

    public void SetActiveScreen(int index)
    {
        if (index < 0 || index >= m_Screens.Count)
        {
            Debug.LogError("Invalid screen index", this);
            return;
        }

        for(int i = 0; i < m_Screens.Count; i++)
        {
            if (i != index)
            {
                m_Screens[i].SetActive(false);
            }
            else
            {
                m_Screens[i].SetActive(true);
            }
        }
    }
}
