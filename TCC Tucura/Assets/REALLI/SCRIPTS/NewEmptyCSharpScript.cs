using UnityEngine;

// Esta linha m�gica permite criar "fichas de personagem" direto no menu do Unity
[CreateAssetMenu(fileName = "NovoPersonagem", menuName = "Tucura/Dados de Personagem")]
public class CharacterData : ScriptableObject
{
    public string nomeDoPersonagem;
    public Sprite icone; // Imagem para a tela de sele��o (nosso placeholder)
    public GameObject prefabDoPersonagem; // O boneco que ser� usado no minigame (pode deixar em branco por agora)
}