using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

/// <summary>
/// Delete prefabs that appear outside a circle.
/// </summary>
public class CullSprites : MonoBehaviour
{
    [Tooltip("Prefab to cull.")]
    public GameObject enemy;
    [Tooltip("Radius of circle beyond which enemies will be destroyed.")]
    [Min(0)]
    public float radius = 1f;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag(enemy.tag);
        foreach (var obj in objs)
        {
            float d = Vector2.Distance(transform.position, obj.transform.position);
            if (d > radius)
            {
                Destroy(obj);
            }
        }
    }


#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.red;
        Handles.DrawWireDisc(transform.position, Vector3.forward, radius);
    }
#endif
}
