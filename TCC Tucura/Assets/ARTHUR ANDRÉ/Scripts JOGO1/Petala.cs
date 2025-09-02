using UnityEngine;

public class Petala : MonoBehaviour
{
    [Tooltip("A imagem da p�tala normal (branca).")]
    public Sprite spriteNormal;

    [Tooltip("A imagem da p�tala selecionada (com borda vermelha).")]
    public Sprite spriteSelecionado;

    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Selecionar()
    {
        spriteRenderer.sprite = spriteSelecionado;
    }

    public void Deselecionar()
    {
        spriteRenderer.sprite = spriteNormal;
    }
}

