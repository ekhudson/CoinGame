#if(UNITY_EDITOR || DEVELOPMENT_BUILD || REFLEXCLI_RELEASE)
#define REFLEXCLI_ENABLED
#endif

#if REFLEXCLI_ENABLED
using System;
using System.Collections.Generic;
using System.Reflection;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Assertions;

#else // !REFLEXCLI_ENABLED
using UnityEngine;
using UnityEngine.UI;
#endif // REFLEXCLI_ENABLED

namespace ReflexCLI.UI
{
	using Parameters;
	public class ReflexTextInputHandler : MonoBehaviour, IUIStyleUpdater
	{
#pragma warning disable 414
		[SerializeField]
		private InputField InputText = null;

		[SerializeField]
		private Text SuggestionText = null;
		
		[SerializeField]
		private ReflexHistoryUIContainer HistoryContainer = null;
#pragma warning restore

#if REFLEXCLI_ENABLED

		private enum EEditMode
		{
			User,
			Auto,
			Clear,
		}

		private enum EBrowseMode
		{
			None,
			History,
			Suggestions,
		}

		private EEditMode EditMode;
		private string UserInput = "";
		private string[] UserInputTerms = new string[0];
		
		//private List<string> CommandHistory = new List<string>();
		private List<Suggestion> Suggestions = new List<Suggestion>();

		private EBrowseMode BrowseMode = EBrowseMode.None;
		private int BrowseIdx = -1;

		public void Init()
		{
			ClearHistory();
			ClearInputText();
			ClearSuggestions();

			if(HistoryContainer)
				HistoryContainer.Init();
		}

		private void Start()
		{
			if(InputText == null)
				InputText = GetComponent<InputField>();
			Assert.IsNotNull(InputText);

			InputText.onValueChanged.AddListener(OnValueChanged);
			InputText.onEndEdit.AddListener(OnEndEdit);
		}

		private void Update()
		{
			Assert.IsTrue(EditMode == EEditMode.User);

			if(!InputText.isFocused)
				SetFocus();

			UpdateBrowseInput();
		}

		private void OnEnable()
		{
			ClearInputText();
			ClearSuggestions();
			SetFocus();
			ResetBrowse();

			EditMode = EEditMode.User;
		}

		public void OnValueChanged(string value)
		{
			if(EditMode == EEditMode.User || EditMode == EEditMode.Clear)
			{
				UserInput = value;
				UserInputTerms = Utils.GetCommandTerms(value);
				ResetBrowse();
			}

			UpdateSuggestions();
			EditMode = EEditMode.User;
		}

		public void OnEndEdit(string value)
		{
			bool isSubmit = Input.GetKeyDown(KeyCode.KeypadEnter) ||
							Input.GetKeyDown(KeyCode.Return);
							
			if(isSubmit && InputText.text.Length > 0)
				DoSubmit();
		}

		private void DoSubmit()
		{
			string commandStr = InputText.text;

			object response = null;
			try
			{
				response = Utils.ExecuteCommand(ref commandStr);			// need to execute before outputing as the command needs sanitizing in Execute command
			}
			catch(CommandException e)
			{
				response = e.Message;
			}
			catch(TargetInvocationException e)
			{
				response = "Command generated internal exception: " + e.InnerException.Message;
			}
			catch(Exception e)
			{
				response = e.Message;
			}

			Output(commandStr.ToString(), response);

			ClearInputText();
			ResetBrowse();
		}

		private void SetFocus()
		{
			if(EventSystem.current)
			{
				EventSystem.current.SetSelectedGameObject(gameObject, null);
				InputText.OnPointerClick(new PointerEventData(EventSystem.current));
			}
		}

		private void UpdateSuggestions()
		{
			ClearSuggestions();
			//if(BrowseMode == EBrowseMode.History)
			//	return;
			
			int termIdx = UserInputTerms.Length - 1;
			if(termIdx == 0)
				DoCommandSuggestions();
			else if(termIdx > 0)
				DoParameterSuggestions(termIdx);
		}

		private void DoCommandSuggestions()
		{
			// TODO - null ptr checks for suggestion text box

			var suggestions = ParameterProcessorRegistry.FindSuggestionsFor(typeof(CommandDef), null, UserInputTerms[0], 20);
			for(int i=0, num=suggestions.Count; i<num; i++)
			{
				var suggestion = suggestions[i];
				var commandDef = (CommandDef)ParameterProcessorRegistry.ConvertString(typeof(CommandDef), suggestion.Value);
				if(commandDef.IsValid())
				{
					Suggestions.Add(commandDef.Name);
					SuggestionText.text += commandDef.GetCommandFormat() + "\n";
				}
				else
				{
					Suggestions.Add(suggestion.Value);
					SuggestionText.text += suggestion.Display + "\n";
				}
			}
		}

		private void DoParameterSuggestions(int termIdx)
		{
			// TODO - null ptr checks for suggestion text box

			CommandDef commandDef = CommandRegistry.GetCommand(UserInputTerms[0]);
			if(!commandDef.IsValid())
			{
				SuggestionText.text = "No Matching Commands!";
				return;
			}
			Assert.IsTrue(commandDef.IsValid());

			if(termIdx < 0 || termIdx >= commandDef.Command.GetNumExpectedTerms())
				return;
				
			string[] terms  = commandDef.GetCommandFormatAsTerms();
			if(termIdx < terms.Length)
				terms[termIdx] = "<b><color=#ffffffff>" + terms[termIdx] + "</color></b>";

			SuggestionText.text += Utils.GetCommand(terms) + "\n";

			string searchTerm = termIdx < UserInputTerms.Length ? UserInputTerms[termIdx] : "";
			int paramIdx = termIdx-1;
			var cmd = commandDef.Command;
			var paramSuggestions = ParameterProcessorRegistry.FindSuggestionsFor(cmd.GetParamType(paramIdx), cmd.GetParameterAttributes(paramIdx), searchTerm, 20);

			if(paramSuggestions != null )
			{
				string suggestionDisplayPrefix = "";
				for(int i=0; i < Mathf.Min(terms.Length, termIdx); i++)
					suggestionDisplayPrefix += terms[i] + " ";

				suggestionDisplayPrefix = "<color=#ffffff00>" + suggestionDisplayPrefix + "</color>";

				for(int i=0, num=paramSuggestions.Count; i<num; i++)
				{
					var paramSuggestion = paramSuggestions[i];
					Suggestions.Add(paramSuggestion);
					SuggestionText.text += suggestionDisplayPrefix + paramSuggestion.Display + "\n";
				}
			}
		}

		private void UpdateBrowseInput()
		{
			if(Input.GetKeyDown(KeyCode.UpArrow))
			{
				Browse(1);
			}
			else if(Input.GetKeyDown(KeyCode.DownArrow))
			{
				Browse(-1);
			}
			else if(Input.GetKeyDown(KeyCode.Tab))
			{
				if(Input.GetKey(KeyCode.LeftShift))
				{
					if(BrowseMode == EBrowseMode.Suggestions)
						BrowseSuggestions(-1);
				}
				else
				{
					if(BrowseMode == EBrowseMode.None || BrowseMode == EBrowseMode.Suggestions)
						BrowseSuggestions(1);
				}
			}
		}

		private void Browse(int move)
		{	
			if(BrowseMode == EBrowseMode.History || BrowseMode == EBrowseMode.None && move > 0)
				BrowseHistory(move);
			else if(BrowseMode == EBrowseMode.Suggestions || BrowseMode == EBrowseMode.None && move < 0)
				BrowseSuggestions(-move);
		}

		private void ResetBrowse()
		{
			BrowseMode = EBrowseMode.None;
			BrowseIdx = -1;
			UpdateSuggestions();
		}

		private void BrowseHistory(int move)
		{
			int oldBrowseIdx = BrowseIdx;
			BrowseIdx = Mathf.Clamp(BrowseIdx + move, -1, HistoryContainer.GetNumEntries() - 1);
			if(BrowseIdx != oldBrowseIdx)
			{
				if(BrowseIdx < 0)
				{
					SetInputText("", EEditMode.Auto);
					ResetBrowse();
				}
				else
				{
					BrowseMode = EBrowseMode.History;
					var entry = HistoryContainer.GetEntry(BrowseIdx);
					SetInputText(HistoryContainer.GetEntry(BrowseIdx), EEditMode.Auto);
					UserInputTerms = Utils.GetCommandTerms(entry);
					UpdateSuggestions();
				}
			}
		}

		private void BrowseSuggestions(int move)
		{
			int oldBrowseIdx = BrowseIdx;
			BrowseIdx = Mathf.Clamp(BrowseIdx + move, -1, Suggestions.Count - 1);
			if(BrowseIdx != oldBrowseIdx)
			{
				if(BrowseIdx < 0)
				{
					SetInputText(UserInput, EEditMode.Auto);
					ResetBrowse();
				}
				else
				{
					BrowseMode = EBrowseMode.Suggestions;
					SetInputText(ExpandSuggestion(BrowseIdx), EEditMode.Auto);
				}
			}
		}

		private string ExpandSuggestion(int suggestionIdx)
		{
			Assert.IsTrue(suggestionIdx >= 0 && suggestionIdx < Suggestions.Count);
			string output = "";
			for(int i=0; i<UserInputTerms.Length-1; i++)
				output+= UserInputTerms[i] + " ";

			return output + Suggestions[suggestionIdx].Value;
		}

		//////////////////////////////////////////////////////////////////////
		// Update text box helper functions

		private void SetInputText(string str, EEditMode editMode)
		{
			if(InputText && str != InputText.text)
			{
				EditMode = editMode;
				InputText.text = str;
				InputText.caretPosition = int.MaxValue;
			}
		}

		private void ClearInputText()
		{
			SetInputText("", EEditMode.Clear);
		}

		private void ClearSuggestions()
		{
			if(SuggestionText)
				SuggestionText.text = "";

			Suggestions = new List<Suggestion>();
		}

		public void ClearHistory()
		{
			if(HistoryContainer)
				HistoryContainer.Clear();
		}

		private void Output(string command, object response = null)
		{
			if(HistoryContainer != null)
				HistoryContainer.AddItem(command, response != null ? response.ToString() : "");
		}

		void IUIStyleUpdater.UpdateStyles()
		{
			var font = Settings.ConsoleFont;
			if(font)
			{
				SuggestionText.font = font;
				InputText.textComponent.font = font;
			}
		}

#else // !REFLEXCLI_ENABLED
		void Awake()
		{
			GameObject.Destroy(gameObject);
		}
		void IUIStyleUpdater.UpdateStyles(){}
#endif // REFLEXCLI_ENABLED

	}
}