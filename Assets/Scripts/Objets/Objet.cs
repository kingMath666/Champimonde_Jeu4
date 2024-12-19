using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// Classe qui gère les fonctionnalités 
/// générales des objets
/// #tp3 Elyzabelle
/// </summary>
public class Objet : MonoBehaviour
{
    [Header("Références")]
    [SerializeField] protected GameObject _particule; //Particules quand l'objet est amassé //#tp4 Elyzabelle
    [SerializeField] Retroaction _retroModele; //Modèle de l'objet de rétroaction
    [SerializeField] protected GameObject _nomPerso; //nom du perso

    [Header("Sons")]
    [SerializeField] protected AudioClip _clip; //Son lorsqu'un objet est amassé //#tp4 Elyzabelle

    [Header("Propriétés")]
    //Quantité que le joueur obtient lorsqu'il amasse l'objet //#tp4 Elyzabelle :
    [SerializeField, Range(1, 500)] protected int _valeur = 10;
    [SerializeField] TypeObjet _type; // Type de l'objet //#tp4 Elyzabelle
    public TypeObjet type => _type; //Getter du type de l'objet //#tp4 Elyzabelle

    //--------------------------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------------------------
    // Rétroaction
    //--------------------------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// Fonction qui permet d'instancier 
    /// une rétroaction au joueur.
    /// Lui indique combien d'objets il obtient
    /// </summary>
    /// <param name="nom">Nom de l'objet</param>
    /// <param name="quantite">Quantité que le joueur obtient</param>
    public virtual void AfficherRetroaction(int quantite)
    {
        //Instanciation de la rétroaction:
        Retroaction retro = Instantiate(_retroModele, transform.position, Quaternion.identity, transform.parent);
        retro.ChangerTexte($"+{quantite} {_type}" + (quantite > 1 ? "s" : "")); //Change le texte de rétroaction
        Destroy(gameObject); //Détruit le gameObject
    }
}
