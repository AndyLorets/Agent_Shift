using DG.Tweening;
using UnityEngine;

public class DissolveMaterialCotroller : MonoBehaviour
{
    [SerializeField] private float _fadeValue = 0.2f; 
    private Material _material;

    private void Awake()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            _material = renderer.material;
            _material = Instantiate(_material);
            renderer.material = _material;
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
        if (value) ActivateInvisibility(.7f);
        else DeactivateInvisibility(.7f);
    }
    public void ActivateInvisibility(float duration)
    {
        SetRenderingMode(_material, RenderingMode.Transparent);
        _material.DOFade(_fadeValue, duration);
    }

    public void DeactivateInvisibility(float duration)
    {
        _material.DOFade(1f, duration).OnComplete(() =>
        {
            SetRenderingMode(_material, RenderingMode.Opaque);
        });
    }

    private void SetRenderingMode(Material material, RenderingMode renderingMode)
    {
        switch (renderingMode)
        {
            case RenderingMode.Opaque:
                material.SetFloat("_Mode", 0);
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                material.SetInt("_ZWrite", 1);
                material.DisableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = -1;
                break;

            case RenderingMode.Transparent:
                material.SetFloat("_Mode", 3);
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ZWrite", 0);
                material.DisableKeyword("_ALPHATEST_ON");
                material.EnableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = 3000;
                break;
        }
    }
    private enum RenderingMode
    {
        Opaque,
        Transparent
    }
}
