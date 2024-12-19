using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

/// <summary>
/// Contient les données du personnage
/// #Tp3 Elyzabelle et Patrick
/// </summary>
[CreateAssetMenu(fileName = "Perso", menuName = "Perso")]
public class SOPerso : ScriptableObject
{
    [Header("Références")]
    [SerializeField] SONavigation _navigation; //Informations de navigation

    [Header("UnityEvents")]
    UnityEvent _modifierInfosDuJeu = new UnityEvent(); //Evenement qui modifie les informations du jeu.//tp4 Elyzabelle
    public UnityEvent modifierInfosDuJeu => _modifierInfosDuJeu; //Accesseur
    UnityEvent _changerArgent = new UnityEvent(); //Evenement qui modifie les informations du jeu.//tp4 Elyzabelle
    public UnityEvent changerArgent => _changerArgent; //Accesseur
    UnityEvent _changerPointage = new UnityEvent(); //Evenement qui modifie les informations du jeu.//tp4 Elyzabelle
    public UnityEvent changerPointage => _changerPointage; //Accesseur
    UnityEvent _bouclierBrise = new UnityEvent(); //Evenement qui modifie les informations du jeu.//tp4 Elyzabelle
    public UnityEvent bouclierBrise => _bouclierBrise; //Accesseur
    UnityEvent _bouclierBloque = new UnityEvent(); //Evenement qui modifie les informations du jeu.//tp4 Elyzabelle
    public UnityEvent bouclierBloque => _bouclierBloque; //Accesseur

    [Header("Valeurs initiales")]
    [SerializeField, Range(1, 5)] int _niveauIni = 1; // Niveau initial du personnage.//tp3 Patrick
    [SerializeField, Range(0, 500)] int _argentIni = 100; // Argent initial du personnage.//tp3 Patrick
    [SerializeField, Range(0, 500)] int _pointageIni = 200; // Pointage initial du personnage.//tp3 Patrick
    [SerializeField] float _vitesseIni = 5f; // Vitesse de déplacement horizontale initiale du personnage.//tp3 Patrick
    [SerializeField, Range(0, 5)] int _vieMax = 3; //Nom de vies max du perso
    [SerializeField, Range(0, 500)] int _valeurBonusMarguerite = 500; //Valeur du bonus de marguerite //#tp4 Elyzabelle
    [SerializeField, Range(0, 500)] int _valeurBonusActivateur = 500; //Valeur du bonus de l'activateur //#tp4 Elyzabelle
    [SerializeField, Range(0, 500)] int _valeurBonusTemps = 10; //Valeur du bonus de temps //#tp4 Elyzabelle 

    [Header("Valeurs actuelles")]
    [SerializeField] bool _possedeClef = false; // Indique si le personnage possède la clef.//tp3 Patrick
    [SerializeField, Range(1, 5)] int _niveau = 1; // Niveau actuel du personnage.//tp3 Patrick
    [SerializeField, Range(0, 500)] int _argent = 100; // Argent actuel du personnage.//tp3 Patrick
    [SerializeField, Range(0, 500)] int _pointage = 100; // Pointage actuel du personnage.//tp3 Patrick
    [SerializeField, Range(0, 3)] int _vies = 3; //Nom de vies du perso
    [SerializeField, Range(1, 5)] int _forceBouclier = 0; // Force du bouclier
    [SerializeField, Range(1, 5)] int _forceBouclierMax = 10; //Force maximale du bouclier
    int _tempsRestant = 0; //Temps restant à la fin du niveau //#tp4 Elyzabelle

    [Header("Bonus")]
    [SerializeField] float _vitesse = 5f; // La vitesse de déplacement horizontale du personnage.
    // Indique si le personnage possède la capacité de faire un double saut :
    [SerializeField] bool _possedeDoubleSaut = false;
    [SerializeField] bool _possedePouvoir = false;
    [SerializeField] bool _possedeAcceleration = false;
    [SerializeField] bool _possedeBouclier = false; // Indique si le personnage possède la capacité du bouclier
    // Indique si le personnage possède le bonus de fleurs.//tp4 Elyzabelle :
    [SerializeField] bool _possedeBonusFleurs = false;
    // Indique si le personnage possède le bonus d'activation de l'activateur.//tp4 Elyzabelle :
    [SerializeField] bool _possedeBonusActivateur = false;

    [Header("Prix des objets de la boutique")]
    [SerializeField] SOObjet _doubleSautPrix; // Référence vers l'objet représentant le prix du double saut.//tp3 Patrick
    // Référence vers l'objet représentant le prix de l'augmentation de vitesse.//tp3 Patrick:
    [SerializeField] SOObjet _vitessePrix;
    [SerializeField] SOObjet _bouclierPrix; // Référence vers l'objet représentant le prix du bouclier.//tp3 Patrick
    [SerializeField] SOObjet _pouvoirPrix; // Référence vers l'objet représentant le prix du pouvoir.//#synthese Elyzabelle



    //--------------------------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------------------------
    // Réinitialisation
    //--------------------------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// Initialise les données du personnage.
    /// Appelé quand le jeu se termine
    /// tp3 Patrick
    /// </summary>
    public void Initialiser()
    {
        niveau = _niveauIni;
        argent = _argentIni;
        pointage = _pointageIni;
        _tempsRestant = 0;
        RestaurerPossession();
    }

    /// <summary>
    /// Restaure les possessions effectués par le personnage.
    /// Appelé quand le joueur entre dans la boutique
    /// tp3 Patrick
    /// </summary>
    public void RestaurerPossession()
    {
        vies = _vieMax;
        vitesse = _vitesseIni; // Réinitialise la vitesse de déplacement horizontale.//tp3 Patrick
        possedeDoubleSaut = false; // Réinitialise la capacité de double saut.//tp3 Patrick
        _possedeClef = false; // Réinitialise la possession de la clef.//tp3 Patrick
        possedeBouclier = false;
        _possedeBonusActivateur = false;
        _possedeBonusFleurs = false;
        _possedeAcceleration = false;
        _possedePouvoir = false;
    }

    //--------------------------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------------------------
    // Gestion des achats
    //--------------------------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// Achète la capacité de double saut.
    /// tp3 Patrick
    /// </summary>
    public void AcheterDoubleSaut()
    {
        if (_argent >= _doubleSautPrix.prixDeBase)
        {
            possedeDoubleSaut = true;
            argent = argent - _doubleSautPrix.prixDeBase;
        }
    }

    /// <summary>
    /// Achète un accélérateur de vitesse.
    /// tp3 Patrick
    /// </summary>
    public void AcheterAccelerateurVitesse()
    {
        if (vitesse <= _vitesseIni)
        {
            if (_argent >= _vitessePrix.prixDeBase)
            {
                possedeAcceleration = true;
                vitesse += vitesse / 2; // Augmente la vitesse de déplacement horizontale.//tp3 Patrick
                argent = argent - _vitessePrix.prixDeBase;
            }
        }
    }

    /// <summary>
    /// Active le bonus de bouclier
    /// #tp3 Elyzabelle
    /// </summary>
    public void AcheterBouclier()
    {
        if (_argent >= _bouclierPrix.prixDeBase)
        {
            possedeBouclier = true;
            _forceBouclier = _forceBouclierMax;
            argent = argent - _bouclierPrix.prixDeBase;
        }
    }

    /// <summary>
    /// Active le bonus de bouclier
    /// #tp3 Elyzabelle
    /// </summary>
    public void AcheterPouvoir()
    {
        if (_argent >= _pouvoirPrix.prixDeBase)
        {
            possedePouvoir = true;
            argent = argent - _pouvoirPrix.prixDeBase;
        }
    }

    //--------------------------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------------------------
    // Possession
    //--------------------------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// Prendre la clef
    /// tp3 Patrick
    /// </summary>
    public void PrendreClef()
    {
        _possedeClef = true;
    }

    //--------------------------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------------------------
    // Gestion des dommages
    //--------------------------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// Sert à retirer des vies et la fonctionnalité du bouclier
    /// #synthese Elyzabelle
    /// </summary>
    /// <param name="dommages"> Valeur des dommages provoqués par l'ennemi </param>
    public void RetirerVies(int dommages)
    {
        if (possedeBouclier) //Si le joueur possède le bouclier //#tp3 Elyzabelle
        {
            Debug.Log(_forceBouclier);
            //Si le bouclier a toujours de la force //#tp3 Elyzabelle:
            if (_forceBouclier <= 0)
            {
                bouclierBrise.Invoke();
                possedeBouclier = false;
            }
            else
            {
                bouclierBloque.Invoke();
                _forceBouclier -= dommages;
            }
        }
        else vies -= dommages;
    }

    //--------------------------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------------------------
    // Bonus de fin de niveau
    //--------------------------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// Si le joueur ramasse toutes les fleurs, il obtient un bonus
    /// #tp4 Elyzabelle
    /// </summary>
    public void ObtientBonusFleurs()
    {
        _possedeBonusFleurs = true;
        pointage += _valeurBonusMarguerite;
    }

    /// <summary>
    /// Si le joueur active l'effector, il obtient un bonus
    /// #tp4 Elyzabelle
    /// </summary>
    public void ObtientBonusActivateur()
    {
        _possedeBonusActivateur = true;
        pointage += _valeurBonusActivateur;
    }

    /// <summary>
    /// Conserve le temps restant à la fin du niveau
    /// Ajoute le bonus de temps au pointage
    /// </summary>
    /// <param name="tempsRestant">Temps restant à la partie à la fin du niveau</param>
    public void ObtientBonusTemps(int tempsRestant)
    {
        _tempsRestant = tempsRestant;
        int valeurBonus = tempsRestant * _valeurBonusTemps;
        pointage += valeurBonus;
    }

    //--------------------------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------------------------
    // Setters et getters
    //--------------------------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------------------------

    public float vitesseIni => _vitesseIni; //Accesseur
    // Propriétés pour accéder et modifier les valeurs du personnage.//tp3 Patrick
    public int niveau
    {
        get => _niveau;
        set
        {
            _niveau = Mathf.Clamp(value, 1, int.MaxValue);
            modifierInfosDuJeu.Invoke();
        }
    }

    public int vies
    {
        get => _vies;
        set
        {
            _vies = Mathf.Clamp(value, 0, _vieMax);
            modifierInfosDuJeu.Invoke();
            if (vies == 0) _navigation.AllerSceneTableauHonneur();
        }
    }

    public int argent
    {
        get => _argent;
        set
        {
            _argent = Mathf.Clamp(value, 0, int.MaxValue);
            changerArgent.Invoke(); //#synthese Elyzabelle
        }
    }
    public int pointage
    {
        get => _pointage;
        set
        {
            _pointage = Mathf.Clamp(value, 0, int.MaxValue);
            changerPointage.Invoke(); //#synthese Elyzabelle
        }
    }
    public bool possedeDoubleSaut
    {
        get => _possedeDoubleSaut;
        set
        {
            _possedeDoubleSaut = value;
            modifierInfosDuJeu.Invoke();
        }
    }

    public bool possedeClef { get => _possedeClef; } // Propriété en lecture seule indiquant si le personnage possède la clef.//tp3 Patrick


    public bool possedeBouclier
    {
        get => _possedeBouclier;
        set
        {
            _possedeBouclier = value;
            modifierInfosDuJeu.Invoke();
        }
    }

    public bool possedeBonusFleurs
    {
        get => _possedeBonusFleurs;
        set
        {
            _possedeBonusFleurs = value;
        }
    }
    public bool possedeBonusActivateur
    {
        get => _possedeBonusActivateur;
        set
        {
            _possedeBonusActivateur = value;
        }
    }
    public bool possedePouvoir
    {
        get => _possedePouvoir;
        set
        {
            _possedePouvoir = value;
            modifierInfosDuJeu.Invoke();
        }
    }
    public bool possedeAcceleration
    {
        get => _possedeAcceleration;
        set
        {
            _possedeAcceleration = value;
            modifierInfosDuJeu.Invoke();
        }
    }
    public float vitesse
    {
        get => _vitesse;
        set
        {
            _vitesse = Mathf.Clamp(value, 0, int.MaxValue);
            modifierInfosDuJeu.Invoke();
        }
    }
    public int tempsRestant
    {
        get => _tempsRestant;
        set
        {
            _tempsRestant = Mathf.Clamp(value, 0, int.MaxValue);
        }
    }
    public int valeurBonusMarguerite
    {
        get => _valeurBonusMarguerite;
        set
        {
            _valeurBonusMarguerite = Mathf.Clamp(value, 0, int.MaxValue);
        }
    }
    public int valeurBonusActivateur
    {
        get => _valeurBonusActivateur;
        set
        {
            _valeurBonusActivateur = Mathf.Clamp(value, 0, int.MaxValue);
        }
    }
}
