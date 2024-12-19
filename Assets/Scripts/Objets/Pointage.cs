using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script qui gère les  
/// fonctionalités des objets à points
/// #tp3 Elyzabelle
/// </summary>
public class Pointage : Objet
{
    [SerializeField] protected SOPerso _donneesPerso; //Permet d'avoir accès au pointage du joueur

    /// <summary>
    /// Ajoute les points au compteur du perso
    /// </summary>
    /// <param name="pointageDeObjet">Nombre de points que l'objet donne</param>
    protected void AjouterPoints(int pointageDeObjet)
    {
        _donneesPerso.pointage += pointageDeObjet;
    }
}
