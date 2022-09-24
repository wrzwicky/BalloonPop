using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manage sprite for the balloon tail.
/// </summary>
public class PopBalloonTail : MonoBehaviour
{
    // Main RB we need to update
    private Rigidbody2D m_rb;
    // Lerping to transform.rotation oscillates badly
    //private float curAngle = float.NegativeInfinity;

    // Start is called before the first frame update
    void Start()
    {
        m_rb = GetComponentInParent<Rigidbody2D>();

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.color = Random.ColorHSV(0f, 1f, 0.4f, 0.6f, 0.6f, 0.6f);
    }

    // Update is called once per frame
    private void Update()
    {
        transform.rotation = Quaternion.Euler(0, 0, m_rb.rotation - m_rb.angularVelocity/6f);
    }

//TODO tail should lag balloon when turning
//TODO probly need to keep track of tail origin manually, compute dir from that

/*
    void Updatez()
    {
        // Balloon's 'up' vector
        //  - Looks like angle is only relative to parent at start
        //  - afterwards, it's absolute
        Vector2 up = Vector2.up; //transform.parent.rotation * Vector2.up;
        // Balloon's direction of travel (and velocity)
        Vector2 dir = m_rb.velocity;

        // rotate tail opposite motion
        float a = Vector2.SignedAngle(up, dir);
        if (curAngle != float.NegativeInfinity)
            a = Mathf.Lerp(curAngle, a, 0.1f);  //slowly
        transform.rotation = Quaternion.Euler(0, 0, a);
        curAngle = a;
        //Debug.DrawRay(transform.position, m_rb.velocity);

        // stretch by velocity
        float v = m_rb.velocity.magnitude;
        transform.localScale = new Vector3(1, v, transform.localScale.z);
    }
*/
}
