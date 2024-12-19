using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe qui s'occupe de la gestion
/// du pouvoir du personnage
/// #synthese Elyzabelle
/// </summary>
public class PouvoirPerso : MonoBehaviour
{

    [Header("Composants")]
    Animator _anim; //Animator du pouvoir

    [Header("Sons")]
    [SerializeField] AudioClip _sonPouvoir; //Son du pouvoir

    [Header("Variables")]
    [SerializeField, Range(0f, 10f)] float _tempsInactif = 5f; //Temps d'inactivité du pouvoir
    bool _estActif = true; //Etat actif du pouvoir
    Coroutine CoroutineActivation; //Coroutine permettant d'activer le pouvoir

    [Header("Références")]
    [SerializeField] Perso _perso; //Permet de s'abonner à l'évènement
    [SerializeField] SOPerso _donneesPerso; //Données du personnage


    void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    void Start()
    {
        //Active le pouvoir lorsque la touche E est appuyée:
        _perso.activerPouvoir.AddListener(ActiverPouvoir);
    }

    //--------------------------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------------------------
    // Gestion du pouvoir
    //--------------------------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// Fonction permettant d'activer le pouvoir
    /// </summary>
    void ActiverPouvoir()
    {
        //Si le pouvoir est actif et que le joueur possède le pouvoir:
        if (_estActif && _donneesPerso.possedePouvoir)
        {
            GestAudio.instance.JouerEffetSonore(_sonPouvoir);
            _anim.SetTrigger("Explosion"); //Animation du pouvoir
            //Déclenchement du délais d'activation:
            CoroutineActivation = StartCoroutine(ActiverPouvoirCoroutine());
            //Animation du bouton sur le canvas flottant:
            RetroactionTextes.instance.AnimerBtnPouvoir(_tempsInactif);
        }
        else return;
    }

    /// <summary>
    /// Déclenchement du délais d'activation
    /// </summary>
    IEnumerator ActiverPouvoirCoroutine()
    {
        _estActif = false;
        yield return new WaitForSeconds(_tempsInactif);
        _estActif = true;
        ArreterCoroutine();
    }

    /// <summary>
    /// Arrêt du délais d'activation
    /// </summary>
    void ArreterCoroutine()
    {
        if (CoroutineActivation != null) StopCoroutine(CoroutineActivation);
    }
}
