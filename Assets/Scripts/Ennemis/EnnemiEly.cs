using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ennemi qui attaque le joueur
/// avec son arme
/// #synthese Elyzabelle
/// </summary>
public class EnnemiEly : Ennemis
{
    [Header("Paramètres")]
    [SerializeField] float _distanceDetectionAttaque = 2.0f; // Définissez la distance de départ

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _distanceDetectionAttaque);
    }

    override protected void FixedUpdate()
    {
        base.FixedUpdate();
        AttaquerPerso();
    }

    /// <summary>
    /// Zone d'attaque
    /// Déclenchement de l'attaque
    /// lorsque le perso est dans cette zone
    /// #synthese Elyzabelle
    /// </summary>
    private void AttaquerPerso()
    {
        if (Vector2.Distance(transform.position, Perso.instance.transform.position) < _distanceDetectionAttaque)
        {
            //si le perso est à droite de l'ennemi (fait le changement de sens du collider)
            if (Perso.instance.transform.position.x > transform.position.x) _anim.SetTrigger("AttaqueDroite"); //Animation de l'attaque vers la droite
            else _anim.SetTrigger("Attaque"); //Animation de l'attaque vers la gauche 
        }
    }

    /// <summary>
    /// Joue le son de l'attaque
    /// au début de l'animation
    /// d'attaque de l'ennemi
    /// #synthese Elyzabelle
    /// </summary>
    void JouerSonAttaque()
    {
        GestAudio.instance.JouerEffetSonore(_clipAttaque); //Joue le son de l'attaque
    }
}
