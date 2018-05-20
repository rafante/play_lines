using System;
using UnityEngine;

namespace Entidades
{
    public class ComposicaoHistoria
    {
        public static Historia historia;
        public static Trecho[] trechos { get { return historia.trechos; } }

        public static Historia criarNovaHistoria()
        {
            historia = new Historia();
            return historia;
        }

        public static Trecho trecho(string id)
        {
            foreach (Trecho trecho in trechos)
            {
                if (trecho.id.Equals(id))
                    return trecho;
            }
            return null;
        }
    }
}