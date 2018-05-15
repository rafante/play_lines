using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControladorCamera : MonoBehaviour
{

    public float perspectiveZoomSpeed = 0.5f;        // The rate of change of the field of view in perspective mode.
    public float orthoZoomSpeed = 0.5f;        // The rate of change of the orthographic size in orthographic mode.
    private Camera cam { get { return Camera.main; } }

    public float horizontalSpeed = 2.0F;
    public float verticalSpeed = 2.0F;

    public float zoomMaximo;
    public float zoomMinimo;

    public static bool ativo = true;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!ativo)
            return;

        // Se o jogador colocou dois dedos na tela ao mesmo tempo
        if (Input.touchCount == 2)
        {
            // Guarda informação do toque de cada um dos dedos.
            Touch touchZero = Input.GetTouch(0); // dedo 1
            Touch touchOne = Input.GetTouch(1); // dedo 2


            // Find the position in the previous frame of each touch.
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            // Find the magnitude of the vector (the distance) between the touches in each frame.
            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            // Find the difference in the distances between each frame.
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            aplicarZoom(deltaMagnitudeDiff);
        }
        else if (Input.touchCount == 1)
        { //Se o jogador colocou um dedo na tela
            var h = -Input.GetTouch(0).deltaPosition.x + transform.position.x;
            var v = -Input.GetTouch(0).deltaPosition.y + transform.position.y;

            transform.position = new Vector3(h, v, -10);

        }
        else if (Input.GetMouseButton(0))
        { //Se estiver rodando mouse e tiver clicado com o botão esquerdo
            float h = horizontalSpeed * Input.GetAxis("Mouse X");
            float v = verticalSpeed * Input.GetAxis("Mouse Y");
            float x = transform.position.x;
            float y = transform.position.y;

            transform.Translate(h, v, 0);
        }

    }

    // public void verTodosOsObjetos(GameObject[] objetosASeremVisualizados)
    // {
    //     bool todosVistos = true;
    //     var planes = GeometryUtility.CalculateFrustumPlanes(cam);
    //     foreach (GameObject objeto in objetosASeremVisualizados)
    //     {
    //         var renderizador = objeto.GetComponent<SpriteRenderer>();
    //         if (renderizador.isVisible)
    //             continue;
    //         aplicarZoom(400);
    //         todosVistos = false;
    //         break;
    //     }
    //     if (!todosVistos)
    //         verTodosOsObjetos(objetosASeremVisualizados);
    // }

    public void aplicarZoom(float deltaMagnitudeDiff)
    {

        // If the camera is orthographic...
        if (cam.orthographic)
        {
            // ... change the orthographic size based on the change in distance between the touches.
            cam.orthographicSize += deltaMagnitudeDiff * orthoZoomSpeed;

            // Make sure the orthographic size never drops below zero.
            cam.orthographicSize = Mathf.Max(cam.orthographicSize, 0.1f);
            if (cam.orthographicSize >= zoomMaximo)
            {
                cam.orthographicSize = zoomMaximo;
            }
			if (cam.orthographicSize <= zoomMinimo)
            {
                cam.orthographicSize = zoomMinimo;
            }
        }
        else
        {
            // Otherwise change the field of view based on the change in distance between the touches.
            cam.fieldOfView += deltaMagnitudeDiff * perspectiveZoomSpeed;

            // Clamp the field of view to make sure it's between 0 and 180.
            cam.fieldOfView = Mathf.Clamp(cam.fieldOfView, 0.1f, 179.9f);
        }
    }
}
