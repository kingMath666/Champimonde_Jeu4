using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.TextCore.Text;

/// <summary>
/// Fonctionnalités spécifique 
/// à l'objet Pomme
/// Ajoute de la vie au joueur
/// #tp3 Elyzabelle
/// </summary>
public class ObjPomme : Bonus
{
    [Header("Références")]
    [SerializeField] SOPerso _donneesPerso; //Permet d'avoir accès au pointage du joueur
    bool _aLeDroitALaSaintePomme = true;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == _nomPerso.tag && _aLeDroitALaSaintePomme) //Si le joueur entre en collision avec une pomme
        {
            _aLeDroitALaSaintePomme = false;
            AjouterVie();
            GestAudio.instance.JouerEffetSonore(_clip); //Joue le son de l'objet// #tp4 Elyzabelle
            Vector3 pos = gameObject.transform.position; //position de la potion// #tp3 Elyzabelle
            Instantiate(_particule, pos, Quaternion.identity); //Instancie les particules// #tp3 Elyzabelle
            Destroy(gameObject); //Détruit l'objet après qu'il ait été activé.//tp3 Patrick
        }
        else return;
    }

    /// <summary>
    /// Ajoute de la vie au perso
    /// </summary>
    void AjouterVie()
    {
        _donneesPerso.vies += _valeur;
    }
}
