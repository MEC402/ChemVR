using UnityEngine;
using UnityEngine.InputSystem;

public class ToggleWithPause : MonoBehaviour
{
    [SerializeField] bool isWebGL;

    [Header("Menus")]
    [SerializeField] WebGLInput input;
    [SerializeField] GameObject[] menusToToggle;

    private void OnEnable()
    {
        if (isWebGL)
            input.OnPausePressed += ToggleMenusWebGL;
        else
            GameEventsManager.instance.inputEvents.onPauseButtonPressed += ToggleMenus;
    }

    private void OnDisable()
    {
        if (isWebGL)
            input.OnPausePressed -= ToggleMenusWebGL;
        else
            GameEventsManager.instance.inputEvents.onPauseButtonPressed -= ToggleMenus;
    }

    private void ToggleMenusWebGL()
    {
        foreach (GameObject menu in menusToToggle)
            menu.SetActive(!menu.activeSelf);
    }

    private void ToggleMenus(InputAction.CallbackContext obj)
    {
        foreach (GameObject menu in menusToToggle)
            menu.SetActive(!menu.activeSelf);
    }
}
