using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Script qui gere les fonctionnalités
/// principales des ennemis
/// #synthese Elyzabelle
/// #synthese Patrick
/// </summary>
public class Ennemis : MonoBehaviour
{
    [Header("Paramètres")]
    [SerializeField] GameObject[] _cadeaux; //Les cadeaux possibles à instancier lors de la mort //#synthese Elyzabelle
    [SerializeField] int _vies = 5; //Nombre de vies //#synthese Elyzabelle
    [SerializeField] float _vitesse = 1f; //Vitesse actuelle //#synthese Elyzabelle
    [SerializeField] protected float _vitesseChasse = 2f; //Vitesse lors de la chasse //#synthese Elyzabelle
    [SerializeField] float _distanceDebutSol = 0.5f;//Définissez la distance de debut du sol//#synthese Patrick
    [SerializeField] float _longueurRayon = 0.6f;//Longueur du rayon//#synthese Patrick
    [SerializeField] float _offsetX = 0.6f;//Offset du rayon en x//#synthese Patrick
    [SerializeField] float _offsetY = 0.6f;//Offset du rayon en y//#synthese Patrick
    [SerializeField] float _offsetY2 = 0.6f;//Offset du rayon en y//#synthese Patrick
    [SerializeField] float _distanceDetectionPerso = 5.0f; // Définissez la distance de départ
    Coroutine _coroutinePatrouille; //Coroutine permettant à l'ennemi de patrouiller //#synthese Elyzabelle

    [Header("Sons")]
    [SerializeField] protected AudioClip _clipAttaque;
    [SerializeField] AudioClip _clipBlessure;
    [SerializeField] AudioClip _clipMort;

    [Header("Booleens")]
    bool _estProchaineTuileVideD = false;//Permet de savoir si la prochaine tuile du sol à droite est vide //#synthese Patrick
    bool _estProchaineTuileVideG = false;//Permet de savoir si la prochaine tuile du sol à gauche est vide//#synthese Patrick
    bool _estMurD = false;//Permet de savoir si la prochaine tuile à droite est un mur//#synthese Patrick
    bool _estMurD2 = false;//Permet de savoir si la prochaine tuile à droite est un mur//#synthese Patrick
    bool _estMurG = false;//Permet de savoir si la prochaine tuile à gauche est un mur//#synthese Patrick
    bool _estMurG2 = false;//Permet de savoir si la prochaine tuile à gauche est un mur//#synthese Patrick
    bool _estUneSeuleAttaque = true; //Permet de savoir si c'est une seule attaque //#synthese Elyzabelle
    protected bool _suitLeJoueur = false; //Permet de savoir si l'ennemi est en chasse //#synthese Elyzabelle
    bool deplacementADroite = true; // Variable pour suivre la direction actuelle de l'ennemi

    [Header("Valeurs initiales")]
    [SerializeField] float _vitesseIni = 0.5f; //Vitesse initiale //#synthese Elyzabelle

    [Header("Composants")]
    [SerializeField] protected Animator _anim; //Permet d'animer les ennemis //#synthese Elyzabelle
    [SerializeField] LayerMask _layerMask;//Permet de filtrer les collisions//#synthese Patrick
    [SerializeField] SpriteRenderer _sr; //Permet de flipper le sprite //#synthese Elyzabelle
    [SerializeField] Rigidbody2D _rb; //Permet de flipper le sprite //#synthese Elyzabelle

    void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    void Start()
    {
        _coroutinePatrouille = StartCoroutine(CoroutinePatrouille());
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "ArmePerso") //si l'ennemi est attaqué //#synthese Elyzabelle
        {
            if (_estUneSeuleAttaque) //si c'est une seule attaque //#synthese Elyzabelle
            {
                StartCoroutine(CoroutineDelaisAttaque()); //lance la coroutine pour faire 1 degat par attaque //#synthese Elyzabelle
                PerdreVie(); //Retire une vie //#synthese Elyzabelle
                Arme arme = other.GetComponent<Arme>();
                arme.AppliquerForceAttaque(_rb); //Applique une force à l'ennemi
            }
            else return;
        }
        //si la collision n'est pas avec le perso, changement de direction //#synthese Elyzabelle:
        else if (other.tag != "Perso" || other.tag != null) deplacementADroite = !deplacementADroite;
    }
    virtual protected void FixedUpdate()
    {
        VerifierSol();
        VerifierMur();
        DetecterPerso();
        ChasserJoueur();
    }

    //--------------------------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------------------------
    // Patrouille
    //--------------------------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// Permet de changer la direction de l'ennemi
    /// lorsqu'il rencontre un obstacle
    /// #synthese Elyzabelle
    /// </summary>
    IEnumerator CoroutinePatrouille()
    {
        while (true)
        {
            // Calcul du déplacement horizontal en fonction de la direction actuelle:
            float deplacement = deplacementADroite ? _vitesse : -_vitesse;

            // Déplacement de l'ennemi:
            transform.Translate(Vector3.right * deplacement * Time.deltaTime);
            // Flip le sprite pour le changement de direction:
            if (deplacement > 0)
            {
                _sr.flipX = true;
                //Flipper le collider de l'arme
                // _colliderArme.transform.Rotate(0, 180, 0);
            }
            else
            {
                _sr.flipX = false;
                // _colliderArme.transform.Rotate(0, 0, 0);
            }

            // Vérification des collisions avec les objets:
            if (_estMurD || _estMurD2 || _estProchaineTuileVideD && deplacementADroite)
            {
                // Si une collision est détectée à droite, changer de direction:
                deplacementADroite = false;
            }
            else if (_estMurG || _estMurG2 || _estProchaineTuileVideG && !deplacementADroite)
            {
                // Si une collision est détectée à gauche, changer de direction:
                deplacementADroite = true;
            }
            yield return null;
        }
    }

    //--------------------------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------------------------
    // L'ennemi pourchasse le joueur
    //--------------------------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// Fonction permettant de faire bouger l'ennemi
    /// vers le joueur pour l'attaquer lorsqu'il est proche
    /// #synthese Elyzabelle  
    /// </summary>
    void ChasserJoueur()
    {
        if (_suitLeJoueur)
        {
            StopCoroutine(_coroutinePatrouille);
            //Trouve l'instance du perso sur la scène:
            GameObject instancePerso = GameObject.FindGameObjectWithTag("Perso");
            if (instancePerso == null) return;
            Vector3 perso = instancePerso.GetComponent<Transform>().position; // Position du perso

            // Distance entre le perso et l'ennemi
            float distance = Vector2.Distance(perso, transform.position);

            // Si le perso est dans la distance de déplacement
            if (distance < _distanceDetectionPerso)
            {

                if (perso.x > transform.position.x) // Déplacement de l'ennemi vers le perso vers la droite
                {
                    _sr.flipX = true; // Flip le sprite pour le changement de direction
                    deplacementADroite = false;
                }
                else // Déplacement de l'ennemi vers le perso vers la gauche
                {
                    _sr.flipX = false; // Flip le sprite pour le changement de direction
                    deplacementADroite = true;
                }

                // Déplacement de l'ennemi vers le joueur
                transform.position = Vector2.MoveTowards(transform.position, perso, _vitesse * Time.deltaTime);
            }
        }
    }

    /// <summary>
    /// Active la chasse
    /// #synthese Elyzabelle
    /// </summary>
    void ActiverChasse()
    {
        _suitLeJoueur = true;
        StopCoroutine(_coroutinePatrouille);
    }

    /// <summary>
    /// Désactive la chasse
    /// #synthese Elyzabelle
    /// </summary>
    void DesactiverChasse()
    {
        _suitLeJoueur = false;
        _vitesse = _vitesseIni;
        //Arreter la coroutine si elle est en cours
        if (_coroutinePatrouille != null) StopCoroutine(_coroutinePatrouille);
        _coroutinePatrouille = StartCoroutine(CoroutinePatrouille()); //Lancer la coroutine
    }

    //--------------------------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------------------------
    // Attaque
    //--------------------------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// Coroutine permettant de faire
    /// 1 de dégats par attaque
    /// #synthese Elyzabelle
    /// </summary>
    IEnumerator CoroutineDelaisAttaque()
    {
        _estUneSeuleAttaque = false;
        yield return new WaitForSeconds(0.4f);
        _estUneSeuleAttaque = true;
    }

    //--------------------------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------------------------
    // Gestion de la vie
    //--------------------------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// Gérer la mort et instancier cadeau
    /// #synthese Elyzabelle
    /// </summary>
    void PerdreVie()
    {
        if (_vies == 0)
        {
            GestAudio.instance.JouerEffetSonore(_clipMort); //Joue le son de mort
            _anim.SetTrigger("Mourir"); //Animation de mort
            DonnerCadeau();
        }
        else
        {
            GestAudio.instance.JouerEffetSonore(_clipBlessure); //Joue le son de blessure
            _vies--; //Diminution du nombre de vies
            _anim.SetTrigger("Blesser"); //Animation lorsque le personnage est blessé
        }
    }

    /// <summary>
    /// Instancie les cadeaux lors
    /// de la mort d'un ennemi
    /// à la fin de l'animation de mort
    /// #synthese Elyzabelle
    /// </summary>
    void DonnerCadeau()
    {
        for (int i = 0; i < _cadeaux.Length; i++) // Boucle pour instancier les cadeaux
        {
            int qte = Random.Range(0, 10); // Quantité de cadeaux possible à instancier
            for (int b = 0; b < qte; b++) Instantiate(_cadeaux[i], transform.position, Quaternion.identity); // Instanciation
        }
        Destroy(gameObject);
    }

    //--------------------------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------------------------
    // Zone de detection du perso
    //--------------------------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// Fonction qui permet de detecter le perso dans une zone predefinie
    /// #synthese Patrick
    /// </summary>
    private void DetecterPerso()
    {
        //Si le perso est dans la distance de déplacement//#synthese Patrick
        if (Vector2.Distance(transform.position, Perso.instance.transform.position) < _distanceDetectionPerso)
        {
            _vitesse = _vitesseChasse; //Augemente la vitesse de déplacement //#synthese Elyzabelle
            ActiverChasse();
        }
        else
        {
            _vitesse = _vitesseIni; //Remet la vitesse initiale //#synthese Elyzabelle
            DesactiverChasse();
        }
    }

    //--------------------------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------------------------
    // Gestion des collisions
    //--------------------------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// Fonction qui permet de savoir si la prochaine tuile est vide
    /// #synthese Patrick
    /// </summary>
    private void VerifierSol()
    {
        // Calcul le point de depart du rayon droit avec la distance de debut du sol et la position de l'ennemi
        Vector2 pointDepartDroite = (Vector2)transform.position + new Vector2(_offsetX, -_distanceDebutSol);
        // crée un rayon vers le bas de la longuer du rayon
        RaycastHit2D hitDroite = Physics2D.Raycast(pointDepartDroite, Vector2.down, _longueurRayon, _layerMask);
        RaycastHit2D hitDroite2 = Physics2D.Raycast(pointDepartDroite, Vector2.down, _longueurRayon, _layerMask);

        // Calcul le point de depart du rayon gauche avec la distance de debut du sol et la position de l'ennemi
        Vector2 pointDepartGauche = (Vector2)transform.position - new Vector2(_offsetX, _distanceDebutSol);
        // crée un rayon vers le haut de la longuer du rayon
        RaycastHit2D hitGauche = Physics2D.Raycast(pointDepartGauche, Vector2.down, _longueurRayon, _layerMask);
        RaycastHit2D hitGauche2 = Physics2D.Raycast(pointDepartGauche, Vector2.down, _longueurRayon, _layerMask);

        // Vérifie si l'un des rayons a frappé quelque chose
        _estProchaineTuileVideD = hitDroite.collider == null && hitDroite2.collider == null;
        _estProchaineTuileVideG = hitGauche.collider == null && hitGauche2.collider == null;
    }

    /// <summary>
    /// Fonction qui permet de savoir si la prochaine tuile est un mur
    /// #synthese Patrick
    /// </summary>
    private void VerifierMur()
    {
        // Calcul le point de depart du rayon droit avec la distance de debut du sol et la position de l'ennemi
        Vector2 pointDepartDroite = (Vector2)transform.position - new Vector2(-_distanceDebutSol, _offsetY);
        // crée un rayon vers la droite de la longuer du rayon
        RaycastHit2D hitDroite = Physics2D.Raycast(pointDepartDroite, Vector2.right, _longueurRayon, _layerMask);

        // Calcul le point de depart du rayon droit avec la distance de debut du sol et la position de l'ennemi
        Vector2 pointDepartDroite2 = (Vector2)transform.position - new Vector2(-_distanceDebutSol, _offsetY2);
        // crée un rayon vers la droite de la longuer du rayon
        RaycastHit2D hitDroite2 = Physics2D.Raycast(pointDepartDroite2, Vector2.right, _longueurRayon, _layerMask);

        // Calcul le point de depart du rayon gauche avec la distance de debut du sol et la position de l'ennemi
        Vector2 pointDepartGauche = (Vector2)transform.position - new Vector2(_distanceDebutSol, _offsetY);
        // crée un rayon vers la gauche de la longuer du rayon
        RaycastHit2D hitGauche = Physics2D.Raycast(pointDepartGauche, Vector2.left, _longueurRayon, _layerMask);

        // Calcul le point de depart du rayon gauche avec la distance de debut du sol et la position de l'ennemi
        Vector2 pointDepartGauche2 = (Vector2)transform.position - new Vector2(_distanceDebutSol, _offsetY2);
        // crée un rayon vers la gauche de la longuer du rayon
        RaycastHit2D hitGauche2 = Physics2D.Raycast(pointDepartGauche2, Vector2.left, _longueurRayon, _layerMask);

        // Vérifie si l'un des rayons a frappé quelque chose
        _estMurD = hitDroite.collider != null;
        _estMurD2 = hitDroite2.collider != null;
        _estMurG = hitGauche.collider != null;
        _estMurG2 = hitGauche2.collider != null;
    }

    /// <summary>
    /// Méthode utilisée pour dessiner des gizmos dans l'éditeur Unity.
    /// #synthese Patrick
    /// </summary>
    virtual protected void OnDrawGizmos()
    {
        // Change la couleur du gizmo en vert si _estProchaineTuileVideD est vrai, sinon en rouge
        Gizmos.color = _estProchaineTuileVideD ? Color.green : Color.red;
        // Détermine le point de départ du rayon à droite basé sur la position de l'objet et un décalage
        Vector2 pointDepartDroite = (Vector2)transform.position + new Vector2(_offsetX, -_distanceDebutSol);
        // Dessine un rayon vers le bas depuis le point de départ
        Gizmos.DrawRay(pointDepartDroite, Vector2.down * _longueurRayon);

        // Change la couleur du gizmo en vert si _estProchaineTuileVideG est vrai, sinon en rouge
        Gizmos.color = _estProchaineTuileVideG ? Color.green : Color.red;
        // Détermine le point de départ du rayon à gauche basé sur la position de l'objet et un décalage
        Vector2 pointDepartGauche = (Vector2)transform.position - new Vector2(_offsetX, _distanceDebutSol);
        // Dessine un rayon vers le bas depuis le point de départ
        Gizmos.DrawRay(pointDepartGauche, Vector2.down * _longueurRayon);

        // Change la couleur du gizmo en vert si _estMurD est vrai, sinon en rouge
        Gizmos.color = _estMurD ? Color.green : Color.red;
        // Détermine le point de départ du rayon à droite (mur) basé sur la position de l'objet et un décalage
        Vector2 pointDepartMurD = (Vector2)transform.position - new Vector2(-_distanceDebutSol, _offsetY);
        // Dessine un rayon vers la droite depuis le point de départ
        Gizmos.DrawRay(pointDepartMurD, Vector2.right * _longueurRayon);

        // Change la couleur du gizmo en vert si _estMurD est vrai, sinon en rouge
        Gizmos.color = _estMurD2 ? Color.green : Color.red;
        // Détermine le point de départ du rayon à droite (mur) basé sur la position de l'objet et un décalage
        Vector2 pointDepartMurD2 = (Vector2)transform.position - new Vector2(-_distanceDebutSol, _offsetY2);
        // Dessine un rayon vers la droite depuis le point de départ
        Gizmos.DrawRay(pointDepartMurD2, Vector2.right * _longueurRayon);

        // Change la couleur du gizmo en vert si _estMurG est vrai, sinon en rouge
        Gizmos.color = _estMurG ? Color.green : Color.red;
        // Détermine le point de départ du rayon à gauche (mur) basé sur la position de l'objet et un décalage
        Vector2 pointDepartMurG = (Vector2)transform.position - new Vector2(_distanceDebutSol, _offsetY);
        // Dessine un rayon vers la gauche depuis le point de départ
        Gizmos.DrawRay(pointDepartMurG, Vector2.left * _longueurRayon);

        // Change la couleur du gizmo en vert si _estMurG est vrai, sinon en rouge
        Gizmos.color = _estMurG2 ? Color.green : Color.red;
        // Détermine le point de départ du rayon à gauche (mur) basé sur la position de l'objet et un décalage
        Vector2 pointDepartMurG2 = (Vector2)transform.position - new Vector2(_distanceDebutSol, _offsetY2);
        // Dessine un rayon vers la gauche depuis le point de départ
        Gizmos.DrawRay(pointDepartMurG2, Vector2.left * _longueurRayon);

        // Change la couleur du gizmo en vert pour dessiner une sphère filaire
        Gizmos.color = Color.green;
        // Dessine une sphère filaire autour de la position de l'objet avec un rayon de _distanceDetectionPerso
        Gizmos.DrawWireSphere(transform.position, _distanceDetectionPerso);
    }

}