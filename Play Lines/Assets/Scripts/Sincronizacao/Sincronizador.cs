using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Entidades;
using UnityEngine;

namespace Sincronizacao
{
    public class Sincronizador : MonoBehaviour
    {
        public static string userName = " ";
        public static string userEmail = " ";

        public static string arquivoHistorias = "historias.json";

        public static string arquivoBaloes = "baloes.json";

        public static string historiasStr = "";
        public static string baloesStr = "";

        public static int maxId = -1;

        public static List<Historia> historias = new List<Historia>();
        // public static Historias historiasArray { get { return new Historias(historias.ToArray()); } }

        //public static Baloes baloes = new Baloes();


        // public static Historia buscaOuCriaHistoria()
        // {
        //     carregarHistorias();
        //     if (historias.Count == 0)
        //         return new Historia();
        //     return historias[0];
        // }

        public static Historia[] minhasHistorias(bool recarregar = true)
        {
            if (recarregar || historiasStr == null || historiasStr == "")
                carregarHistorias();

            return historias.FindAll(delegate (Historia historia)
            {
                return historia.autor == Jogo.instancia.usuario;
            }).ToArray();
        }

        public static Historia[] historiasPorCategoria(Categoria categoria, bool recarregar = true)
        {
            if (recarregar || historiasStr == null || historiasStr == "")
                carregarHistorias();
            var historiasDaCategoria = historias;
            if (categoria.tags.Length > 0) ;
            historiasDaCategoria = historiasDaCategoria.FindAll(delegate (Historia historia)
            {
                return new List<string>(categoria.tags).TrueForAll(delegate (string tag)
                {
                    return new List<string>(historia.tags).Contains(tag);
                });
            });
            return historiasDaCategoria.ToArray();
        }

        public static Historia buscarHistoria(string id)
        {
            // if (recarregar || historiasStr == null || historiasStr == "")
            //     carregarHistorias();
            return historias.Find(historia => historia.id == id);
        }

        public static void buscarNoServidor()
        {
            Servidor.instancia.buscarHistorias();
        }

        public static void carregarHistorias(string histsStr = null)
        {
            if (histsStr == null)
                historiasStr = PlayerPrefs.GetString("historias");
            else
                historiasStr = histsStr;

            if (historiasStr == null || historiasStr == "")
            {
                historias = new List<Historia>();
                historiasStr = JsonUtility.ToJson(historias.ToArray());
                salvarHistorias();
                return;
            }

            Historias hist = JsonUtility.FromJson<Historias>(historiasStr);
            if (hist.historias == null)
                hist.historias = new Historia[0];
            historias = new List<Historia>(hist.historias);
        }

        public static void salvarHistorias(bool online = true, string histsStr = null)
        {
            if (histsStr != null)
                historiasStr = histsStr;
            var historiasArray = historias.ToArray();
            Historias hists = new Historias(historiasArray);
            historiasStr = JsonUtility.ToJson(hists);
            PlayerPrefs.SetString("historias", historiasStr);
            PlayerPrefs.Save();
            if (online)
                Jogo.instancia.salvarHistoriasOnline();
        }

        public static void salvarHistoria(Historia historia, bool persistir = false)
        {
            var historiaGravada = buscarHistoria(historia.id);
            if (historiaGravada == null)
                historias.Remove(historiaGravada);
            historias.Add(historia);
            if (persistir)
                salvarHistorias();
        }

        public static byte[] GetHash(string inputString)
        {
            HashAlgorithm algorithm = MD5.Create();  //or use SHA256.Create();
            return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }

        public static string GetHashString(string inputString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHash(inputString))
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }

        public static int getMaiorId()
        {
            int maior = 0;
            foreach (Trecho trecho in ComposicaoHistoria.historia.trechos)
            {
                int trechoId = trecho.ordem;
                if (trechoId > maior)
                {
                    maior = trechoId;
                }
            }
            return maior;
        }

        public static string getNovoId()
        {
            var maiorId = getMaiorId();
            return (++maiorId).ToString(); // GetHashString(userName) + GetHashString(userEmail) + ++maxId;
        }

        public static void salvarHistoriaOnline(Historia historia)
        {
            salvarHistoria(historia, true);
            Servidor.instancia.salvarHistorias(historiasStr);
        }

    }
}