using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace Kutie.Editor.Math {
    [CustomEditor(typeof(SpringTransform))]
    public class SpringTransformEditor : UnityEditor.Editor {
        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();
            PopulateInspector(root);
            return root;
        }

        void PopulateInspector(VisualElement root)
        {
            var spring = target as SpringTransform;

            var targetValue = new PropertyField(serializedObject.FindProperty("target"));
            root.Add(targetValue);


            var frequencyProp = serializedObject.FindProperty("_frequency");
            var frequencyField = new PropertyField(frequencyProp);
            root.Add(frequencyField);

            var dampingCoefficientProp = serializedObject.FindProperty("_dampingCoefficient");
            var dampingCoefficientField = new PropertyField(dampingCoefficientProp);
            root.Add(dampingCoefficientField);

            var responsivenessProp = serializedObject.FindProperty("_responsiveness");
            var responsivenessField = new PropertyField(responsivenessProp);
            root.Add(responsivenessField);

            var visualization = new VisualElement();
            root.Add(visualization);

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
                    if(spring.Spring != null) {
                        spring.Frequency = evt.changedProperty.floatValue;
                    }
                    graph.MarkDirtyRepaint();
                    recalculateOscillationTypeLabel();
                }
            );

            dampingCoefficientField.RegisterValueChangeCallback(
                (evt) => {
                    if(spring.Spring != null) {
                        spring.DampingCoefficient = evt.changedProperty.floatValue;
                    }
                    graph.MarkDirtyRepaint();
                    recalculateOscillationTypeLabel();
                }
            );

            responsivenessField.RegisterValueChangeCallback(
                (evt) => {
                    if(spring.Spring != null) {
                        spring.Responsiveness = evt.changedProperty.floatValue;
                    }
                    graph.MarkDirtyRepaint();
                    recalculateOscillationTypeLabel();
                }
            );
        }
    }
}