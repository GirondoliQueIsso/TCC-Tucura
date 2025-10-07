using UnityEngine;
using UnityEngine.SceneManagement; // Importante para gerenciar cenas

public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Checa se o objeto que entrou na zona de morte tem a tag "Player"
        if (other.CompareTag("Player"))
        {
            // Reinicia a cena atual
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}