using System;
using System.Collections;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using Random = UnityEngine.Random;

namespace EnhancedHierarchy {
    internal static partial class Preferences {

        private const string DEVELOPER_EMAIL = "samuelschultze@gmail.com";

        public static Action onResetPreferences = new Action(() => { });

        private static readonly Version pluginVersion = new Version(2, 3, 2);
        private static readonly DateTime pluginDate = new DateTime(2018, 03, 15);

        private static readonly GUIContent resetSettingsContent = new GUIContent("Use Defaults", "Reset all settings to default");
        private static readonly GUIContent unlockAllContent = new GUIContent("Unlock All Objects", "Unlock all objects in the current scene, it's highly recommended to do this when disabling or removing the extension to prevent data loss\nThis might take a few seconds on large scenes");
        private static readonly GUIContent mailDeveloperContent = new GUIContent("Support Email", "Request support from the developer\n\n" + DEVELOPER_EMAIL);
        private static readonly GUIContent versionContent = new GUIContent(string.Format("Version: {0} ({1:d})", pluginVersion, pluginDate));

        private static Vector2 scroll;

        private static ReorderableList leftIconsList, rightIconsList, rowColorsList;

        private static GenericMenu LeftIconsMenu { get { return GetGenericMenuForIcons(LeftIcons, LeftSideIcon.AllIcons); } }
        private static GenericMenu RightIconsMenu { get { return GetGenericMenuForIcons(RightIcons, RightSideIcon.AllIcons); } }

        private static GenericMenu RowColorsMenu {
            get {
                var menu = new GenericMenu();
                var randomColor = Random.ColorHSV(0f, 1f, 0.5f, 1f, 1f, 1f);

                randomColor.a = 0.3019608f;

                for(var i = 0; i < 32; i++) {
                    if(PerLayerRowColors.Value.Contains(new LayerColor(i)))
                        continue;

                    var layerName = LayerMask.LayerToName(i);
                    var layer = new LayerColor(i, randomColor);

                    if(string.IsNullOrEmpty(layerName))
                        layerName = string.Format("Layer: {0}", i);

                    menu.AddItem(new GUIContent(layerName), false, () => {
                        rowColorsList.list.Add(layer);
                        PerLayerRowColors.ForceSave();
                    });
                }

                return menu;
            }
        }

        private static GenericMenu GetGenericMenuForIcons<T>(PrefItem<T> preferenceItem, IconBase[] icons) where T : IList {
            var menu = new GenericMenu();

            foreach(var i in icons) {
                var icon = i;

                if(!preferenceItem.Value.Contains(icon) && icon != IconBase.leftNone && icon != IconBase.rightNone)
                    menu.AddItem(new GUIContent(icon.Name), false, () => {
                        preferenceItem.Value.Add(icon);
                        preferenceItem.ForceSave();
                    });

            }

            return menu;
        }

        private static ReorderableList GenerateReordableList<T>(PrefItem<T> preferenceItem) where T : IList {
            var result = new ReorderableList(preferenceItem.Value, typeof(T), true, true, true, true);

            result.elementHeight = 18f;
            result.drawHeaderCallback = rect => { rect.xMin -= EditorGUI.indentLevel * 16f; EditorGUI.LabelField(rect, preferenceItem, EditorStyles.boldLabel); };
            result.drawElementCallback = (rect, index, focused, active) => EditorGUI.LabelField(rect, result.list[index].ToString());
            result.onChangedCallback += list => preferenceItem.ForceSave();

            onResetPreferences += () => result.list = preferenceItem.Value;

            return result;
        }

        [PreferenceItem("Hierarchy")]
        private static void OnPreferencesGUI() {

            scroll = EditorGUILayout.BeginScrollView(scroll, false, false);

            EditorGUILayout.Separator();
            Enabled.DoGUI();
            EditorGUILayout.Separator();
            EditorGUILayout.HelpBox("Each item has a tooltip explaining what it does, keep the mouse over it to see.", MessageType.Info);
            EditorGUILayout.Separator();

            using(Enabled.GetEnabledScope()) {
                using(new GUIIndent("Misc settings")) {

                    using(new GUIIndent("Margins")) {
                        RightMargin.DoGUISlider(-50, 50);
                        LeftMargin.DoGUISlider(-50, 50);
                        Indent.DoGUISlider(0, 35);
                    }

                    Tree.DoGUI();

                    using(new GUIIndent())
                    using(SelectOnTree.GetFadeScope(Tree))
                        SelectOnTree.DoGUI();

                    Tooltips.DoGUI();

                    using(new GUIIndent())
                    using(RelevantTooltipsOnly.GetFadeScope(Tooltips))
                        RelevantTooltipsOnly.DoGUI();

                    EnhancedSelection.DoGUI();
                    Trailing.DoGUI();
                    ChangeAllSelected.DoGUI();
                    NumericChildExpand.DoGUI();

                    using(HideDefaultIcon.GetFadeScope(IsButtonEnabled(new Icons.GameObjectIcon())))
                        HideDefaultIcon.DoGUI();

                    GUI.changed = false;

                    using(AllowSelectingLocked.GetFadeScope(IsButtonEnabled(new Icons.Lock())))
                        AllowSelectingLocked.DoGUI();

                    using(AllowSelectingLockedSceneView.GetFadeScope(IsButtonEnabled(new Icons.Lock()) && AllowSelectingLocked))
                        AllowSelectingLockedSceneView.DoGUI();

                    if(GUI.changed && EditorUtility.DisplayDialog("Relock all objects",
                        "Would you like to relock all objects?\n" +
                        "This is recommended when changing this setting and might take a few seconds on large scenes" +
                        "\nIt's also recommended to do this on all scenes", "Yes", "No"))
                        Utility.RelockAllObjects();

                    HoverTintColor.DoGUI();
                }

                using(new GUIIndent("Row separators")) {
                    LineSize.DoGUISlider(0, 6);

                    using(LineColor.GetFadeScope(LineSize > 0))
                        LineColor.DoGUI();

                    OddRowColor.DoGUI();
                    EvenRowColor.DoGUI();

                    GUI.changed = false;
                    var rect = EditorGUILayout.GetControlRect(false, rowColorsList.GetHeight());
                    rect.xMin += EditorGUI.indentLevel * 16f;
                    rowColorsList.DoList(rect);
                }

                MiniLabel.DoGUI();
                using(new GUIIndent()) {
                    using(SmallerMiniLabel.GetFadeScope(MiniLabel.Value != MiniLabelType.None))
                        SmallerMiniLabel.DoGUI();
                    using(HideDefaultTag.GetFadeScope(MiniLabelTagEnabled))
                        HideDefaultTag.DoGUI();
                    using(HideDefaultLayer.GetFadeScope(MiniLabelLayerEnabled))
                        HideDefaultLayer.DoGUI();
                    using(CentralizeMiniLabelWhenPossible.GetFadeScope((HideDefaultLayer || HideDefaultTag) && (MiniLabel.Value == MiniLabelType.TagAndLayer || MiniLabel.Value == MiniLabelType.LayerAndTag)))
                        CentralizeMiniLabelWhenPossible.DoGUI();
                }

                LeftSideButtonPref.DoGUI();
                using(new GUIIndent())
                using(LeftmostButton.GetFadeScope(LeftSideButton != IconBase.rightNone))
                    LeftmostButton.DoGUI();

                using(new GUIIndent("Children behaviour on change")) {
                    using(LockAskMode.GetFadeScope(IsButtonEnabled(new Icons.Lock())))
                        LockAskMode.DoGUI();
                    using(LayerAskMode.GetFadeScope(IsButtonEnabled(new Icons.Layer()) || MiniLabelLayerEnabled))
                        LayerAskMode.DoGUI();
                    using(TagAskMode.GetFadeScope(IsButtonEnabled(new Icons.Tag()) || MiniLabelTagEnabled))
                        TagAskMode.DoGUI();
                    using(StaticAskMode.GetFadeScope(IsButtonEnabled(new Icons.Static())))
                        StaticAskMode.DoGUI();
                    using(IconAskMode.GetFadeScope(IsButtonEnabled(new Icons.GameObjectIcon())))
                        IconAskMode.DoGUI();

                    EditorGUILayout.HelpBox(string.Format("Pressing down {0} while clicking on a button will make it temporary have the opposite children change mode", Utility.CtrlKey), MessageType.Info);
                }

                leftIconsList.displayAdd = LeftIconsMenu.GetItemCount() > 0;
                leftIconsList.DoLayoutList();

                rightIconsList.displayAdd = RightIconsMenu.GetItemCount() > 0;
                rightIconsList.DoLayoutList();

                EditorGUILayout.HelpBox("Alt + Click on child expand toggle makes it expand all the grandchildren too", MessageType.Info);

                if(IsButtonEnabled(new Icons.Memory()))
                    EditorGUILayout.HelpBox("\"Memory Used\" may create garbage and consequently framerate stutterings, leave it disabled if maximum performance is important for your project", MessageType.Warning);

                if(IsButtonEnabled(new Icons.Lock()))
                    EditorGUILayout.HelpBox("Remember to always unlock your game objects when removing or disabling this extension, as you won't be able to unlock without it and may lose scene data", MessageType.Warning);

                GUI.enabled = true;
                EditorGUILayout.EndScrollView();

                using(new EditorGUILayout.HorizontalScope()) {
                    GUILayout.FlexibleSpace();
                    EditorGUILayout.LabelField(versionContent, GUILayout.Width(170f));
                }

                using(new EditorGUILayout.HorizontalScope()) {
                    if(GUILayout.Button(resetSettingsContent, GUILayout.Width(120f)))
                        onResetPreferences();

                    if(GUILayout.Button(unlockAllContent, GUILayout.Width(120f)))
                        Utility.UnlockAllObjects();

                    if(GUILayout.Button(mailDeveloperContent, GUILayout.Width(120f)))
                        Application.OpenURL(GetEmailURL());

                }

                EditorGUILayout.Separator();
                Styles.ReloadTooltips();
                EditorApplication.RepaintHierarchyWindow();
            }

        }

        private static string GetEmailURL() {
            var full = new StringBuilder();
            var body = new StringBuilder();

            Func<string, string> EscapeURL = url => { return WWW.EscapeURL(url).Replace("+", "%20"); };

            body.Append(EscapeURL("\r\nDescribe your problem or make your request here\r\n"));
            body.Append(EscapeURL("\r\nAdditional Information:"));
            body.Append(EscapeURL("\r\nVersion: " + pluginVersion.ToString(3)));
            body.Append(EscapeURL("\r\nUnity " + InternalEditorUtility.GetFullUnityVersion()));
            body.Append(EscapeURL("\r\n" + SystemInfo.operatingSystem));

            full.Append("mailto:");
            full.Append(DEVELOPER_EMAIL);
            full.Append("?subject=");
            full.Append(EscapeURL("Enhanced Hierarchy - Support"));
            full.Append("&body=");
            full.Append(body);

            return full.ToString();
        }

        private static LayerColor LayerColorField(Rect rect, LayerColor layerColor) {
            var value = layerColor;
            var rect1 = rect;
            var rect2 = rect;
            var rect3 = rect;

            rect1.xMax = rect1.xMin + 175f;
            rect2.xMin = rect1.xMax;
            rect2.xMax = rect2.xMin + 80f;
            rect3.xMin = rect2.xMax;

            value.layer = EditorGUI.LayerField(rect1, value.layer);
            value.layer = EditorGUI.DelayedIntField(rect2, value.layer);
            value.color = EditorGUI.ColorField(rect3, value.color);

            if(value.layer > 31 || value.layer < 0)
                return layerColor;

            return value;
        }

        private static void DoGUI(this PrefItem<int> prefItem) {
            if(prefItem.Drawing)
                prefItem.Value = EditorGUILayout.IntField(prefItem, prefItem);
        }

        private static void DoGUI(this PrefItem<float> prefItem) {
            if(prefItem.Drawing)
                prefItem.Value = EditorGUILayout.FloatField(prefItem, prefItem);
        }

        private static void DoGUISlider(this PrefItem<int> prefItem, int min, int max) {
            if(prefItem.Drawing)
                prefItem.Value = EditorGUILayout.IntSlider(prefItem, prefItem, min, max);
        }

        private static void DoGUISlider(this PrefItem<float> prefItem, float min, float max) {
            if(prefItem.Drawing)
                prefItem.Value = EditorGUILayout.Slider(prefItem, prefItem, min, max);
        }

        private static void DoGUI(this PrefItem<bool> prefItem) {
            if(prefItem.Drawing)
                prefItem.Value = EditorGUILayout.Toggle(prefItem, prefItem);
        }

        private static void DoGUI(this PrefItem<string> prefItem) {
            if(prefItem.Drawing)
                prefItem.Value = EditorGUILayout.TextField(prefItem.Label, prefItem);
        }

        private static void DoGUI(this PrefItem<Color> prefItem) {
            if(prefItem.Drawing)
                prefItem.Value = EditorGUILayout.ColorField(prefItem, prefItem);
        }

        private static void DoGUI<T>(this PrefItem<T> prefItem) where T : struct, IConvertible {
            if(prefItem.Drawing)
                prefItem.Value = (T)(object)EditorGUILayout.EnumPopup(prefItem, (Enum)(object)prefItem.Value);
        }

        private static void DoGUI(this PrefItem<IconData> prefItem) {
            if(!prefItem.Drawing)
                return;

            var icons = RightSideIcon.AllIcons;
            var index = Array.IndexOf(icons, prefItem.Value.Icon);
            var labels = (from icon in icons
                          select new GUIContent(icon)).ToArray();

            index = EditorGUILayout.Popup(prefItem, index, labels);

            if(index < 0 || index >= icons.Length)
                return;

            if(prefItem.Value.Icon.Name == icons[index].Name)
                return;

            prefItem.Value.Icon = icons[index];
            prefItem.ForceSave();
        }
    }
}