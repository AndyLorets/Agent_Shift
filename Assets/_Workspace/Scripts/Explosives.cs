using UnityEngine;
using UnityEngine.Rendering;

public class Explosives : MonoBehaviour, ITaskable
{
    [SerializeField] private Sprite _interactSprite;
    [SerializeField] private InteractableHandler _interactHandler;
    [SerializeField] private Shader _shader;
   
    private Material _material;
    public string taskName { get; set; }

    private void Awake()
    {
        Renderer renderer = transform.GetChild(0).GetComponent<Renderer>();
        if (renderer != null)
        {
            _material = Instantiate(renderer.material);
            renderer.material = _material;
        }
        else
        {
            Debug.LogError("Renderer not found on the object.");
        }
    }
    private void Start()
    {
        _interactHandler.Init(_interactSprite, Active);
    }
    private void Active()
    {
        SetRenderingMode(_material);
        _interactHandler.SetEnable(false);
        ServiceLocator.GetService<TaskManager>().CompleteTask(taskName);
        ServiceLocator.GetService<AudioManager>().PlayItem();
    }
    private void SetRenderingMode(Material material)
    {
        material.shader = _shader;
        material.SetFloat("_Mode", 0);
        material.SetInt("_SrcBlend", (int)BlendMode.One);
        material.SetInt("_DstBlend", (int)BlendMode.Zero);
        material.SetInt("_ZWrite", 1);
        material.DisableKeyword("_ALPHATEST_ON");
        material.DisableKeyword("_ALPHABLEND_ON");
        material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        material.EnableKeyword("_NORMALMAP");
        material.renderQueue = (int)RenderQueue.Geometry;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TagsObj.PLAYER))
            _interactHandler.SetInteractable(true);
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(TagsObj.PLAYER))
            _interactHandler.SetInteractable(false);
    }
}
