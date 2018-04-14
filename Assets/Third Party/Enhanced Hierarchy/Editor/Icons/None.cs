using UnityEngine;

namespace EnhancedHierarchy.Icons {
    internal sealed class LeftNone : LeftSideIcon {
        public override float Width { get { return 0f; } }
        public override string Name { get { return "None"; } }
        public override void DoGUI(Rect rect) { }
    }

    internal sealed class RightNone : RightSideIcon {
        public override float Width { get { return 0f; } }
        public override string Name { get { return "None"; } }
        public override void DoGUI(Rect rect) { }
    }
}