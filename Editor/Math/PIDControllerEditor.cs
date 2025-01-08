using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Kutie.Editor.Math {
	[CustomEditor(typeof(PIDControllerValue))]
	public class PIDControllerValueEditor : UnityEditor.Editor {
        public override VisualElement CreateInspectorGUI()
        {
			PIDControllerValue controller = (PIDControllerValue)target;

            var root = new VisualElement();

			var targetProp = serializedObject.FindProperty("target");
			var controllerProp = serializedObject.FindProperty("Controller");
			var forceScaleProp = serializedObject.FindProperty("forceScale");
			var rbProp = serializedObject.FindProperty("rb");

			var targetField = new PropertyField(targetProp);
			var controllerField = new PropertyField(controllerProp);
			var forceScaleField = new PropertyField(forceScaleProp);
			var rbField = new PropertyField(rbProp);

			root.Add(targetField);
			root.Add(controllerField);
			root.Add(forceScaleField);
			root.Add(rbField);

			controllerField.RegisterCallback<SerializedPropertyChangeEvent>((evt) => {
				controller.Controller.Parameters = controller.Controller.Parameters;
			});

			return root;
        }
    }
}