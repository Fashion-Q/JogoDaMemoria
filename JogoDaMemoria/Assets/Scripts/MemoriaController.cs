using System.Collections;
using UnityEngine;

public class MemoriaController : MonoBehaviour
{
    //public delegate int PerformCalculation(int x, int y);
    public delegate void Play();
    public Play play;
    private Animator[] memorias;
    [SerializeField] private GameObject[] gameObjectMemorias;
    private int[] levelMemoria;
    [SerializeField] int countMaxGanhou;
    private int level = 1;
    private int animacoesIndex, animacoesIndexJogadas;

    private void Start()
    {
        memorias = new Animator[gameObjectMemorias.Length];
        for(int i=0;i< gameObjectMemorias.Length; i++)
            memorias[i] = gameObjectMemorias[i].GetComponent<Animator>();
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
        levelMemoria = new int[level];
        for(int i=0;i<level;i++)
        {
            levelMemoria[i] = Random.Range(0, 4);
        }
        sleepCount = 0;
        animacoesIndex = 0;
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
            TriggerClickAnimacao(levelMemoria[animacoesIndex]);
            animacoesIndex++;
            if (animacoesIndex >= level)
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
        if(indexMemoriaClicado == levelMemoria[animacoesIndexJogadas])
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
        if (memorias[0].GetCurrentAnimatorStateInfo(0).IsName("idle"))
        {
            for (int i = 0; i < memorias.Length; i++)
                TriggerEventoFinal(i,eventoGanhouOuPerdeu);
            countGanhouPlay++;
            if (countGanhouPlay == countMaxGanhou)
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
    private void TriggerClickAnimacao(int index) => memorias[index].SetTrigger("animacao");
    private void TriggerEventoFinal(int index,string nome) => memorias[index].SetTrigger(nome);
}
