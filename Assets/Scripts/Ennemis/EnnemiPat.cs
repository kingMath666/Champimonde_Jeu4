using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe EnnemiPat dérivée de la classe Ennemis.
/// Cette classe représente un ennemi qui peut changer de comportement 
/// en fonction de la poursuite du joueur.
/// #synthese Patrick
/// </summary>
public class EnnemiPat : Ennemis
{
    [SerializeField] SOPerso _donneesPerso;//lien avec le SOPerso
    [SerializeField] int _nbDommages = 1;//nb de dommages provoqués par l'ennemi

    override protected void FixedUpdate()
    {
        // Appelle la méthode FixedUpdate de la classe parente Ennemis
        base.FixedUpdate();

        // Vérifie si l'ennemi suit le joueur
        if (_suitLeJoueur)
        {
            // Change la vitesse de l'ennemi lorsqu'il chasse le joueur
            _vitesseChasse = 5f;
        }
    }
    /// <summary>
    /// Fonction qui permet de detecter le contact avec le un gameobject qui a un collider 
    /// </summary>
    /// <param name="other">Le gameobject avec lequel l'ennemi est en contact</param>
    void OnCollisionEnter2D(Collision2D other)
    {
        //Si le gameobject avec lequel l'ennemi est en contact est le joueur, il perd 1 vies
        if (other.gameObject.tag == "Perso")
        {
            _donneesPerso.RetirerVies(_nbDommages);
        }

    }
}
