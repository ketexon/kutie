using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Kutie.Editor {
	internal static class SpringEditorUtil {
        public static void PopulateVisualization(
			VisualElement root,
			SpringParameters springParameters,
			out System.Action recalculateOscillationTypeLabel,
			out VisualElement graph,
			float startTime = -0.5f,
			float endTime = 2.0f
		){
			var oscillationTypeLabel = new Label();
            root.Add(oscillationTypeLabel);

            void RecalculateOscillationTypeLabel(){
				var coeff = springParameters.DampingCoefficient;

                oscillationTypeLabel.text = "Oscillation Type: ";
                if(Mathf.Approximately(coeff, 0)){
                    oscillationTypeLabel.text += "Undamped";
                }
                else if(Mathf.Approximately(coeff, 1)){
                    oscillationTypeLabel.text += "Critically Damped";
                }
                else if(coeff < 1){
                    oscillationTypeLabel.text += "Underdamped";
                }
                else {
                    oscillationTypeLabel.text += "Overdamped";
                }
            }
            RecalculateOscillationTypeLabel();
			recalculateOscillationTypeLabel = RecalculateOscillationTypeLabel;

            graph = new VisualElement();

            graph.style.width = new StyleLength(Length.Auto());
            graph.style.height = new StyleLength(128);

            graph.generateVisualContent += (MeshGenerationContext mgc) => {
                var rect = mgc.visualElement.contentRect;
                var width = rect.width;
                var height = rect.height;

                var springFloat = new SpringFloat(
                    0,
                    springParameters
                )
                {
                    TargetValue = 1
                };

                const float MARGIN_X = 16.0f;
                const float MARGIN_Y = 16.0f;

                const float PADDING_Y = 0.1f;

                float workingWidth = width - MARGIN_X * 2;
                float workingHeight = height - MARGIN_Y * 2;

                int nSamples = (int)(endTime/(endTime - startTime) * workingWidth);
                float dt = endTime / nSamples;

                Vector2[] points = new Vector2[nSamples];
                float minY = float.PositiveInfinity;
                float maxY = float.NegativeInfinity;
                for(int i = 0; i < nSamples; i++){
                    var t = i * dt;
                    points[i] = new(t, springFloat.CurrentValue);
                    minY = Mathf.Min(minY, points[i].y);
                    maxY = Mathf.Max(maxY, points[i].y);
                    springFloat.Update(dt);
                }
				maxY = Mathf.Max(maxY, 1);
                minY -= PADDING_Y;
                maxY += PADDING_Y;

                Vector2 PointToPixel(Vector2 point){
                    return new Vector2(
                        (point.x - startTime) / (endTime - startTime) * workingWidth + MARGIN_X,
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
                painter.MoveTo(PointToPixel(new(startTime, 0)));
                painter.LineTo(PointToPixel(new(endTime, 0)));
                painter.Stroke();

                painter.BeginPath();
                painter.MoveTo(PointToPixel(new(0, minY)));
                painter.LineTo(PointToPixel(new(0, maxY)));
                painter.Stroke();

                // draw x = END_TIME tick
                painter.BeginPath();
                painter.MoveTo(PointToPixel(new(endTime, minY)));
                painter.LineTo(PointToPixel(new(endTime, 0)));
                painter.Stroke();

                // draw y = 1 line
                painter.strokeColor = new Color(0, 1.0f, 0, 0.3f);
                painter.BeginPath();
                painter.MoveTo(PointToPixel(new(0, 1)));
                painter.LineTo(PointToPixel(new(endTime, 1)));
                painter.Stroke();

                // Graph
                painter.strokeColor = Color.red;

                painter.BeginPath();
                painter.MoveTo(PointToPixel(new(startTime, 0)));
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
                    endTime.ToString(),
                    PointToPixel(new(endTime, minY)) - new Vector2(4 , 0),
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


		public static void PopulateVisualization(
			VisualElement root,
			SerializedProperty frequencyProp,
			SerializedProperty dampingCoefficientProp,
			SerializedProperty responsivenessProp,
			out System.Action recalculateOscillationTypeLabel,
			out VisualElement graph,
			float startTime = -0.5f,
			float endTime = 2.0f
		){
			var oscillationTypeLabel = new Label();
            root.Add(oscillationTypeLabel);

            void RecalculateOscillationTypeLabel(){
				var coeff = dampingCoefficientProp.floatValue;

                oscillationTypeLabel.text = "Oscillation Type: ";
                if(Mathf.Approximately(coeff, 0)){
                    oscillationTypeLabel.text += "Undamped";
                }
                else if(Mathf.Approximately(coeff, 1)){
                    oscillationTypeLabel.text += "Critically Damped";
                }
                else if(coeff < 1){
                    oscillationTypeLabel.text += "Underdamped";
                }
                else {
                    oscillationTypeLabel.text += "Overdamped";
                }
            }
            RecalculateOscillationTypeLabel();
			recalculateOscillationTypeLabel = RecalculateOscillationTypeLabel;

            graph = new VisualElement();

            graph.style.width = new StyleLength(Length.Auto());
            graph.style.height = new StyleLength(128);

            graph.generateVisualContent += (MeshGenerationContext mgc) => {
                var rect = mgc.visualElement.contentRect;
                var width = rect.width;
                var height = rect.height;

                var springFloat = new SpringFloat(
                    0,
                    new SpringParameters()
                    {
                        Frequency = frequencyProp.floatValue,
                        DampingCoefficient = dampingCoefficientProp.floatValue,
                        Responsiveness = responsivenessProp.floatValue
                    }
                )
                {
                    TargetValue = 1
                };

                const float MARGIN_X = 16.0f;
                const float MARGIN_Y = 16.0f;

                const float PADDING_Y = 0.1f;

                float workingWidth = width - MARGIN_X * 2;
                float workingHeight = height - MARGIN_Y * 2;

                int nSamples = (int)(endTime/(endTime - startTime) * workingWidth);
                float dt = endTime / nSamples;

                Vector2[] points = new Vector2[nSamples];
                float minY = float.PositiveInfinity;
                float maxY = float.NegativeInfinity;
                for(int i = 0; i < nSamples; i++){
                    var t = i * dt;
                    points[i] = new(t, springFloat.CurrentValue);
                    minY = Mathf.Min(minY, points[i].y);
                    maxY = Mathf.Max(maxY, points[i].y);
                    springFloat.Update(dt);
                }
				maxY = Mathf.Max(maxY, 1);
                minY -= PADDING_Y;
                maxY += PADDING_Y;

                Vector2 PointToPixel(Vector2 point){
                    return new Vector2(
                        (point.x - startTime) / (endTime - startTime) * workingWidth + MARGIN_X,
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
                painter.MoveTo(PointToPixel(new(startTime, 0)));
                painter.LineTo(PointToPixel(new(endTime, 0)));
                painter.Stroke();

                painter.BeginPath();
                painter.MoveTo(PointToPixel(new(0, minY)));
                painter.LineTo(PointToPixel(new(0, maxY)));
                painter.Stroke();

                // draw x = END_TIME tick
                painter.BeginPath();
                painter.MoveTo(PointToPixel(new(endTime, minY)));
                painter.LineTo(PointToPixel(new(endTime, 0)));
                painter.Stroke();

                // draw y = 1 line
                painter.strokeColor = new Color(0, 1.0f, 0, 0.3f);
                painter.BeginPath();
                painter.MoveTo(PointToPixel(new(0, 1)));
                painter.LineTo(PointToPixel(new(endTime, 1)));
                painter.Stroke();

                // Graph
                painter.strokeColor = Color.red;

                painter.BeginPath();
                painter.MoveTo(PointToPixel(new(startTime, 0)));
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
                    endTime.ToString(),
                    PointToPixel(new(endTime, minY)) - new Vector2(4 , 0),
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