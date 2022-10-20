using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextoFlotante : MonoBehaviour
{
    public Camera camara;
    public float tiempoDeVida = 1f;
    public Vector3 offset = new Vector3 (0, 1, 0);
        // Start is called before the first frame update
    void Start()
    {
        camara = GameObject.Find("FirstPersonCharacter").GetComponent<Camera>();
        Destroy(gameObject, tiempoDeVida);
        transform.localPosition += offset;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(transform.position+camara.transform.rotation*
            Vector3.forward, camara.transform.rotation * Vector3.up);
    }
}
