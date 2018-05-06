using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayoutHistoria : MonoBehaviour {

	public BalaoTrecho[] arrayTrechos;
	public BalaoTrecho trechoSelecionado1;
	public BalaoTrecho trechoSelecionado2;
	public BalaoTrecho prefabBalaoTrecho;
	public Connection prefabConexao;
	public Canvas tela;

	// Use this for initialization
	void Awake () {
		tela = GameObject.FindObjectOfType<Canvas> ();
		atualizaArrayTrechos ();
	}
	
	// Update is called once per frame
	void Update () {
		atualizaArrayTrechos ();
	}

	public void deSelecionar(BalaoTrecho balaoTrecho){
		if (trechoSelecionado1 == balaoTrecho)
			trechoSelecionado1 = null;
		if (trechoSelecionado2 == balaoTrecho)
			trechoSelecionado2 = null;
	}

	public void selecionar(BalaoTrecho balaoTrecho){
		//Se já tiver 2 selecionados, remove a seleção de ambos
		if (trechoSelecionado1 != null && trechoSelecionado2 != null) {
			trechoSelecionado1 = balaoTrecho;
			trechoSelecionado2 = null;
		} else {
			if (trechoSelecionado1 == null) {
				trechoSelecionado1 = balaoTrecho;
				return;
			}
			if (trechoSelecionado1 != null && trechoSelecionado2 == null) {
				trechoSelecionado2 = balaoTrecho;
				return;
			}
		}
	}

	/// <summary>
	/// Cria um novo balão de trecho e caso ele não seja um início já o conecta com outro
	/// </summary>
	public void criarBalaoTrecho(){
		//Se não tiver nenhum balão cria um balão de início
		TipoBalaoTrecho tipoDoBalao = defineTipoBalaoNovo();

		BalaoTrecho novoBalao = Instantiate (prefabBalaoTrecho) as BalaoTrecho;
		novoBalao.transform.SetParent (tela.transform);
		novoBalao.transform.localScale = Vector3.one;
		novoBalao.transform.localPosition = Vector3.zero;
		selecionar (novoBalao);

		//Cria a conexão;
		if (tipoDoBalao != TipoBalaoTrecho.INICIO) {
			criaConexao ();
		}
	}

	public void iluminaCaminho(BalaoTrecho origem){
		
	}

	public Connection buscaConexao(BalaoTrecho balao){
		Connection[] conexoes = GameObject.FindObjectsOfType<Connection> ();
		foreach (Connection conexao in conexoes) {

		}
		return null;
	}

	public void criaConexao(){
		if (trechoSelecionado1 != null && trechoSelecionado2 != null) {
			Connection conexao = Instantiate (prefabConexao) as Connection;
			conexao.SetTargets (trechoSelecionado1.GetComponent<RectTransform> (), trechoSelecionado2.GetComponent<RectTransform> ());
		}
	}

	/// <summary>
	/// Define o tipo do balão ao criar um balão novo, verificando se ele
	/// vai ser por padrão um início, normal ou fim
	/// </summary>
	/// <returns>The tipo balao novo.</returns>
	public TipoBalaoTrecho defineTipoBalaoNovo(){
		TipoBalaoTrecho tipoDoBalao;

		if (arrayTrechos.Length == 0)
			tipoDoBalao = TipoBalaoTrecho.INICIO;
		else {
			//Verifica se já existe um balão de FIM
			bool existeAoMenosUmFim = false;
			foreach (BalaoTrecho balao in arrayTrechos) {
				if (balao.tipo == TipoBalaoTrecho.FIM) {
					existeAoMenosUmFim = true;
					break;
				}
			}
			if (existeAoMenosUmFim) {
				tipoDoBalao = TipoBalaoTrecho.NORMAL;
			} else {
				tipoDoBalao = TipoBalaoTrecho.FIM;
			}
		}
		return tipoDoBalao;
	}

	public void atualizaArrayTrechos(){
		arrayTrechos = GameObject.FindObjectsOfType<BalaoTrecho> ();
	}
}
