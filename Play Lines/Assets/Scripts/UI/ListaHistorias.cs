﻿using Entidades;
using Sincronizacao;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ListaHistorias : MonoBehaviour
    {

        public Categoria categoria;
        public Historia[] historias;
        public ItemHistoria itemHistoriaPrefab;
        public GameObject container;
        public TipoListaHistorias tipoLista;
        public Text tituloLista;

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        void Start()
        {
            carregar();
        }

        public void carregar()
        {
            if (tipoLista == TipoListaHistorias.CATEGORIA)
                historias = Sincronizador.historiasPorCategoria(categoria);
            else
                historias = Sincronizador.minhasHistorias();
            tituloLista.text = categoria.nome;
            foreach (Transform t in container.transform)
            {
                Destroy(t.gameObject);
            }
            foreach (var historia in historias)
            {
                var instancia = Instantiate<ItemHistoria>(itemHistoriaPrefab);
                instancia.historia = historia;
                instancia.atualizaUI();
                instancia.transform.SetParent(container.transform);
                instancia.transform.localScale = Vector3.one;
            }
        }
    }
}
public enum TipoListaHistorias
{
    CATEGORIA, MINHAS
}