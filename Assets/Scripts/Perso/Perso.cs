using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Events;

/// <summary>
/// Classe qui controle les déplacements du personnage
/// Auteurs du code: Elyzabelle Rollin et Patrick Watt-Charron
/// Auteur des commentaires: Patrick Watt-Charron
/// </summary>
public class Perso : DetecteurTuilesPerso
{
    [Header("Booleens")]
    bool _veutSauter = false; // Indique si le personnage veut sauter.
    bool _peutDoubleSauter = false; // Indique si le personnage veut effectuer un double saut.
    bool _estUneSeuleAttaque = true; //Permet de savoir si c'est une seule attaque //#synthese Elyzabelle

    [Header("UnityEvents")]
    UnityEvent _activerPouvoir = new UnityEvent(); //Evenement qui modifie les informations du jeu.//tp4 Elyzabelle
    public UnityEvent activerPouvoir => _activerPouvoir; //Accesseur

    [Header("Parametres")]
    [SerializeField] float GraviteChute = 2.5f; // Facteur de gravité supplémentaire pour la chute
    [SerializeField] float _forceSaut = 75f; // La force de saut à appliquer sur le personnage.
    [SerializeField] float _tempsBonus = 10f;
    [SerializeField] int _nbFramesMax = 10; // Le nombre maximum de frames pendant lesquelles le saut est appliqué.
    int _nbFramesRestants = 0; // Le nombre de frames restantes pour le saut.
    float _axeHorizontal; // L'axe de déplacement horizontal.

    [Header("Composants")]
    [SerializeField] SpriteRenderer _sr; // Le composant SpriteRenderer attaché au GameObject.
    [SerializeField] Rigidbody2D _rb; // Le composant Rigidbody2D attaché au GameObject.
    [SerializeField] Collider2D _collider; //Collider du perso
    Animator _anim;

    [Header("Références")]
    [SerializeField] SOPerso _donneesPerso;// Référence vers les données du personnage.//tp3 Patrick
    [SerializeField] Particule _particule; // Référence au script Particule
    [SerializeField] ParticleSystem _particleSystem;// Référence au ParticleSystem
    [SerializeField] GameObject _retroBouclierBrise;
    [SerializeField] GameObject _retroBouclierBloque;

    [Header("Sons")]
    [SerializeField] AudioClip _clipSaut; // Le son du saut.//#tp4 Elyzabelle
    [SerializeField] AudioClip _clipAtterissage; // Le son de l'atterissage.//#tp4 Elyzabelle
    [SerializeField] AudioClip _clipAttaque; // Le son de l'attaque.//#synthese Elyzabelle

    [Header("Instance")]
    static Perso _instance; //Contient l'info du niveau
    static public Perso instance => _instance; //Crée une instance accessible


    void Awake()
    {
        DevenirSingleton();
        _anim = gameObject.GetComponent<Animator>(); //Récupère le composant animator
        _donneesPerso.bouclierBloque.AddListener(ProtegeBouclier);
        _donneesPerso.bouclierBrise.AddListener(BriseBouclier);
    }

    override protected void FixedUpdate()
    {
        base.FixedUpdate();
        VeutBouger();
        AnimerChute();
    }

    void Start()
    {
        Porte.instance.finDuNiveau.AddListener(DesactiverPerso);
        gameObject.SetActive(true);
        _donneesPerso.vies = 3;
        // Si la vitesse actuelle du personnage est supérieure à sa vitesse initiale,multiplier la vitesse actuelle par 3 et changer la vitesse du système de particules en conséquence.//tp3 Patrick
        if (_donneesPerso.vitesse > _donneesPerso.vitesseIni)
        {
            _particule.ChangerVitesse(1.4f); //changer la vitesse des particules //tp3 Patrick
            _particule.ChangerCouleur(ColorUtility.TryParseHtmlString("#00FFEE", out Color color) ? color : Color.white); //changer la couleur des particules //#synthese Elyzabelle
        }
        ControleCinemachineBonus.instance.ChangerCible(transform);//Appel de la fonction ChangerCible de la classe ControleCinemachineBonus//#tp4 Patrick
        ControleCinemachinePrincipale.instance.ChangerCible(transform);//Appel de la fonction ChangerCible de la classe ControleCinemachinePrincipale//#tp4 Patrick
    }

    void OnApplicationQuit()
    {
        _donneesPerso.Initialiser();
    }

    //--------------------------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------------------------
    // Mouvement du personnage
    //--------------------------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// Gère les mouvements du personnage
    /// Vérifie si il peut sauter
    /// Vérifie si il peut double sauter
    /// Fait marcher le personnage  
    /// #synthese Elyzabelle
    /// </summary>
    void VeutBouger()
    {
        if (_veutSauter) // Vérifie si le personnage veut sauter.
        {
            // Calcule la fraction de la force de saut à appliquer:
            float fractionForce = (float)_nbFramesRestants / _nbFramesMax;
            Vector2 vecteurForce = Vector2.up * _forceSaut * fractionForce; // Calcule le vecteur de force de saut.
            _rb.AddForce(vecteurForce); // Applique la force de saut au Rigidbody2D.
            if (_nbFramesRestants > 0) _nbFramesRestants--; // Décrémente le nombre de frames restantes pour le saut.
        }
        else if (_estAuSol) // Vérifie si le personnage est au sol.
        {
            _nbFramesRestants = _nbFramesMax; // Réinitialise le nombre de frames restantes pour le saut au maximum.
            // Active le double saut si le personnage possède cette capacité:
            if (_donneesPerso.possedeDoubleSaut) _peutDoubleSauter = true;
        }
        else // Si le personnage n'est pas au sol.
        {
            _nbFramesRestants = 0; // Réinitialise le nombre de frames restantes pour le saut à zéro.
            // Applique la gravité supplémentaire pour accélérer la chute:
            if (_rb.velocity.y < 0) _rb.velocity += Vector2.up * Physics2D.gravity.y * (GraviteChute - 1) * Time.deltaTime;
        }
        // Applique la vitesse de déplacement horizontal.:
        _rb.velocity = new Vector2(_axeHorizontal * _donneesPerso.vitesse, _rb.velocity.y);
    }


    /// <summary>
    /// Gère le saut du personnage
    /// Anime le saut
    /// Joue le son du saut
    ///  #synthèse Elyzabelle
    /// </summary>
    void Sauter()
    {
        if (_estAuSol || _peutDoubleSauter) // Vérifie si le personnage est au sol ou veut effectuer un double saut.
        {
            _veutSauter = true; // Indique que le personnage veut sauter.
            _anim.SetTrigger("Sauter");
            GestAudio.instance.JouerEffetSonore(_clipSaut, 0.2f); //Joue le son du saut.//#tp4 Elyzabelle
            _nbFramesRestants = _nbFramesMax; // Initialise le nombre de frames restantes pour le saut au maximum.
            // Annule la demande de double saut après l'avoir utilisée:
            if (!_estAuSol && _peutDoubleSauter) _peutDoubleSauter = false;
        }
    }

    /// <summary>
    /// Fonction qui gère l'attaque de l'ennemi
    /// lorsque le personnage est touché.
    /// Retire la vie au joueur selon les dommages provoqués
    /// par l'ennemi
    /// #synthese Elyzabelle
    /// </summary>
    /// <param name="colliderEnnemi"> Le collider de l'ennemi.</param>
    void GererAttaqueEnnemi(Collider2D colliderEnnemi)
    {
        if (_estUneSeuleAttaque) //si c'est une seule attaque //#synthese Elyzabelle
        {
            int dommages; //dommages provoqués par l'ennemi
            //lance la coroutine pour faire 1 degat par attaque //#synthese Elyzabelle:
            StartCoroutine(CoroutineDelaisAttaque());
            if (colliderEnnemi.GetComponent<ArmeEnnemi>() != null)
            {
                ArmeEnnemi armeEnnemi = colliderEnnemi.GetComponent<ArmeEnnemi>();
                armeEnnemi.AppliquerForceAttaque(_rb); //Applique une force au personnage
                dommages = colliderEnnemi.GetComponent<ArmeEnnemi>().Blesser(); //Retire la vie au personnage
            }
            else //si l'ennemi n'a pas de script 'ArmeEnnemi'
            {
                Debug.LogWarning("L'ennemi n'a pas de script 'ArmeEnnemi'. L'attaque n'a pas pu se faire.");
                dommages = 0;
            }
            _donneesPerso.RetirerVies(dommages); //Retire une vie //#synthese Elyzabelle
        }
        else return;
    }

    /// <summary>
    /// Coroutine permettant de faire
    /// 1 de dégats par attaque
    /// #synthese Elyzabelle
    /// </summary>
    IEnumerator CoroutineDelaisAttaque()
    {
        _estUneSeuleAttaque = false;
        yield return new WaitForSeconds(1f);
        _estUneSeuleAttaque = true;
    }

    //--------------------------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------------------------
    // Animation du personnage
    //--------------------------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// Permet d'activer l'animation de course
    /// lorsque le personnage est en déplacement
    /// #tp3 Elyzabelle
    /// </summary>
    void AnimerMarche()
    {
        if (_axeHorizontal != 0) _anim.SetBool("Courir", true);
        else { _anim.SetBool("Courir", false); }
    }

    /// <summary>
    /// Si le personnage est au sol, l'animation de chute est arretée
    /// sinon, l'animation de chute est activée
    /// #synthese Elyzabelle
    /// </summary>
    void AnimerChute()
    {
        if (_estAuSol) _anim.SetBool("ArreterChute", true);
        else { _anim.SetBool("ArreterChute", false); }
    }

    //--------------------------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------------------------
    // Input
    //--------------------------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// Controle le déplace horizontale du personnage
    /// à l'aide du clavier et d'une manette.
    /// </summary>
    /// <param name="value">Les déférente touche du clavier (A/D) ou du jostique de la manette</param>
    void OnMove(InputValue value)
    {
        if (_estAuSol) _particule.ArreterOuDemarrer(true);
        else _particule.ArreterOuDemarrer(false);

        // Récupère la valeur de l'axe horizontal à partir de l'entrée utilisateur:
        _axeHorizontal = value.Get<Vector2>().x;
        AnimerMarche();

        if (_axeHorizontal < 0)
        {
            _sr.flipX = true; // Retourne le personnage horizontalement s'il se déplace vers la gauche.
            _particule.ChangerDirection(true); // Change la direction du système de particules.//tp3 Patrick
        }
        else if (_axeHorizontal > 0)
        {
            _sr.flipX = false; // Ne retourne pas le personnage s'il se déplace vers la droite.
            _particule.ChangerDirection(false); // Change la direction du système de particules.//tp3 Patrick
        }
    }

    void OnPouvoir()
    {
        activerPouvoir.Invoke();
    }

    void OnJump(InputValue value)
    {
        if (value.isPressed) Sauter(); //Fait sauter le personnage.
        else _veutSauter = false; // Indique que le personnage ne veut pas sauter.
    }

    void OnFire()
    {
        GestAudio.instance.JouerEffetSonore(_clipAttaque); //Joue le son de l'attaque
        if (_sr.flipX) _anim.SetTrigger("AttaqueGauche"); //Anime l'attaque vers la gauche (changement de sens du collider)
        else if (!_sr.flipX) _anim.SetTrigger("Attaque"); //Anime l'attaque vers la droite
    }

    //--------------------------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------------------------
    // Rétroaction
    //--------------------------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// Instancie une rétroaction à la position du personnage
    /// pour indiquer que sonte bouclier est brisé.
    /// #synthese Elyzabelle
    /// </summary>
    void BriseBouclier()
    {
        Instantiate(_retroBouclierBrise, transform.position, Quaternion.identity);
    }

    /// <summary>
    /// Instancie une rétroaction à la position du personnage
    /// pour indiquer que son bouclier a contré les dégats.
    /// #synthese Elyzabelle
    /// </summary>
    void ProtegeBouclier()
    {
        Instantiate(_retroBouclierBloque, transform.position, Quaternion.identity);
    }

    //--------------------------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------------------------
    // Collisions
    //--------------------------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------------------------

    void OnCollisionEnter2D(Collision2D other)
    {
        //Joue le son de l'atterissage.//#tp4 Elyzabelle:
        if (_estAuSol) GestAudio.instance.JouerEffetSonore(_clipAtterissage);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "ArmeEnnemi") GererAttaqueEnnemi(other);
    }

    //--------------------------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------------------------
    // Malus
    //--------------------------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// Démarre une coroutine pour changer la taille de l'objet.
    /// #synthese Patrick
    /// </summary>
    /// <param name="nouvelleTaille">La nouvelle taille cible.</param>
    /// <param name="tempsChangement">Le temps en secondes pour effectuer le changement.</param>
    public void ChangerGrandeur(float nouvelleTaille, float tempsChangement)
    {
        // Lance la coroutine pour changer la taille de l'objet.
        StartCoroutine(CoroutineChangerGrandeur(nouvelleTaille, tempsChangement));
    }

    /// <summary>
    /// Coroutine pour interpoler la taille de l'objet vers une nouvelle taille sur une durée spécifiée.
    /// #synthese Patrick
    /// </summary>
    /// <param name="nouvelleTaille">La nouvelle taille cible.</param>
    /// <param name="tempsChangement">Le temps en secondes pour effectuer le changement.</param>
    IEnumerator CoroutineChangerGrandeur(float nouvelleTaille, float tempsChangement)
    {
        // Stocke la nouvelle taille initiale
        float nouvelleTailleIni = nouvelleTaille;
        // Diminue légèrement la taille cible
        nouvelleTaille = nouvelleTaille - 0.2f;
        // Crée un vecteur pour la nouvelle taille cible
        Vector3 tailleCible = new Vector3(nouvelleTaille, nouvelleTaille, 1);
        // Sauvegarde la taille actuelle de l'objet
        Vector3 tailleInitialeActuelle = transform.localScale;

        // Initialise le compteur de temps
        float temps = 0;

        // Interpole la taille de l'objet sur la durée spécifiée
        while (temps < tempsChangement)
        {
            // Lerp la taille de l'objet entre la taille initiale et la taille cible
            transform.localScale = Vector3.Lerp(tailleInitialeActuelle, tailleCible, temps / tempsChangement);
            // Augmente le compteur de temps
            temps += Time.deltaTime;
            // Attend le prochain frame
            yield return null;
        }

        // Fixe la taille de l'objet à la nouvelle taille initiale
        transform.localScale = new Vector3(nouvelleTailleIni, nouvelleTailleIni, 1);
        // Lance la coroutine pour revenir à la taille initiale après un délai
        StartCoroutine(CoroutineRevenirGrandeurInitial(tempsChangement));
    }

    /// <summary>
    /// Coroutine pour revenir à la taille initiale de l'objet après un délai.
    /// #synthèse Patrick
    /// </summary>
    /// <param name="tempsChangement">Le temps en secondes pour revenir à la taille initiale.</param>
    IEnumerator CoroutineRevenirGrandeurInitial(float tempsChangement)
    {
        // Attend le temps du bonus (_tempsBonus doit être défini ailleurs dans la classe)
        yield return new WaitForSeconds(_tempsBonus);

        // Vérifie si l'objet est toujours sur une tuile
        if (_estTuileDessus)
        {
            // Relance la coroutine si l'objet est toujours sur une tuile
            StartCoroutine(CoroutineRevenirGrandeurInitial(tempsChangement));
        }
        else
        {
            // Sauvegarde la taille actuelle de l'objet
            Vector3 tailleInitialeActuelle = transform.localScale;
            // Définit la taille cible légèrement augmentée
            float tailleCible = 1f + 0.2f;
            Vector3 tailleInitiale = new Vector3(tailleCible, tailleCible, 1);
            // Initialise le compteur de temps
            float temps = 0;

            // Interpole la taille de l'objet vers la taille initiale sur la durée spécifiée
            while (temps < tempsChangement)
            {
                // Lerp la taille de l'objet entre la taille actuelle et la taille initiale
                transform.localScale = Vector3.Lerp(tailleInitialeActuelle, tailleInitiale, temps / tempsChangement);
                // Augmente le compteur de temps
                temps += Time.deltaTime;
                // Attend le prochain frame
                yield return null;
            }

            // Fixe la taille de l'objet à la taille initiale
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    //--------------------------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------------------------
    // Généraux
    //--------------------------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// Fonction qui crée un singleton
    /// #synthese Elyzabelle
    /// </summary>
    void DevenirSingleton()
    {
        if (_instance != null) Destroy(gameObject);//destruction du gameObject//#tp4 Patrick
        else _instance = this;//initialisation de l'instance//#tp4 Patrick
    }

    void DesactiverPerso()
    {
        gameObject.SetActive(false);
    }
}