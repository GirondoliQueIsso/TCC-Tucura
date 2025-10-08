using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public enum MontagemState
{
    CorAsa,
    CorCorpo,
    FormaAsa,
    FormaCorpo,
    Finalizado
}

[System.Serializable]
public struct ButterflyData
{
    public int indexAsaSprite;
    public int indexAsaCor;
    public int indexCorpoSprite;
    public int indexCorpoCor;

    public bool IsEqual(ButterflyData other)
    {
        return indexAsaSprite == other.indexAsaSprite &&
               indexAsaCor == other.indexAsaCor &&
               indexCorpoSprite == other.indexCorpoSprite &&
               indexCorpoCor == other.indexCorpoCor;
    }
}

public class ButterflyDraft : MonoBehaviour
{
    [Header("Referências Visuais da Borboleta")]
    public SpriteRenderer asaEsquerda;
    public SpriteRenderer asaDireita;
    public SpriteRenderer corpo;

    [Header("Opções de Customização")]
    public Sprite[] spritesAsa;
    public Color[] coresAsa;
    public Sprite[] spritesCorpo;
    public Color[] coresCorpo;

    [Header("UI Elementos")]
    public TextMeshProUGUI contadorText;
    public TextMeshProUGUI instrucaoText;
    public GameObject painelControles;
    public GameObject painelResposta;
    public TextMeshProUGUI resultadoText;

    [Header("Botões")]
    public Button botaoTrocarAsa;
    public Button botaoTrocarCorpo;
    public Button botaoConfirmar;
    public Button botaoReiniciar;

    [Header("Tempos")]
    public float tempoMemorizar = 5f;
    public float tempoReproduzir = 20f; // Aumentei o tempo

    private ButterflyData borboletaModelo;
    private ButterflyData borboletaDoJogador;
    private MontagemState estadoAtualMontagem;

    void Start()
    {
        if (botaoReiniciar != null) botaoReiniciar.onClick.AddListener(StartGameLoop);
        if (botaoTrocarAsa != null) botaoTrocarAsa.onClick.AddListener(OnTrocarAsaClicked);
        if (botaoTrocarCorpo != null) botaoTrocarCorpo.onClick.AddListener(OnTrocarCorpoClicked);
        if (botaoConfirmar != null) botaoConfirmar.onClick.AddListener(ConfirmarEtapa);

        StartGameLoop();
    }

    public void StartGameLoop()
    {
        painelControles.SetActive(false);
        painelResposta.SetActive(false);
        if (botaoReiniciar != null) botaoReiniciar.gameObject.SetActive(false);
        StartCoroutine(GameLoop());
    }

    IEnumerator GameLoop()
    {
        // FASE 1: MEMORIZAR
        instrucaoText.text = "Memorize!";
        contadorText.gameObject.SetActive(true);
        painelControles.SetActive(false);

        GerarBorboletaModeloAleatoria();
        AplicarVisual(borboletaModelo);
        yield return StartCoroutine(ContagemRegressiva(tempoMemorizar));

        // FASE 2: JOGADOR
        painelControles.SetActive(true);
        ResetarBorboletaDoJogador();
        AplicarVisual(borboletaDoJogador);

        estadoAtualMontagem = MontagemState.CorAsa;
        AtualizarControlesEInstrucoes();

        float tempoRestante = tempoReproduzir;
        while (tempoRestante > 0 && estadoAtualMontagem != MontagemState.Finalizado)
        {
            contadorText.text = Mathf.Ceil(tempoRestante).ToString();
            yield return new WaitForSeconds(1f);
            tempoRestante--;
        }

        // FASE 3: RESPOSTA
        instrucaoText.text = "Resultado";
        painelControles.SetActive(false);
        contadorText.gameObject.SetActive(false);
        painelResposta.SetActive(true);

        int acertos = 0;
        if (borboletaModelo.indexAsaCor == borboletaDoJogador.indexAsaCor) acertos++;
        if (borboletaModelo.indexCorpoCor == borboletaDoJogador.indexCorpoCor) acertos++;
        if (borboletaModelo.indexAsaSprite == borboletaDoJogador.indexAsaSprite) acertos++;
        if (borboletaModelo.indexCorpoSprite == borboletaDoJogador.indexCorpoSprite) acertos++;

        float porcentagem = (float)acertos / 4f * 100f;
        resultadoText.text = $"VOCÊ ACERTOU {porcentagem}% !";

        AplicarVisual(borboletaModelo);
        if (botaoReiniciar != null) botaoReiniciar.gameObject.SetActive(true);
    }

    // --- FUNÇÕES DOS BOTÕES ---
    public void ConfirmarEtapa()
    {
        if (estadoAtualMontagem < MontagemState.Finalizado)
        {
            estadoAtualMontagem++;
            AtualizarControlesEInstrucoes();
        }
    }

    public void OnTrocarAsaClicked()
    {
        if (estadoAtualMontagem == MontagemState.CorAsa)
        {
            borboletaDoJogador.indexAsaCor = (borboletaDoJogador.indexAsaCor + 1) % coresAsa.Length;
        }
        else if (estadoAtualMontagem == MontagemState.FormaAsa)
        {
            borboletaDoJogador.indexAsaSprite = (borboletaDoJogador.indexAsaSprite + 1) % spritesAsa.Length;
        }
        AplicarVisual(borboletaDoJogador);
    }

    public void OnTrocarCorpoClicked()
    {
        if (estadoAtualMontagem == MontagemState.CorCorpo)
        {
            borboletaDoJogador.indexCorpoCor = (borboletaDoJogador.indexCorpoCor + 1) % coresCorpo.Length;
        }
        else if (estadoAtualMontagem == MontagemState.FormaCorpo)
        {
            borboletaDoJogador.indexCorpoSprite = (borboletaDoJogador.indexCorpoSprite + 1) % spritesCorpo.Length;
        }
        AplicarVisual(borboletaDoJogador);
    }

    // --- FUNÇÕES DE LÓGICA ---
    void AtualizarControlesEInstrucoes()
    {
        // Desativa ambos por padrão para evitar erros
        botaoTrocarAsa.interactable = false;
        botaoTrocarCorpo.interactable = false;

        switch (estadoAtualMontagem)
        {
            case MontagemState.CorAsa:
                instrucaoText.text = "Escolha a COR da Asa";
                botaoTrocarAsa.interactable = true;
                break;
            case MontagemState.CorCorpo:
                instrucaoText.text = "Escolha a COR do Corpo";
                botaoTrocarCorpo.interactable = true;
                break;
            case MontagemState.FormaAsa:
                instrucaoText.text = "Escolha a FORMA da Asa";
                botaoTrocarAsa.interactable = true;
                break;
            case MontagemState.FormaCorpo:
                instrucaoText.text = "Escolha a FORMA do Corpo";
                botaoTrocarCorpo.interactable = true;
                break;
        }
    }

    IEnumerator ContagemRegressiva(float tempo)
    {
        float restante = tempo;
        while (restante > 0)
        {
            contadorText.text = Mathf.Ceil(restante).ToString();
            yield return new WaitForSeconds(1f);
            restante--;
        }
        contadorText.text = "";
    }

    void GerarBorboletaModeloAleatoria()
    {
        borboletaModelo.indexAsaSprite = Random.Range(0, spritesAsa.Length);
        borboletaModelo.indexAsaCor = Random.Range(0, coresAsa.Length);
        borboletaModelo.indexCorpoSprite = Random.Range(0, spritesCorpo.Length);
        borboletaModelo.indexCorpoCor = Random.Range(0, coresCorpo.Length);
    }

    void ResetarBorboletaDoJogador()
    {
        borboletaDoJogador.indexAsaSprite = 0;
        borboletaDoJogador.indexAsaCor = 0;
        borboletaDoJogador.indexCorpoSprite = 0;
        borboletaDoJogador.indexCorpoCor = 0;
    }

    void AplicarVisual(ButterflyData data)
    {
        asaEsquerda.sprite = spritesAsa[data.indexAsaSprite];
        asaEsquerda.color = coresAsa[data.indexAsaCor];
        asaDireita.sprite = spritesAsa[data.indexAsaSprite];
        asaDireita.color = coresAsa[data.indexAsaCor];
        corpo.sprite = spritesCorpo[data.indexCorpoSprite];
        corpo.color = coresCorpo[data.indexCorpoCor];
    }
}