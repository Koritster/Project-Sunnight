using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.Rendering.PostProcessing;
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


    [SerializeField] private PostProcessVolume volume;
    [SerializeField] private Vignette vignette;

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

        volume.profile.TryGetSettings(out vignette);
    }

    void Update()
    {
        // Controlar la disminución del hambre y el agua
        UpdateHungry();
        UpdateThirst();

        if (hambre == 0f || agua == 0f)
        {
            UpdateLifePoints();
        }

        UpdatePostProcessing();
    }

    #region Public Functions

    // Método para recibir daño
    public void RecibirDaño(float lifePoints)
    {
        vida -= lifePoints;
        UpdateLifePoints();
    }

    public void Curar(float lifePoints)
    {
        vida += lifePoints;
        UpdateLifePoints();
    }

    public void ChangeHungry(float hungryPoints)
    {
        hambre += hungryPoints;
        UpdateHungry();
    }

    public void ChangeThirst(float thirstPoints)
    {
        agua += thirstPoints;
        UpdateThirst();
    }

    #endregion

    #region Private Functions

    private void ActualizarFillAmount(Image img, float valor)
    {
        // Actualizar el  porcentaje de hambre o agua
        float porcentaje = valor / llenoCAV;
        img.fillAmount = porcentaje;
    }

    private void UpdateHungry()
    {
        tiempoHambre -= Time.deltaTime;
        if (tiempoHambre <= 0f)
        {
            hambre -= cantidadDisminucionHambre;
            if (hambre <= 0f)
            {
                hambre = 0f;
            }
            tiempoHambre = intervaloDisminucionHambre;
        }

        ActualizarFillAmount(hambre_img, hambre);
    }

    private void UpdateThirst()
    {
        tiempoAgua -= Time.deltaTime;
        if (tiempoAgua <= 0f)
        {
            agua -= cantidadDisminucionAgua;
            if (agua < 0f)
            {
                agua = 0f;
            }
            tiempoAgua = intervaloDisminucionAgua;
        }

        ActualizarFillAmount(agua_img, agua);
    }

    private void UpdateLifePoints()
    {
        tiempoPerdidaVida -= Time.deltaTime;
        if (tiempoPerdidaVida <= 0f)
        {
            vida -= cantidadPerdidaVida;
            if (vida <= 0f)
            {
                vida = 0f;
                Debug.LogWarning("El jugador ha muerto");
            }
            tiempoPerdidaVida = velocidadPerdidaVida;
        }

        ActualizarFillAmount(vida_img, vida);
    }

    private void UpdatePostProcessing()
    {
        if (volume == null)
            return;

        if (vignette == null)
            return;

        if (hambre < 20f || agua < 20f)
        {

            // Calcular la intensidad basada en hambre y agua
            float hambreRatio = hambre;
            float aguaRatio = agua;

            // Determinar la intensidad máxima si el hambre o agua están bajos
            float minIntensity = 0.3f;
            float maxIntensity = 0.8f;

            // Encontrar el valor mínimo entre hambre y agua
            float minHealth = Mathf.Min(hambreRatio, aguaRatio);

            // Calcular la nueva intensidad entre el máximo de intensidad y el mínimo, a base de la formula 1 - minHealth
            float newIntensity = Mathf.Lerp(maxIntensity, minIntensity, minHealth * 0.1f);

            vignette.intensity.value = newIntensity;
        }
        else
        {
            vignette.intensity.value = 0.3f;
        }
    }

    #endregion
}
