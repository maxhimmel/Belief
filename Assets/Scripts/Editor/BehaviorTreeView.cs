using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace MaulGrab.Editor.BehaviorTree
{
    public class BehaviorTreeView : GraphView
    {
        public readonly Vector2 DefaultNodeSize = new Vector2( 100, 150 );

        public BehaviorTreeView()
		{
            SetupGrid();
            SetupZoom( ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale );

            this.AddManipulator( new ContentDragger() );
            this.AddManipulator( new SelectionDragger() );
            this.AddManipulator( new RectangleSelector() );

            AddElement( CreateRootNode() );
		}

        private void SetupGrid()
        {
            StyleSheet gridStyle = Resources.Load<StyleSheet>( "BehaviorTreeGraph" );
            styleSheets.Add( gridStyle );

            GridBackground gridBg = new GridBackground();
            gridBg.StretchToParentSize();
            Insert( 0, gridBg );
        }

        private TreeNode CreateRootNode()
        {
            TreeNode root = new TreeNode()
            {
                title = "Root",
                Guid = Guid.NewGuid().ToString(),
            };

            Port outputPort = CreatePort( root, Direction.Output );
            outputPort.portName = "";

            root.RefreshExpandedState();
            root.RefreshPorts();

            root.SetPosition( new Rect( contentRect.center, DefaultNodeSize ) );

            return root;
        }

        private Port CreatePort( TreeNode node, Direction direction, Port.Capacity capacity = Port.Capacity.Single )
		{
            Port result = node.InstantiatePort( Orientation.Vertical, direction, capacity, typeof( bool ) );

            switch ( direction )
            {
                case Direction.Input:
                    node.inputContainer.Add( result );
                    break;

                case Direction.Output:
                    node.outputContainer.Add( result );
                    break;
            }

            return result;
		}
    }
}
