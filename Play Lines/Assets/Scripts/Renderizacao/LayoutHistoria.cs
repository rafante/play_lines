using System;
using System.Collections;
using System.Collections.Generic;
using Entidades;
using EnumsHistoria;
using Sincronizacao;
using UnityEngine;
using UnityEngine.UI;

namespace Renderizacao
{
    public class LayoutHistoria : MonoBehaviour
    {

        public BalaoTrecho[] arrayTrechos { get { return GameObject.FindObjectsOfType<BalaoTrecho>(); } }
        public BalaoTrecho trechoSelecionado1;
        public BalaoTrecho trechoSelecionado2;
        public BalaoTrecho prefabBalaoTrecho;
        public Connection prefabConexao;
        public Color corTrecho1, corTrecho2, corNaoSelecionado;
        public Canvas tela;
        public ControladorCamera controladorCamera;
        public RectTransform telaTransform { get { return tela.GetComponent<RectTransform>(); } }
        public float larguraTela { get { return telaTransform.sizeDelta.x; } }
        public float alturaTela { get { return telaTransform.sizeDelta.y; } }
        public Vector2 inicioTela { get { return new Vector2(telaTransform.sizeDelta.x / 2, telaTransform.sizeDelta.y / 2); } }
        public InputField atualizadorTextos;
        public int id;

        public GameObject visualizadorDetalhes;
        public InputField nomeHistoria;
        public InputField descricaoHistoria;
        public InputField thumbnailHistoria;
        public InputField autorHistoria;
        public Image fundoPreto;

        public Historia historia;
        public bool detalhesAtivo;

        public Transform posicaoDetalhesAtivo, posicaoDetalhesInativo;

        // Use this for initialization
        void Start()
        {
            tela = GameObject.FindObjectOfType<Canvas>();
            controladorCamera = GameObject.FindObjectOfType<ControladorCamera>();
            carregarHistoria();
        }

        void OnGUI()
        {
            controlaCorBorda();
        }

        void controlaCorBorda()
        {
            int trecho1 = trechoSelecionado1 != null ? trechoSelecionado1.trecho.ordem : -1;
            int trecho2 = trechoSelecionado2 != null ? trechoSelecionado2.trecho.ordem : -1;

            foreach (BalaoTrecho balao in arrayTrechos)
            {
                if (trecho1 >= 0 && trecho1 == balao.trecho.ordem)
                    balao.borda.color = corTrecho1;
                else if (trecho2 >= 0 && trecho2 == balao.trecho.ordem)
                    balao.borda.color = corTrecho2;
                else
                    balao.borda.color = corNaoSelecionado;
            }
        }

        public void salvarHistoria()
        {
            atualizarEntidadesTrecho();
            Sincronizador.salvarHistoria(ComposicaoHistoria.historia, true);
        }

        public void carregarHistoria()
        {
            // historia = Sincronizador.carregarHistoria();
            ComposicaoHistoria.historia = historia;

            mostraDetalhes();
            montarDiagrama();
        }

        public void atualizarEntidadesTrecho()
        {
            var listaTrechos = new List<Trecho>();
            ComposicaoHistoria.historia = historia;
            foreach (BalaoTrecho balao in arrayTrechos)
            {
                balao.trecho.setResumo(balao.resumo.text);
                balao.trecho.representacao.posicao = new Vector2(balao.transform.localPosition.x, balao.transform.localPosition.y);

                var trecho = balao.trecho;
                listaTrechos.Add(trecho);
            }
            ComposicaoHistoria.historia.trechos = listaTrechos.ToArray();

            gravaDetalhes();
        }

        public void mostraDetalhes()
        {

            nomeHistoria.text = historia.nome;
            descricaoHistoria.text = historia.descricao;
            thumbnailHistoria.text = historia.thumbnail;
            autorHistoria.text = historia.autor;
        }

        public void gravaDetalhes()
        {
            //detalhes
            ComposicaoHistoria.historia.nome = nomeHistoria.textComponent.text;
            ComposicaoHistoria.historia.descricao = descricaoHistoria.textComponent.text;
            ComposicaoHistoria.historia.thumbnail = thumbnailHistoria.textComponent.text;
            ComposicaoHistoria.historia.autor = autorHistoria.textComponent.text;
        }

        public void montarDiagrama()
        {
            if (historia == null || historia.trechos == null)
                return;
            limpaDiagrama();
            for (int i = 0; i < historia.trechos.Length; i++)
            {
                var trecho = historia.trechos[i];
                BalaoTrecho balao = novoBalaoTrecho();
                balao.trecho = trecho;
                var transf = balao.GetComponent<RectTransform>();

                Vector2 novaPosicao = transf.localPosition;
                novaPosicao.x = trecho.representacao.posicao.x;
                novaPosicao.y = trecho.representacao.posicao.y;

                transf.localPosition = novaPosicao;
            }
            atualizaConexoes();
        }

        public void limpaDiagrama()
        {
            int baloesNumero = arrayTrechos.Length;
            for (int i = 0; i < baloesNumero; i++)
            {
                DestroyImmediate(arrayTrechos[i].gameObject);
            }
            ConnectionManager.CleanConnections();
        }

        public void atualizaTextosDoTrechoSelecionado()
        {
            trechoSelecionado1.trecho.setResumo(atualizadorTextos.text);
            trechoSelecionado1.resumo.text = atualizadorTextos.text;
        }

        public void alternarDetalhes()
        {
            detalhesAtivo = !detalhesAtivo;
            if (detalhesAtivo)
            {
                visualizadorDetalhes.transform.position = posicaoDetalhesAtivo.position;
                fundoPreto.enabled = true;
            }
            else
            {
                visualizadorDetalhes.transform.position = posicaoDetalhesInativo.position;
                fundoPreto.enabled = false;
            }
        }

        public bool apenasUmSelecionado()
        {
            return trechoSelecionado1 != null && trechoSelecionado2 == null;
        }

        public void criarBalaoTrecho()
        {
            BalaoTrecho balao = novoBalaoTrecho();
            var transf = balao.GetComponent<RectTransform>();
            Vector2 novaPosicao = transf.localPosition;

            var trechos = new List<Trecho>(historia.trechos);
            trechos.Add(balao.trecho);
            historia.trechos = trechos.ToArray();

            if (apenasUmSelecionado())
            {
                trechoSelecionado1.trecho.setPai(balao.trecho);
                novaPosicao.x = trechoSelecionado1.trecho.representacao.posicao.x;
                novaPosicao.y = trechoSelecionado1.trecho.representacao.posicao.y - balao.altura - 10;
            }
            else
            {
                balao.transform.localPosition = Vector3.zero;
                balao.transform.localScale = Vector3.one;
            }

            transf.localPosition = novaPosicao;
            alternarBalao(balao);
        }

        public void alternarBalao(BalaoTrecho balaoTrecho)
        {
            bool ehOtrecho1 = trechoSelecionado1 != null && trechoSelecionado1 == balaoTrecho;
            bool ehOtrecho2 = trechoSelecionado2 != null && trechoSelecionado2 == balaoTrecho;

            if (!ehOtrecho1 && !ehOtrecho2 && trechoSelecionado1 != null && trechoSelecionado2 != null)
            {
                trechoSelecionado1 = balaoTrecho;
                trechoSelecionado2 = null;
            }
            else if (ehOtrecho1)
            {
                trechoSelecionado1 = null;
            }
            else if (ehOtrecho2)
            {
                trechoSelecionado2 = null;
            }
            else if (trechoSelecionado1 == null)
            {
                trechoSelecionado1 = balaoTrecho;
            }
            else if (trechoSelecionado2 == null)
            {
                trechoSelecionado2 = balaoTrecho;
            }

            if (trechoSelecionado1 == null && trechoSelecionado2 != null)
            {
                trechoSelecionado1 = trechoSelecionado2;
                trechoSelecionado2 = null;
            }

            alternarAtualizador(trechoSelecionado1 != null && trechoSelecionado2 == null);
        }

        public void alternarAtualizador(bool ativar)
        {
            atualizadorTextos.textComponent.text = "";
            if (ativar)
                atualizadorTextos.textComponent.text = trechoSelecionado1.trecho.getResumoStr();
            atualizadorTextos.gameObject.SetActive(ativar);
        }

        public void apagarBaloes()
        {
            if (trechoSelecionado2 != null)
            {
                Destroy(trechoSelecionado2.gameObject);
                alternarBalao(trechoSelecionado2);
            }
            if (trechoSelecionado1 != null)
            {
                Destroy(trechoSelecionado1.gameObject);
                alternarBalao(trechoSelecionado1);
            }
        }

        public BalaoTrecho novoBalaoTrecho()
        {
            BalaoTrecho novoBalao = Instantiate(prefabBalaoTrecho) as BalaoTrecho;
            novoBalao.trecho = new Trecho(TipoTrecho.NORMAL);
            novoBalao.trecho.ordem = historia.trechos.Length + 1;
            novoBalao.transform.SetParent(tela.transform);
            novoBalao.transform.localScale = Vector3.one;
            novoBalao.transform.localPosition = Vector3.zero;
            return novoBalao;
        }

        public void atualizaConexoes()
        {
            limpaConexoes();
            foreach (var balao in arrayTrechos)
            {
                foreach (int pai in balao.trecho.pais)
                {
                    foreach (var bal in arrayTrechos)
                    {
                        if (bal.trecho.ordem == pai)
                        {
                            ConnectionManager.criaConexao(balao.gameObject, bal.gameObject);
                            break;
                        }
                    }
                }
            }
        }

        public void limpaConexoes()
        {
            Connection[] conexoes = GameObject.FindObjectsOfType<Connection>();
            int numero = conexoes.Length;
            for (int i = 0; i < numero; i++)
            {
                DestroyImmediate(conexoes[i]);
            }
        }

        public void desconectar(BalaoTrecho balaoPai, BalaoTrecho balaoFilho)
        {
            balaoFilho.removerPai(balaoPai);
            atualizaConexoes();
        }

        public void conectar(BalaoTrecho balaoPai, BalaoTrecho balaoFilho)
        {
            balaoFilho.adicionaPai(balaoPai);
            atualizaConexoes();
        }

        public void alternaConectado()
        {
            if (trechoSelecionado1 != null && trechoSelecionado2 != null)
            {
                bool relacionar = !trechoSelecionado1.ehPai(trechoSelecionado2);
                if (relacionar)
                {
                    trechoSelecionado1.adicionaPai(trechoSelecionado2);
                    var conexao = ConnectionManager.criaConexao(trechoSelecionado1.gameObject, trechoSelecionado2.gameObject);
                }
                else
                {
                    trechoSelecionado1.removerPai(trechoSelecionado2);
                    ConnectionManager.CleanConnections();
                }


            }
        }

        // public void selecionar(BalaoTrecho balaoTrecho)
        // {
        //     //Se estiver deselecionando remove das selecoes
        //     if (!balaoTrecho.toggle.isOn)
        //     {
        //         if (trechoSelecionado1 == balaoTrecho)
        //             trechoSelecionado1 = null;
        //         if (trechoSelecionado2 == balaoTrecho)
        //             trechoSelecionado2 = null;
        //     }
        //     else
        //     {
        //         //Se já tiver 2 selecionados, remove a seleção de ambos
        //         if (trechoSelecionado1 != null && trechoSelecionado2 != null)
        //         {
        //             // trechoSelecionado1.toggle.isOn = false;
        //             // trechoSelecionado2.toggle.isOn = false;

        //             trechoSelecionado1 = balaoTrecho;
        //             trechoSelecionado2 = null;
        //         }
        //         else
        //         {
        //             if (trechoSelecionado1 == null)
        //             {
        //                 trechoSelecionado1 = balaoTrecho;
        //             }
        //             else if (trechoSelecionado1 != null && trechoSelecionado2 == null)
        //             {
        //                 trechoSelecionado2 = balaoTrecho;
        //             }
        //         }
        //     }

        //     bool acionarAtualizadorTextos = false;
        //     if (trechoSelecionado1 != null && trechoSelecionado2 == null)
        //         acionarAtualizadorTextos = true;

        //     if (acionarAtualizadorTextos)
        //         atualizadorTextos.text = trechoSelecionado1.resumo.text;
        //     else
        //         atualizadorTextos.textComponent.text = "";
        //     atualizadorTextos.gameObject.SetActive(acionarAtualizadorTextos);

        //     if (trechoSelecionado1 == null && trechoSelecionado2 != null)
        //     {
        //         trechoSelecionado1 = trechoSelecionado2;
        //         trechoSelecionado2 = null;
        //     }
        // }

        // public void criaHistoriaExemplo()
        // {
        //     historia = ComposicaoHistoria.criarNovaHistoria();
        //     List<Trecho> trechos = new List<Trecho>();

        //     //Inicios
        //     trechos.Add(new Trecho(TipoTrecho.INICIO)); //0
        //     trechos.Add(new Trecho(TipoTrecho.INICIO)); //1
        //     trechos.Add(new Trecho(TipoTrecho.INICIO)); //2

        //     //Normais
        //     trechos.Add(new Trecho(TipoTrecho.NORMAL)); //3
        //     trechos.Add(new Trecho(TipoTrecho.NORMAL)); //4
        //     trechos.Add(new Trecho(TipoTrecho.NORMAL)); //5
        //     trechos.Add(new Trecho(TipoTrecho.NORMAL)); //6
        //     trechos.Add(new Trecho(TipoTrecho.NORMAL)); //7
        //     trechos.Add(new Trecho(TipoTrecho.NORMAL)); //8
        //     trechos.Add(new Trecho(TipoTrecho.NORMAL)); //9
        //     trechos.Add(new Trecho(TipoTrecho.NORMAL)); //10
        //     trechos.Add(new Trecho(TipoTrecho.NORMAL)); //11

        //     trechos[3].setPai(trechos[0]);
        //     trechos[4].setPai(trechos[3]);
        //     trechos[5].setPai(trechos[3]);
        //     trechos[6].setPai(trechos[3]);
        //     trechos[7].setPai(trechos[1]);
        //     trechos[8].setPai(trechos[2]);
        //     trechos[9].setPai(trechos[8]);
        //     trechos[10].setPai(trechos[9]);
        //     trechos[11].setPai(trechos[7]);

        //     historia.trechos = trechos.ToArray();
        // }

        // public void desenhaBaloes()
        // {
        //     var trechos = historia.trechos;
        //     if (trechos.Length == 0)
        //         return;

        //     Dictionary<int, List<Trecho>> trechosPorNivel = historia.trechosPorNivel();
        //     List<BalaoTrecho> baloes = new List<BalaoTrecho>();

        //     //itera os trechos de acordo com seus níveis
        //     foreach (var nivel in trechosPorNivel.Keys)
        //     {
        //         for (var i = 0; i < trechosPorNivel[nivel].Count; i++)
        //         {
        //             var trecho = trechosPorNivel[nivel][i];
        //             BalaoTrecho balao = criarBalaoTrecho(trecho);
        //             var transf = balao.GetComponent<RectTransform>();

        //             Vector2 novaPosicao = transf.localPosition;

        //             var divisaoX = larguraTela / trechosPorNivel[nivel].Count;
        //             var divisaoY = alturaTela / trechosPorNivel.Keys.Count;

        //             novaPosicao.x -= divisaoX * i - (larguraTela / 2);
        //             novaPosicao.y -= divisaoY * nivel - (alturaTela / 2);

        //             transf.localPosition = novaPosicao;
        //             baloes.Add(balao);
        //         }

        //     }

        //     foreach (var balao in baloes)
        //     {
        //         foreach (string pai in balao.trecho.pais)
        //         {
        //             foreach (var bal in baloes)
        //             {
        //                 if (bal.trecho.id == pai)
        //                 {
        //                     conectar(balao, bal);
        //                     ConnectionManager.criaConexao(balao.gameObject, bal.gameObject);
        //                     break;
        //                 }
        //             }
        //         }
        //     }

        //     controladorCamera.aplicarZoom(controladorCamera.zoomMinimo * trechosPorNivel.Keys.Count);
        // }
    }
}
