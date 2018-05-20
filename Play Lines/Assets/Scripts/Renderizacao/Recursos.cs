using System.Collections;
using System.Collections.Generic;
using Entidades;
using Sincronizacao;
using UnityEngine;
using UnityEngine.UI;

public class Recursos : MonoBehaviour
{

    public static Dictionary<string, Sprite> imagens = new Dictionary<string, Sprite>();

    // Use this for initialization
    void Awake()
    {
        // criaHistoria();
        DontDestroyOnLoad(gameObject);
        carregarImagens();
    }

    public void criaHistoria(){
        Historia historia = new Historia();
        historia.nome = "Pânico na estrada";
        historia.descricao = "Uma família viajando a noite por uma estrada deserta e um estranho aparece no caminho.";
        historia.autor = "rafante2";
        historia.thumbnail = "estrada_floresta_noite";
        Sincronizador.salvarHistoria(historia, true);

        Historia historia2 = new Historia();
        historia2.nome = "Pânico na estrada 2";
        historia2.descricao = "Continuação: Uma família viajando a noite por uma estrada deserta e um estranho aparece no caminho.";
        historia2.autor = "rafante2";
        historia2.thumbnail = "timer";
        Sincronizador.salvarHistoria(historia2, true);
    }

    public void carregarImagens()
    {
        Sprite[] imagensLista = Resources.LoadAll<Sprite>("Imagens");
        foreach (var imagem in imagensLista)
        {
            if (!imagens.ContainsKey(imagem.name))
                imagens.Add(imagem.name, imagem);
            else
                imagens[imagem.name] = imagem;
        }
    }
}
