using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace EnhancedHierarchy {
    /// <summary>
    /// Log Entries from the console, to check if a game object has any errors or warnings.
    /// </summary>
    internal sealed class LogEntry {

        private readonly object referenceEntry;

        private static readonly Type logEntriesType;
        private static readonly Type logEntryType;

        public string Condition { get { return referenceEntry.GetFieldValue<string>("condition"); } }
        public int ErrorNum { get { return referenceEntry.GetFieldValue<int>("errorNum"); } }
        public string File { get { return referenceEntry.GetFieldValue<string>("file"); } }
        public int Line { get { return referenceEntry.GetFieldValue<int>("line"); } }
        public EntryMode Mode { get { return referenceEntry.GetFieldValue<EntryMode>("mode"); } }
        public int InstanceID { get { return referenceEntry.GetFieldValue<int>("instanceID"); } }
        public int Identifier { get { return referenceEntry.GetFieldValue<int>("identifier"); } }
        public int IsWorldPlaying { get { return referenceEntry.GetFieldValue<int>("isWorldPlaying"); } }
        public Object Obj { get { return InstanceID == 0 ? null : EditorUtility.InstanceIDToObject(InstanceID); } }

        public static Dictionary<GameObject, List<LogEntry>> referencedObjects = new Dictionary<GameObject, List<LogEntry>>(100);

        private static bool needLogReload;

        static LogEntry() {
            try {
                logEntriesType = ReflectionHelper.FindClass("UnityEditorInternal.LogEntries");
                logEntryType = ReflectionHelper.FindClass("UnityEditorInternal.LogEntry");

                if(logEntriesType == null)
                    logEntriesType = ReflectionHelper.FindClass("UnityEditor.LogEntries");
                if(logEntryType == null)
                    logEntryType = ReflectionHelper.FindClass("UnityEditor.LogEntry");

                ReloadReferences();
            }
            catch(Exception e) {
                Debug.LogException(e);
                Preferences.ForceDisableButton(new Icons.Warnings());
            }

            Application.logMessageReceivedThreaded += (logString, stackTrace, type) => needLogReload = true;

            EditorApplication.update += () => {
                if(needLogReload && Preferences.IsButtonEnabled(new Icons.Warnings()) && Preferences.Enabled) {
                    ReloadReferences();
                    needLogReload = false;
                }
            };
        }

        private LogEntry(object referenceEntry) {
            this.referenceEntry = referenceEntry;
        }

        private static void ReloadReferences() {
            referencedObjects.Clear();

            try {
                var count = logEntriesType.InvokeMethod<int>("StartGettingEntries");

                for(var i = 0; i < count; i++) {
                    var logEntry = Activator.CreateInstance(logEntryType);
                    var entry = new LogEntry(logEntry);
                    var go = (GameObject)null;

                    logEntriesType.InvokeMethod("GetEntryInternal", i, logEntry);

                    if(entry.Obj) {
                        go = entry.Obj as GameObject;

                        if(!go && entry.Obj is Component)
                            go = ((Component)entry.Obj).gameObject;
                    }

                    if(!go) continue;

                    if(referencedObjects.ContainsKey(go))
                        referencedObjects[go].Add(entry);
                    else
                        referencedObjects.Add(go, new List<LogEntry>() { entry });
                }

                EditorApplication.RepaintHierarchyWindow();
            }
            catch(Exception e) {
                Debug.LogException(e);
                Preferences.ForceDisableButton(new Icons.Warnings());
            }
            finally {
                logEntriesType.InvokeMethod("EndGettingEntries");
            }
        }

        public bool HasMode(EntryMode mode) {
            return (Mode & mode) != 0;
        }

        public override string ToString() {
            return Condition;
        }

    }
}