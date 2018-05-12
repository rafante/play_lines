using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LayoutHistoria : MonoBehaviour {

	public BalaoTrecho[] arrayTrechos;
	public BalaoTrecho trechoSelecionado1;
	public BalaoTrecho trechoSelecionado2;
	public BalaoTrecho prefabBalaoTrecho;
	public Connection prefabConexao;
	public Canvas tela;
	public InputField atualizadorTextos;
	public int id;

	public Historias historias;

	// Use this for initialization
	void Start () {
		tela = GameObject.FindObjectOfType<Canvas> ();
		Sincronizador.carregar();
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
		historias = Sincronizador.historias;
	}

	public void salvarDiagrama(){
		Sincronizador.salvarDiagrama(this);
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
				// trechoSelecionado1.toggle.isOn = false;
				// trechoSelecionado2.toggle.isOn = false;

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

		bool acionarAtualizadorTextos = false;
		if(trechoSelecionado1 != null && trechoSelecionado2 == null)
			acionarAtualizadorTextos = true;

		if(acionarAtualizadorTextos)
			atualizadorTextos.text = trechoSelecionado1.resumo.text;
		else
			atualizadorTextos.textComponent.text = "";
		atualizadorTextos.gameObject.SetActive(acionarAtualizadorTextos);

		if(trechoSelecionado1 == null && trechoSelecionado2 != null){
			trechoSelecionado1 = trechoSelecionado2;
			trechoSelecionado2 = null;
		}
	}

	public void atualizaTextosDoTrechoSelecionado(){
		trechoSelecionado1.resumo.text = atualizadorTextos.text;
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
			criaConexao();
		}
	}

	public void deletarBalaoTrecho(){
		if(trechoSelecionado1 != null){
			desconectar(trechoSelecionado1, TipoConexao.FILHO, TipoConexao.PAI);
		}
		if(trechoSelecionado2 != null){
			desconectar(trechoSelecionado2, TipoConexao.FILHO, TipoConexao.PAI);
		}
	}

	public void desconectar(BalaoTrecho balaoTrecho, params TipoConexao[] tipos){
		foreach(TipoConexao tipo in tipos){
			Connection conexao = buscaConexao(balaoTrecho, tipo);
			
			Destroy(conexao);
		}
	}

	public void iluminaCaminho(BalaoTrecho origem){
		
	}

	public Connection buscaConexao(BalaoTrecho balao, TipoConexao tipoConexao){
		var conexoes = ConnectionManager.FindConnections(balao.gameObject);
		foreach(Connection conexao in conexoes){
			
		}
		return null;
		// Connection[] conexoes = GameObject.FindObjectsOfType<Connection> ();
		// foreach (Connection conexao in conexoes) {
		// 	bool temTrechoSelecionado1 = conexao.target[0] != null && conexao.target[0] == balao.GetComponent<RectTransform>();
		// 	bool temTrechoSelecionado2 = conexao.target[1] != null && conexao.target[1] == balao.GetComponent<RectTransform>();
		// 	if(tipoConexao == TipoConexao.PAI && temTrechoSelecionado2)
		// 		return conexao;
		// 	if(tipoConexao == TipoConexao.FILHO && temTrechoSelecionado1)
		// 		return conexao;
		// }
		// return null;
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
		if(trechoSelecionado1 != null && trechoSelecionado2 != null){
			ConnectionManager.CreateConnection(trechoSelecionado1.gameObject, trechoSelecionado2.gameObject);
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
