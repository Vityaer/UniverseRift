using UnityEngine;

public class OutlineController : MonoBehaviour
{
    [SerializeField] private Material defaultMaterial, outlineMaterial;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SwitchOn()
    {
        spriteRenderer.material = outlineMaterial;
    }

    public void SwitchOff()
    {
        spriteRenderer.material = defaultMaterial;
    }
}
