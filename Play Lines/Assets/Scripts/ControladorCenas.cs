using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControladorCenas : MonoBehaviour {

	public void carregarCena(string nomeDaCena){
		SceneManager.LoadScene (nomeDaCena);
	}
}
