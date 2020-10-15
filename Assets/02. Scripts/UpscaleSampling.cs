using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpscaleSampling : MonoBehaviour
{
    // Caching variable of the RenderTexure to be created
    // RT stands for RenderTexure.
    RenderTexture mRt;

    // Resolution ratio
    float mRatio = 0.5f;

    [Header("Canvas object with RenderTexure")]
    [SerializeField]
    GameObject mCanvasForRT;

    [Header("Camera to output rendering results to RenderTexure")]
    [SerializeField]
    Camera mRenderCam;

    /// <summary>
    /// Generates a RenderTexure proportional to the Ratio, 
    /// <para/>receives the result from the camera, and renders it through the RawImage.
    /// </summary>
    /// <param name="scale">== ratio</param>
    public void CreateRT(float scale)
    {
        // If there is a pre-made RT, release it.
        ReleaseRT();

        // Generate RenderTexure according to the ratio
        mRatio = scale;
        int w = (int)((float)Screen.width * mRatio);
        int h = (int)((float)Screen.height * mRatio);
        mRt = new RenderTexture(w, h, 24, RenderTextureFormat.DefaultHDR);
        mRt.Create();

        // Output camera results to RT
        mRenderCam.targetTexture = mRt;

        // Output RT in raw image
        RawImage raw = mCanvasForRT.GetComponentInChildren<RawImage>();
        raw.texture = mRt;
    }

    /// <summary>
    /// RT is unmanaged data, so release it.
    /// </summary>
    public void ReleaseRT()
    {
        try
        { 
            mRenderCam.targetTexture = null;
        }
        catch(MissingReferenceException mre)
        {
            Debug.Log($"We'll fix this later. \n Because this problem is not important now. \n LOG : {mre}");
        }

        if (mRt != null)
            mRt.Release();
    }



    private void OnEnable()
    {
        // Init FullHD
        Screen.SetResolution(1920, 1080, false);

        // As soon as the game starts, it is output in its original resolution
        CreateRT(1.0f);

    }

    void OnDisable()
    {
        ReleaseRT();
    }
}
