using System;
using UnityEngine;

namespace Entidades
{
    public class RepresentacaoRecurso
    {
        public Vector2 posicao;
        public Vector2 dimensoes;

        public RepresentacaoRecurso() : this(Vector2.zero, Vector2.zero)
        {

        }

        public RepresentacaoRecurso(Vector2 posicao, Vector2 dimensoes)
        {
            this.posicao = posicao;
            this.dimensoes = dimensoes;
        }
    }
}

