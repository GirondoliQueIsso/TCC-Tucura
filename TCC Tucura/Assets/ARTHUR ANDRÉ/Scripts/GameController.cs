using UnityEngine;
using System.Collections.Generic; // Necessário para usar Listas
using UnityEngine.SceneManagement; // Necessário para reiniciar o jogo

public class GameController : MonoBehaviour
{
    [Tooltip("Arraste todos os objetos das pétalas da sua cena para cá.")]
    public List<GameObject> todasAsPetalas;

    [Tooltip("Arraste o objeto com a imagem da abelha normal para cá.")]
    public GameObject mioloAbelhaNormal;

    [Tooltip("Arraste o objeto com a imagem da abelha irritada para cá.")]
    public GameObject mioloAbelhaIrritada;

    private GameObject petalaRuim; // Variável para guardar qual pétala é a errada.

    // A função Start é chamada uma vez quando o jogo começa.
    void Start()
    {
        InicializarJogo();
    }

    void InicializarJogo()
    {
        // Garante que no início a abelha normal está visível e a irritada está invisível.
        mioloAbelhaNormal.SetActive(true);
        mioloAbelhaIrritada.SetActive(false);

        // Escolhe uma pétala aleatoriamente da lista para ser a pétala ruim.
        int indiceAleatorio = Random.Range(0, todasAsPetalas.Count);
        petalaRuim = todasAsPetalas[indiceAleatorio];

        // Avisa o script de cada pétala se ela é a ruim ou não.
        foreach (GameObject petalaObj in todasAsPetalas)
        {
            // Pega o componente "Petala" do objeto.
            Petala scriptDaPetala = petalaObj.GetComponent<Petala>();
            // Passa a referência deste GameController para o script da pétala.
            scriptDaPetala.gameController = this;

            if (petalaObj == petalaRuim)
            {
                scriptDaPetala.ehPetalaRuim = true;
            }
        }
    }

    // Esta função é chamada pelo script da pétala quando ela é clicada.
    public void PetalaFoiClicada(GameObject petalaClicada)
    {
        if (petalaClicada == petalaRuim)
        {
            // Se for a pétala errada, fim de jogo!
            mioloAbelhaNormal.SetActive(false);
            mioloAbelhaIrritada.SetActive(true);
            Debug.Log("Você perdeu! A abelha ficou irritada!");
        }
        else
        {
            // Se for uma pétala boa, ela some.
            petalaClicada.SetActive(false);
            Debug.Log("Ufa, pétala certa!");
        }
    }
}
