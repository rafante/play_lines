using System;
using System.Collections;
using System.Collections.Generic;
using Entidades;
using EnumsHistoria;
using UnityEngine;
using UnityEngine.UI;

namespace Renderizacao
{
    public class EditorTrecho : MonoBehaviour
    {

        public Trecho trecho;

        public Canvas tela;

        public Text prefabTexto;
        public Image prefabImagem;
        public AudioClip prefabSom;
        public AudioClip prefabMusica;

        public List<Text> textos;
        public List<Image> imagens;
        public List<AudioClip> sons;
        public List<AudioClip> musicas;

        // Use this for initialization
        void Start()
        {
            carregarRecursos();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void carregarRecursos()
        {
            foreach (AplicacaoRecurso aplicacao in trecho.aplicacoesRecurso)
            {
                Transform transformRecurso = null;
                switch (aplicacao.recurso.tipoRecurso)
                {
                    case TipoRecursoTrecho.TEXTO:
                        Text texto = Instantiate(prefabTexto);
                        texto.text = aplicacao.recurso.valor;
                        aplicarTexto(aplicacao, texto);
                        transformRecurso = texto.transform;
                        break;
                    case TipoRecursoTrecho.IMAGEM:
                        Image imagem = Instantiate(prefabImagem);
                        imagem.sprite = Resources.Load<Sprite>(aplicacao.recurso.valor);
                        aplicarImagem(aplicacao, imagem);
                        transformRecurso = imagem.transform;
                        break;
                    case TipoRecursoTrecho.SOM:
                        AudioClip som = Instantiate(prefabSom);
                        aplicarSom(aplicacao, som);
                        break;
                    case TipoRecursoTrecho.MUSICA:
                        AudioClip musica = Instantiate(prefabMusica);
                        aplicarMusica(aplicacao, musica);
                        break;
                }
                aplicarTransform(aplicacao, transformRecurso);
            }
        }

        private void aplicarMusica(AplicacaoRecurso aplicacao, AudioClip musica)
        {

        }

        private void aplicarSom(AplicacaoRecurso aplicacao, AudioClip som)
        {

        }

        private void aplicarImagem(AplicacaoRecurso aplicacao, Image imagem)
        {

        }

        private void aplicarTexto(AplicacaoRecurso aplicacao, Text texto)
        {

        }

        private void aplicarTransform(AplicacaoRecurso aplicacao, Transform transf)
        {
            RectTransform telaTransform = tela.GetComponent<RectTransform>();
            float deslocamentoX = telaTransform.sizeDelta.x / 2;
            float deslocamentoY = telaTransform.sizeDelta.y / 2;

            float x = aplicacao.posicao.x - deslocamentoX;
            float y = aplicacao.posicao.y - deslocamentoY;
            float largura = aplicacao.dimensoes.x;
            float altura = aplicacao.dimensoes.y;

            transf.SetParent(tela.transform);
            transf.localScale = Vector3.one;

            transf.localPosition = new Vector2(x, y);
            transf.GetComponent<RectTransform>().sizeDelta = new Vector2(largura, altura);
        }
    }
}
