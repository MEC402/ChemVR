using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SensitivityAdjuster : MonoBehaviour
{
    [SerializeField] FirstPersonMovement firstPersonMovement; // Reference to the FirstPersonMovement script
    [SerializeField] Slider sensitivitySlider; // Reference to the UI Slider for sensitivity adjustment
    [SerializeField] TextMeshProUGUI sensitivityValueText; // Reference to the UI Text for displaying the sensitivity value

    private void Start()
    {
        // Add a listener to the slider to call UpdateSensitivity when the value changes
        sensitivitySlider.onValueChanged.AddListener(UpdateSensitivity);
    }

    private void UpdateSensitivity(float newValue)
    {
        // Update the mouse sensitivity in the FirstPersonMovement script
        firstPersonMovement.mouseSensitivity = newValue / 100f;
        // Update the text to display the new sensitivity value
        sensitivityValueText.text = $"{newValue}";
    }
}
