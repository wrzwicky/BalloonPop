using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//TODO purge *.dll from Android build

/// <summary>
/// General game manager.
/// Handle gravity, quit button.
/// </summary>
public class Playfield : MonoBehaviour
{
    private float secretHeight = int.MaxValue;

    private void Awake()
    {
        Input.gyro.enabled = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        Text scoreText = GetComponentInChildren<Text>();
        RectTransform rt = scoreText.transform as RectTransform;
        Vector3[] fca = new Vector3[4];
        rt.GetWorldCorners(fca);
        secretHeight = Camera.main.WorldToScreenPoint(fca[0]).y;
        Debug.Log(secretHeight);
    }

    // Update is called once per frame
    void Update()
    {
        //TODO: Hints online that Input.acceleration != Input.gyro, some phones might only have one

        // Update current gravity
        if (SystemInfo.supportsGyroscope)
        {
            Vector3 proj = Vector3.ProjectOnPlane(Input.gyro.gravity, Vector3.forward);
            Physics2D.gravity = proj * 9.8f;
        }

        // Check if Back was pressed this frame
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Quit the application
            Application.Quit();
        }

        // Secret panic mode
        if(Input.GetMouseButton(0) && Input.mousePosition.y > secretHeight)
        {
            //curRate = spawnRate * 10f;
        }
        else
        {
            //curRate = spawnRate;
        }
    }
}
