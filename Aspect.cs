using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Aspect : MonoBehaviour {
    private CanvasScaler cs;
    private float orthographicSize = 0;
    private float screenWidth = 0;
    private float screenHeight = 0;
    public float scaleFactor;

    public Action<float> OnReaspectResolution;

    // Start is called before the first frame update
    void Start() {
        cs = GetComponent<CanvasScaler>();
        orthographicSize = cs.GetComponent<Canvas>().worldCamera.orthographicSize;
    }

    // Update is called once per frame
    void Update() {
        if (screenWidth != Screen.width || screenHeight != Screen.height) {
            screenWidth = Screen.width;
            screenHeight = Screen.height;
            float designResolution = cs.referenceResolution.x / cs.referenceResolution.y;
            float realResolution = screenWidth / screenHeight;
            float aspect = realResolution / designResolution;
            if (aspect > 1) {
                cs.matchWidthOrHeight = 1;
                cs.GetComponent<Canvas>().worldCamera.orthographicSize = aspect;
                scaleFactor = cs.referenceResolution[1] / Screen.height;
            } else {
                cs.matchWidthOrHeight = 0;
                cs.GetComponent<Canvas>().worldCamera.orthographicSize = orthographicSize;
                scaleFactor = cs.referenceResolution[0] / Screen.width;
            }
            OnReaspectResolution?.Invoke(scaleFactor);
        }
    }
}
