using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

sealed internal class CameraFilter : MonoBehaviour
{
    #region Values
    [SerializeField]
    private Shader _shader;

    private ModuleEnvironmentalCameraSettingsData baseEffect = new ModuleEnvironmentalCameraSettingsData
    {
        brightness = 1f,
        saturation = 1f,
        contrast = 1f,
        lightenColor = new SerializableColor(Color.black),
        redLevel = 1f,
        greenLevel = 1f,
        blueLevel = 1f,
    };
    internal ModuleEnvironmentalCameraSettingsData cameraEffectData = null;

    private Material _material;

    private Camera _camera;
    #endregion

    #region Unity Framework Entry Functions
    private void Awake()
    {
        _material = new Material(_shader);
    }

    private void Update()
    {
        UpdateEffects();
    }

    private void OnRenderImage(RenderTexture sourceTexture, RenderTexture destTexture)
    {
        if (_material != null)
            Graphics.Blit(sourceTexture, destTexture, _material);
        else
            Graphics.Blit(sourceTexture, destTexture);
    }

    private void OnDestroy()
    {
        Destroy(_material);
    }
    #endregion

    #region Functions
    private void UpdateEffects()
    {
        ApplyAdjustments();

    }
    private void ApplyAdjustments()
    {
        ModuleEnvironmentalCameraSettingsData effect = cameraEffectData != null ? Add(baseEffect, cameraEffectData) : baseEffect;

        _material.SetFloat("_Brightness", effect.brightness);
        _material.SetFloat("_Saturation", effect.saturation);
        _material.SetFloat("_Contrast", effect.contrast);

        _material.SetColor("_LightenColor", effect.lightenColor.color);

        _material.SetFloat("_RedLevel", effect.redLevel);
        _material.SetFloat("_GreenLevel", effect.greenLevel);
        _material.SetFloat("_BlueLevel", effect.blueLevel);
    }

    private ModuleEnvironmentalCameraSettingsData Add(ModuleEnvironmentalCameraSettingsData cameraEffectDataA, ModuleEnvironmentalCameraSettingsData cameraEffectDataB)
    {
        return new ModuleEnvironmentalCameraSettingsData
        {
            brightness = cameraEffectDataA.brightness + cameraEffectDataB.brightness,
            saturation = cameraEffectDataA.saturation + cameraEffectDataB.saturation,
            contrast = cameraEffectDataA.contrast + cameraEffectDataB.contrast,

            lightenColor = cameraEffectDataB.lightenColor,

            redLevel = cameraEffectDataA.redLevel + cameraEffectDataB.redLevel,
            blueLevel = cameraEffectDataA.blueLevel + cameraEffectDataB.blueLevel,
            greenLevel = cameraEffectDataA.greenLevel + cameraEffectDataB.greenLevel,
        };
    }
    #endregion
}