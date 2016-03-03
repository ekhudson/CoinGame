using UnityEngine;
using System.Collections;

[System.Serializable]
public class GrendelConsoleOptions
{
    #region PUBLIC VARIABLES
    public int NumberOfCommandsToShow = 16; //number of previous commands to show
    public float ConsoleHeight = 400;
    public bool PauseGameWhenOpen = false;
    public bool DetailView = false; //detail view shows time, date and user of each command line
    public bool OutputLogOnQuit = false; //does the console output a log on quit?
    public Color ConsoleColor = Color.white; //the colorization of the console;
    public GUIStyle Style_UserCurrent; //User Style, this is the Style used in the text field of the console
    public GUIStyle Style_UserPrevious; //The Style of previous console commands entered by the user
    public GUIStyle Style_Admin; //the Style of responses from the console
    public GUIStyle Style_Error; //the Style of errors from the console
    public GUIStyle Style_Detail; //the Style of details in the console
    #endregion
}
