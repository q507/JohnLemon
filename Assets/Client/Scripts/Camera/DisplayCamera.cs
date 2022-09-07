using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayCamera : MonoBehaviour
{
    public Camera _WindowCamera;
    public RenderTexture _windowTexture;

    private static DisplayCamera instance;
    public delegate void deleSetText(GameObject go);
    public deleSetText OnSetText;
    public static DisplayCamera Instance
    {
        get { return instance = instance ?? new DisplayCamera(); }
    }

    public void InitWindowCamera()
    {
        _windowTexture = Resources.Load<RenderTexture>("Window");
        _WindowCamera = new GameObject("WindowCamera").AddComponent<Camera>();
        _WindowCamera.clearFlags = CameraClearFlags.SolidColor;
        _WindowCamera.orthographic = true;
        _WindowCamera.orthographicSize = 0.2f;
        _WindowCamera.nearClipPlane = 0f;
        _WindowCamera.farClipPlane = 0.8f;
        _WindowCamera.targetTexture = _windowTexture;
    }
}
