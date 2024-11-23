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

            var targetValue = new PropertyField(serializedObject.FindProperty("TargetValue"));
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

            var oscillationTypeLabel = new Label();
            root.Add(oscillationTypeLabel);

            void RecalculateOscillationTypeLabel(){
                oscillationTypeLabel.text = "Oscillation Type: ";
                if(Mathf.Approximately(dampingCoefficientProp.floatValue, 0)){
                    oscillationTypeLabel.text += "Undamped";
                }
                else if(Mathf.Approximately(dampingCoefficientProp.floatValue, 1)){
                    oscillationTypeLabel.text += "Critically Damped";
                }
                else if(dampingCoefficientProp.floatValue < 1){
                    oscillationTypeLabel.text += "Underdamped";
                }
                else {
                    oscillationTypeLabel.text += "Overdamped";
                }
            }
            RecalculateOscillationTypeLabel();

            var graph = new VisualElement();

            // Allow changes in editor to affect the SpringTransform at runtime
            frequencyField.RegisterValueChangeCallback(
                (evt) => {
                    if(spring.Spring != null) {
                        spring.Frequency = evt.changedProperty.floatValue;
                    }
                    graph.MarkDirtyRepaint();
                    RecalculateOscillationTypeLabel();
                }
            );

            dampingCoefficientField.RegisterValueChangeCallback(
                (evt) => {
                    if(spring.Spring != null) {
                        spring.DampingCoefficient = evt.changedProperty.floatValue;
                    }
                    graph.MarkDirtyRepaint();
                    RecalculateOscillationTypeLabel();
                }
            );

            responsivenessField.RegisterValueChangeCallback(
                (evt) => {
                    if(spring.Spring != null) {
                        spring.Responsiveness = evt.changedProperty.floatValue;
                    }
                    graph.MarkDirtyRepaint();
                    RecalculateOscillationTypeLabel();
                }
            );

            graph.style.width = new StyleLength(Length.Auto());
            graph.style.height = new StyleLength(128);

            graph.generateVisualContent += (MeshGenerationContext mgc) => {
                var rect = mgc.visualElement.contentRect;
                var width = rect.width;
                var height = rect.height;

                var springVector = new SpringVector3(
                    Vector3.zero,
                    2 * Mathf.PI * frequencyProp.floatValue,
                    dampingCoefficientProp.floatValue,
                    responsivenessProp.floatValue
                )
                {
                    TargetValue = new(1, 0, 0)
                };

                const float START_TIME = -0.5f;
                const float END_TIME = 2.0f;
                const float MARGIN_X = 16.0f;
                const float MARGIN_Y = 16.0f;

                const float PADDING_Y = 0.1f;

                float workingWidth = width - MARGIN_X * 2;
                float workingHeight = height - MARGIN_Y * 2;

                int nSamples = (int)(END_TIME/(END_TIME - START_TIME) * workingWidth);
                float dt = END_TIME / nSamples;

                Vector2[] points = new Vector2[nSamples];
                float minY = float.PositiveInfinity;
                float maxY = float.NegativeInfinity;
                for(int i = 0; i < nSamples; i++){
                    var t = i * dt;
                    points[i] = new(t, springVector.CurrentValue.x);
                    minY = Mathf.Min(minY, points[i].y);
                    maxY = Mathf.Max(maxY, points[i].y);
                    springVector.Update(dt);
                }
                minY -= PADDING_Y;
                maxY += PADDING_Y;

                Vector2 PointToPixel(Vector2 point){
                    return new Vector2(
                        (point.x - START_TIME) / (END_TIME - START_TIME) * workingWidth + MARGIN_X,
                        (1 - (point.y - minY) / (maxY - minY)) * workingHeight + MARGIN_Y
                    );
                }

                var painter = mgc.painter2D;

                painter.lineWidth = 1.0f;
                painter.lineCap = LineCap.Round;
                painter.lineJoin = LineJoin.Round;

                // Axes lines
                painter.strokeColor = Color.white;

                painter.BeginPath();
                painter.MoveTo(PointToPixel(new(START_TIME, 0)));
                painter.LineTo(PointToPixel(new(END_TIME, 0)));
                painter.Stroke();

                painter.BeginPath();
                painter.MoveTo(PointToPixel(new(0, minY)));
                painter.LineTo(PointToPixel(new(0, maxY)));
                painter.Stroke();

                // draw x = END_TIME tick
                painter.BeginPath();
                painter.MoveTo(PointToPixel(new(END_TIME, minY)));
                painter.LineTo(PointToPixel(new(END_TIME, 0)));
                painter.Stroke();

                // draw y = 1 line
                painter.strokeColor = new Color(0, 1.0f, 0, 0.3f);
                painter.BeginPath();
                painter.MoveTo(PointToPixel(new(0, 1)));
                painter.LineTo(PointToPixel(new(END_TIME, 1)));
                painter.Stroke();

                // Graph
                painter.strokeColor = Color.red;

                painter.BeginPath();
                painter.MoveTo(PointToPixel(new(START_TIME, 0)));
                painter.LineTo(PointToPixel(new(0, 0)));

                foreach(var point in points){
                    painter.LineTo(PointToPixel(point));
                }
                painter.Stroke();

                // draw x = 0 and x = END_TIME labels
                mgc.DrawText(
                    "0",
                    PointToPixel(new(0, minY)) - new Vector2(4 , 0),
                    12,
                    Color.white
                );

                mgc.DrawText(
                    END_TIME.ToString(),
                    PointToPixel(new(END_TIME, minY)) - new Vector2(4 , 0),
                    12,
                    Color.white
                );

                // draw y = 1 label
                mgc.DrawText(
                    "1",
                    PointToPixel(new(0, 1)) - new Vector2(8, 6),
                    12,
                    Color.green
                );
            };

            root.Add(graph);
        }
    }
}