using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

/// <summary>
/// Fonctionnalités spécifique 
/// à l'objet Feuille
/// Ajoute de l'argent au joueur
/// #tp3 Elyzabelle
/// </summary>
public class ObjFeuille : Joyaux
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == _nomPerso.tag) //Si le joueur entre en collision avec une feuille
        {
            GestAudio.instance.JouerEffetSonore(_clip); //Joue le son de l'objet// #tp4 Elyzabelle
            Vector3 pos = gameObject.transform.position; //position de la potion// #tp3 Elyzabelle
            Instantiate(_particule, pos, Quaternion.identity); //Instancie les particules// #tp3 Elyzabelle
            AjouterJoyaux(_valeur);
            AfficherRetroaction(_valeur);
        }
        else return;
    }
}


