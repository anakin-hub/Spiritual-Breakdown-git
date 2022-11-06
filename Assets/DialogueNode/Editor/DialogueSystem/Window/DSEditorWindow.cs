using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System;

namespace DS.Window
{
    using Utilities;
    public class DSEditorWindow : EditorWindow
    {
        [MenuItem("Window/Dialogue System/DSEditorWindow")]
        public static void Open()
        {
            GetWindow<DSEditorWindow>("Dialogue Graph");
        }
        private void OnEnable()
        {
            AddGraphView();

            AddStyles();
        }

        private void AddStyles()
        {
            rootVisualElement.AddStyleSheets("DialogueSystem/DSVariables.uss");
        }

        private void AddGraphView()
        {
            DSGraphView graphView = new DSGraphView();

            graphView.StretchToParentSize();

            rootVisualElement.Add(graphView);
        }
    }
}
