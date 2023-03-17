using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[ExecuteAlways]
public class FollowCamera : MonoBehaviour
{
    void OnEnable()
    {
        RenderPipelineManager.beginCameraRendering += PreRender;
    }

    private void OnDisable()
    {
        RenderPipelineManager.beginCameraRendering -= PreRender;
    }

    void PreRender(ScriptableRenderContext src, Camera cam)
    {
        transform.position = cam.transform.position + cam.transform.forward * (cam.nearClipPlane + 0.00001f);
        transform.forward = cam.transform.forward;
    }
}
