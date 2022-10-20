using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PantallaVida : MonoBehaviour
{
    public Text texto;
    public Vida vida;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        texto.text = vida.valor + "/100";
    }

}
