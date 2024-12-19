using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Classe qui détecte si le personnage est en contact avec le sol.
/// Auteurs du code: Elyzabelle Rollin et Patrick Watt-Charron.
/// Auteur des commentaires: Patrick Watt-Charron.
/// </summary>
public class DetecteurTuilesPerso : MonoBehaviour
{
    [Header("Références")]
    [SerializeField] GameObject _perso; // Référence au GameObject du personnage.

    [Header("Composants")]
    [SerializeField] LayerMask _layerMask; // Détermine le layer de l'obstacle à vérifier la collision.

    [Header("Paramètres")]
    [SerializeField] float _distanceDebutSol = 0.72f; // Détermine la distance du début du sol.
    [SerializeField] float _distanceDebutHaut = 0.22f; // Détermine la distance du début du haut.
    [SerializeField] Vector2 _grandeurBoite; // Variable pour la taille de la boîte de détection de la collision.
    protected bool _estAuSol = false; // Indique si le personnage est au sol, utilisable dans les scripts enfants.
    protected bool _estTuileDessus = false; // Indique si le personnage est en dessus d'une tuile, utilisable dans les scripts enfants.
    [SerializeField] float _longueurRayon = 0.6f; // Longueur du rayon de détection de la collision.//#synthese Patrick

    virtual protected void FixedUpdate()
    {
        VerifierSol();
        VerifierTuileDeçu();
    }

    //--------------------------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------------------------
    // Vérification si le personnage est en contact avec le sol.
    //--------------------------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// Vérifie si la boîte de collision touche le sol.
    /// </summary>
    private void VerifierSol()
    {
        float scaleX = _perso.transform.localScale.x; // Récupère l'échelle X du personnage.
        _grandeurBoite = new Vector2(scaleX / 2, _distanceDebutSol); // Calcule la taille de la boîte de détection de la collision.
        Vector2 pointDepart = (Vector2)transform.position - new Vector2(0, _distanceDebutSol); // Positionne le début de la boîte de collision.
        Collider2D col = Physics2D.OverlapBox(pointDepart, _grandeurBoite, 0, _layerMask); // Vérifie si la boîte a touché le sol.
        _estAuSol = col != null; // Met à jour la valeur de _estAuSol en fonction du résultat.<
    }

    /// <summary>
    /// Méthode pour vérifier si l'objet est au-dessus d'une tuile.
    /// Synthese Patrick
    /// </summary>
    private void VerifierTuileDeçu()
    {
        // Détermine le point de départ du rayon à partir de la position de l'objet
        Vector2 pointDepartDroite = (Vector2)transform.position - new Vector2(0, -_distanceDebutHaut);

        // Tire un rayon vers le haut (Vector2.up) à partir du point de départ
        RaycastHit2D hitDroite = Physics2D.Raycast(pointDepartDroite, Vector2.up, _longueurRayon, _layerMask);

        // Vérifie si le rayon a touché quelque chose en vérifiant si le collider n'est pas null
        _estTuileDessus = hitDroite.collider != null;
    }

    /// <summary>
    /// Dessine des bordures à la boîte en vert si le personnage touche le sol et en rouge s'il est dans les airs.
    /// </summary>
    void OnDrawGizmos()
    {
        if (Application.isPlaying == false) VerifierSol(); // Si le jeu n'est pas en cours, appelle la fonction VerifierSol().
        Gizmos.color = _estAuSol ? Color.green : Color.red; // Change la couleur de la boîte en fonction de l'état du personnage.

        Vector2 pointDepart = (Vector2)transform.position - new Vector2(0, _distanceDebutSol); // Positionne le début de la boîte de collision.
        Gizmos.DrawWireCube(pointDepart, _grandeurBoite); // Dessine la boîte de collision.

        Gizmos.color = _estTuileDessus ? Color.green : Color.red;// Change la couleur de la boîte en fonction de l'état du personnage.
        Vector2 pointDepartDroite = (Vector2)transform.position + new Vector2(0, -_distanceDebutHaut); // Détermine le point de départ du rayon, basé sur la position de l'objet et un décalage
        Gizmos.DrawRay(pointDepartDroite, Vector2.up * _longueurRayon);//Dessine un rayon vers la gauche depuis le point de départ
    }
}