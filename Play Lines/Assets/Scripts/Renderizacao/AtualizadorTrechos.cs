using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Renderizacao
{
    public class AtualizadorTrechos : MonoBehaviour
    {

        public InputField input;
        // Use this for initialization
        public LayoutHistoria layout;

        void Start()
        {
            layout = GameObject.FindObjectOfType<LayoutHistoria>();
        }

        // Update is called once per frame
        void Update()
        {
            var pos = GetComponent<RectTransform>().localPosition;
            pos.z = 0;
            GetComponent<RectTransform>().localPosition = pos;
        }

    }
}
