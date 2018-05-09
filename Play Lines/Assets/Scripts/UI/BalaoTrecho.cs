using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class BalaoTrecho : MonoBehaviour, IDragHandler, IEndDragHandler {

    private RectTransform retangulo;
    public Toggle toggle;
	public TipoBalaoTrecho tipo;
	public LayoutHistoria layoutHistoria;
	public Text resumo;
	public Image borda;
	public bool dragging;
	public Color selecionadoUnico, selecionadoUm, selecionadoDois, naoSelecionado;

	// Use this for initialization
	void Awake () {
		retangulo = GetComponent<RectTransform>();
		toggle = GetComponent<Toggle>();
		layoutHistoria = GameObject.FindObjectOfType<LayoutHistoria> ();

		toggle.onValueChanged.AddListener(delegate {
			mudarCorToggle();
		});

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
		if (dadosDoPonteiro == null) {
			return;
		}

		var posicaoAtual = retangulo.position;
		//Soma o delta (em x e em y) da movimentação na posição do componente
		//RectTransform do objeto
		posicaoAtual.x += (dadosDoPonteiro.delta.x / 2); 
		posicaoAtual.y += (dadosDoPonteiro.delta.y / 2);
		retangulo.position = posicaoAtual;
    }

	public void OnEndDrag (PointerEventData eventData)
	{
		ControladorCamera.ativo = true;
	}

	public void selecionar(){	
		layoutHistoria.selecionar (this);
	}
	
	// Update is called once per frame
	void mudarCorToggle () {
		
	}

	void OnGUI(){
		if(layoutHistoria.trechoSelecionado1 != this && layoutHistoria.trechoSelecionado2 != this && toggle.isOn)
			toggle.isOn = false;
		if(!toggle.isOn)
			borda.color = naoSelecionado;
		else{
			if(layoutHistoria.trechoSelecionado1 == this && layoutHistoria.trechoSelecionado2 == null)
				borda.color = selecionadoUnico;
			else if(layoutHistoria.trechoSelecionado1 == this && layoutHistoria.trechoSelecionado2 != null)
				borda.color = selecionadoUm;
			else if(layoutHistoria.trechoSelecionado2 == this)
				borda.color = selecionadoDois;
			else
				borda.color = naoSelecionado;
		}
	}
}
