using System.Collections;
using System.Collections.Generic;
using Entidades;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class ItemHistoria : MonoBehaviour
    {

        public Historia historia;

        public Image thumbnail;
        public Text nome;
        public Text descricao;
        public Text autor;

        // Use this for initialization
        void Start()
        {
            carregarFuncaoBotao();
        }

        public void carregarFuncaoBotao()
        {
            Button botao = gameObject.GetComponent<Button>();
            ComposicaoHistoria.historia = historia;
            botao.onClick.AddListener(delegate ()
            {
                SceneManager.LoadScene("jogar");
            });
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void atualizaUI()
        {
            if (historia.thumbnail != null && historia.thumbnail != "")
                thumbnail.sprite = Recursos.imagens[historia.thumbnail];
            nome.text = historia.nome;
            descricao.text = historia.descricao;
            autor.text = historia.autor;
        }
    }
}
