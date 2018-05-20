using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using EnumsHistoria;

namespace Entidades
{
    [Serializable]
    public class Trecho
    {

        public string id;
        public TipoTrecho tipoTrecho;
        public string[] pais;
        public BalaoRepresentacao representacao;
        public AplicacaoRecurso[] aplicacoesRecurso;

        public string getResumoStr()
        {
            return getResumo() != null ? getResumo().recurso.valor : "";
        }

        public AplicacaoRecurso getResumo()
        {
            foreach (AplicacaoRecurso aplicacao in aplicacoesRecurso)
            {
                if (aplicacao.recurso.tipoRecurso == TipoRecursoTrecho.TEXTO)
                    return aplicacao;
            }

            return null;
        }

        public void setResumo(string texto)
        {
            var resumo = getResumo();
            if (resumo == null)
                resumo = criaAplicacaoTexto(texto);
            else
                resumo.recurso.valor = texto;
        }

        public AplicacaoRecurso criaAplicacaoTexto(string texto)
        {
            Array.Resize(ref aplicacoesRecurso, aplicacoesRecurso.Length + 1);
            int posicao = aplicacoesRecurso.Length - 1;
            aplicacoesRecurso[posicao] = new AplicacaoRecurso(TipoRecursoTrecho.TEXTO);
            aplicacoesRecurso[posicao].recurso.valor = texto;
            return aplicacoesRecurso[posicao];
        }

        public Trecho(TipoTrecho tipo)
        {
            tipoTrecho = tipo;
            pais = new string[0];
            representacao = new BalaoRepresentacao();
            aplicacoesRecurso = new AplicacaoRecurso[0];
            id = (ComposicaoHistoria.historia.trechos.Length + 1).ToString();
            criaAplicacaoTexto("");
        }

        public int getNivel()
        {
            int nivel = 1;
            foreach (string pai in pais)
            {
                nivel += ComposicaoHistoria.trecho(pai).getNivel();
            }
            return nivel;
        }

        public void setPai(Trecho pai)
        {
            setPai(pai.id);
        }

        public void setPai(string pai)
        {
            //marca "pai" como pai de this
            var paisLista = new List<string>(pais);
            if (!paisLista.Contains(pai))
                paisLista.Add(pai);
            pais = paisLista.ToArray();

            //desmarca (se for o caso) this como pai de "pai"
            Trecho trechoPai = ComposicaoHistoria.trecho(pai);
            var novaLista = new List<string>(trechoPai.pais);
            if (novaLista.Contains(id))
            {
                novaLista.Remove(id);
                trechoPai.pais = novaLista.ToArray();
            }
        }

        public void removePai(string pai)
        {
            var paisLista = new List<string>(pais);
            if (paisLista.Contains(pai))
                paisLista.Remove(pai);
            pais = paisLista.ToArray();
        }
    }
}
