using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class Sincronizador
{
    public static string userName = " ";
    public static string userEmail = " ";

    public static string arquivoHistorias = "historias.json";
    public static string arquivoBaloes = "baloes.json";

    public static string historiasStr = "";
    public static string baloesStr = "";

    public static int maxId = -1;

    public static Historias historias = new Historias();
    public static Baloes baloes = new Baloes();

    public static void salvar()
    {
        baloesStr = JsonUtility.ToJson(baloes);
        historiasStr = JsonUtility.ToJson(historias);
        string path = Application.dataPath + "/Resources/" + arquivoHistorias;
        File.WriteAllText(path, historiasStr);
    }

    public static void carregar()
    {
        // historiasStr = Resources.Load<TextAsset>(arquivoHistorias.Replace(".json", "")).text;
        historiasStr = PlayerPrefs.GetString("historia");
        Debug.Log(historiasStr);
        historias = JsonUtility.FromJson<Historias>(historiasStr);
        Debug.Log(historias);
        //baloes = JsonUtility.FromJson<Baloes>(arquivoBaloes);

    }

    public static Historia carregarHistoria()
    {
        carregar();
        Historia historia = JsonUtility.FromJson<Historia>(historiasStr);
        // var histStr = Resources.Load<TextAsset>(arquivoHistorias.Replace(".json", "")).text;
        // Historia historia = JsonUtility.FromJson<Historia>(histStr);
        return historia;
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

    public static int getMaiorId(){
        int maior = 0;
        foreach(Trecho trecho in ComposicaoHistoria.historia.trechos){
            int trechoId = int.Parse(trecho.id);
            if(trechoId > maior){
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

    public static void salvarHistoria()
    {
        Historia historia = ComposicaoHistoria.historia;
        string historiasStr = JsonUtility.ToJson(historia);
        PlayerPrefs.SetString("historia", historiasStr);
        PlayerPrefs.Save();
        // string path = Application.dataPath + "/Resources/" + arquivoHistorias;
        // File.WriteAllText(path, historiasStr);
    }
}