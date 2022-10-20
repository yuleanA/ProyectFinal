using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MunicionVisible : MonoBehaviour
{
    public Text texto;
    public LogicaArma logicaArma;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        texto.text = logicaArma.balasEnCartucho + "/" + logicaArma.tama�oDeCartucho
            + "\n" + logicaArma.balasRestantes;
    }
}
