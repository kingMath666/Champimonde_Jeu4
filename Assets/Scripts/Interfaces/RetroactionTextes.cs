using System;
using System.Collections;
using TMPro;
using UnityEngine;

/// <summary>
/// Classe qui s'occupe de l'affichage des
/// champ de rétroaction au joueur
/// #tp3 Elyzabelle
/// </summary>
public class RetroactionTextes : MonoBehaviour
{
    private Coroutine _coroutineChangerTemps; //Coroutine pour changer le temps #tp4 Elyzabelle
    [SerializeField] SOPerso _donneesPerso; //Donnees du personnage
    [SerializeField] SONavigation _navigation; //Informations de navigation

    [Header("Champs de texte")]
    [SerializeField] TextMeshProUGUI _texteArgent; //Champ de texte qui affiche l'argent du joueur
    [SerializeField] TextMeshProUGUI _textePointage; //Champ de texte qui affiche le pointage du joueur
    [SerializeField] TextMeshProUGUI _texteNiveau; //Champ de texte qui affiche le niveau auquel le joueur est
    [SerializeField] TextMeshProUGUI _texteTemps; //Champ de texte qui affiche le temps restant du niveau //#tp4 Elyzabelle

    [Header("Valeurs initiales")]
    [SerializeField, Range(0, 200)] int _tempsIni = 180; //Durée du jeu
    [SerializeField, Range(0, 10)] int _nbChiffresMax = 6; //Nb de chiffres affichés pour le pointage

    [Header("Valeurs actuelles")]
    [SerializeField, Range(0, 180)] int _temps = 180; //Temps restant du jeu

    [Header("Sprites")]
    [SerializeField] Sprite[] _spritesVies; //Sprites des états de vies //#tp4 Elyzabelle
    [SerializeField] Sprite[] _spritesVitesse; //Sprites des états de vitesse //#tp4 Elyzabelle
    [SerializeField] Sprite[] _spritesDoubleSaut; //Sprites des états de double saut //#tp4 Elyzabelle
    [SerializeField] Sprite[] _spritesBouclier; //Sprites des états du bouclier //#tp4 Elyzabelle
    [SerializeField] Sprite[] _spritesPouvoir; //Sprites des états inactifs et actifs du pouvoir //#tp4 Elyzabelle

    [Header("GameObjects")]
    [SerializeField] GameObject _vie; //UI des vies //#tp4 Elyzabelle
    [SerializeField] GameObject _bouclier; //UI du bouclier //#tp4 Elyzabelle
    [SerializeField] GameObject _pouvoir; //UI du pouvoir //#synthese Elyzabelle
    [SerializeField] GameObject _vitesse; //UI des vitesse //#tp4 Elyzabelle
    [SerializeField] GameObject _doubleSaut; //UI des double saut //#tp4 Elyzabelle
    static RetroactionTextes _instance;
    static public RetroactionTextes instance => _instance;

    void Awake()
    {
        _instance = this;
        _temps = _tempsIni; //Initialise le temps #tp4 Elyzabelle
        _donneesPerso.modifierInfosDuJeu.AddListener(ModifierInfosDuJeu);
        _donneesPerso.changerArgent.AddListener(ChangerTexteArgent);
        _donneesPerso.changerPointage.AddListener(ChangerTextePointage);
    }

    void Start()
    {
        _donneesPerso.modifierInfosDuJeu.Invoke();
        _coroutineChangerTemps = StartCoroutine(CoroutineChangerTemps()); //Démarre le temps #tp4 Elyzabelle
        Porte.instance.finDuNiveau.AddListener(BonusTemps);
    }

    /// <summary>
    /// Fait diminuer le temps restant du niveau
    /// Modifie le texte du champ de texte
    /// #tp4 Elyzabelle
    /// </summary>
    /// <returns>Délais de temps en secondes</returns>
    IEnumerator CoroutineChangerTemps()
    {
        while (_temps > 0) //Tant que le temps n'est pas arrivé à 0
        {
            yield return new WaitForSeconds(1.0f); //Attend 1 seconde
            _temps--; //Diminue le temps
            _texteTemps.text = $"{_temps}"; //Modifie le texte
            if (_temps == 30)
            {
                Debug.Log("<color=orange><size=15>Reste 30 secondes au jeu (Début de la musique de l'événement B)</size></color>");
                GestAudio.instance.ChangerEtatLecturePiste(TypePiste.MusiqueEvenB, true); //Change la musique //#tp4 Elyzabelle
            }
            else if (_temps <= 0) _navigation.AllerSceneTableauHonneur(); //Si le temps est arrivé à 0, on a perdu
        }
    }

    /// <summary>
    /// Modifie les informations du jeu
    /// lorsqu'il y a un changement
    /// #tp4 Elyzabelle 
    /// </summary>
    void ModifierInfosDuJeu()
    {
        GererVies();
        ChangerTexteNiveau();
        AfficherAmeliorations();
    }

    /// <summary>
    /// Change le texte pour la nouvelle 
    /// valeur d'argent que possède le joueur
    /// </summary>
    void ChangerTexteArgent()
    {
        if (_donneesPerso.argent < 10) _texteArgent.text = $" 0{_donneesPerso.argent}$";
        else _texteArgent.text = $"{_donneesPerso.argent}$";
        if (_donneesPerso.argent > 0)
        {
            _texteArgent.GetComponent<Animator>(); //#synthese Elyzabelle
            _texteArgent.GetComponent<Animator>().SetTrigger("AjoutArgent"); //#synthese Elyzabelle
        }
    }

    /// <summary>
    /// Change le texte pour la nouvelle 
    /// valeur de points que possède le joueur
    /// #tp4 Elyzabelle
    /// </summary>
    void ChangerTextePointage()
    {
        string nbChiffresPointage = _donneesPerso.pointage.ToString(); //compte la quantite de nombre dans le pointage
        int nbZeros = _nbChiffresMax - nbChiffresPointage.Length; // calcule combien de zéros à ajouter
        string zeros = ""; //chaine de zéros

        //Ajoute les zéros au texte:
        for (int i = 0; i < nbZeros; i++) zeros += "0";
        _textePointage.text = $"{zeros + _donneesPerso.pointage}"; //Affiche le pointage

        if (_donneesPerso.pointage > 0)
        {
            _textePointage.GetComponent<Animator>(); //#synthese Elyzabelle
            _textePointage.GetComponent<Animator>().SetTrigger("AjoutPoints"); //#synthese Elyzabelle
        }
    }

    /// <summary>
    /// Change le texte pour la nouvelle 
    /// valeur de vies que possède le joueur
    /// </summary>
    void ChangerTexteNiveau()
    {
        _texteNiveau.text = $"Niveau:{_donneesPerso.niveau}";
    }

    /// <summary>
    /// Fonction qui gère l'affichage des ameliorations (UI)
    /// #tp4 Elyzabelle
    /// </summary>
    void AfficherAmeliorations()
    {
        //Si le joueur possède le double saut, on affiche le sprite correspondant:
        if (_donneesPerso.possedeDoubleSaut) _doubleSaut.GetComponent<UnityEngine.UI.Image>().sprite = _spritesDoubleSaut[0];
        else _doubleSaut.GetComponent<UnityEngine.UI.Image>().sprite = _spritesDoubleSaut[1];

        //Si le joueur possède le bouclier, on affiche le sprite correspondant:
        if (_donneesPerso.possedeBouclier) _bouclier.GetComponent<UnityEngine.UI.Image>().sprite = _spritesBouclier[0];
        else _bouclier.GetComponent<UnityEngine.UI.Image>().sprite = _spritesBouclier[1];

        if (_donneesPerso.possedePouvoir) _pouvoir.GetComponent<UnityEngine.UI.Image>().sprite = _spritesPouvoir[0];
        else _pouvoir.GetComponent<UnityEngine.UI.Image>().sprite = _spritesPouvoir[1];

        //Si le joueur possède l'amélioration de vitesse, on affiche le sprite correspondant:
        if (_donneesPerso.possedeAcceleration) _vitesse.GetComponent<UnityEngine.UI.Image>().sprite = _spritesVitesse[0];
        else _vitesse.GetComponent<UnityEngine.UI.Image>().sprite = _spritesVitesse[1];
    }

    /// <summary>
    /// Gere l'affichage des vies (UI)
    /// #tp4 Elyzabelle
    /// </summary>
    void GererVies()
    {
        if (_donneesPerso.vies > _spritesVies.Length - 1)
        {
            Debug.LogWarning("Le nombre de vies est supérieur au nombre de sprites de vies");
            _donneesPerso.vies = _spritesVies.Length;
        }
        //Affiche le sprite correspondant au nombre de vies:
        else _vie.GetComponent<UnityEngine.UI.Image>().sprite = _spritesVies[_donneesPerso.vies];
    }

    /// <summary>
    /// Fonction qui attribue un bonus de temps au joueur et arrête toutes les coroutines en cours.
    /// #tp4 Elyzabelle
    /// </summary>
    public void BonusTemps()
    {
        ArreterTemps();
        _donneesPerso.ObtientBonusTemps(_temps);

    }

    /// <summary>
    /// Arrête le temps à la fin du niveau
    /// #tp4 Elyzabelle
    /// </summary>
    public void ArreterTemps()
    {
        StopCoroutine(_coroutineChangerTemps);
    }
    private Coroutine _animationCoroutine;

    /// <summary>
    ///  Fonction qui permet d'animer les sprites du pouvoir
    ///  en appelant une lancemment d'une coroutine
    ///  #synthese Elyzabelle 
    /// </summary>
    /// <param name="temps"> Temps que le pouvoir est inactif </param>
    public void AnimerBtnPouvoir(float temps)
    {
        if (_animationCoroutine != null) StopCoroutine(_animationCoroutine);
        _animationCoroutine = StartCoroutine(AnimerSprites(temps));
    }

    /// <summary>
    /// Fonction qui permet d'animer les sprites du pouvoir
    /// au fur et à mesure que le décompte passe
    /// #synthese Elyzabelle
    /// </summary>
    /// <param name="temps"> Temps que le pouvoir est inactif </param>
    IEnumerator AnimerSprites(float temps)
    {
        //Parcours tous les sprites du pouvoir:
        for (int i = 0; i < _spritesPouvoir.Length; i++)
        {
            //Change le sprite du pouvoir:
            _pouvoir.GetComponent<UnityEngine.UI.Image>().sprite = _spritesPouvoir[i];
            yield return new WaitForSeconds(temps / _spritesPouvoir.Length);
        }
        _animationCoroutine = null;
        AfficherAmeliorations(); //Permet d'afficher le bon sprite à la fin de l'animation
    }

}
