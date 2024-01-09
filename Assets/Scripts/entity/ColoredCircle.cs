using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
[RequireComponent(typeof (SpriteRenderer))]
public class ColoredCircle : MonoBehaviour
{

    private SpriteRenderer _spriteRenderer;
    
    private void Awake()
    {
        this._spriteRenderer = this.GetComponent<SpriteRenderer>();
    }
    
    private void Start()
    {
        //this.color = new Color(Random.Range(0F, 1F), Random.Range(0F, 1F), Random.Range(0F, 1F));
    }

    private void Update()
    {
    }

    public Color GetColor()
    {
        return this._spriteRenderer.color;
    }
    
}
