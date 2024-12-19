using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Fonctionnalités spécifique 
/// à l'objet Marguerite
/// Ajoute des points au joueur
/// #tp3 Elyzabelle
/// </summary>
public class ObjMarguerite : Pointage
{
    [Header("Propriétés")]
    static int _nbMarguerites = 0; //Nombre de marguerites dans le niveau// #tp4 Elyzabelle

    void Awake()
    {
        _nbMarguerites = 0; //Réinitialise le nombre de marguerites// #tp4 Elyzabelle
    }
    void Start()
    {
        _nbMarguerites++;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == _nomPerso.tag) //Si l'objet est en collision avec le perso
        {
            GestAudio.instance.JouerEffetSonore(_clip); //Joue le son de l'objet// #tp4 Elyzabelle
            Vector3 pos = gameObject.transform.position; //position de la potion// #tp3 Elyzabelle
            Instantiate(_particule, pos, Quaternion.identity); //Instancie les particules// #tp3 Elyzabelle
            AjouterPoints(_valeur);
            AfficherRetroaction(_valeur);
            ReduireMargeurite();
        }
    }

    //--------------------------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------------------------
    // Compteur marguerites
    //--------------------------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// Reduit le nombre de marguerites
    /// Si il n'y a plus de marguerite, on active le bonus de fleurs
    /// #tp4 Elyzabelle  
    /// /// </summary>
    void ReduireMargeurite()
    {
        _nbMarguerites--; //Diminue le nombre de marguerites
        if (_nbMarguerites == 0) _donneesPerso.ObtientBonusFleurs(); //Si il n'y a plus de marguerite, on active le bonus de fleurs // #tp4 Elyzabelle
    }
}
