using System;
using UnityEngine;

namespace Padronizacao
{
    public sealed class Padroes
    {
        public static Vector2 POSICAO_RECURSO_TEXTO = new Vector2(-150, -80);
        public static Vector2 POSICAO_RECURSO_IMAGEM = new Vector2(0, 0);
        public static Vector2 DIMENSOES_RECURSO_TEXTO = new Vector2(300, 160);
        public static Vector2 DIMENSOES_RECURSO_IMAGEM = new Vector2(300, 160);
        public static float posTextX { get { return POSICAO_RECURSO_TEXTO.x; } }
        public static float posTextY { get { return POSICAO_RECURSO_TEXTO.y; } }
        public static float dimTextX { get { return DIMENSOES_RECURSO_TEXTO.x; } }
        public static float dimTextY { get { return DIMENSOES_RECURSO_TEXTO.y; } }
        public static string FONTE_RECURSO_TEXTO = "Arial";
        public static string COR_RECURSO_TEXTO = "000000";
        public static string COR_RECURSO_IMAGEM = "000000";
    }
}