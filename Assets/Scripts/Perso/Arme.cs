using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Gere les proprétés de l'arme du personnage
/// #synthese Elyzabelle
/// </summary>
public class Arme : MonoBehaviour
{
    [SerializeField] int _forceAttaque = 1; // Ajuster la force de propulsion de l'arme

    public void AppliquerForceAttaque(Rigidbody2D ennemiRb)
    {
        // Application d'une force au Rigidbody2D de l'ennemi
        Vector2 direction = (ennemiRb.transform.position - transform.position).normalized;

        // Ajouter une force à l'ennemi dans la direction calculée
        ennemiRb.AddForce(direction * _forceAttaque, ForceMode2D.Impulse);
    }

}
