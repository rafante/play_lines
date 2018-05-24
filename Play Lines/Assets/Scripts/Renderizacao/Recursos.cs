using System.Collections;
using System.Collections.Generic;
using Entidades;
using EnumsHistoria;
using Sincronizacao;
using UnityEngine;
using UnityEngine.UI;

public class Recursos : MonoBehaviour
{

    public static Dictionary<string, Sprite> imagens = new Dictionary<string, Sprite>();
    public static Dictionary<string, AudioClip> sons = new Dictionary<string, AudioClip>();
    public static Dictionary<string, Font> fontes = new Dictionary<string, Font>();


    // Use this for initialization
    void Awake()
    {        
        // PlayerPrefs.SetString("historias","");
        // PlayerPrefs.Save();
        // DontDestroyOnLoad(gameObject);
        carregarImagens();
        carregarSons();
        carregarFontes();
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

    public void carregarSons()
    {
        AudioClip[] sonsLista = Resources.LoadAll<AudioClip>("Sons");
        foreach (var som in sonsLista)
        {
            if (!sons.ContainsKey(som.name))
                sons.Add(som.name, som);
            else
                sons[som.name] = som;
        }
    }

    public void carregarFontes(){
        Font[] fontesList = Resources.LoadAll<Font>("Fontes");
        foreach(var fonte in fontesList){
            if(!fontes.ContainsKey(fonte.name))
                fontes.Add(fonte.name, fonte);
            else
                fontes[fonte.name] = fonte;
        }
    }
}
