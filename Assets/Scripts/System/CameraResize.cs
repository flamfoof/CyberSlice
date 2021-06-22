using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CameraResize : MonoBehaviour {

    public Camera cameraMain;

        // Use this for initialization
        void Start()
        {
        cameraMain.orthographicSize = 5; // Size u want to start with
        }

    // Update is called once per frame
        void Update()
        {
        cameraMain.orthographicSize = 5;
        }
    }
