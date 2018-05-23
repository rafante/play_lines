using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using EnumsHistoria;

namespace Entidades
{
    [Serializable]
    public class Trecho
    {
        public int ordem;
        public string textoCondicao;
        public TipoTrecho tipoTrecho;
        public int[] pais;
        public BalaoRepresentacao representacao;
        public AplicacaoRecurso[] aplicacoesRecurso;

        public Trecho(TipoTrecho tipo = TipoTrecho.NORMAL, int ordem = -1, string texto = "")
        {
            tipoTrecho = tipo;
            pais = new int[0];
            representacao = new BalaoRepresentacao();
            aplicacoesRecurso = new AplicacaoRecurso[0];
            if (ordem >= 0)
                this.ordem = ordem;
            else
                ordem = (ComposicaoHistoria.historia.trechos.Length + 1);
            criaAplicacaoTexto(texto);
        }

        public void adicionaAplicacaoTexto(string texto)
        {
            var aplicacaoTexto = new AplicacaoRecurso(TipoRecursoTrecho.TEXTO);
            aplicacaoTexto.recurso.valor = texto;
            adicionaAplicacao(aplicacaoTexto);
        }

        public void adicionaAplicacao(AplicacaoRecurso aplicacao)
        {
            var aplicacoes = new List<AplicacaoRecurso>(aplicacoesRecurso);
            aplicacoes.Add(aplicacao);
            aplicacoesRecurso = aplicacoes.ToArray();
        }

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


        public int getNivel()
        {
            int nivel = 1;
            foreach (int pai in pais)
            {
                nivel += ComposicaoHistoria.trecho(pai).getNivel();
            }
            return nivel;
        }

        public void setPai(Trecho pai)
        {
            setPai(pai.ordem);
        }

        public void setPai(int pai)
        {
            //marca "pai" como pai de this
            var paisLista = new List<int>(pais);
            if (!paisLista.Contains(pai))
                paisLista.Add(pai);
            pais = paisLista.ToArray();

            //desmarca (se for o caso) this como pai de "pai"
            // Trecho trechoPai = ComposicaoHistoria.trecho(pai);
            // var novaLista = new List<int>(trechoPai.pais);
            // if (novaLista.Contains(ordem))
            // {
            //     novaLista.Remove(ordem);
            //     trechoPai.pais = novaLista.ToArray();
            // }
        }

        public void removePai(int pai)
        {
            var paisLista = new List<int>(pais);
            if (paisLista.Contains(pai))
                paisLista.Remove(pai);
            pais = paisLista.ToArray();
        }


        public List<AplicacaoRecurso> getTextos()
        {
            var textos = new List<AplicacaoRecurso>();
            foreach (var aplicacao in aplicacoesRecurso)
            {
                if (aplicacao.recurso.tipoRecurso == TipoRecursoTrecho.TEXTO)
                {
                    textos.Add(aplicacao);
                }
            }
            return textos;
        }

        public List<AplicacaoRecurso> getImagens()
        {
            var imagens = new List<AplicacaoRecurso>();
            foreach (var aplicacao in aplicacoesRecurso)
            {
                if (aplicacao.recurso.tipoRecurso == TipoRecursoTrecho.IMAGEM)
                {
                    imagens.Add(aplicacao);
                }
            }
            return imagens;
        }

        public Image getFundo()
        {
            Image fundo = null;
            foreach (var aplicacao in aplicacoesRecurso)
            {
                if (aplicacao.recurso.tipoRecurso == TipoRecursoTrecho.FUNDO)
                {
                    fundo = aplicacao.recurso.criaImagem();
                    break;
                }
            }
            return fundo;
        }

        public List<AplicacaoRecurso> getSons()
        {
            var sons = new List<AplicacaoRecurso>();
            foreach (var aplicacao in aplicacoesRecurso)
            {
                if (aplicacao.recurso.tipoRecurso == TipoRecursoTrecho.SOM)
                {
                    sons.Add(aplicacao);
                }
            }
            return sons;
        }

        public List<AplicacaoRecurso> getMusica()
        {
            var musicas = new List<AplicacaoRecurso>();
            foreach (var aplicacao in aplicacoesRecurso)
            {
                if (aplicacao.recurso.tipoRecurso == TipoRecursoTrecho.MUSICA)
                {
                    musicas.Add(aplicacao);
                }
            }
            return musicas;
        }
    }
}
