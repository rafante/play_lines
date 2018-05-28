using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class TelaPrincipal : MonoBehaviour
{

    public InputField usuarioInput;
    public Text usuarioLogado;
    public ListaHistorias lista;

    // Use this for initialization
    void Start()
    {
        Jogo.instancia.menuPrincipalInit();
        buscar();
    }

    public void novaHistoria(){
        Jogo.instancia.criarHistoria();
        ControladorCenas.instancia.carregarCena("layout_historia");
    }

    public void carregarMinhasHistorias()
    {
        lista.carregar();
    }

    public void logar()
    {
        Jogo.instancia.logar(usuarioInput.text);
        buscar();
    }

    public void buscar()
    {
        Jogo.instancia.buscarLogado();
        if (usuarioInput != null)
            usuarioInput.text = Jogo.instancia.usuario;
        if (usuarioLogado != null)
            usuarioLogado.text = Jogo.instancia.usuario;
        carregarMinhasHistorias();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
