using System;
using UnityEngine;

namespace EnhancedHierarchy {
    [Serializable]
    internal class IconData : ISerializationCallbackReceiver {

        [SerializeField]
        public string name;
        [SerializeField]
        public bool isLeftSide;
        [SerializeField]
        public bool isRightSide;

        private IconBase icon;

        public IconBase Icon {
            get {
                return icon;
            }
            set {
                icon = value;
            }
        }

        public void OnAfterDeserialize() {
            if(isLeftSide)
                icon = (LeftSideIcon)name;
            else if(isRightSide)
                icon = (RightSideIcon)name;
            else
                icon = null;
            //    throw new ArgumentException("Data isn't a right side nor left side icon");
        }

        public void OnBeforeSerialize() {
            if(icon == null)
                return;

            name = icon.Name;
            isLeftSide = icon is LeftSideIcon;
            isRightSide = icon is RightSideIcon;
        }

    }
}