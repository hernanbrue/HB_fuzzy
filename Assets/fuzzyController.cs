using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class fuzzyController : MonoBehaviour
{
    Rigidbody2D rb;
    // float gravedad = 9.81 ya aplicada por tener el componente rigidbody
    float velocidad_en_Y;
    float posicion_en_x;
    float posicion_en_y;
    float distancia;
    float gravedad;
    float caos;
    float velocidad_ventilador;
    float altura_objetivo = 225f;
    float grado_de_pertenencia;
    float centrado;
    float cercaA;
    float cercaB;
    float normalA;
    float normalB;
    float lejosA;
    float lejosB;
    float suma_ponderada;
    public Text texto;
    float AreaCentrado;
    float AreaCerca;
    float AreaNormal;
    float AreaLejos;

    private void Start()
    {

    }


    private void FixedUpdate()
    {
        posicion_en_y = gameObject.transform.position.y;

        caos = Random.Range(-500, 500);


        gravedad = Physics.gravity.y;


        aplicar_logica_difusa();


        velocidad_en_Y = (gravedad - velocidad_ventilador + caos) * 0.01f;

        //en la siguiente línea obtengo el rigidbody del círculo y le cambip la posición de acuerdo a la previamente calculada
        gameObject.GetComponent<Rigidbody2D>().position = new Vector2(gameObject.GetComponent<Rigidbody2D>().velocity.x, velocidad_en_Y);

        texto.text = gameObject.GetComponent<Rigidbody2D>().position.y.ToString();


    }

    void aplicar_logica_difusa()
    {

        distancia = altura_objetivo - posicion_en_y;

        centrado = membresia_triangular(distancia, -40, 0, 40);
        AreaCentrado = area_triangular(distancia, 80);

        cercaA = membresia_trapezoide(distancia, 20, 80, 120, 180);
        AreaCerca = area_trapecio(cercaA, 160, 40);
        normalA = membresia_trapezoide(distancia, 120, 160, 240, 280);
        AreaNormal = area_trapecio(normalA, 160, 80);
        lejosA = membresia_grado(distancia, 240, 300);
        AreaLejos = area_triangular(lejosA, 60)/2;


        cercaB = membresia_trapezoide(distancia, -180, -120, -80, -20);
        normalB = membresia_trapezoide(distancia, -280, -240, -160, -120);
        lejosB = membresia_grado(distancia, -300, -240);

        //DESFUZZIFICACIÓN

        suma_ponderada = (centrado * AreaCentrado + cercaA * AreaCerca + normalA * AreaNormal + lejosA * AreaLejos + cercaB * AreaCerca + normalB * AreaNormal + lejosB * AreaLejos);
        //suma_ponderada = (centrado * gravedad + cercaA * 2 + normalA * 4 + lejosA + cercaB * 14 + normalB * 15.5f + lejosB * 18);


        velocidad_ventilador = suma_ponderada / (centrado + cercaA + normalA + lejosA + cercaB + normalB + lejosB);

    }

    public float membresia_grado(float x, float a, float b)
    {
        if (x <= a)
        {
            grado_de_pertenencia = 0;
        }

        if (a < x && x < b)
        {
            grado_de_pertenencia = ((x - a) / (a - b)); // sale de la ecuación de la recta entre los puntos (a, 0) y (b, 1)
        }

        if (x >= b)
        {
            grado_de_pertenencia = 1;
        }

        return grado_de_pertenencia;
    }

     public float membresia_trapezoide(float x, float a, float b, float c, float d)
    {
        if (x <= a)
        {
            grado_de_pertenencia = 0;
        }

        if (a < x && x <= b)
        {
            grado_de_pertenencia = ((x - a) / (b - a));
        }

        if (b < x && x <= c)
        {
            grado_de_pertenencia = 1;
        }

        if (c < x && x < d)
        {
            grado_de_pertenencia = ((d - x) / (d - c));
        }

        if (x >= d)
        {
            grado_de_pertenencia = 0;
        }

        return grado_de_pertenencia;
    }

    public float membresia_triangular(float x, float a, float b, float m)
    {
        
        if(x <= a)
        {
            grado_de_pertenencia = 0;
        }

        if (a < x && x <= m)
        {
            grado_de_pertenencia = ((x - a) / (m - a));
        }

        if (m < x && x < b)
        {
            grado_de_pertenencia = ((b - x) / (b - m));
        }

        if (x >= b)
        {
            grado_de_pertenencia = 0;
        }

        return grado_de_pertenencia;
    }

    public float area_triangular(float h, float b)
    {
        // b es la base, es decir, la diferencia entre los valores de X 
        
        float area;

        area = (b * h) / 2;

        return area;

    }

    public float area_trapecio(float h, float b, float c)
    {
        // b es la base inferior y c la superior

        float area;

        area = ((b + c) / 2) * h;

        return area;
        
    }
}

