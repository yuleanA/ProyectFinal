using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ModoDeDisparo
{
    SemiAuto,
    FullAuto

}
public class LogicaArma : MonoBehaviour
{
    protected Animator animator;
    protected AudioSource audioSource;
    public bool tiempoNoDisparo = false;
    public bool puedeDisparar = false;
    public bool recargando = false;

    [Header("Referencia de Objetos")]
    public ParticleSystem fuegoDeArma;
    public Camera camaraPrincipal;
    public Transform puntoDeDisparo;
    public GameObject efectoDañoPrefab;

    [Header("Referencia de Sonidos")]
    public AudioClip SonDisparo;
    public AudioClip SonSinBalas;
    public AudioClip SonCartuchoEntra;
    public AudioClip SonCartuchoSale;
    public AudioClip SonVacio;
    public AudioClip SonDesenfundar;

    [Header("Atributos de Arma")]
    public ModoDeDisparo modoDeDisparo = ModoDeDisparo.FullAuto;
    public float daño = 20f;
    public float ritmoDeDisparo = 0.3f;
    public int balasRestantes;
    public int balasEnCartucho;
    public int tamañoDeCartucho = 12;
    public int maximoDeBalas = 100;
    public bool estaADS = false;
    public Vector3 disCadera;
    public Vector3 ADS;
    public float tiempoApuntar;
    public float zoom;
    public float normal;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();

        balasEnCartucho = tamañoDeCartucho;
        balasRestantes = maximoDeBalas;

        Invoke("HabilitarArmar", 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (modoDeDisparo == ModoDeDisparo.FullAuto && Input.GetButton("Fire1"))
        {
            RevisarDisparo();
        }
        else if (modoDeDisparo == ModoDeDisparo.SemiAuto && Input.GetButtonDown("Fire1"))
        {
            RevisarDisparo();
        }

        if (Input.GetButtonDown("Reload"))
        {
            RevisarRecargar();
        }

        if (Input.GetMouseButton(1))
        {
            transform.localPosition = Vector3.Slerp(transform.localPosition, ADS, tiempoApuntar * Time.deltaTime);
            estaADS = true;
            camaraPrincipal.fieldOfView = Mathf.Lerp(camaraPrincipal.fieldOfView, zoom, tiempoApuntar * Time.deltaTime);
        }

        if (Input.GetMouseButtonUp(1))
        {
            estaADS = false;
        }

        if (estaADS == false)
        {
            transform.localPosition = Vector3.Slerp(transform.localPosition, disCadera, tiempoApuntar * Time.deltaTime);
            camaraPrincipal.fieldOfView = Mathf.Lerp(camaraPrincipal.fieldOfView, normal, tiempoApuntar * Time.deltaTime);
        }

    }

    void HabilitarArmar()
    {
        puedeDisparar = true;
    }

    void RevisarDisparo()
    {
        if (!puedeDisparar) return;
        if (tiempoNoDisparo) return;
        if (recargando) return;
        if (balasEnCartucho > 0)
        {
            Disparar();
        }
        else
        {
            SinBalas();
        }
    }

    void Disparar()
    {
        audioSource.PlayOneShot(SonDisparo);
        tiempoNoDisparo = true;
        fuegoDeArma.Stop();
        fuegoDeArma.Play();
        ReproducirAnimacionDisparo();
        balasEnCartucho--;
        StartCoroutine(ReiniciarTiempoNoDisparo());
        DisparoDirecto();
    }

    public void CrearEfectoDaño(Vector3 pos, Quaternion rot)
    {
        GameObject efectoDaño = Instantiate(efectoDañoPrefab, pos, rot);
        Destroy(efectoDaño, 1f);
    }
    void DisparoDirecto()
    {
        RaycastHit hit;
        if (Physics.Raycast(puntoDeDisparo.position, puntoDeDisparo.forward, out hit))
        {
            if (hit.transform.CompareTag("Enemigo"))
            {
                Vida vida = hit.transform.GetComponent<Vida>();
                if (vida == null)
                {
                    throw new System.Exception("No se encontró el componente de Vida del Enemigo");
                }
                else
                {
                    vida.RecibirDaño(daño);
                    CrearEfectoDaño(hit.point, hit.transform.rotation);
                }
            }
        }
    }

    public virtual void ReproducirAnimacionDisparo()
    {
        if (gameObject.name == "Police9mm")
        {
            if (balasEnCartucho > 1)
            {
                animator.CrossFadeInFixedTime("Fire", 0.1f);
            }
            else
            {
                animator.CrossFadeInFixedTime("FireLast", 0.1f);
            }
        }
        else
        {
            animator.CrossFadeInFixedTime("Fire", 0.1f);
        }

    }

    void SinBalas()
    {
        audioSource.PlayOneShot(SonSinBalas);
        tiempoNoDisparo = true;
        StartCoroutine(ReiniciarTiempoNoDisparo());
    }

    IEnumerator ReiniciarTiempoNoDisparo()
    {
        yield return new WaitForSeconds(ritmoDeDisparo);
        tiempoNoDisparo = false;
    }

    void RevisarRecargar()
    {
        if (balasRestantes > 0 && balasEnCartucho < tamañoDeCartucho)
        {
            Recargar();
        }
    }

    void Recargar()
    {
        if (recargando) return;
        recargando = true;
        animator.CrossFadeInFixedTime("Reload", 0.1f);
    }

    void RecargarMuniciones()
    {
        int balasParaRecargar = tamañoDeCartucho - balasEnCartucho;
        int restarBalas = (balasRestantes >= balasParaRecargar) ? balasParaRecargar : balasRestantes;

        balasRestantes -= restarBalas;
        balasEnCartucho += balasParaRecargar;
    }

    public void DesenfundarOn()
    {
        audioSource.PlayOneShot(SonDesenfundar);
    }

    public void CartuchoEntraOn()
    {
        audioSource.PlayOneShot(SonCartuchoEntra);
        RecargarMuniciones();
    }

    public void CartuchoSaleOn()
    {
        audioSource.PlayOneShot(SonCartuchoSale);
        RecargarMuniciones();
    }

    public void VacioOn()
    {
        audioSource.PlayOneShot(SonVacio);
        Invoke("ReiniciarRecargar", 0.1f);
    }
    void ReiniciarRecargar()
    {
        recargando = false;
    }
}


