using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BalaoTrecho : MonoBehaviour, IDragHandler, IEndDragHandler
{

    public Trecho trecho;
    public TipoTrecho tipoTrecho { get { return trecho.tipoTrecho; } set { trecho.tipoTrecho = value; } }
    public RectTransform balaoTransform { get { return this.GetComponent<RectTransform>(); } }
    public float largura { get { return balaoTransform.sizeDelta.x; } }
    public float altura { get { return balaoTransform.sizeDelta.y; } }
    private RectTransform retangulo;
    public LayoutHistoria layoutHistoria;
    public Text resumo;
    public Image borda;
    public bool dragging;
    public string balaoId { get { return trecho.id; } }

    // Use this for initialization
    void Awake()
    {
        retangulo = GetComponent<RectTransform>();
        layoutHistoria = GameObject.FindObjectOfType<LayoutHistoria>();
    }

    /// <summary>
    /// This function is called when the MonoBehaviour will be destroyed.
    /// </summary>
    void OnDestroy()
    {
        var trechos = new List<Trecho>(layoutHistoria.historia.trechos);
        trechos.Remove(trecho);
        layoutHistoria.historia.trechos = trechos.ToArray();
    }

    void OnGUI()
    {
        atualizarBalao();
    }

    /// <summary>
    /// Este evendo OnDrag acontece enquanto o jogador estiver tentando arrastar o componente
    /// com o mouse ou com o dedo (se for mobile)
    /// </summary>
    /// <param name="dadosDoEvento">Este parâmetro traz informações do toque ou clique do mouse</param>
    public void OnDrag(PointerEventData dadosDoEvento)
    {
        ControladorCamera.ativo = false;
        var dadosDoPonteiro = dadosDoEvento;
        if (dadosDoPonteiro == null)
        {
            return;
        }

        var posicaoAtual = retangulo.position;
        //Soma o delta (em x e em y) da movimentação na posição do componente
        //RectTransform do objeto
        posicaoAtual.x += (dadosDoPonteiro.delta.x / 2);
        posicaoAtual.y += (dadosDoPonteiro.delta.y / 2);
        retangulo.position = posicaoAtual;
        trecho.representacao.posicao = posicaoAtual;
    }

    public bool ehPai(BalaoTrecho balaoPai)
    {
        var pais = new List<string>(trecho.pais);
        return pais.Contains(balaoPai.trecho.id);
    }

    public void adicionaPai(BalaoTrecho balaoPai)
    {
        if (!balaoPai.ehPai(this))
        {
            var pais = new List<string>(trecho.pais);
            pais.Add(balaoPai.trecho.id);
            trecho.pais = pais.ToArray();
        }
    }

    public void removerPai(BalaoTrecho balaoPai)
    {
        var pais = new List<string>(trecho.pais);
        if (pais.Contains(balaoPai.trecho.id))
        {
            pais.Remove(balaoPai.trecho.id);
            trecho.pais = pais.ToArray();
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        ControladorCamera.ativo = true;
    }
    
    public void selecionar(){
        layoutHistoria.alternarBalao(this);
    }

    void mudarCorToggle()
    {

    }

    //Atualiza o balão com os dados do trecho
    public void atualizarBalao()
    {
        resumo.text = trecho.getResumoStr();
    }

    //Atualiza o trecho com os dados do balão
    public void atualizarTrecho()
    {
        trecho.setResumo(resumo.text);
    }
}
