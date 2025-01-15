using UnityEngine;

public class IconHider : MonoBehaviour
{
    #region Variables
    [Header("References")]
    [SerializeField] WebGLInput input;
    [SerializeField] GameObject[] icons;
    #endregion

    #region Unity Methods
    private void OnEnable()
    {
        input.OnPausePressed += ToggleIcons;
    }

    private void OnDisable()
    {
        input.OnPausePressed -= ToggleIcons;
    }
    #endregion

    #region Event Listeners
    /// <summary>
    /// Toggles the visibility of the icons when the pause button is pressed
    /// </summary>
    private void ToggleIcons()
    {
        foreach (GameObject icon in icons)
            icon.SetActive(!icon.activeSelf);
    }
    #endregion
}
