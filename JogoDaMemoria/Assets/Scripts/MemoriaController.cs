using UnityEngine;

public class MemoriaController : MonoBehaviour
{
    public delegate void Play();
    public Play play;
    private Animator[] memoriasAnimations;
    [SerializeField] private GameObject[] gameObjectMemorias;
    private int[] filaDeMemoria;
    [SerializeField] int countMaxGanhouPiscaPisca;
    private int level = 1;
    private int animacoesIndexInicio, animacoesIndexJogadas;

    private void Start()
    {
        memoriasAnimations = new Animator[gameObjectMemorias.Length];
        for(int i=0;i< gameObjectMemorias.Length; i++)
            memoriasAnimations[i] = gameObjectMemorias[i].GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        play?.Invoke();
    }

    public void PlayButton()
    {
        if (DesabilitarBotoes)
        {
            return;
        }
        if (PlayButtonPressed)
        {
            return;
        }
        PlayButtonPressed = true;
        filaDeMemoria = new int[level];
        for(int i=0;i<level;i++)
            filaDeMemoria[i] = Random.Range(0, 4);
        animacoesIndexInicio = 0;
        animacoesIndexJogadas = 0;
        sleepCount = sleep;
        DesabilitarBotoes = true;
        play = FazerAnimacoes;
    }

    public void FazerAnimacoes()
    {
        sleepCount++;
        if(sleepCount >= sleep)
        {
            sleepCount = 0;
            TriggerClickAnimacao(filaDeMemoria[animacoesIndexInicio]);
            animacoesIndexInicio++;
            if (animacoesIndexInicio >= level)
            {
                play = null;
                DesabilitarBotoes = false;
            }
        }
    }

    public void MemoriaClicada(int indexMemoriaClicado)
    {
        if (DesabilitarBotoes)
        {
            return;
        }
        if (!PlayButtonPressed)
        {
            return;
        }
        if(indexMemoriaClicado == filaDeMemoria[animacoesIndexJogadas])
        {
            if(animacoesIndexJogadas +1 < level)
                TriggerClickAnimacao(indexMemoriaClicado);
            animacoesIndexJogadas++;
            if (animacoesIndexJogadas >= level)
                Ganhou();
        }
        else
        {
            Perdeu();
        }
    }

    private void GanhouOuPerdeuPlay()
    {
        if (memoriasAnimations[0].GetCurrentAnimatorStateInfo(0).IsName("idle"))
        {
            for (int i = 0; i < memoriasAnimations.Length; i++)
                TriggerEventoFinal(i,eventoGanhouOuPerdeu);
            countGanhouPlay++;
            if (countGanhouPlay == countMaxGanhouPiscaPisca)
            {
                play = null;
                DesabilitarBotoes = false;
            }
        }
    }
    
    private int countGanhouPlay = 0;
    public void Ganhou()
    {
        PlayButtonPressed = false;
        level++;
        eventoGanhouOuPerdeu = "ganhou";
        DesabilitarBotoes = true;
        countGanhouPlay = 0;
        play = GanhouOuPerdeuPlay;
    }

    public void Perdeu()
    {
        
        PlayButtonPressed = false;
        level = 1;
        eventoGanhouOuPerdeu = "perdeu";
        play = GanhouOuPerdeuPlay;
        DesabilitarBotoes = true;
        countGanhouPlay = 0;
    }
    public void Resetar()
    {
        if (DesabilitarBotoes) return;
        Perdeu();
    }
    public void DesabilitarBotoesFunc() => DesabilitarBotoes = true;
    public void HabilitarBotoesFunc() => DesabilitarBotoes = false;

    private string eventoGanhouOuPerdeu;
    public int sleep;
    private int sleepCount { get; set; } = 0;
    private bool PlayButtonPressed { get; set; } = false;
    private bool DesabilitarBotoes { get; set; } = false;
    private void TriggerClickAnimacao(int index) => memoriasAnimations[index].SetTrigger("animacao");
    private void TriggerEventoFinal(int index,string nome) => memoriasAnimations[index].SetTrigger(nome);
}