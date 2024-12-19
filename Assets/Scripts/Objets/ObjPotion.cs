using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Fonctionnalités spécifique 
/// à l'objet Potion
/// Élargie le champ de vision du joueur
/// #tp3 Elyzabelle
/// </summary>
public class ObjPotion : Bonus
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == _nomPerso.tag) //Si le joueur entre en collision avec une potion//#tp3 Elyzabelle
        {
            GestAudio.instance.JouerEffetSonore(_clip); //Joue le son de l'objet// #tp4 Elyzabelle
            Vector3 pos = gameObject.transform.position; //position de la potion// #tp3 Elyzabelle
            Instantiate(_particule, pos, Quaternion.identity); //Instancie les particules// #tp3 Elyzabelle
            AfficherRetroaction(_valeur);
            ControleCinemachineBonus.instance.ChangerCamera();//Appel de la fonction ChangerCamera de la classe ControleCinemachineBonus//#tp4 Patrick
            Destroy(gameObject); // Détruit l'objet après qu'il ait été activé.//tp3 Patrick
        }
        else return;
    }
}