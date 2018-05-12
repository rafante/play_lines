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

    public static int maxId = 0;

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
        historiasStr = Resources.Load<TextAsset>(arquivoHistorias.Replace(".json", "")).text;
        Debug.Log(historiasStr);
        historias = JsonUtility.FromJson<Historias>(historiasStr);
        Debug.Log(historias);
        //baloes = JsonUtility.FromJson<Baloes>(arquivoBaloes);

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

    public static string getNovoId()
    {
        return GetHashString(userName) + GetHashString(userEmail) + ++maxId;
    }

    public static void salvarDiagrama(LayoutHistoria layout)
    {
        Connection[] conexoes = GameObject.FindObjectsOfType<Connection>();
        Diagrama diagrama = new Diagrama();
        diagrama.conexoes = new Conexao[conexoes.Length];
        for (int i = 0; i < conexoes.Length; i++)
        {
            var con = new Conexao();
            con.balao1 = conexoes[i].target[0].gameObject.GetComponent<BalaoTrecho>().balaoId;
            con.balao2 = conexoes[i].target[1].gameObject.GetComponent<BalaoTrecho>().balaoId;
            diagrama.conexoes[i] = con;
        }
        historias.diagramas = new Diagrama[1];
        historias.diagramas[0] = diagrama;
        salvar();
    }
}