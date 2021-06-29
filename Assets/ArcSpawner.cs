using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

//TODO more timing variation
//TODO 'clustered randomness' - sparse for a while, dense for a while
//TODO Panic Mode! randomly spawn 10+x more balloons

// spawn enemies on a 90-degree arc at the bottom of a circle
// (as determined by Physics2D.gravity)

public class ArcSpawner : MonoBehaviour
{
    [Tooltip("Prefab to spawn as enemy.")]
    public GameObject enemy;
    [Tooltip("Radius of circle where enemies will spawn.")]
    [Min(0)]
    public float radius = 1f;
    [Tooltip("Whether to remove all objects matching enemy's tag before game starts.")]
    public bool purgeEnemies = true;
    [Tooltip("Approx number of enemies per minute.")]
    public float spawnRate = 60;

    private float nextSpawn = 0;

    
    // Start is called before the first frame update
    void Start()
    {
        // Purge any testing enemies.
        if (purgeEnemies && enemy.tag != null)
        {
            GameObject[] objs = GameObject.FindGameObjectsWithTag(enemy.tag);
            foreach (var obj in objs)
                Destroy(obj);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //TODO: Hints online that Input.acceleration != Input.gyro, some phones might only have one

        // Manage enemies
        nextSpawn -= Time.deltaTime;
        if (nextSpawn <= 0)
        {
            spawnEnemy();
            nextSpawn = Random.Range(60f / spawnRate / 4f, 60f / spawnRate);
        }
    }

    private void spawnEnemy()
    {
        // Compute random point on arc at bottom of circle
        Vector3 v = Quaternion.AngleAxis(Random.Range(-45f, 45f), Vector3.forward) * Physics2D.gravity;
        v = v.normalized * radius;
        Debug.DrawRay(transform.position, v, Color.white, 0.2f);
        Vector2 pos = v + transform.position;

        // NOTE: As of v2020.1, Poly Collider does not always resolve overlaps.

        //float targetAngle = Vector3.SignedAngle(Vector3.down, Physics2D.gravity, Vector3.forward);
        Quaternion rot = Quaternion.LookRotation(Vector3.forward, -Physics2D.gravity);
        //Quaternion.identity

        GameObject obj = Instantiate(enemy, pos, rot);
        // Initial nudge. Does NOT help with poly collider problem.
        obj.GetComponent<Rigidbody2D>().AddForce(Physics2D.gravity * -0.2f, ForceMode2D.Impulse);
    }


#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        //Gizmos.DrawWireSphere(transform.position, 1f);
        //Handles.color = new Color(1f, 1f, 1f, 0.25f);

        Handles.color = Color.gray;
        Handles.DrawWireDisc(transform.position, Vector3.forward, radius);
        Handles.color = Color.magenta;
        Vector3 v = Quaternion.AngleAxis(-45f, Vector3.forward) * Physics2D.gravity;
        Handles.DrawWireArc(transform.position, Vector3.forward, v, 90f, radius);
    }
#endif
}
