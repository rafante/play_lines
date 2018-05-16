using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemHistoria : MonoBehaviour {

	public Historia historia;

	public Sprite _thumbnailBuffer;
	public Image thumbnail;
	public Text nome;
	public Text descricao;
	public Text autor;

	// Use this for initialization
	void Start () {
		historia = Sincronizador.carregarHistoria();
		atualizaUI();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void atualizaUI(){
		thumbnail.sprite = _thumbnailBuffer;
		nome.text = historia.nome;
		descricao.text = historia.descricao;
		autor.text = historia.autor;
	}
}
