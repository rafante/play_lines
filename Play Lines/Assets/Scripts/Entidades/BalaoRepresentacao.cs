using System;
using UnityEngine;

[Serializable]
public class BalaoRepresentacao
{
    public Vector2 posicao;
    public Vector2 dimensoes;

    public BalaoRepresentacao() : this(Vector2.zero, Vector2.zero)
    {

    }

    public BalaoRepresentacao(Vector2 posicao, Vector2 dimensoes)
    {
        this.posicao = posicao;
        this.dimensoes = dimensoes;
    }
}