using System;
using System.Collections;
using System.Collections.Generic;
using Sincronizacao;
using UnityEngine;

public class Servidor : MonoBehaviour
{

    private static Servidor _instancia;
    public static Servidor instancia
    {
        get
        {
            if (_instancia == null)
                _instancia = new GameObject("servidor", typeof(Servidor)).GetComponent<Servidor>();
            return _instancia;
        }
    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        if (_instancia == null)
            _instancia = this;
    }

    public void salvarHistorias(string historiasJson, Action promessa = null)
    {
        if (historiasJson == null)
            historiasJson = Sincronizador.historiasStr;
        Dictionary<string, string> postHeader = new Dictionary<string, string>();
        postHeader.Add("Content-Type", "application/json");
        var form = System.Text.Encoding.UTF8.GetBytes(historiasJson);
        // WWW www = new WWW("http://herokuapps.play-lines.com/salvar", form);
        WWW www = new WWW("http://localhost:3000/salvar", form, postHeader);
        StartCoroutine(EsperarPorRequest(www, promessa));
    }

    public void buscarHistorias(Action promessa = null)
    {
        WWW www = new WWW("http://localhost:3000/");
        StartCoroutine(EsperarPorRequest(www, promessa)); 
    }

    IEnumerator EsperarPorRequest(WWW dados, Action promessa)
    {
        yield return new WaitUntil(() => dados.isDone);
        if (dados.error != null)
        {
            Debug.Log(dados.error);
        }
        else
        {
            PlayerPrefs.SetString("historias", dados.text);
            if (promessa != null)
                promessa.Invoke();
        }
    }
}