using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Historia
{

    public string nome;
    public string descricao;
    public Classificacao classificacao;
    public Tag[] tags;
    public Idioma[] idiomas;
    public Trecho[] trechos;

    private Trecho[] _iniciosBuffer, _finsBuffer, _normaisBuffer, _quebrasBuffer;

    // public bool testarHistoria()
    // {
    //     atualizaBuffers();
    //     return temAoMenosUmInicio() && temAoMenosUmFim(); // && trechosLevamAUmFim();
    // }

    public bool ehValida(){
        bool valido = false;

        valido &= nome != null && nome != "";
        valido &= descricao != null && descricao != "";
        return valido;
    }

    public Dictionary<int,List<Trecho>> trechosPorNivel(){
        Dictionary<int,List<Trecho>> trechosPorNivel = new Dictionary<int, List<Trecho>>();
        foreach(Trecho trecho in trechos){
            var nivel = trecho.getNivel();
            if(!trechosPorNivel.ContainsKey(nivel)){
                trechosPorNivel.Add(nivel, new List<Trecho>());
            }
            trechosPorNivel[nivel].Add(trecho);
        }
        return trechosPorNivel;
    }

    public Trecho[] getInicios()
    {
        atualizaBuffers();
        return _iniciosBuffer;
    }

    public Trecho[] getNomais()
    {
        atualizaBuffers();
        return _normaisBuffer;
    }

    public Trecho[] getFins()
    {
        atualizaBuffers();
        return _finsBuffer;
    }

    public Trecho[] getQuebras()
    {
        atualizaBuffers();
        return _quebrasBuffer;
    }

    public void atualizaBuffers()
    {
        limpaBuffers();

        foreach (Trecho trecho in trechos)
        {
            switch (trecho.tipoTrecho)
            {
                case TipoTrecho.INICIO:
                    Array.Resize<Trecho>(ref _iniciosBuffer, _iniciosBuffer.Length + 1);
                    _iniciosBuffer[_iniciosBuffer.Length - 1] = trecho;
                    break;
                case TipoTrecho.FIM:
                    Array.Resize<Trecho>(ref _finsBuffer, _finsBuffer.Length + 1);
                    _finsBuffer[_finsBuffer.Length - 1] = trecho;
                    break;
                case TipoTrecho.NORMAL:
                    Array.Resize<Trecho>(ref _normaisBuffer, _normaisBuffer.Length + 1);
                    _normaisBuffer[_normaisBuffer.Length - 1] = trecho;
                    break;
                case TipoTrecho.QUEBRA:
                    Array.Resize<Trecho>(ref _quebrasBuffer, _quebrasBuffer.Length + 1);
                    _quebrasBuffer[_quebrasBuffer.Length - 1] = trecho;
                    break;
            }
        }
    }

    public void limpaBuffers()
    {
        _iniciosBuffer = _finsBuffer = _normaisBuffer = _quebrasBuffer = null;
        _iniciosBuffer = _finsBuffer = _normaisBuffer = _quebrasBuffer = new Trecho[0];
    }

    public bool temAoMenosUmInicio()
    {
        return _iniciosBuffer.Length > 0;
    }

    public bool temAoMenosUmFim()
    {
        return _finsBuffer.Length > 0;
    }

    // public bool trechosLevamAUmFim()
    // {
    //     foreach (Trecho trecho in trechos)
    //     {
    //         if (!trechoLevaAUmFim(trecho))
    //             return false;
    //     }
    //     return true;
    // }

    // public bool trechoLevaAUmFim(Trecho trecho)
    // {

    //     bool levaAUmFim = true;

    //     //Se o trecho é um fim retorna true;
    //     if (trechoEUmFim(trecho))
    //         return true;

    //     //Se só tem um filho e ele é um fim, retorna verdadeiro
    //     if (trecho.filhos.Length == 1 && trechoEUmFim(trecho.filhos[0]))
    //         return true;

    //     //Se não tem nenhum filho e não é um fim ou quebra, retorna falso
    //     if (trecho.filhos.Length == 0)
    //         return false;

    //     //Se o trecho não leva diretamente a um fim ou é um fim,
    //     //checa os filhos recursivamente
    //     foreach (Trecho filho in trecho.filhos)
    //     {
    //         levaAUmFim &= trechoLevaAUmFim(filho);
    //     }

    //     return levaAUmFim;
    // }

    //Retorna verdadeiro se o trecho é um fim (FIM ou QUEBRA)
    public bool trechoEUmFim(Trecho trecho)
    {
        return trecho.tipoTrecho == TipoTrecho.FIM || trecho.tipoTrecho == TipoTrecho.QUEBRA;
    }

}
