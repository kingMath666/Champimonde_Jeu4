using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

/// <summary>
/// Classe qui permet d'afficher les informations des objets dans le Panneau Objet
/// </summary>
public class PanneauObjet : MonoBehaviour
{
    [Header("LES DONNÉES")]
    [SerializeField] SOObjet _donnees; // Données pour le premier objet
    public SOObjet donnees => _donnees; // Propriété publique pour accéder aux données du premier objet
    [SerializeField] SOObjet _donnees2; // Données pour le deuxième objet
    public SOObjet donnees2 => _donnees2; // Propriété publique pour accéder aux données du deuxième objet
    private SOObjet _donneesActuelle; // Variable pour garder une référence à l'objet actuellement affiché
    private int clic = 0; // Variable pour compter le nombre de clics et alterner entre les objets

    [Header("LES RÉFÉRENCES")]
    [SerializeField] SOPerso _donneesPerso; // Référence aux données du personnage
    [SerializeField] Retroaction _retroaction; // Préfabriqué pour afficher les messages de rétroaction
    [SerializeField] GameObject _repere; // GameObject servant de repère pour positionner les messages de rétroaction

    [Header("LES CONTENEURS")]
    [SerializeField] TextMeshProUGUI _champNom; // Champ texte pour le nom de l'objet
    [SerializeField] TextMeshProUGUI _champPrix; // Champ texte pour le prix de l'objet
    [SerializeField] TextMeshProUGUI _champDescription; // Champ texte pour la description de l'objet
    [SerializeField] Image _image; // Image pour afficher le sprite de l'objet
    [SerializeField] CanvasGroup _canvasGroup; // Groupe de canvas pour gérer la visibilité du panneau
    [SerializeField] Button _bouton; // Bouton pour acheter l'objet


    [Header("LES DICTIONNAIRES")]
    private Dictionary<string, UnityAction> actionsAchat; // Dictionnaire pour stocker les actions d'achat des objets
    private Dictionary<string, System.Func<bool>> conditionsDesactivation; // Dictionnaire pour stocker les conditions de désactivation des objets

    [Header("LES SONS")]
    [SerializeField] AudioClip _clipAchat; // Clip audio pour l'achat
    [SerializeField] AudioClip _clipBouton; // Clip audio pour l'achat

    void Start()
    {
        _donneesActuelle = _donnees; // Initialiser avec les données du premier objet

        // Initialiser le dictionnaire des actions d'achat
        actionsAchat = new Dictionary<string, UnityAction>
        {
            { "Bouclier", () => AcheterObjet(_donneesPerso.AcheterBouclier, "Bouclier acheté!") },
            { "Pouvoir d'impulsion", () => AcheterObjet(_donneesPerso.AcheterPouvoir, "Pouvoir d'impulsion acheté!") },
            { "Botte de vitesse", () => AcheterObjet(_donneesPerso.AcheterAccelerateurVitesse, "Botte de vitesse achetée!") },
            { "Bâton sauteur", () => AcheterObjet(_donneesPerso.AcheterDoubleSaut, "Bâton sauteur acheté!") }
        };

        // Initialiser le dictionnaire des conditions de désactivation
        conditionsDesactivation = new Dictionary<string, System.Func<bool>>
        {
            { "Bouclier", () => _donneesPerso.possedeBouclier },
            { "Pouvoir d'impulsion", () => _donneesPerso.possedePouvoir },
            { "Botte de vitesse", () => _donneesPerso.possedeAcceleration },
            { "Bâton sauteur", () => _donneesPerso.possedeDoubleSaut }
        };

        // Ajouter un listener pour vérifier et désactiver les boutons lors de la modification des informations du jeu
        _donneesPerso.modifierInfosDuJeu.AddListener(VerifierEtDesactiverBoutons);

        GererAffichage(); // Mettre à jour l'affichage initial
    }

    void Update()
    {
        GererBoutonAchat(); // Mettre à jour l'état du bouton d'achat chaque frame
    }

    //----------------------------------------------------------------------------------------------
    // Gestion des clics
    //----------------------------------------------------------------------------------------------

    /// <summary>
    /// Méthode pour gérer le clic droit (passer à l'objet suivant)
    /// synthese Elyzabelle
    /// </summary>
    public void CliquerBtnDroite()
    {
        clic++;
        ChangerPanneauObjets();
        if (_clipBouton != null) GestAudio.instance.JouerEffetSonore(_clipBouton);
    }


    /// <summary>
    /// Méthode pour gérer le clic gauche (revenir à l'objet précédent)
    /// #synthese Elyzabelle
    /// </summary>
    public void CliquerBtnGauche()
    {
        clic--;
        ChangerPanneauObjets();
        if (_clipBouton != null) GestAudio.instance.JouerEffetSonore(_clipBouton);
    }

    /// <summary>
    /// Méthode pour gérer l'état du bouton d'achat
    /// #synthese Elyzabelle 
    /// </summary>
    void GererBoutonAchat()
    {
        // Assurez-vous de désactiver le bouton si l'objet est déjà possédé
        if (conditionsDesactivation.ContainsKey(_donneesActuelle.nom) && conditionsDesactivation[_donneesActuelle.nom]())
        {
            _bouton.interactable = false;
            return;
        }

        // Sinon, vérifiez si l'utilisateur a assez d'argent
        _bouton.interactable = _donneesActuelle.prixDeBase <= _donneesPerso.argent;
        _bouton.onClick.RemoveAllListeners();

        // Ajouter l'action d'achat correspondante au bouton
        if (actionsAchat.ContainsKey(_donneesActuelle.nom))
        {
            _bouton.onClick.AddListener(actionsAchat[_donneesActuelle.nom]);
        }

        VerifierEtDesactiverBoutons();
    }

    // Méthode pour vérifier et désactiver les boutons si l'objet est déjà possédé
    void VerifierEtDesactiverBoutons()
    {
        if (conditionsDesactivation.ContainsKey(_donneesActuelle.nom) && conditionsDesactivation[_donneesActuelle.nom]())
        {
            _bouton.interactable = false;
        }
    }

    //----------------------------------------------------------------------------------------------
    // Gestion de l'affichage
    //----------------------------------------------------------------------------------------------

    /// <summary>
    /// Méthode pour changer les objets affichés
    /// #synthese Elyzabelle
    /// /// </summary>
    void ChangerPanneauObjets()
    {
        _donneesActuelle = (clic % 2 == 0) ? _donnees : _donnees2;
        GererAffichage();
    }

    // Méthode pour mettre à jour l'affichage des informations de l'objet
    void GererAffichage()
    {
        _champNom.text = _donneesActuelle.nom;
        _champPrix.text = _donneesActuelle.prixDeBase + "$";
        _champDescription.text = _donneesActuelle.description;
        _image.sprite = _donneesActuelle.sprite;

        // Mettez à jour l'état du bouton à chaque changement d'affichage
        VerifierEtDesactiverBoutons();
    }

    // Méthode pour gérer l'achat d'un objet
    void AcheterObjet(UnityAction actionAchat, string message)
    {
        GestAudio.instance.JouerEffetSonore(_clipAchat);
        actionAchat.Invoke(); // Exécuter l'action d'achat
        AfficherMessage(message); // Afficher le message de rétroaction
        VerifierEtDesactiverBoutons(); // Vérifier et désactiver les boutons après l'achat
    }

    // Méthode pour afficher un message de rétroaction
    void AfficherMessage(string message)
    {
        // Instancier l'objet de rétroaction à la position du repère
        Retroaction retro = Instantiate(_retroaction, _repere.transform.position, Quaternion.identity, transform.parent.transform.parent.transform.parent);
        retro.ChangerTexte(message); // Changer le texte de rétroaction
    }
}
