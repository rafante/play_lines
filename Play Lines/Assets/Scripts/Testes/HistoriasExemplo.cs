using System.Collections;
using System.Collections.Generic;
using Entidades;
using EnumsHistoria;
using Sincronizacao;
using UnityEngine;

public class HistoriasExemplo : MonoBehaviour {

	public Historia historia;

	// Use this for initialization
	void Awake () {
		historia = historiaDeExemplo();
		ComposicaoHistoria.historia = historia;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public Historia historiaDeExemplo(){
		var historia = new Historia();
        historia.nome = "Pânico na estrada";
        historia.descricao = "Uma família viajando a noite em uma estrada no meio da floresta, encontra um estranho";
        historia.autor = "rafante2";
		historia.thumbnail = "estrada_floresta_noite";

		var texto = "";

		texto = "Onde estou? Quanto sangue! Meu Deus! Lora! Dane! Cadê vocês!?";
        var inicio = new Trecho(TipoTrecho.NORMAL, 0, texto);

		texto = "Elas não estão no carro. Me lembro de ter desviado de um homem na pista.";
		var t1 = new Trecho(TipoTrecho.NORMAL, 1, texto);

		texto = "Acho que o carro acabou saindo da estrada e batido nessa árvore";
		var t2 = new Trecho(TipoTrecho.NORMAL, 2, texto);

		texto = "Onde elas podem estar? Não sei se procuro na floresta ou na estrada";
		var t3 = new Trecho(TipoTrecho.NORMAL, 3, texto);

		texto = "Esta floresta é muito densa, não tem como elas terem vindo por aqui. Preciso voltar";
		var t4 = new Trecho(TipoTrecho.NORMAL, 4, texto);
		t4.textoCondicao = "Ir pela floresta";

		texto = "Uma hora andando nesta estrada e até agora nem uma viva alma";
		var t5 = new Trecho(TipoTrecho.NORMAL, 5, texto);
		t5.textoCondicao = "Ir pela estrada";

		t1.setPai(inicio);
		t2.setPai(t1);
		t3.setPai(t2);
		t4.setPai(t3);
		t5.setPai(t3);

		var trechos = new Trecho[6];
		trechos[0] = inicio;
		trechos[1] = t1;
		trechos[2] = t2;
		trechos[3] = t3;
		trechos[4] = t4;
		trechos[5] = t5;

		historia.trechos = trechos;
        Sincronizador.salvarHistoria(historia, true);
		return historia;
	}
}
