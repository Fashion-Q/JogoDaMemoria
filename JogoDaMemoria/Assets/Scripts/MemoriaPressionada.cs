using UnityEngine;

public class MemoriaPressionada : MonoBehaviour
{
    [SerializeField] private MemoriaController memoriaController;
    [SerializeField] private int index;

    public void Clicado()
    {
        memoriaController.MemoriaClicada(index);
    }


    public void DesabilitarBotoesFunc() => memoriaController.DesabilitarBotoesFunc();
    public void HabilitarBotoesFunc() => memoriaController.HabilitarBotoesFunc();
    private void OnMouseDown()
    {
        Clicado();
    }
}
