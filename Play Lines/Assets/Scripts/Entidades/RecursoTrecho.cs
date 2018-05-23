using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using EnumsHistoria;
using UnityEngine.UI;

namespace Entidades
{
    [Serializable]
    public class Recurso
    {
        public TipoRecursoTrecho tipoRecurso;
        public string valor;

        public Recurso(TipoRecursoTrecho tipo)
        {
            tipoRecurso = tipo;
        }

        public Image criaImagem()
        {
            Image imagem = new GameObject(valor, typeof(Image)).GetComponent<Image>();
            imagem.sprite = Recursos.imagens[valor];
            return imagem;
        }

        public Text criaTexto()
        {
            Text texto = new GameObject("texto", typeof(Text)).GetComponent<Text>();
            texto.text = valor;
            return texto;
        }

        public AudioSource criaSom()
        {
            AudioSource fonteSonora = new GameObject("tocadorSom", typeof(AudioSource)).GetComponent<AudioSource>();
            fonteSonora.clip = Recursos.sons[valor];
            return fonteSonora;
        }
    }
}
