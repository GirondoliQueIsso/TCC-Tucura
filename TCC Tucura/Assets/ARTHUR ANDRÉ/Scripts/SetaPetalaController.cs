using UnityEngine;

// Este script agora � super simples. Ele serve apenas como um "identificador"
// para que o GameController saiba qual objeto � a seta.
// Toda a l�gica de movimento e input foi centralizada no GameController1_JOGO1.
public class SetaPetalaController : MonoBehaviour
{
    // N�o precisa de mais nada aqui dentro!
    // Podemos apagar o Start, Update, MoverSeta, etc.
    // O GameController vai acessar diretamente o "transform" deste objeto
    // e mov�-lo/rotacion�-lo.
}