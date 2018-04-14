using UnityEditor;
using UnityEngine;

namespace EnhancedHierarchy.Icons {
    internal sealed class GameObjectIcon : RightSideIcon {

        private GUIContent lastContent;

        public override string Name { get { return "Icon"; } }

        public override float Width { get { return lastContent.image ? base.Width : 0f; } }

        public override void Init() {
            if(lastContent == null)
                lastContent = new GUIContent();

            lastContent.text = string.Empty;
            lastContent.image = AssetPreview.GetMiniThumbnail(EnhancedHierarchy.CurrentGameObject);
            lastContent.tooltip = Preferences.Tooltips && !Preferences.RelevantTooltipsOnly ? "Change Icon" : string.Empty;

            if(Preferences.HideDefaultIcon && lastContent.image) {
                var imageName = lastContent.image.name;

                if(imageName == "GameObject Icon" || imageName == "PrefabNormal Icon" || imageName == "PrefabModel Icon")
                    lastContent.image = null;
            }
        }

        public override void DoGUI(Rect rect) {
            using(ProfilerSample.Get()) {
                rect.yMin++;
                rect.xMin++;

                GUI.changed = false;
                GUI.Button(rect, lastContent, EditorStyles.label);

                if(!GUI.changed)
                    return;

                var selectedObjects = GetSelectedObjectsAndCurrent();
                var changeMode = AskChangeModeIfNecessary(selectedObjects, Preferences.IconAskMode.Value, "Change Icons", "Do you want to change children icons as well?");

                switch(changeMode) {
                    case ChildrenChangeMode.ObjectOnly:
                        foreach(var obj in selectedObjects)
                            Undo.RegisterCompleteObjectUndo(obj, "Icon Changed");
                        break;

                    case ChildrenChangeMode.ObjectAndChildren:
                        foreach(var obj in selectedObjects)
                            Undo.RegisterFullObjectHierarchyUndo(obj, "Icon Changed");
                        break;
                }

                ReflectionHelper.ShowIconSelector(EnhancedHierarchy.CurrentGameObject, rect, true, icon => {
                    foreach(var obj in selectedObjects)
                        switch(changeMode) {
                            case ChildrenChangeMode.ObjectOnly:
                                ReflectionHelper.SetObjectIcon(obj, icon);
                                break;

                            case ChildrenChangeMode.ObjectAndChildren:
                                var transforms = obj.GetComponentsInChildren<Transform>(true);

                                ReflectionHelper.SetObjectIcon(obj, icon);

                                foreach(var transform in transforms)
                                    ReflectionHelper.SetObjectIcon(transform.gameObject, icon);
                                break;
                        }
                });
            }
        }

    }
}