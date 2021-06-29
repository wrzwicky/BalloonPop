using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// DEPRECATED - use PopBalloon instead
public class Balloon : MonoBehaviour
{
    public Rigidbody2D rb;
    private Collider2D coll;

    // Start is called before the first frame update
    void Start()
    {
        coll = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Handle mouse
        if (Input.GetMouseButton(0))
        {
            Poke(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }

        // Handle phone touch (ignore >1 fingers)
        //foreach (Touch theTouch in Input.touches)
        if (Input.touchCount > 0)
        {
            Touch theTouch = Input.GetTouch(0);
            //if (theTouch.phase == TouchPhase.Ended)
            Poke(Camera.main.ScreenToWorldPoint(theTouch.position));
        }

        // Right side up
        float down = 0f;
        if (SystemInfo.supportsGyroscope)
        {
            float angle = Vector3.SignedAngle(Vector3.down, Physics2D.gravity, Vector3.forward);
            down = angle;
        }
        float av = rb.angularVelocity;
        Mathf.SmoothDampAngle(rb.rotation, down, ref av, 0.5f);
        rb.angularVelocity = av;

    }

    // pos in world coords
    private void Poke(Vector2 position)
    {
#if DEBUG
        Debug.DrawLine(position, coll.transform.position, Color.white, 0.1f);
#endif
        Vector2 d = (Vector2)(coll.transform.position) - position;
        d /= (1f / .4f);   //(left) distance yields (right) velocity
        rb.velocity += d;
    }
}



//Some gravity notes:

//Input.deviceOrientation
//    DeviceOrientation.Portrait
//    DeviceOrientation.PortraitUpsideDown
// -- not needed, gravity is relative to screen

//Input.gyro.gravity returns:
//  a vector that points down where screen horiz(+right),vert(+up),deep(+face)
//portrait = 0,-1,0
//portrait inv = 0,1,0 -- except screen flips, then becomes 0,-1,0
//on right side = 1,0,0
//face up = 0,0,-1
