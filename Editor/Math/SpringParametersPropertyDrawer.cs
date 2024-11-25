using System.Diagnostics;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Kutie.Editor {
	[CustomPropertyDrawer(typeof(SpringParameters))]
	public class SpringParametersPropertyDrawer : PropertyDrawer {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var root = new VisualElement();

            var foldout = new Foldout
            {
                text = property.displayName
            };
            root.Add(foldout);

			var frequencyProp = property.FindPropertyRelative("Frequency");
			var dampingCoefficientProp = property.FindPropertyRelative("DampingCoefficient");
			var responsivenessProp = property.FindPropertyRelative("Responsiveness");

			var frequencyField = new PropertyField(frequencyProp);
			var dampingCoefficientField = new PropertyField(dampingCoefficientProp);
			var responsivenessField = new PropertyField(responsivenessProp);

			foldout.Add(frequencyField);
			foldout.Add(dampingCoefficientField);
			foldout.Add(responsivenessField);

			var visualization = new VisualElement();
            foldout.Add(visualization);


            SpringEditorUtil.PopulateVisualization(
                visualization,
                frequencyProp,
                dampingCoefficientProp,
                responsivenessProp,
                out var recalculateOscillationTypeLabel,
                out var graph
            );

            // Allow changes in editor to affect the SpringTransform at runtime
            frequencyField.RegisterValueChangeCallback(
                (evt) => {
                    if(frequencyProp.floatValue <= 0) {
                        frequencyProp.floatValue = 0.01f;
                        property.serializedObject.ApplyModifiedProperties();
                    }
                    graph.MarkDirtyRepaint();
                    recalculateOscillationTypeLabel();
                }
            );

            dampingCoefficientField.RegisterValueChangeCallback(
                (evt) => {
                    graph.MarkDirtyRepaint();
                    recalculateOscillationTypeLabel();
                }
            );

            responsivenessField.RegisterValueChangeCallback(
                (evt) => {
                    graph.MarkDirtyRepaint();
                    recalculateOscillationTypeLabel();
                }
            );

			return root;
        }
    }
}