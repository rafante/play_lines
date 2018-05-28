using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControladorCenas : MonoBehaviour
{

    public static ControladorCenas instancia;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        if (instancia == null)
            instancia = this;
    }
	
    public void carregarCena(string nomeDaCena)
    {
        SceneManager.LoadScene(nomeDaCena);
    }
}
