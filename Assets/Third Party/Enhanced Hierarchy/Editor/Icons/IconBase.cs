using System;
using System.Collections.Generic;
using System.Linq;
using EnhancedHierarchy.Icons;
using UnityEditor;
using UnityEngine;

namespace EnhancedHierarchy {

    internal abstract class LeftSideIcon : IconBase {

        static LeftSideIcon() {
            var baseType = typeof(LeftSideIcon);
            var assembly = baseType.Assembly;
            var iconsTypes = (from type in assembly.GetTypes()
                              where type != baseType && baseType.IsAssignableFrom(type)
                              select type).ToArray();

            foreach(var instance in iconsTypes.Select(type => (LeftSideIcon)Activator.CreateInstance(type)))
                icons.Add(instance, instance);
        }

        private static readonly Dictionary<string, LeftSideIcon> icons = new Dictionary<string, LeftSideIcon>();

        public static LeftSideIcon[] AllIcons { get { return icons.Values.ToArray(); } }

        public static implicit operator LeftSideIcon(string name) {
            try { return icons[name]; }
            catch { return IconBase.leftNone; }
        }

        public static implicit operator string(LeftSideIcon icon) {
            return icon.ToString();
        }

    }

    internal abstract class RightSideIcon : IconBase {

        static RightSideIcon() {
            var baseType = typeof(RightSideIcon);
            var assembly = baseType.Assembly;
            var iconsTypes = (from type in assembly.GetTypes()
                              where type != baseType && baseType.IsAssignableFrom(type)
                              select type).ToArray();

            foreach(var instance in iconsTypes.Select(type => (RightSideIcon)Activator.CreateInstance(type)))
                icons.Add(instance, instance);
        }

        private static readonly Dictionary<string, RightSideIcon> icons = new Dictionary<string, RightSideIcon>();

        public static RightSideIcon[] AllIcons { get { return icons.Values.ToArray(); } }

        public static implicit operator RightSideIcon(string name) {
            try { return icons[name]; }
            catch { return IconBase.rightNone; }
        }

        public static implicit operator string(RightSideIcon icon) {
            return icon.ToString();
        }

    }

    internal abstract class IconBase {

        private const float DEFAULT_WIDTH = 16f;

        public static LeftNone leftNone = new LeftNone();
        public static RightNone rightNone = new RightNone();

        public virtual string Name { get { return GetType().Name; } }
        public virtual float Width { get { return DEFAULT_WIDTH; } }

        public virtual void Init() { }
        public abstract void DoGUI(Rect rect);

        protected static ChildrenChangeMode AskChangeModeIfNecessary(List<GameObject> objs, ChildrenChangeMode reference, string title, string message) {
            var isControl = Event.current.control || Event.current.command;

            switch(reference) {
                case ChildrenChangeMode.ObjectOnly:
                    if(!isControl)
                        return ChildrenChangeMode.ObjectOnly;
                    else
                        return ChildrenChangeMode.ObjectAndChildren;

                case ChildrenChangeMode.ObjectAndChildren:
                    if(!isControl)
                        return ChildrenChangeMode.ObjectAndChildren;
                    else
                        return ChildrenChangeMode.ObjectOnly;

                default:
                    foreach(var obj in objs)
                        if(obj && obj.transform.childCount > 0)
                            try {
                                return (ChildrenChangeMode)EditorUtility.DisplayDialogComplex(title, message, "Yes, change children", "No, this object only", "Cancel");
                            }
                            finally {
                                //Unity bug, DisplayDialogComplex makes the unity partially lose focus
                                if(EditorWindow.focusedWindow)
                                    EditorWindow.focusedWindow.Focus();
                            }

                    return ChildrenChangeMode.ObjectOnly;
            }
        }

        protected static List<GameObject> GetSelectedObjectsAndCurrent() {
            if(!Preferences.ChangeAllSelected || Selection.gameObjects.Length <= 1)
                return new List<GameObject> { EnhancedHierarchy.CurrentGameObject };

            var selection = new List<GameObject>(Selection.gameObjects);

            for(var i = 0; i < selection.Count; i++)
                if(EditorUtility.IsPersistent(selection[i]))
                    selection.RemoveAt(i);

            if(!selection.Contains(EnhancedHierarchy.CurrentGameObject))
                selection.Add(EnhancedHierarchy.CurrentGameObject);

            selection.Remove(null);
            return selection;
        }

        public static bool operator ==(IconBase left, IconBase right) {
            if(ReferenceEquals(left, right))
                return true;

            if(ReferenceEquals(left, null))
                return false;

            if(ReferenceEquals(right, null))
                return false;

            return left.Name == right.Name;
        }

        public static bool operator !=(IconBase left, IconBase right) {
            return !(left == right);
        }

        public override string ToString() {
            return Name;
        }

        public override int GetHashCode() {
            return Name.GetHashCode();
        }

        public override bool Equals(object obj) {
            return obj as IconBase == this;
        }
    }

}