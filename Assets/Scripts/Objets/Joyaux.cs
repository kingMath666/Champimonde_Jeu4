using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using TMPro;

/// <summary>
/// Script qui gère les  
/// fonctionalités des joyaux
/// #tp3 Elyzabelle
/// </summary>
public class Joyaux : Objet
{
    [SerializeField] SOPerso _donneesPerso; //Permet d'avoir accès à l'argent du joueur

    /// <summary>
    /// Ajoute l'argent au joueur
    /// #tp3 Elyzabelle
    /// </summary>
    /// <param name="valeurDuJoyau">Valeur du joyau</param>
    protected void AjouterJoyaux(int valeurDuJoyau)
    {
        _donneesPerso.argent += valeurDuJoyau;
    }
}