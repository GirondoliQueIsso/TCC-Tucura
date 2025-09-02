using UnityEngine;
using System.Collections.Generic; // Adicione esta linha para poder usar "List<T>"

public class GameManager : MonoBehaviour
{
    // A 'inst�ncia' est�tica (Singleton) permite acessar este GameManager
    // de qualquer outro script de forma f�cil, usando "GameManager.instance".
    public static GameManager instance;

    // Esta vari�vel vai guardar a quantidade de jogadores (2 ou 4)
    // que foi escolhida na tela de sele��o.
    public int numberOfPlayers;

    // --- MUDAN�A PRINCIPAL ---
    // Em vez de guardar apenas o GameObject, agora guardamos a "ficha" completa
    // do personagem (que cont�m o nome, �cone e o futuro prefab).
    // Esta lista ser� preenchida na tela de sele��o de personagem.
    public List<CharacterData> selectedCharacters = new List<CharacterData>();

    // A fun��o Awake � chamada antes de qualquer fun��o Start, garantindo
    // que nossa inst�ncia esteja pronta desde o in�cio.
    private void Awake()
    {
        // L�gica do Singleton: garante que S� EXISTA UM GameManager no jogo inteiro.
        if (instance == null)
        {
            // Se n�o existe nenhuma inst�ncia, esta se torna a inst�ncia principal.
            instance = this;

            // Esta � a linha mais importante: impede que este objeto (o GameManager)
            // seja destru�do quando uma nova cena � carregada.
            // Assim, mantemos as informa��es (n� de jogadores, personagens escolhidos) entre as cenas.
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Se um GameManager j� existe (por exemplo, se voc� voltou para o menu principal),
            // este novo que tentou ser criado � destru�do para evitar duplicatas e erros.
            Destroy(gameObject);
        }
    }

    // Fun��o p�blica para limpar a lista de personagens escolhidos.
    // Isso � �til para chamar quando um novo jogo come�a (ao sair do menu),
    // garantindo que as escolhas da partida anterior sejam apagadas.
    public void ClearSelections()
    {
        selectedCharacters.Clear();
    }
}