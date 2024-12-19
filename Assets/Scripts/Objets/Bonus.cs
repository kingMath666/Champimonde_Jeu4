using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;


/// <summary>
/// Classe qui permet de gérer les 
/// fonctionnalités générale des objets
/// #tp3 Elyzabelle
/// </summary>
public class Bonus : Objet
{
    [Header("Composants")]
    Collider2D _collider; // Collider d'un objet
    SpriteRenderer _sr; //SpriteRenderer d'un objet

    [Header("Sprites")]
    [SerializeField] Sprite _imgInactif; //image de l'objet quand il est désactivé
    [SerializeField] Sprite _imgActif; //image de l'objet quand il est activé

    [Header("Paramètres")]
    // Liste qui contient les instances de tous les objets:
    static List<Bonus> lesObjets = new List<Bonus>();
    [SerializeField] Light2D _lumiere; //Lumière des bonus //synthese Elyzabelle
    [SerializeField] float _lumiereIntensiteIni = 0f; //Intensité de la lumière initiale //synthese Elyzabelle
    [SerializeField] float _lumiereIntensiteActif = 1f; //Intensité de la lumière lorsque l'objet est activé //synthese Elyzabelle

    void Start()
    {
        lesObjets.Add(this); //Ajoute les instaces à la liste
        _collider = GetComponent<Collider2D>(); //Permet d'accéder au collider
        _sr = GetComponent<SpriteRenderer>(); //Permet d'accéder au SpriteRenderer
        Reinitialisation();
        _lumiere.intensity = _lumiereIntensiteIni; //Désactive la lumière //synthese Elyzabelle
    }

    void OnApplicationQuit()
    {
        lesObjets.Clear(); //Vide la liste
    }

    //--------------------------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------------------------
    // Réinitialisation
    //--------------------------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// Remet les objets à leur état de base
    /// </summary>
    protected void Reinitialisation()
    {
        _sr.sprite = _imgInactif; //Modifie le sprite pour l'img inactif
        _collider.enabled = false; //Désactive le collider
    }

    //--------------------------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------------------------
    // Activation des objets
    //--------------------------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// Fonction appelé par l'événement
    /// </summary>
    public virtual void ActiverTousLesObjets()
    {
        foreach (Bonus bonus in lesObjets)
        {
            bonus.ActiverObjet();
            bonus._lumiere.intensity = _lumiereIntensiteActif; //Active la lumière //synthese Elyzabelle
        }
    }

    /// <summary>
    /// Active les objets
    /// </summary>
    protected void ActiverObjet()
    {
        if (_collider == null) return;
        if (_sr == null) return;

        _collider.enabled = true; //Active le collider
        _sr.sprite = _imgActif; //Modifie le sprite pour l'img actif
    }
}
