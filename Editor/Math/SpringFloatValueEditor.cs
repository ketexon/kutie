using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace Kutie.Editor.Math {
    [CustomEditor(typeof(SpringFloatValue))]
    public class SpringFloatValueEditor : UnityEditor.Editor {
        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();
            PopulateInspector(root);
            return root;
        }

        void PopulateInspector(VisualElement root)
        {
            var spring = target as SpringFloatValue;

            var defaultTargetField = new PropertyField(serializedObject.FindProperty("_defaultTarget"));
            root.Add(defaultTargetField);

            var defaultParametersField = new PropertyField(serializedObject.FindProperty("_defaultParameters"));
            root.Add(defaultParametersField);

            var frequencyProp = serializedObject.FindProperty("_defaultParameters.Frequency");
            var dampingCoefficientProp = serializedObject.FindProperty("_defaultParameters.DampingCoefficient");
            var responsivenessProp = serializedObject.FindProperty("_defaultParameters.Responsiveness");

            defaultParametersField.RegisterValueChangeCallback((evt) => {
                spring.Spring?.SetParameters(new SpringParameters {
                    Frequency = frequencyProp.floatValue,
                    DampingCoefficient = dampingCoefficientProp.floatValue,
                    Responsiveness = responsivenessProp.floatValue
                });
            });

            // var frequencyProp = serializedObject.FindProperty("_frequency");
            // var frequencyField = new PropertyField(frequencyProp);
            // root.Add(frequencyField);

            // var dampingCoefficientProp = serializedObject.FindProperty("_dampingCoefficient");
            // var dampingCoefficientField = new PropertyField(dampingCoefficientProp);
            // root.Add(dampingCoefficientField);

            // var responsivenessProp = serializedObject.FindProperty("_responsiveness");
            // var responsivenessField = new PropertyField(responsivenessProp);
            // root.Add(responsivenessField);

            // var visualization = new VisualElement();
            // root.Add(visualization);

            // SpringEditorUtil.PopulateVisualization(
            //     visualization,
            //     frequencyProp,
            //     dampingCoefficientProp,
            //     responsivenessProp,
            //     out var recalculateOscillationTypeLabel,
            //     out var graph
            // );

            // // Allow changes in editor to affect the SpringTransform at runtime
            // frequencyField.RegisterValueChangeCallback(
            //     (evt) => {
            //         if(spring.Spring != null) {
            //             spring.Frequency = evt.changedProperty.floatValue;
            //         }
            //         graph.MarkDirtyRepaint();
            //         recalculateOscillationTypeLabel();
            //     }
            // );

            // dampingCoefficientField.RegisterValueChangeCallback(
            //     (evt) => {
            //         if(spring.Spring != null) {
            //             spring.DampingCoefficient = evt.changedProperty.floatValue;
            //         }
            //         graph.MarkDirtyRepaint();
            //         recalculateOscillationTypeLabel();
            //     }
            // );

            // responsivenessField.RegisterValueChangeCallback(
            //     (evt) => {
            //         if(spring.Spring != null) {
            //             spring.Responsiveness = evt.changedProperty.floatValue;
            //         }
            //         graph.MarkDirtyRepaint();
            //         recalculateOscillationTypeLabel();
            //     }
            // );
        }
    }
}