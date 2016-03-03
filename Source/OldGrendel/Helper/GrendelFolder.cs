using UnityEngine;
using System.Collections;

public class GrendelFolder : MonoBehaviour
{
    [HideInInspector]public Color FolderColor = GrendelColor.Orange;
    [HideInInspector]public Texture DefaultTexture;
    [HideInInspector]public Texture DefaultGradient;

    public static Color[] FolderColors = new Color[]
    {
        Color.red,
        Color.gray,
        Color.green,
        GrendelColor.DarkYellow,
        Color.cyan,
        GrendelColor.Orange,
        GrendelColor.Pink,
        GrendelColor.DarkCyan,
        GrendelColor.DarkMagenta,
        GrendelColor.GrendelYellow,
        GrendelColor.SeaGreen,
        GrendelColor.SlateGrey,
        GrendelColor.Teal,
    };

    public bool IsVisible
    {
        get
        {
            return gameObject.activeSelf;
        }
        set
        {
            gameObject.SetActive(value);
        }
    }
}
