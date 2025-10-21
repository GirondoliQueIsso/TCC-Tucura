using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Pega o script Player e chama a função de morte
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                // Para evitar chamar a morte múltiplas vezes, o script do Player agora controla isso.
                // A colisão com obstáculo já chama Die(), então essa zona pode ser para quedas.
                // O ideal é unificar: o obstáculo e a death zone devem chamar a mesma função Die().
                // A lógica de colisão no Player já faz isso, então a DeathZone pode ser apenas mais um obstáculo com tag "Obstaculo".

                // Se esta zona for para quedas e não obstáculos, a lógica abaixo é válida:
                // player.SendMessage("Die", SendMessageOptions.DontRequireReceiver);
            }
        }
        else if (other.CompareTag("Obstaculo"))
        {
            // Opcional: Destruir obstáculos que saem da tela
            Destroy(other.gameObject);
        }
    }
}