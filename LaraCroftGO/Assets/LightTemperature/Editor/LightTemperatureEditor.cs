using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CanEditMultipleObjects]
[CustomEditor(typeof(LightTemperature))]
public class LightTemperatureEditor : Editor {

    private const float minTemp = LightTemperature.minTemperature;
    private const float maxTemp = LightTemperature.maxTemperature;

    private readonly static Dictionary<string, float> presets = new Dictionary<string, float>() {
        { "Overcast Sky\n(7500K)", 7500f },
        { "LED\n(7000K)", 7000f },
        { "Sun Light\n(6750K)", 6750f },
        { "Fluorescent Light\n(5250K)", 5250f },
        { "Tungsten Light\n(3300K)", 3300f },
        { "Sunset/Sunrise\n(2700K)", 2700f },
        { "High Pressure Sodium\n(2100K)", 2100f },
        { "Candle\n(1650K)", 1650f }
    };

    private SerializedProperty temperature;

    public override void OnInspectorGUI() {
        if(temperature == null)
            temperature = serializedObject.FindProperty("m_temperature");

        EditorGUI.showMixedValue = true;
        EditorGUILayout.Space();
        serializedObject.Update();
        TemperatureSlider(temperature);
        EditorGUILayout.Space();
        DrawPresets(temperature);
        serializedObject.ApplyModifiedProperties();
    }

    private void TemperatureSlider(SerializedProperty property) {
        var rect = EditorGUILayout.GetControlRect();
        var controlID = GUIUtility.GetControlID(FocusType.Passive, rect);
        var line = new Rect(rect);

        EditorGUI.Slider(rect, property, minTemp, maxTemp, new GUIContent("Temperature (K)"));

        if(Event.current.GetTypeForControl(controlID) != EventType.Repaint)
            return;

        rect = EditorGUI.PrefixLabel(rect, controlID, new GUIContent(" "));
        rect.xMax -= 55f;

        line.width = 1f;
        line.x = rect.xMin;

        for(int x = 0; x < rect.width - 1; x++, line.x++) {
            var temperature = Mathf.Lerp(minTemp, maxTemp, x / rect.width);
            EditorGUI.DrawRect(line, LightTemperature.ColorFromTemperature(temperature));
        }

        new GUIStyle("ColorPickerBox").Draw(rect, GUIContent.none, controlID);

        line.width = 2f;
        line.yMin--;
        line.yMin++;
        line.x = rect.xMin + Mathf.Lerp(0f, rect.width - 1f, (property.floatValue - minTemp) / (maxTemp - minTemp));

        EditorGUI.DrawRect(line, new Color32(56, 56, 56, 255));
    }

    private void DrawPresets(SerializedProperty prop) {
        EditorGUILayout.LabelField("Presets", EditorStyles.boldLabel);

        var selected = ArrayUtility.IndexOf(presets.Values.ToArray(), prop.floatValue);
        var result = GUILayout.SelectionGrid(selected, presets.Keys.ToArray(), 2, EditorStyles.miniButton);

        if(result != -1)
            prop.floatValue = presets.ElementAt(result).Value;
    }

    [MenuItem("GameObject/Light/Temperature Light", false, -100)]
    private static void CreateNew() {
        if(EditorApplication.ExecuteMenuItem("GameObject/Light/Directional Light"))
            Selection.activeGameObject.AddComponent<LightTemperature>();
    }

}