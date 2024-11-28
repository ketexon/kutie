using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Kutie.Editor.Math {
	[CustomPropertyDrawer(typeof(PIDVector3Controller))]
	public class PIDVector3ControllerPropertyDrawer : PropertyDrawer {
		void SetProperty(SerializedProperty property, SerializedProperty value){
			switch(value.propertyType){
				case SerializedPropertyType.Float:
					property.floatValue = value.floatValue;
					break;
				case SerializedPropertyType.Enum:
					property.enumValueIndex = value.enumValueIndex;
					break;
				case SerializedPropertyType.Boolean:
					property.boolValue = value.boolValue;
					break;
			}
		}

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var root = new VisualElement();

			var parametersProp = property.FindPropertyRelative("_parameters");
			var xControllerProp = property.FindPropertyRelative("xController");
			var yControllerProp = property.FindPropertyRelative("yController");
			var zControllerProp = property.FindPropertyRelative("zController");

			var xParametersProp = xControllerProp.FindPropertyRelative("_parameters");
			var yParametersProp = yControllerProp.FindPropertyRelative("_parameters");
			var zParametersProp = zControllerProp.FindPropertyRelative("_parameters");

			var parametersField = new PropertyField(parametersProp);

			parametersField.RegisterCallback<SerializedPropertyChangeEvent>((evt) => {
				string lastSegment = evt.changedProperty.propertyPath.Split('.')[^1];
				SetProperty(xParametersProp.FindPropertyRelative(lastSegment), evt.changedProperty);
				SetProperty(yParametersProp.FindPropertyRelative(lastSegment), evt.changedProperty);
				SetProperty(zParametersProp.FindPropertyRelative(lastSegment), evt.changedProperty);

				property.serializedObject.ApplyModifiedProperties();
			});


			root.Add(parametersField);

			return root;
        }
    }

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