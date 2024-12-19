using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

/// <summary>
/// Classe qui permet à l'activateur d'activer les 
/// objets une fois activée
/// #tp3 Elyzabelle
/// </summary>
public class Activateur : MonoBehaviour
{
    [Header("Paramètres")]
    [SerializeField] bool _estActive = false; //Vérifie si l'activateur est activé

    [Header("Sprites")]
    [SerializeField] Sprite _imgIni; //Image initiale de l'activateur
    [SerializeField] Sprite _imgActif; //Image pour état actif

    [Header("Composants")]
    SpriteRenderer _sr; //Permet de changer les sprites quand activateur est activé
    Collider2D _collider; //Collider de l'activateur

    [Header("Références")]
    [SerializeField] GameObject _nomPerso; //Permet d'accéder au nom du GameObject
    [SerializeField] SOPerso _donneesPerso; //Donnée du perso
    [SerializeField] Retroaction _retroModele; //Modèle de l'objet de rétroaction //#synthese Elyzabelle

    [Header("UnityEvents")]
    [SerializeField] UnityEvent _activerObjets; //Active les bonus sur la scène

    void Awake()
    {
        _collider = gameObject.GetComponent<Collider2D>(); //Permet d'accéder au
        _sr = gameObject.GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        _sr.sprite = _imgIni;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == _nomPerso.tag) //Si l'objet est en collision avec le perso
        {
            _donneesPerso.ObtientBonusActivateur(); //Obtient le bonus de l'effector
            _estActive = true;
            ActiverLesObjets();
        }
    }

    //--------------------------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------------------------
    // Activation des objets
    //--------------------------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// Active tous les objets sur la scène
    /// </summary>
    protected void ActiverLesObjets()
    {
        //Instanciation de la rétroaction:
        Retroaction retro = Instantiate(_retroModele, transform.position, Quaternion.identity, transform.parent);
        retro.ChangerTexte("Activer bonus"); //Change le texte de rétroaction
        _sr.sprite = _imgActif;
        _collider.enabled = false; //Désactive le collider de l'activateur
        if (_estActive) _activerObjets.Invoke(); //Si les active les objets
    }

}
