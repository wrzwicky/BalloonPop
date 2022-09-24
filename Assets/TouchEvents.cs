using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Sent OnTouchDown(Vector2) events for touchscreen touches.
Position is world pos of touch.
*/
public class TouchEvents : MonoBehaviour
{
    [Tooltip("Disable Unity's conversion of touches to OnMouse events (recommended, since OnTouch will be handling these).")]
    public bool disableTouchToMouse = true;
    [Tooltip("Enable our own conversion of mouse clicks to OnTouch events (not recommended if you want both OnMouse and OnTouch).")]
    public bool handleMouseClicks = false;

    private Camera _camera;

    // Start is called before the first frame update
    void Start()
    {
        Input.simulateMouseWithTouches = false;
    }

    void Awake() {
        _camera = Camera.main;
    }

    // Update is called once per frame
    void Update() {
        // https://docs.unity3d.com/Manual/MobileInput.html

        // https://docs.unity3d.com/ScriptReference/Touch.html
            //phase, radius
            //fingreId, position, radius

        //get all touches
        //raycast
        //for all hits
        //  (Touchable).OnTouchown

        if(handleMouseClicks) {
            for (var i = 0; i < 3; i++) {
                if(Input.GetMouseButtonDown(i)) {
                    Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
                    RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

                    if(hit.collider == null) {
                        // hit nothing, ignore all other buttons
                        break;
                    }
                    else {
                        //Debug.Log("  hit "+hit.collider.gameObject.name);
                        hit.transform.gameObject.SendMessage(
                            "OnTouchDown",
                            hit.point,
                            SendMessageOptions.DontRequireReceiver);
                    }
                }
            }
        }

        for (var i = 0; i < Input.touchCount; i++) {
            if (Input.GetTouch(i).phase == TouchPhase.Began) {

                // Construct a ray from the current touch coordinates
                Ray ray = _camera.ScreenPointToRay(Input.GetTouch(i).position);
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
                if(hit.collider != null) {
                    hit.transform.gameObject.SendMessage(
                        "OnTouchDown",
                        hit.point,
                        SendMessageOptions.DontRequireReceiver);
                }
            }
        }
    }
}
