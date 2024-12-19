using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

/// <summary>
/// Gère le comportement d'un système de particules attaché à un GameObject.//tp3 Patrick
/// </summary>
public class Particule : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] float _vitesseParticules = 8f; // Vitesse des particules
    bool _estActif; // Indique si le système de particules est actif ou non//tp3 Patrick
    private ParticleSystem _particleSystem; // Référence au composant ParticleSystem attaché à cet objet//tp3 Patrick
    ParticleSystem.EmissionModule _emissionModule;

    void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>(); // Récupère le composant ParticleSystem attaché à cet objet//tp3 Patrick
    }

    void Update()
    {
        //Détruit les particules quand à la fin de leur cycle.//#tp4 Elyzabelle:
        if (!_particleSystem.IsAlive()) Destroy(gameObject);
    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        _emissionModule = _particleSystem.emission;
    }

    //--------------------------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------------------------
    // Changement de direction
    //--------------------------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// Change la direction du système de particules.//tp3 Patrick
    /// </summary>
    /// <param name="versLaGauche">True si le système doit être inversé vers la gauche, sinon False.</param>//tp3 Patrick
    public void ChangerDirection(bool versLaGauche)
    {
        ParticleSystem.MainModule mainModule = _particleSystem.main;
        // Change la vitesse des particules pour les faire aller vers la gauche:
        if (versLaGauche) mainModule.startSpeed = -_vitesseParticules; //#synthese Elyzabelle
        // Rétablit la vitesse des particules pour les faire aller vers la droite
        else mainModule.startSpeed = _vitesseParticules; //#synthese Elyzabelle
    }

    //--------------------------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------------------------
    // Arrêt et démarrage
    //--------------------------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// Arrête ou démarre le système de particules.
    /// //#synthese Elyzabelle
    /// </summary>
    public void ArreterOuDemarrer(bool estActif)
    {
        if (!estActif) _emissionModule.rateOverDistanceMultiplier = 0f; //N'affiche pas des particules
        else _emissionModule.rateOverDistanceMultiplier = 3f;  //affiche des particules
    }

    //--------------------------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------------------------
    // Changement de vitesse
    //--------------------------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// Change la vitesse du système de particules.//tp3 Patrick
    /// </summary>
    /// <param name="nouvelleVitesse">La nouvelle vitesse du système de particules.</param>//tp3 Patrick
    public void ChangerVitesse(float nouvelleVitesse)
    {
        var main = _particleSystem.main; // Récupère le module principal du système de particules//tp3 Patrick
        main.simulationSpeed = nouvelleVitesse; // Modifie la vitesse de simulation du système de particules//tp3 Patrick
    }


    /// <summary>
    /// Change la couleur des particules du système.
    /// //#synthese Elyzabelle
    /// </summary>
    /// <param name="nouvelleCouleur">La nouvelle couleur des particules.</param>
    public void ChangerCouleur(Color nouvelleCouleur)
    {
        // Assurez-vous que le système de particules est défini
        if (_particleSystem == null)
        {
            Debug.LogError("Le système de particules n'est pas défini !");
            return;
        }

        // Obtenez la référence à la module principale du système de particules
        var main = _particleSystem.main;

        // Modifiez la couleur de départ des particules
        main.startColor = nouvelleCouleur;
    }
}