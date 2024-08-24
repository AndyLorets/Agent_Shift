using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;

public class DissolveMaterialController : MonoBehaviour
{
    [SerializeField] private float _fadeValue = 0.2f;
    [SerializeField] private Shader _shaderMobileInvisibility;

    private bool _hasEmission;
    private Color _emissionColor;
    private Shader _shader;
    private Material _material;
    private int _renderQueue; 

    private const float MAT_FADE_DURATION = 0.7f;
    private const float MAT_EMISSION_DURATION = 0.3f;
    private void Awake()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            _material = Instantiate(renderer.material);
            renderer.material = _material;
            _shader = _material.shader;
            _renderQueue = _material.renderQueue; 
            _hasEmission = _material.HasProperty("_EmissionColor");

            if (_hasEmission)
            {
                _emissionColor = _material.GetColor("_EmissionColor");
            }
        }
        else
        {
            Debug.LogError("Renderer not found on the object.");
        }

        PlayerAbilities.onInvisibility += ChangeDissolveValue;
    }

    private void OnDestroy()
    {
        PlayerAbilities.onInvisibility -= ChangeDissolveValue;
    }

    private void ChangeDissolveValue(bool value)
    {
        if (value) ActivateInvisibility(MAT_FADE_DURATION);
        else DeactivateInvisibility(MAT_FADE_DURATION);
    }

    public void ActivateInvisibility(float duration)
    {
        if (_hasEmission)
        {
            _material.SetColor("_EmissionColor", Color.black);
        }
        SetRenderingMode(_material, RenderingMode.Transparent);
        //_material.DOFade(_fadeValue, duration);
        _material.DOFloat(_fadeValue, "_Alpha", duration);
    }

    public void DeactivateInvisibility(float duration)
    {
        _material.DOFloat(1, "_Alpha", duration).OnComplete(() =>
        {
            SetRenderingMode(_material, RenderingMode.Opaque);
            if (_hasEmission)
            {
                _material.DOColor(_emissionColor, "_EmissionColor", MAT_EMISSION_DURATION);
            }
        });
    }

    private void SetRenderingMode(Material material, RenderingMode renderingMode)
    {
        switch (renderingMode)
        {
            case RenderingMode.Opaque:
                material.shader = _shader;
                material.SetFloat("_Mode", 0);
                material.SetInt("_SrcBlend", (int)BlendMode.One);
                material.SetInt("_DstBlend", (int)BlendMode.Zero);
                material.SetInt("_ZWrite", 1);
                material.DisableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.EnableKeyword("_NORMALMAP");
                material.renderQueue = _renderQueue;
                break;

            case RenderingMode.Transparent:
                material.shader = _shaderMobileInvisibility;
                material.SetFloat("_Mode", 3);
                material.SetInt("_SrcBlend", (int)BlendMode.SrcAlpha);
                material.SetInt("_DstBlend", (int)BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ZWrite", 0);
                material.DisableKeyword("_ALPHATEST_ON");
                material.EnableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.EnableKeyword("_NORMALMAP");
                material.renderQueue = (int)RenderQueue.Transparent;
                break;
        }
    }

    private enum RenderingMode
    {
        Opaque,
        Transparent
    }
}
