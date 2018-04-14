using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace EnhancedHierarchy.Icons {
    internal sealed class Warnings : LeftSideIcon {

        private const int MAX_STRING_LEN = 750;
        private const float ICONS_WIDTH = 16f;

        public static StringBuilder goLogs = new StringBuilder(MAX_STRING_LEN);
        public static StringBuilder goWarnings = new StringBuilder(MAX_STRING_LEN);
        public static StringBuilder goErrors = new StringBuilder(MAX_STRING_LEN);
        private static readonly GUIContent tempTooltipContent = new GUIContent();

        public override string Name { get { return "Logs, Warnings and Errors"; } }
        public override float Width {
            get {
                var result = 0f;

                if(goLogs.Length > 0)
                    result += ICONS_WIDTH;
                if(goWarnings.Length > 0)
                    result += ICONS_WIDTH;
                if(goErrors.Length > 0)
                    result += ICONS_WIDTH;

                return result;
            }
        }

        public override void Init() {
            if(!EnhancedHierarchy.IsRepaintEvent || !EnhancedHierarchy.IsGameObject)
                return;

            goLogs.Length = 0;
            goWarnings.Length = 0;
            goErrors.Length = 0;

            var contextEntries = (List<LogEntry>)null;
            var components = EnhancedHierarchy.Components;

            for(var i = 0; i < components.Count; i++)
                if(!components[i])
                    goWarnings.AppendLine("Missing MonoBehaviour\n");

            if(LogEntry.referencedObjects.TryGetValue(EnhancedHierarchy.CurrentGameObject, out contextEntries))
                for(var i = 0; i < contextEntries.Count; i++)
                    if(goLogs.Length < MAX_STRING_LEN && contextEntries[i].HasMode(EntryMode.ScriptingLog))
                        goLogs.AppendLine(contextEntries[i].ToString());

                    else if(goWarnings.Length < MAX_STRING_LEN && contextEntries[i].HasMode(EntryMode.ScriptingWarning))
                        goWarnings.AppendLine(contextEntries[i].ToString());

                    else if(goErrors.Length < MAX_STRING_LEN && contextEntries[i].HasMode(EntryMode.ScriptingError))
                        goErrors.AppendLine(contextEntries[i].ToString());
        }

        public override void DoGUI(Rect rect) {
            if(!EnhancedHierarchy.IsRepaintEvent || !EnhancedHierarchy.IsGameObject)
                return;

            rect.xMax = rect.xMin + 17f;
            rect.yMax += 1f;

            if(goLogs.Length > 0) {
                if(Utility.ShouldCalculateTooltipAt(rect))
                    tempTooltipContent.tooltip = Preferences.Tooltips ? goLogs.ToString().TrimEnd('\n', '\r') : string.Empty;

                GUI.DrawTexture(rect, Styles.infoIcon, ScaleMode.ScaleToFit);
                EditorGUI.LabelField(rect, tempTooltipContent);
                rect.x += ICONS_WIDTH;
            }

            if(goWarnings.Length > 0) {
                if(Utility.ShouldCalculateTooltipAt(rect))
                    tempTooltipContent.tooltip = Preferences.Tooltips ? goWarnings.ToString().TrimEnd('\n', '\r') : string.Empty;

                GUI.DrawTexture(rect, Styles.warningIcon, ScaleMode.ScaleToFit);
                EditorGUI.LabelField(rect, tempTooltipContent);
                rect.x += ICONS_WIDTH;
            }

            if(goErrors.Length > 0) {
                if(Utility.ShouldCalculateTooltipAt(rect))
                    tempTooltipContent.tooltip = Preferences.Tooltips ? goErrors.ToString().TrimEnd('\n', '\r') : string.Empty;

                GUI.DrawTexture(rect, Styles.errorIcon, ScaleMode.ScaleToFit);
                EditorGUI.LabelField(rect, tempTooltipContent);
                rect.x += ICONS_WIDTH;
            }
        }
    }
}