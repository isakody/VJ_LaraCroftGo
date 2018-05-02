using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Light))]
public sealed class LightTemperature : MonoBehaviour {

    public const float minTemperature = 1000f;
    public const float maxTemperature = 15000f;

    [SerializeField]
    [Range(minTemperature, maxTemperature)]
    private float m_temperature = 6500f;
    private Light m_light;

    /// <summary>
    /// Temperature of the light in Kelvin
    /// </summary>
    public float temperature {
        set { m_temperature = Mathf.Clamp(value, minTemperature, maxTemperature); }
        get { return m_temperature; }
    }
    /// <summary>
    /// Light attached to the same GameObject as this script
    /// </summary>
    public Light currentLight {
        get { return m_light ? m_light : (m_light = GetComponent<Light>()); }
    }

    private void OnEnable() {
        Update();
    }

    private void Update() {
        if(currentLight)
            currentLight.color = ColorFromTemperature(temperature);
    }

    /// <summary>
    /// Returns a color from the given temperature, thanks to tannerhelland.com for the algorithm
    /// </summary>
    /// <param name="temperature">Temperature of the color in Kelvin</param>
    public static Color ColorFromTemperature(float temperature) {
        temperature /= 100f;

        var red = 255f;
        var green = 255f;
        var blue = 255f;

        if(temperature >= 66f) {
            red = temperature - 60f;
            red = 329.698727446f * Mathf.Pow(red, -0.1332047592f);
        }

        if(temperature < 66f) {
            green = temperature;
            green = 99.4708025861f * Mathf.Log(green) - 161.1195681661f;
        }
        else {
            green = temperature - 60f;
            green = 288.1221695283f * Mathf.Pow(green, -0.0755148492f);
        }

        if(temperature <= 19f) {
            blue = 0f;
        }
        else if(temperature <= 66f) {
            blue = temperature - 10f;
            blue = 138.5177312231f * Mathf.Log(blue) - 305.0447927307f;
        }

        red /= 255f;
        green /= 255f;
        blue /= 255f;

        return new Color(red, green, blue);
    }

    public static implicit operator Light(LightTemperature obj) {
        return obj.currentLight;
    }
}