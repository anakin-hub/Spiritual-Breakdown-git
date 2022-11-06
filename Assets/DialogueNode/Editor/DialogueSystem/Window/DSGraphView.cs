using System.Collections;
using System.Collections.Generic;
using System;
using UnityEditor;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;


namespace DS.Window
{
    using Elements;
    using Enumarations;
    using Utilities;
    public class DSGraphView : GraphView
    {
        public DSGraphView()
        {
            AddManipulators();

            AddGridBackground();

            AddStyles();
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            List<Port> compatiblePorts = new List<Port>();

            ports.ForEach(port =>
            {
                if (startPort == port)
                {
                    return;
                }

                if (startPort.node == port.node)
                {
                    return;
                }

                if (startPort.direction == port.direction)
                {
                    return;
                }

                compatiblePorts.Add(port);
            });

            return compatiblePorts;
        }

        private DSNode CreateNode(DSDialogueType dialogueType,Vector2 position)
        {
            System.Type nodeType = System.Type.GetType($"DS.Elements.DS{dialogueType}Node");

            DSNode node = (DSNode) Activator.CreateInstance(nodeType);

            node.Initialize(position);
            node.Draw();

            return node;
        }

        private void AddManipulators()
        {
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            this.AddManipulator(CreateNodeContextualMenu("Add Node (Single Choice)", DSDialogueType.SingleChoice));
            this.AddManipulator(CreateNodeContextualMenu("Add Node (Multiple Choice)", DSDialogueType.MultipleChoice));
            this.AddManipulator(CreateGroupContextualMenu());
        }

        private IManipulator CreateGroupContextualMenu()
        {
            ContextualMenuManipulator contextualMenuManipulator = new ContextualMenuManipulator(
                menuEvent => menuEvent.menu.AppendAction("Add Group", actionEvent => AddElement(CreateGroup("DialogueGroup", actionEvent.eventInfo.localMousePosition)))
            );

            return contextualMenuManipulator;
        }

        private IManipulator CreateNodeContextualMenu(string actionTitle, DSDialogueType dialogueType)
        {
            ContextualMenuManipulator contextualMenuManipulator = new ContextualMenuManipulator(
                menuEvent => menuEvent.menu.AppendAction(actionTitle, actionEvent => AddElement(CreateNode(dialogueType,actionEvent.eventInfo.localMousePosition)))
            );

            return contextualMenuManipulator;
        }

        public Group CreateGroup(string title, Vector2 LocalMousePosition)
        {
            Group group = new Group();
            {
                group.title = title;
            }

            group.SetPosition(new Rect(LocalMousePosition, Vector2.zero));

            return group;
        }

        private void AddGridBackground()
        {
            GridBackground gridBackground = new GridBackground();

            gridBackground.StretchToParentSize();

            Insert(0, gridBackground);
        }

        private void AddStyles()
        {
            this.AddStyleSheets("DialogueSystem/DSGraphViewStyles.uss", "DialogueSystem/DSNodeStyles.uss");
        }
    }
}
