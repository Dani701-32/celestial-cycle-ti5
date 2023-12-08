using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostProcessing : MonoBehaviour
{
    private Material mat;

    void Awake()
    {
        mat = new Material(Shader.Find("Custom/ShaderGrayscale"));
    }

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (mat == null) return;
        Graphics.Blit(src, dest, mat);
    }
}
