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
	void Start () {
		tela = GameObject.FindObjectOfType<Canvas> ();
		atualizaArrayTrechos ();
		if (arrayTrechos.Length == 1) {
			arrayTrechos [0].toggle.isOn = true;
		}
		if (arrayTrechos.Length > 1) {
			foreach (var balao in arrayTrechos) {
				if (balao.tipo == TipoBalaoTrecho.INICIO) {
					balao.toggle.isOn = true;
					break;
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		atualizaArrayTrechos ();
	}

	public void selecionar(BalaoTrecho balaoTrecho){
		//Se estiver deselecionando remove das selecoes
		if (!balaoTrecho.toggle.isOn) {
			if (trechoSelecionado1 == balaoTrecho)
				trechoSelecionado1 = null;
			if (trechoSelecionado2 == balaoTrecho)
				trechoSelecionado2 = null;
		} else {
			//Se já tiver 2 selecionados, remove a seleção de ambos
			if (trechoSelecionado1 != null && trechoSelecionado2 != null) {
				trechoSelecionado1.toggle.isOn = false;
				trechoSelecionado2.toggle.isOn = false;

				trechoSelecionado1 = balaoTrecho;
				trechoSelecionado2 = null;
			} else {
				if (trechoSelecionado1 == null) {
					trechoSelecionado1 = balaoTrecho;
				}
				else if (trechoSelecionado1 != null && trechoSelecionado2 == null) {
					trechoSelecionado2 = balaoTrecho;
				}
			}
		}

		atualizaBaloes ();

	}

	public void atualizaBaloes(){
		foreach (BalaoTrecho balao in arrayTrechos) {
			balao.grupoBotoes.SetActive (balao.toggle.isOn);
		}
	}

	/// <summary>
	/// Cria um novo balão de trecho e caso ele não seja um início já o conecta com outro
	/// </summary>
	public void criarBalaoTrecho(){
		//Se não tiver nenhum balão cria um balão de início
		TipoBalaoTrecho tipoDoBalao = defineTipoBalaoNovo();

		BalaoTrecho novoBalao = Instantiate (prefabBalaoTrecho) as BalaoTrecho;
		novoBalao.tipo = tipoDoBalao;
		novoBalao.transform.SetParent (tela.transform);
		novoBalao.transform.localScale = Vector3.one;
		novoBalao.transform.localPosition = Vector3.zero;
		//selecionar (novoBalao);
		novoBalao.toggle.isOn = true;
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

	public void atualizaConexoes(){
		var conexoes = GameObject.FindObjectsOfType<Connection> ();
		foreach (var conexao in conexoes) {
			if((conexao.target[0] == null && conexao.target[1] == null) || (conexao.target[0] == conexao.target[1])){
				Destroy (conexao);
			}
		}
	}

	public void criaConexao(){
		if (trechoSelecionado1 != null && trechoSelecionado2 != null) {
			var conexoes = GameObject.FindObjectsOfType<Connection> ();
			Connection con = null;
			foreach (Connection conexao in conexoes) {
				BalaoTrecho balao1 = conexao.target [0].GetComponent<BalaoTrecho>();
				BalaoTrecho balao2 = conexao.target [1].GetComponent<BalaoTrecho>();
				//se ambos os baloes forem os trechos selecionados e um for diferente do outro, cria uma conexao
				if ((balao1 == trechoSelecionado1 || balao1 == trechoSelecionado2) && (balao2 == trechoSelecionado1 || balao2 == trechoSelecionado2)) {
					if (balao1 != balao2) {
						con = conexao;
						break;
					}
				}
			}
			if(con == null)
				con = Instantiate (prefabConexao) as Connection;
			con.SetTargets (trechoSelecionado1.GetComponent<RectTransform> (), trechoSelecionado2.GetComponent<RectTransform> ());
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
