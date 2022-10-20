using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneradorDeEnemigos : MonoBehaviour {
    public GameObject zombiePrefab;
    public Transform[] puntosDeGeneracion;   //Array que va a contener el numero de generadores. Esto se define dentro de unity.
    public float tiempoDeGeneracion = 5f;    //tiempo que va a tardar en aparecer un zombie.
	// Use this for initialization
	void Start () {
        puntosDeGeneracion = new Transform[transform.childCount];
        for(int i = 0; i<transform.childCount; i++)
        {
            puntosDeGeneracion[i] = transform.GetChild(i); //Casi para todos los arreglos se usa el mismo formato. Aqui va a iniciar desde cero, y ya tendremos que asignarle el numero de generadores que nosotros queramos.

        }

        StartCoroutine(AparecerEnemigo());
	}
	
    IEnumerator AparecerEnemigo()  //Aqui va a contar el numero de generadores que tenemos, va a esperar el tiempo de generación, y la posición que nosotros le demos a cada generador.
    {
        while (true)
        {
            for(int i = 0; i < puntosDeGeneracion.Length; i++)
            {
                Transform puntoDeGeneracion = puntosDeGeneracion[i];
                Instantiate(zombiePrefab, puntoDeGeneracion.position, puntoDeGeneracion.rotation);
            }
            yield return new WaitForSeconds(tiempoDeGeneracion);
        }
    }


	// Update is called once per frame
	void Update () {
		
	}
}
