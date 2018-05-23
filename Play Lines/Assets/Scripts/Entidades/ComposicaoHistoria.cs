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

        public static Trecho trecho(int ordem)
        {
            foreach (Trecho trecho in trechos)
            {
                if (trecho.ordem == ordem)
                    return trecho;
            }
            return null;
        }
    }
}