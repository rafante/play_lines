using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using EnumsHistoria;
using Padronizacao;

namespace Entidades
{
    [Serializable]
    public class AplicacaoRecurso
    {
        public Recurso recurso;
        public Vector2 posicao;
        public Vector2 dimensoes;
        public float duracao;
        public float inicio;
        public float fim;
        public bool loop;

        public AplicacaoRecurso(TipoRecursoTrecho tipo)
        {
            recurso = new Recurso(tipo);
            setPadroes(tipo);
        }

        private void setPadroes(TipoRecursoTrecho tipo)
        {
            switch (tipo)
            {
                case TipoRecursoTrecho.TEXTO:
                    posicao = Padroes.POSICAO_RECURSO_TEXTO;
                    dimensoes = Padroes.DIMENSOES_RECURSO_TEXTO;
                    duracao = 0;
                    inicio = 0;
                    fim = 0;
                    loop = false;
                    break;
                case TipoRecursoTrecho.IMAGEM:
                    break;
                case TipoRecursoTrecho.SOM:
                    break;
                case TipoRecursoTrecho.MUSICA:
                    break;
            }
        }
    }

}