using UnityEditor;
using UnityEngine.UIElements;

namespace MaulGrab.Editor.BehaviorTree
{
    public class BehaviorTree : EditorWindow
    {
        private BehaviorTreeView _treeView;

        [MenuItem( "Custom/Behavior Tree" )]
		public static void Open()
		{
            CreateWindow<BehaviorTree>( "Behavior Tree" );
		}

		private void OnEnable()
		{
			_treeView = new BehaviorTreeView()
			{
				name = "Behavior Tree"
			};

			_treeView.StretchToParentSize();
			rootVisualElement.Add( _treeView );
		}

		private void OnDisable()
		{
			rootVisualElement.Remove( _treeView );
		}
	}
}
