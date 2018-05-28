using System.Collections;
using System.Collections.Generic;
using Sincronizacao;
using UI;
using UnityEngine;
using UnityEngine.UI;
using System;
using Entidades;

public class Jogo : MonoBehaviour
{
    public static Jogo instancia;

    public bool jaOcorreu = true;
    public string servidor;

    public string historiasTeste;
    public string usuario;

    // Use this for initialization
    void Awake()
    {
        buscarLogado();
        // DontDestroyOnLoad(this.gameObject);
        if (instancia == null)
            instancia = this;
    }

    public void menuPrincipalInit()
    {
        // testes();

        var primeira = PlayerPrefs.GetString("ja_ocorrido");
        jaOcorreu = primeira == "true";
        if (!jaOcorreu)
            primeiraExecucao();

        buscarHistoriasOnline();
    }

    public void logar(string usuarioLogado)
    {
        usuario = usuarioLogado;
        PlayerPrefs.SetString("usuario", usuario);
        PlayerPrefs.Save();
    }

    public void buscarLogado()
    {
        usuario = PlayerPrefs.GetString("usuario");
    }

    public void criarHistoria()
    {
        Historia historia = new Historia();
        historia.autor = usuario;
        ComposicaoHistoria.historia = historia;

    }

    public void testes()
    {
        PlayerPrefs.SetString("ja_ocorrido", "true");
        PlayerPrefs.SetString("historias", historiasTeste);
        PlayerPrefs.Save();
    }

    public void primeiraExecucao()
    {
        salvarHistoriasOnline();

        //Salva o parâmetro para não acontecer novamente
        PlayerPrefs.SetString("ja_ocorrido", "true");
        PlayerPrefs.Save();
    }

    public void salvarHistoriasOnline()
    {
        Sincronizador.carregarHistorias();

		var historias = Sincronizador.historiasStr;
        var request = criaRequest("salvar", "application/json", Sincronizador.historiasStr);
        StartCoroutine(salvarOnline(request));
    }

    public void buscarHistoriasOnline()
    {
        var request = criaRequest();
        StartCoroutine(buscaOnline(request));
    }

    IEnumerator buscaOnline(WWW www)
    {
        yield return new WaitUntil(() => www.isDone);
        PlayerPrefs.SetString("historias", www.text);
        PlayerPrefs.Save();
        atualizaListas();
    }

    IEnumerator salvarOnline(WWW www)
    {
        yield return new WaitUntil(() => www.isDone);
        Sincronizador.carregarHistorias(www.text);
        Sincronizador.salvarHistorias(false);
    }


    public void atualizaListas()
    {
        Sincronizador.carregarHistorias();
        var listas = GameObject.FindObjectsOfType<ListaHistorias>();
        foreach (var lista in listas)
        {
            lista.carregar();
        }
    }

    public WWW criaRequest(string acao = "", string tipoDeConteudo = null, string dadosPost = null)
    {
        Dictionary<string, string> headers = new Dictionary<string, string>();
        if (tipoDeConteudo != null)
            headers.Add("Content-Type", tipoDeConteudo);

        byte[] form = null;
        if (dadosPost != null)
            form = System.Text.Encoding.UTF8.GetBytes(dadosPost);

        var url = servidor + "/" + acao;

        if (form != null && headers.Keys.Count > 0)
            return new WWW(url, form, headers);

        return new WWW(url);
    }
}
