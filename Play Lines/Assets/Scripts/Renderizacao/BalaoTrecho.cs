using System;
using System.Collections;
using System.Collections.Generic;
using Entidades;
using EnumsHistoria;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Renderizacao
{
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
        public Text ordem;
        public InputField condicao;
        public Image borda;
        public bool arrastando;
        public int balaoId { get { return trecho.ordem; } }
        public List<int> pais { get { return new List<int>(trecho.pais); } }

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
            arrastando = true;
            ControladorCamera.ativo = false;
            var dadosDoPonteiro = dadosDoEvento;
            if (dadosDoPonteiro == null)
            {
                return;
            }

            var posicaoAtual = retangulo.position;
            //Soma o delta (em x e em y) da movimentação na posição do componente
            //RectTransform do objeto
            posicaoAtual.x += (dadosDoPonteiro.delta.x);
            posicaoAtual.y += (dadosDoPonteiro.delta.y);
            retangulo.position = posicaoAtual;
            trecho.representacao.posicao = posicaoAtual;
        }

        public bool ehPai(BalaoTrecho balaoPai)
        {
            var pais = new List<int>(trecho.pais);
            return pais.Contains(balaoPai.trecho.ordem);
        }

        public void adicionaPai(BalaoTrecho balaoPai)
        {
            var pais = new List<int>(trecho.pais);
            if (!pais.Contains(balaoPai.balaoId))
            {
                pais.Add(balaoPai.trecho.ordem);
            }
            trecho.pais = pais.ToArray();
        }

        public void removerPai(BalaoTrecho balaoPai)
        {
            var pais = new List<int>(trecho.pais);
            if (pais.Contains(balaoPai.trecho.ordem))
            {
                pais.Remove(balaoPai.trecho.ordem);
            }
            trecho.pais = pais.ToArray();
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            arrastando = false;
            ControladorCamera.ativo = true;
        }

        public void selecionar()
        {
            layoutHistoria.alternarBalao(this);
        }

        void mudarCorToggle()
        {

        }

        //Atualiza o balão com os dados do trecho
        public void atualizarBalao()
        {
            resumo.text = trecho.getResumoStr();
            ordem.text = trecho.ordem.ToString();
        }

        //Atualiza o trecho com os dados do balão
        public void atualizarTrecho()
        {
            trecho.setResumo(resumo.text);
        }
    }
}
