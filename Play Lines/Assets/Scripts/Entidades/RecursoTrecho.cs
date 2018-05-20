using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using EnumsHistoria;

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
    }
}
