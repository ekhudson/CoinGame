using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

        /// <summary>
        /// Title: Grendel Engine
        /// Author: Elliot Hudson
        /// Date: Mar 12, 2012
        /// 
        /// Filename: Console.cs
        /// 
        /// Summary: Pops up the console and parses
        /// commands entered
        /// 
        /// </summary>

public class Console : Singleton<Console> 
{
        
    #region ConsoleLine Data Type
    //this class holds previous console text and Style
    public class ConsoleLine
    {
            public string User = "none";
            public string CommandDateTime = "none";                
            public string TextString = "";
            public GUIStyle Style;
            public bool NewLine = true;

            public string DetailText
            {
                    get
                    {
                            return Console.Instance.DetailView == true && User != string.Empty ? System.String.Format("{0} {1}: ", StringTool.ForceLength(User, 8), CommandDateTime) : string.Empty;
                    }
            }                
    }
    #endregion

    #region PUBLIC VARIABLES        
    public static List<ConsoleLine> ConsoleLineList = new List<ConsoleLine>(0); //list of previous console commands
    public int NumberOfCommandsToShow = 16; //number of previous commands to show
    public float ConsoleHeight = 400;
    public bool PauseGameWhenOpen = false;
    public bool DetailView = false; //detail view shows time, date and user of each command line
    public bool OutputLogOnQuit = false; //does the console output a log on quit?
    public string InputString = ""; //default console text, should be empty        
    public Color ConsoleColor = Color.white; //the colorization of the console;
    public GUIStyle Style_UserCurrent; //User Style, this is the Style used in the text field of the console
    public GUIStyle Style_UserPrevious; //The Style of previous console commands entered by the user
    public GUIStyle Style_Admin; //the Style of responses from the console
    public GUIStyle Style_Error; //the Style of errors from the console
    public GUIStyle Style_Detail; //the Style of details in the console
    #endregion

    #region PRIVATE VARIABLES
    private bool _showConsole = false; //is the FPS Counter visible?
    private float _previousTimeScale = 1f; //the time scale before opening console
    private Vector2 _scrollPoint = Vector2.zero;
    private int _previousCommandCount = 0;
    private bool mLineOpen = false; //a new line was created, but not yet closed
    #endregion

    #region PRIVATE CONSTANTS
    private const string kConsoleTitle = "Console";
    #endregion
    
    //Toggles the visibility of the console
    public void ToggleConsole()
    {                
            if (!_showConsole)
            {
                    _showConsole = true;
                    Cursor.visible = true;
                    if(PauseGameWhenOpen){_previousTimeScale = Time.timeScale; Time.timeScale = 0f;}
            }
            else
            {                        
                    _showConsole = false;
                    if (GameOptions.Instance.HideDefaultCursor) { Cursor.visible = false; }
                    if(PauseGameWhenOpen){Time.timeScale = _previousTimeScale;}
            }

    }

	private void Start()
	{
		EventManager.Instance.AddHandler<UserInputKeyEvent>(UserInputHandler);
	}

	private void UserInputHandler(object sender, UserInputKeyEvent evt)
	{
        //if (evt.KeyBind == UserInput<MonoBehaviour>.Instance.ToggleConsole && evt.Type == UserInputKeyEvent.TYPE.KEYDOWN)
        //{
        //    ToggleConsole();
        //}
	}
    
    void OnGUI()
    {                
            //Check if the console should be showing
            if (_showConsole)
            {
                    GUI.color = ConsoleColor;
                    //GUI.Box(new Rect(0, 0,Screen.width, ConsoleHeight), new GUIContent("Console"), GrendelCustomStyles.CustomElement(GUI.skin.box, ConsoleColor, Color.white,TextAnchor.UpperCenter)); //define the console box
                    
                    if(_previousCommandCount < ConsoleLineList.Count)
                    {
                            _scrollPoint.y = 100000000; //gotta be a better solution to this
                    }
                    _scrollPoint = GUILayout.BeginScrollView(_scrollPoint, GrendelCustomStyles.CustomElement(GUI.skin.box, ConsoleColor, Color.white,TextAnchor.UpperCenter), new GUILayoutOption[]{ GUILayout.Height(ConsoleHeight), GUILayout.Width(Screen.width) }); //define the console box
                            GUI.color = Color.white;                                
                            
                            //Do our key input checks first, otherwise the Textfield below will eat our input
                            
                            //check for enter key input and run the command entered
                            if (Event.current.type == EventType.KeyDown && (Event.current.keyCode == KeyCode.KeypadEnter || Event.current.keyCode == KeyCode.Return))
                            {                                
                                    if (string.IsNullOrEmpty( InputString )) { return; }
                                    OutputToConsole(InputString, Style_UserPrevious);                                
                                    RunCommand(InputString); //attempt to run the command entered by the user
                                    InputString = ""; //reset the textfield string                                
                            }
                            
                            //check for UP ARROW input and cycle through previous commands
                            if (Event.current.type == EventType.KeyDown && (Event.current.keyCode == KeyCode.UpArrow))
                            {                                        
                                    for (int i = ConsoleLineList.Count - 1; i >= 0; i--)
                                    {                                        
                                            if (ConsoleLineList[i].Style == Style_UserPrevious)
                                            {                                                
                                                    InputString = ConsoleLineList[i].TextString;
                                                    break;
                                            }
                                    }
                            }
                            
                            //check for escape or tilde input and close the console
                            if (Event.current.type == EventType.KeyDown && (Event.current.keyCode == KeyCode.Escape || Event.current.keyCode == KeyCode.BackQuote))
                            {        
                                    ToggleConsole(); //toggle the console
                                    InputString = string.Empty; //reset the textfield string
                            }
                    
                            GUILayout.Label(string.Empty, GUILayout.Height(ConsoleHeight)); //blank space to put new commands at the bottom of the console
                                                    
                            //check if we have previous commands to show
                            if (ConsoleLineList.Count > 0)
                            {
                                    //if so, cycle through those commands                                
                                    for(int i = ConsoleLineList.Count - 1; i >= 0; i--)
                                    {
                                            int count = ConsoleLineList.Count - i; //set our count.                                
                                            
                                            //Show Command Detail Text
                                            if (!mLineOpen)
                                            {
                                                GUILayout.BeginHorizontal();
                                                mLineOpen = true;
                                            }

                                                    if (DetailView)
                                                    {
                                                        GUILayout.TextArea(ConsoleLineList[count - 1].DetailText, Style_Detail, GUILayout.Width(192));
                                                    }

                                                    //create previous commands as buttons, so the user can click on them to reuse previous commands
                                                    if(GUILayout.Button(new GUIContent(ConsoleLineList[count - 1].TextString,
                                                                                       ConsoleLineList[count - 1].Style == Style_UserPrevious ? string.Format("Click to add '{0}' to input field", ConsoleLineList[count - 1].TextString) : ""),
                                                                                       ConsoleLineList[count - 1].Style))
                                                    {
                                                            //only allow the user to reuse their commands (as opposed to reusing Admin or Error prompts)
                                                            if (ConsoleLineList[count -1].Style == Style_UserPrevious){ InputString = ConsoleLineList[count - 1].TextString; }                        
                                                    }

                                            if (i > 0 && ConsoleLineList[i - 1].NewLine)
                                            {
                                                GUILayout.FlexibleSpace();
                                                GUILayout.EndHorizontal();
                                                mLineOpen = false;
                                            }
                                    }

                                    if (mLineOpen)
                                    {
                                        GUILayout.FlexibleSpace();
                                        GUILayout.EndHorizontal();
                                        mLineOpen = false;
                                    }
                             }

                    GUILayout.EndScrollView();

                    GUI.SetNextControlName(kConsoleTitle); //set the name of the console
                    
                    InputString = GUILayout.TextField(InputString,  GrendelCustomStyles.CustomElement(GUI.skin.box, ConsoleColor, Style_UserCurrent.normal.textColor, TextAnchor.MiddleLeft)); //Set up the textfield for accepting user input                
                    
                    DetailView = GUILayout.Toggle(DetailView, "Detailed Console");

                    GUI.FocusControl(kConsoleTitle); //focus on the textfield
                    
                    if (InputString == "`") { InputString = string.Empty;} //clear the tilde if it shows up in the input string (happens sometimes, probably damn onGUI
                    
                    _previousCommandCount = ConsoleLineList.Count;                        
            }
    }//end OnGUI        
            
    void RunCommand(string command)
    {
            string[] splitCommand = command.Split(new char[] {' '}); //split up the user's input. Strings are split whenever a 'space' is encountered

            string newCommand = splitCommand[0];
            
            //assess special cases
            switch(newCommand)
            {
                    case "?":
                            newCommand = "ShowCommands";
                    break;
                    
                    case "Help":
                            newCommand = "ShowCommands";
                    break;

                    case "help":
                            newCommand = "ShowCommands";
                    break;
                    
                    default:
                            //do nothing if no special case
                    break;
            }

            object[] newCommandParameters = new object[splitCommand.Length];
            
            for(int i = 1; i < (splitCommand.Length); i++)
            {                        
                    newCommandParameters[i - 1] = splitCommand[i] as object;
            }
            
            ConsoleCommandRegistry.Instance.HandleCommand(newCommand, new ConsoleCommandParams(newCommandParameters));        
    }

    //OUTPUT TO CONSOLE
    //
    //This method can be called from anywhere to output a line to the console, useful for logging events
    public void OutputToConsole(string text, GUIStyle style, bool showDetail, bool newLine)
    {
            ConsoleLine line = new ConsoleLine();

            if (showDetail)
            {
                    
                    if (style == Style_Admin)
                    {
                            line.User = "Grendel";
                    }
                    else if(style == Style_Error)
                    {
                            line.User = "Grendel";
                    }
                    else
                    {                        
                            line.User = Environment.UserName;                        
                    }                
                    
                    line.CommandDateTime = DateTime.Now.ToString("yy/mm/dd HH:mm:ss");        
            }
            else
            {
                    line.User = "";
                    line.CommandDateTime = "";
            }
            
            line.TextString =  text;
            line.Style = style;
            line.NewLine = newLine;

            ConsoleLineList.Add(line);                
            
            //if the console isn't visible, push a notification to the screen
            if (!_showConsole && GameOptions.Instance.DebugMode)
            {
                    new ScreenNotification(text, style.normal.textColor);
            }
    }

    public void OutputToConsole(string text, GUIStyle style, bool showDetail)
    {
        OutputToConsole(text, style, showDetail, true);
    }

    public void OutputToConsole(string text,  GUIStyle style) 
    {
            OutputToConsole(text, style, true, true);
    }
    
    public void OnApplicationQuit()
    {
            OutputToConsole("Quitting", Style_Admin);
            if (OutputLogOnQuit){ ConsoleLog.OutputLog(ConsoleLineList, System.DateTime.Now.ToString("yymmdd HHmmss")); }
                    
    }

    //PUBLIC ACCESSORS
    public bool ShowConsole
    {
            get { return _showConsole; }
            set { _showConsole = value; }
    }
}
