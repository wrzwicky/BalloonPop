using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringScaler : MonoBehaviour
{
    public SpringJoint2D tailEnd;
    private SpriteRenderer sprite;

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //get dist from my world pos to tailEnd world pos
        //change Y scale to match
        float curH = sprite.bounds.size.y;
        float newH = Vector2.Distance(transform.position, tailEnd.transform.position);
        transform.localScale = new Vector3(transform.localScale.x, newH / curH, transform.localScale.z);

        float a = AngleTo(transform.position, tailEnd.transform.position) + 90f;
        sprite.gameObject.transform.localRotation = Quaternion.Euler(transform.localRotation.x, transform.localRotation.y, a);
    }

    private static float AngleTo(Vector2 pos, Vector2 target)
    {
        Vector2 diference = target - pos;
        float sign = (target.y < pos.y) ? -1.0f : 1.0f;
        return Vector2.Angle(Vector2.right, diference) * sign;
    }

}
