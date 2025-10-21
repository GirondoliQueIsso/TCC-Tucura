using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Pega o script Player e chama a fun��o de morte
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                // Para evitar chamar a morte m�ltiplas vezes, o script do Player agora controla isso.
                // A colis�o com obst�culo j� chama Die(), ent�o essa zona pode ser para quedas.
                // O ideal � unificar: o obst�culo e a death zone devem chamar a mesma fun��o Die().
                // A l�gica de colis�o no Player j� faz isso, ent�o a DeathZone pode ser apenas mais um obst�culo com tag "Obstaculo".

                // Se esta zona for para quedas e n�o obst�culos, a l�gica abaixo � v�lida:
                // player.SendMessage("Die", SendMessageOptions.DontRequireReceiver);
            }
        }
        else if (other.CompareTag("Obstaculo"))
        {
            // Opcional: Destruir obst�culos que saem da tela
            Destroy(other.gameObject);
        }
    }
}