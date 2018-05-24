using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using EnumsHistoria;

namespace Entidades
{
    [Serializable]
    public class Historia
    {

        public string nome;
        public string descricao;
        public string thumbnail;
        public string autor;
        public Classificacao classificacao;
        public Tag[] tags;
        public Idioma[] idiomas = new Idioma[0];
        public Trecho[] trechos = new Trecho[0];

        public string getId()
        {
            return nome + ":" + autor;
        }

        public List<Trecho> filhosDoTrecho(Trecho trecho){
            List<Trecho> filhos = new List<Trecho>();
            foreach(var possivelFilho in trechos){
                if(possivelFilho.ordem == trecho.ordem)
                continue;
                foreach(var pai in possivelFilho.pais){
                    if(pai == trecho.ordem){
                        filhos.Add(possivelFilho);
                        break;
                    }
                }
            }
            return filhos;
        }

        public Trecho getInicio()
        {
            foreach(Trecho trecho in trechos){
                if(paisDoTrecho(trecho).Count == 0)
                    return trecho;
            }
            return trechos[0];
        }

        public List<Trecho> paisDoTrecho(Trecho trecho){
            var pais = new List<Trecho>();
            foreach(var pai in trecho.pais){
                pais.Add(trechoPelaOrdem(pai));
            }
            return pais;
        }

        public Trecho trechoPelaOrdem(int ordem){
            foreach(var trecho in trechos){
                if(trecho.ordem == ordem)
                    return trecho;
            }
            return null;
        }

        private Trecho[] _iniciosBuffer, _finsBuffer, _normaisBuffer, _quebrasBuffer;

        // public bool testarHistoria()
        // {
        //     atualizaBuffers();
        //     return temAoMenosUmInicio() && temAoMenosUmFim(); // && trechosLevamAUmFim();
        // }

        public bool ehValida()
        {
            bool valido = false;

            valido &= nome != null && nome != "";
            valido &= descricao != null && descricao != "";
            return valido;
        }

        public Dictionary<int, List<Trecho>> trechosPorNivel()
        {
            Dictionary<int, List<Trecho>> trechosPorNivel = new Dictionary<int, List<Trecho>>();
            foreach (Trecho trecho in trechos)
            {
                var nivel = trecho.getNivel();
                if (!trechosPorNivel.ContainsKey(nivel))
                {
                    trechosPorNivel.Add(nivel, new List<Trecho>());
                }
                trechosPorNivel[nivel].Add(trecho);
            }
            return trechosPorNivel;
        }

    }
}
