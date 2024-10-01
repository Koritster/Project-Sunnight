using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class hambre_vida_Agua : MonoBehaviour
{
    // Al utilizar Imagen.fillAmount cambiar la propiedad Type en el inspector de cada imagen a filled
    // Al utilizar Imagen.fillAmount cambiar la propiedad Fill Method a Radial en el inspector de cada imagen a filled

    public Image hambre_img;
    public Image agua_img;
    public Image vida_img;

    public float hambre;
    public float agua;
    public float vida;

    // Velocidad y cantidad a disminuir el hambre y el agua 
    public float intervaloDisminucionHambre = 5f;
    public float intervaloDisminucionAgua = 5f;

    // Velocidad y cantidad a disminuir de el agua y Hambre
    public float cantidadDisminucionHambre = 1f;
    public float cantidadDisminucionAgua = 1f;

    // Velocidad y cantidad a la que se pierde la vida
    public float velocidadPerdidaVida = 1f;  
    public float cantidadPerdidaVida = 1f;

    float llenoCAV = 100f;
    private float tiempoHambre;
    private float tiempoAgua;
    private float tiempoPerdidaVida;

    // Start is called before the first frame update
    void Start()
    {
        hambre = llenoCAV;
        agua = llenoCAV;
        vida = llenoCAV;

        tiempoHambre = intervaloDisminucionHambre;
        tiempoAgua = intervaloDisminucionAgua;
        tiempoPerdidaVida = velocidadPerdidaVida;

        hambre_img.fillAmount = 1f;
        agua_img.fillAmount = 1f;
        vida_img.fillAmount = 1f;
    }

    void Update()
    {
        // Controlar la disminución del hambre y el agua
        tiempoHambre -= Time.deltaTime;
        if (tiempoHambre <= 0f)
        {
            hambre -= cantidadDisminucionHambre;
            if (hambre <= 0f)
            {
                hambre = 0f;
            }
            ActualizarFillAmount(hambre_img, hambre);
            tiempoHambre = intervaloDisminucionHambre;
        }

        tiempoAgua -= Time.deltaTime;
        if (tiempoAgua <= 0f)
        {
            agua -= cantidadDisminucionAgua;
            if (agua < 0f)
            {
                agua = 0f;
            }
            ActualizarFillAmount(agua_img, agua);
            tiempoAgua = intervaloDisminucionAgua;
        }

        if (hambre == 0f || agua == 0f)
        {
            tiempoPerdidaVida -= Time.deltaTime;
            if (tiempoPerdidaVida <= 0f)
            {
                vida -= cantidadPerdidaVida;
                if (vida <= 0f)
                {
                    vida = 0f;
                }
                ActualizarFillAmount(vida_img, vida);
                tiempoPerdidaVida = velocidadPerdidaVida;
            }
        }
        //----------------------------------------------------------------------------------------------------------
        if (Input.GetKey(KeyCode.W))
        {
            tiempoHambre -= Time.deltaTime * 0.5f;
            tiempoAgua -= Time.deltaTime * 0.5f;
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            tiempoHambre -= Time.deltaTime * 2f;
            tiempoAgua -= Time.deltaTime * 2f;
        }
    }

    void ActualizarFillAmount(Image img, float valor)
    {
        // Actualizar el  porcentaje de hambre o agua
        float porcentaje = valor / llenoCAV;
        img.fillAmount = porcentaje;
    }
}
