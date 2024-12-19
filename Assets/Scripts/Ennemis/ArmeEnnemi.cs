using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Permet de blesser le joueur
/// #synthese Elyzabelle
/// </summary>
public class ArmeEnnemi : MonoBehaviour
{
    [SerializeField] int _nbDommages = 1; //Dommages provoqués par l'arme
    [SerializeField] int _forceAttaque = 1; // Ajuster la force de propulsion de l'arme

    /// <summary>
    /// Fonction permettant de blesser 
    /// le joueur
    /// </summary>
    /// <returns> Retourne le nombre de dommages provoqués par l'ennemi</returns>
    public int Blesser()
    {
        return _nbDommages;
    }

    /// <summary>
    /// Fonction permettant d'appliquer une
    /// force au joueur lors de l'attaque
    /// </summary>
    /// <param name="persoRb"> Le rigidbody du joueur </param>
    public void AppliquerForceAttaque(Rigidbody2D persoRb)
    {
        // Application d'une force au Rigidbody2D du personnage
        Vector2 direction = (persoRb.transform.position - transform.position).normalized;

        // Ajouter une force à l'ennemi dans la direction calculée
        persoRb.AddForce(direction * _forceAttaque, ForceMode2D.Impulse);
    }
}
