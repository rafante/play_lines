﻿using System;
using System.Collections;
using System.Collections.Generic;
using Entidades;
using Padronizacao;
using UnityEngine;
using UnityEngine.UI;

public class ContadorHistoria : MonoBehaviour
{

    public Historia historia;
    public Trecho trechoAtual;
    public Canvas tela;

    public Image fundo;
    public AudioSource tocadorMusicaLista;
    public AudioSource tocadorSonsLista;
    public List<Image> imagensLista;
    public List<Text> textosLista;

    public GameObject escolhasContainer;
    public GameObject proximoTrecho;
    public GameObject botaoEscolhaPrefab;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        historia = ComposicaoHistoria.historia;
        trechoAtual = historia.getInicio();
        renderizaTrechoAtual();
        proximoTrecho.GetComponent<Button>().onClick.AddListener(delegate
        {
            avancar(0);
        });
    }

    public void avancar(Trecho trecho)
    {
        var filhos = historia.filhosDoTrecho(trechoAtual);
        int i = filhos.IndexOf(trecho);
        avancar(i);
    }

    public void avancar(int posicao = 0)
    {
        var filhos = historia.filhosDoTrecho(trechoAtual);
        trechoAtual = filhos[posicao];
        renderizaTrechoAtual();
    }

    public void recuar(int posicao = 0)
    {
        var pais = historia.paisDoTrecho(trechoAtual);
        trechoAtual = pais[posicao];
        renderizaTrechoAtual();
    }

    public void renderizaTrechoAtual()
    {
        limpar();

        renderizarFundo();
        renderizarImagens();
        renderizarTextos();
        tocarSons();
        tocarMusica();

        renderizaEscolhas();
    }

    private void tocarMusica()
    {

    }

    private void tocarSons()
    {

    }

    private void renderizarTextos()
    {
        var textos = trechoAtual.getTextos();
        foreach (var texto in textos)
        {
            var tex = texto.recurso.criaTexto();

            // if (texto.fonte == null)
            tex.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            // else
            // tex.font = Recursos.fontes[texto.fonte];

            var rectTransform = tex.GetComponent<RectTransform>();

            tex.transform.SetParent(tela.transform);
            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(0, 0);
            rectTransform.pivot = new Vector2(0.5f, 0.5f);

            tex.transform.localScale = Vector3.one;
            tex.transform.localPosition = Vector3.zero;

            var dimX = texto.dimensoes.x;
            var dimY = texto.dimensoes.y;

            var posX = texto.posicao.x;
            var posY = texto.posicao.y;

            if ((dimX != Padroes.dimTextX && dimX != 0) || (dimY != Padroes.dimTextY && dimY != 0))
                rectTransform.sizeDelta = new Vector2(dimX, dimY);
            else
                rectTransform.sizeDelta = new Vector2(Padroes.dimTextX, Padroes.dimTextY);

            if (posX != Padroes.posTextX || posY != Padroes.posTextY)
                rectTransform.transform.localPosition = new Vector3(posX, posY, 0);
            else
                rectTransform.transform.localPosition = new Vector3(Padroes.posTextX, Padroes.posTextY);

            Color cor;
            if (ColorUtility.TryParseHtmlString(texto.cor, out cor))
                tex.color = cor;
            else
                tex.color = Color.black;
            textosLista.Add(tex);
        }
    }

    private void renderizarImagens()
    {
        List<AplicacaoRecurso> imagens = trechoAtual.getImagens();
        foreach (AplicacaoRecurso imagem in imagens)
        {
            var img = imagem.recurso.criaImagem();

            var rectTransform = img.GetComponent<RectTransform>();

            img.transform.SetParent(tela.transform);
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.zero;

            img.transform.localScale = Vector3.one;
            img.transform.localPosition = Vector3.zero;

            if (imagem.dimensoes.x != 0 || imagem.dimensoes.y != 0)
                rectTransform.sizeDelta = new Vector2(imagem.dimensoes.x, imagem.dimensoes.y);
            else
                rectTransform.sizeDelta = Padroes.DIMENSOES_RECURSO_TEXTO;

            if (imagem.posicao.x != 0 || imagem.posicao.y != 0)
                rectTransform.position = new Vector3(imagem.posicao.x, imagem.posicao.y, 0);
            else
                rectTransform.position = Padroes.POSICAO_RECURSO_IMAGEM;

            Color cor;
            if (ColorUtility.TryParseHtmlString(imagem.cor, out cor))
                img.color = cor;
            else
                img.color = Color.black;
            imagensLista.Add(img);
        }
    }

    private void renderizaEscolhas()
    {
        var filhos = historia.filhosDoTrecho(trechoAtual);
        if (filhos.Count > 1)
        {
            for (int i = 0; i < filhos.Count; i++)
            {
                var filho = filhos[i];
                Button botaoProximo = Instantiate(botaoEscolhaPrefab).GetComponent<Button>();
                botaoProximo.onClick.AddListener(delegate
                {
                    avancar(filho);
                });
                botaoProximo.GetComponentInChildren<Text>().text = filho.textoCondicao;
                botaoProximo.transform.SetParent(escolhasContainer.transform);

                botaoProximo.transform.localScale = Vector3.one;
                botaoProximo.transform.localPosition = Vector3.zero;
            }
            escolhasContainer.SetActive(true);
        }
        else if (filhos.Count == 1)
        {
            proximoTrecho.SetActive(true);
        }
        else
        {

        }
    }

    private void renderizarFundo()
    {

    }

    public void limpar()
    {
        foreach (var imagem in imagensLista)
        {
            Destroy(imagem.gameObject);
        }
        imagensLista.Clear();
        foreach (var texto in textosLista)
        {
            Destroy(texto.gameObject);
        }
        foreach (Transform escolha in escolhasContainer.transform)
        {
            Destroy(escolha.gameObject);
        }
        textosLista.Clear();
        proximoTrecho.SetActive(false);
        escolhasContainer.SetActive(false);
    }
}
