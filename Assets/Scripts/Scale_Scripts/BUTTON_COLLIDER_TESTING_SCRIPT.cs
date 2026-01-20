using UnityEngine;

public class BUTTON_COLLIDER_TESTING_SCRIPT : MonoBehaviour
{
    public enum button { on, off, mode, tare, weighBoat };
    public button whichButton;
    
    
    

    void OnEnable()
    {
        GameEventsManager.instance.webGLEvents.OnInteractPressed += WebGLButtonPress;
    }

    void OnDisable()
    {
        GameEventsManager.instance.webGLEvents.OnInteractPressed -= WebGLButtonPress;
    }

    public void ButtonPress()
    {
        // Debug.Log(whichButton + " Button Pressed");

        switch (whichButton)
        {
            case button.on:
                GameEventsManager.instance.miscEvents.ScalePowerOn();
                break;
            case button.off:
                GameEventsManager.instance.miscEvents.ScalePowerOff();
                break;
            case button.mode:
                GameEventsManager.instance.miscEvents.ScaleMode();
                break;
            case button.tare:
                GameEventsManager.instance.miscEvents.ScaleTare();
                break;
            case button.weighBoat:
                GameEventsManager.instance.miscEvents.BoatToScale();
                break;
        }
    }

    public void WebGLButtonPress(GameObject obj)
    {
        if (obj == gameObject)
            ButtonPress();
    }

}
