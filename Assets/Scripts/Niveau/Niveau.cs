using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Classe qui contrôle le niveau et qui instentie les differente salle aléatoirement.
/// elle permet aussi d'avoir les tuiles des tilemap des salles 
/// Auteurs du code : Elyzabelle Rollin et Patrick Watt-Charron
/// Auteur des commentaires : Patrick Watt-Charron
/// </summary>
public class Niveau : MonoBehaviour
{
    [Header("Références")]
    [SerializeField] Clef _clef;//Prefab de la clef //tp3 Patrick
    [SerializeField] Porte _porte;//Prefab de la porte //tp3 Patrick
    [SerializeField] Perso _perso;//Prefab du perso //tp3 Patrick
    [SerializeField] Activateur _activateur;//Prefab de l'activateur //tp3 Patrick
    [SerializeField] Salle[] _tSallesModeles; // Tableau des salles modèles à instancier
    [SerializeField] SOPerso _donneesPerso;
    [SerializeField] Tilemap _tilemap; // Tilemap utilisée pour le rendu des tuiles
    [SerializeField] GameObject[] _tBonusModeles; //Prefab des objets //#tp3 Elyzabelle
    [SerializeField] GameObject[] _tJoyauxModeles; //Prefab des objets //#tp3 Elyzabelle
    [SerializeField] TileBase _tuileModele; // Tuile de modèle pour fermer les limites du jeu
    [SerializeField] GameObject[] _ennemis; //Prefab des ennemis //#tp3 Elyzabelle
    [SerializeField] GameObject _effecteur; //Pour obtenir les positions des effecteurs //#tp3 Elyzabelle

    [Header("Paramètres")]
    [SerializeField, Range(0, 100)] int _probabiliteBonus; //Probabilité d'apparition des bonus //#tp3 Elyzabelle
    [SerializeField, Range(0, 100)] int _probabiliteJoyaux; //Probabilité d'apparition des joyaux //#tp3 Elyzabelle
    [SerializeField] Vector2Int _nbSalles = new Vector2Int(3, 3); // Nb de salles instanciées en x et y
    List<Vector2> _posPredefinieObject = Salle.lesPosObject; // Liste des positions prédéfinies des objets dans la salle.//tp3 Patrick
    List<Vector2> _lesPosSalle = new List<Vector2>(); // Liste des positions des salles où les objets peuvent être instanciés.//tp3 Patrick
    Vector2 _activateurDeplacement = new Vector2(0, -0.7f); // Vecteur de déplacement pour l'instanciation de l'activateur.//tp3 Patrick
    Vector2 _clefDeplacement = new Vector2(0, -0.5f); // Vecteur de déplacement pour l'instanciation de la clé.//tp3 Patrick
    [SerializeField] int _nbObjetsParSalle = 200; //Nb d'objets instanciés par parcelle
    Vector2Int _tailleAvecUneBordure; // Taille des salles avec une bordure supplémentaire
    List<Vector2Int> _lesPosDisponibles = new List<Vector2Int>();//Liste des positions disponibles //#tp3 Elyzabelle
    [SerializeField, Range(0, 500)] int _nbEnnemis = 4;//Nb d'ennemis instanciés//#synthese Elyzabelle

    [Header("Instance")]
    static Niveau _instance; //Contient l'info du niveau
    static public Niveau instance => _instance; //Crée une instance accessible



    void Awake()
    {
        if (_donneesPerso.niveau == 1) _nbSalles = new Vector2Int(_donneesPerso.niveau + 1, 2);
        else _nbSalles = new Vector2Int(_donneesPerso.niveau + 1, 3);
        _nbEnnemis = _nbEnnemis * _donneesPerso.niveau;
        if (_instance != null) { Destroy(gameObject); return; } //Singleton
        _instance = this;
        InstancierSalles();
        PlacerLesObjets();
        InstancierEnnemis();
    }

    //--------------------------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------------------------
    // Instancier les salles
    //--------------------------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// Cette fonction instancie des salles dans le niveau en utilisant des salles modèles prédéfinies. 
    /// Elle parcourt une grille de taille définie, crée une salle aléatoire à chaque position et lui assigne un nom. 
    /// </summary>
    private void InstancierSalles()
    {

        _tailleAvecUneBordure = Salle.tailleAvecBordures - Vector2Int.one; // Définition de la taille avec une bordure
        for (int y = 0; y < _nbSalles.y; y++) // Boucle sur l'axe Y
        {
            for (int x = 0; x < _nbSalles.x; x++) // Boucle sur l'axe X
            {
                Vector2 pos = new Vector2(_tailleAvecUneBordure.x * x, _tailleAvecUneBordure.y * y); // Position de la salle à instancier
                Salle salle = Instantiate(_tSallesModeles[Random.Range(0, _tSallesModeles.Length)], pos, Quaternion.identity, transform); // Instanciation de la salle
                salle.name = "Salle_" + x + "_" + y; // Définition du nom de la salle
                _lesPosSalle.Add(pos);//Rajoute la postion de la salle a la liste des position de salle//tp3 Patrick
            }
        }
        InstancierClef();// Appel de la méthode pour instantier l'activateur //tp3 Patrick
        FermerLimiteJeu(); // Appel de la méthode pour fermer les limites du jeu
    }


    /// <summary>
    /// Cette fonction est responsable de la fermeture des limites du jeu en plaçant des tuiles modèles le long des bords. 
    /// Elle calcule la taille de la zone de jeu, détermine les coins inférieur gauche et supérieur droit, 
    /// puis boucle à travers cette zone pour placer les tuiles modèles sur les bords gauche, droit, haut et bas.
    /// </summary>
    private void FermerLimiteJeu()
    {
        Vector2Int tailleTable = new Vector2Int(_nbSalles.x, _nbSalles.y) * _tailleAvecUneBordure; // Calcul de la taille de la table
        Vector2Int min = Vector2Int.zero - Salle.tailleAvecBordures / 2; // Calcul du coin inférieur gauche
        Vector2Int max = min + tailleTable; // Calcul du coin supérieur droit
        for (int y = min.y; y <= max.y; y++) // Boucle sur l'axe Y
        {
            for (int x = min.x; x <= max.x; x++) // Boucle sur l'axe X
            {
                Vector3Int pos = new Vector3Int(x, y); // Position de la tuile
                if (x == min.x || x == max.x) _tilemap.SetTile(pos, _tuileModele); // Si sur le bord gauche ou droit, placer la tuile modèle
                if (y == min.y || y == max.y) _tilemap.SetTile(pos, _tuileModele); // Si sur le bord haut ou bas, placer la tuile modèle
            }
        }
        TrouverLesPosLibre();
    }

    /// <summary>
    /// Transfère les tuiles choisies de 
    /// l'ancienne tilemap sur la nouvelle 
    /// </summary>
    /// <param name="pos">position de la tuile</param>
    /// <param name="tuile">la tuile</param>
    /// <param name="posTilemap">position de la tilemap</param>
    public void TransfererTuile(Vector3Int pos, TileBase tuile, Vector3 posTilemap)
    {
        Vector3 decalage = posTilemap - _tilemap.transform.position;
        pos += Vector3Int.FloorToInt(decalage);
        if (tuile != null)
        {
            _tilemap.SetTile(pos, _tuileModele); // Si la tuile existe, placer la même tuile sur la tilemap actuelle //#tp3 Elyzabelle   
        }
    }

    /// <summary>
    /// Fonction qui permet de trouver les positions libres 
    /// dans la tilemap cible
    /// #tp3 Elyzabelle
    /// </summary>
    void TrouverLesPosLibre()
    {
        BoundsInt bornes = _tilemap.cellBounds; //Les limites de la tilemap
        for (int x = bornes.xMin; x < bornes.xMax; x++) //Parcours les tuiles en x
        {
            for (int y = bornes.yMin; y < bornes.yMax; y++) //Parcours les tuiles en y
            {
                Vector2Int posTuile = new Vector2Int(x, y); //position de la tuile 
                TileBase tuile = _tilemap.GetTile((Vector3Int)posTuile); //tuile à la position sur la tilemap
                Vector2 posEffecteur = _effecteur.transform.position; //position de l'effecteur //#tp3 Elyzabelle
                Vector2Int posEffecteurTuile = (Vector2Int)_tilemap.WorldToCell(posEffecteur); //position de l'effecteur par rapport à la tilemap //#tp3 Elyzabelle
                if (tuile == null && posTuile != posEffecteurTuile) _lesPosDisponibles.Add(posTuile); //Si il n'y a pas de tuiles, ajouter la position //#tp3 Elyzabelle
            }
        }
    }

    //--------------------------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------------------------
    // Instancier la clé
    //--------------------------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// Cette fonction est responsable d'instancier la clef dans le niveau.//tp3 Patrick
    /// #synthese Elyzabelle
    /// </summary>
    private void InstancierClef()
    {
        int posSalleClef = Random.Range(0, _lesPosSalle.Count); // Sélectionne une position aléatoire parmi les salles disponibles pour la clé.//tp3 Patrick

        if (_lesPosSalle[posSalleClef].y != 0) // Vérifie si la salle contient une sortie pour la clé.//tp3 Patrick
        {
            Vector3Int posAleatoire; // Position de la clé
            do posAleatoire = ChoisirUnePosAleatoireDansUneSalle(posSalleClef);// Sélectionne une position aléatoire parmi les salles disponibles pour la clé//#synthese Elyzabelle
            while (!AUnSolDessous(posAleatoire));// Tant qu'il n'y a pas de sol en dessous de la clé//#synthese Elyzabelle
            Vector3 clefPosition = _tilemap.GetCellCenterWorld(posAleatoire) + Vector3.up * _tilemap.cellSize.y;// Position de la clé dans la salle//#synthese Elyzabelle
            // Instancie la clé à la position trouvée avec un sol en dessous.//tp3 Patrick
            Instantiate(_clef, (Vector2)clefPosition - Vector2.up / 2 + _clefDeplacement, Quaternion.identity);
            _lesPosDisponibles.Remove(Vector2Int.FloorToInt(clefPosition));
            InstancierPorte(posSalleClef); // Appelle la méthode pour instancier la porte.//tp3 Patrick
        }
        else InstancierClef(); // Retry l'instanciation de la clé dans une autre salle si la salle sélectionnée n'a pas de sortie.//tp3 Patrick
    }

    /// <summary>
    /// Cette fonction retourne une position aléatoire dans une salle donnée.
    /// #synthese Elyzabelle
    /// </summary>
    /// <param name="posSalleClef">Index de la salle</param>
    /// <returns>Position aléatoire dans la salle</returns>
    private Vector3Int ChoisirUnePosAleatoireDansUneSalle(int posSalleClef)
    {
        Vector2 sallePos = _lesPosSalle[posSalleClef]; // Position de la salle
        Vector2Int grandeurSalle = Salle.tailleAvecBordures - Vector2Int.one; // Grandeur de la salle
        int x = Random.Range((int)sallePos.x, (int)sallePos.x + grandeurSalle.x); // Position x
        int y = Random.Range((int)sallePos.y, (int)sallePos.y + grandeurSalle.y); // Position y
        return new Vector3Int(x, y, 0); // Retourne la position dans la salle
    }

    /// <summary>
    /// Vérifie si il y a du sol en dessous de la position donnée.
    /// #synthese Elyzabelle
    /// </summary>
    /// <param name="pos">Position à vérifier</param>
    /// <returns>True si il y a du sol en dessous, sinon False</returns>
    private bool AUnSolDessous(Vector3Int pos)
    {
        Vector3Int dessous = new Vector3Int(pos.x, pos.y - 1, pos.z);
        return _tilemap.GetTile(dessous) != null && _tilemap.GetTile(pos) == null;
    }

    //--------------------------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------------------------
    // Instancier la porte
    //--------------------------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------------------------

    private Vector3 _porteDeplacement = Vector3.left;
    private Vector3 _portePosition;

    /// <summary>
    /// Cette fonction est responsable d'instancier la porte 
    /// et le personnage dans le niveau.//tp3 Patrick
    /// #synthese Elyzabelle
    /// </summary>
    /// <param name="posSalleClef"> La position de la salle de la clé pour vérifier si elle correspond à celle de la porte en x.</param>//tp3 Patrick
    private void InstancierPorte(int posSalleClef)
    {
        int posSallePorte = Random.Range(0, _lesPosSalle.Count); // Sélectionne une position aléatoire parmi les salles disponibles pour la porte.//tp3 Patrick
        // Vérifie si la salle sélectionnée pour la porte est différente de la salle contenant la clé et s'il s'agit d'une sortie.//tp3 Patrick
        if (_lesPosSalle[posSallePorte].x != _lesPosSalle[posSalleClef].x && _lesPosSalle[posSallePorte].y == 0)
        {
            Vector3Int posAleatoire;
            do
            {
                posAleatoire = ChoisirUnePosAleatoireDansUneSalle(posSallePorte); // Sélectionne une position aléatoire parmi les salles disponibles pour la porte
                _portePosition = _tilemap.GetCellCenterWorld(posAleatoire) + Vector3.up * _tilemap.cellSize.y; // Position de la porte dans la salle
            } while (!AUnSolDessousPorte(Vector3Int.FloorToInt(_portePosition))); // Tant qu'il n'y a pas de sol en dessous de la porte

            // Instancie la porte et le personnage à la position trouvée avec un sol en dessous.//tp3 Patrick
            Instantiate(_porte, (Vector2)_portePosition + Vector2.up / 2, Quaternion.identity);
            _lesPosDisponibles.Remove(Vector2Int.FloorToInt(_portePosition));
            Instantiate(_perso, _portePosition + _porteDeplacement, Quaternion.identity);
            _lesPosDisponibles.Remove(Vector2Int.FloorToInt(_portePosition + _porteDeplacement));
            InstancierActivateur(posSalleClef, posSallePorte);
        }
        else InstancierPorte(posSalleClef); // Retry l'instanciation de la porte dans une autre salle si les conditions ne sont pas remplies.//tp3 Patrick
    }

    /// <summary>
    /// Vérifie que la position est approprié pour
    /// l'instanciation de la porte et du personnage
    /// #synthese Elyzabelle
    /// </summary>
    /// <param name="pos"> La position de la porte </param>
    /// <returns> Retourne true si la position est appropriée </returns>
    private bool AUnSolDessousPorte(Vector3Int pos)
    {
        // Position directly below
        Vector3Int dessous = new Vector3Int(pos.x, pos.y - 1, pos.z);
        // Position below and to the left
        Vector3Int dessousAGauche = new Vector3Int(pos.x - 1, pos.y - 1, pos.z);
        // Position directly above
        Vector3Int auDessus = new Vector3Int(pos.x, pos.y + 1, pos.z);
        // Position above and to the left
        Vector3Int auDessusGauche = new Vector3Int(pos.x - 1, pos.y + 1, pos.z);
        Vector3Int aGauche = new Vector3Int(pos.x - 1, pos.y, pos.z);

        // Check the conditions
        bool hasTileBelow = _tilemap.GetTile(dessous) != null;
        bool hasTileBelowLeft = _tilemap.GetTile(dessousAGauche) != null;
        // bool isCurrentPosEmpty = _tilemap.GetTile(pos) == null;
        bool isAboveEmpty = _tilemap.GetTile(auDessus) == null;
        bool isAboveLeftEmpty = _tilemap.GetTile(auDessusGauche) == null;

        return hasTileBelow && hasTileBelowLeft && isAboveEmpty && isAboveLeftEmpty
        && _tilemap.GetTile(aGauche) == null && _tilemap.GetTile(pos) == null;
    }

    /// <summary>
    /// Cette fonction est responsable de calculer la position du niveau
    /// #tp4 patrick
    /// </summary>
    /// <returns>retourne la position du niveau</returns>
    public Vector2Int CalculerPositionNiveau()
    {
        //retourne la position du niveau//#tp4 patrick:
        return new Vector2Int(_nbSalles.x, _nbSalles.y) * _tailleAvecUneBordure + Vector2Int.one;
    }

    //--------------------------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------------------------
    // Instancier les objets
    //--------------------------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// Sert à placer les objets sur la scène
    /// Évite la superposition des objets
    /// #tp3 Elyzabelle
    /// </summary>
    void PlacerLesObjets()
    {
        // Crée un conteneur qui contiendra tous les objets:
        Transform conteneurObjets = new GameObject("Objets").transform;
        conteneurObjets.parent = transform; // Positionne le conteneur dans son parent

        // Crée un conteneur qui contiendra les objets nombreux:
        Transform conteneur = new GameObject("Conteneur").transform;
        conteneur.parent = conteneurObjets.transform; // Positionne le conteneur dans son parent

        int nbObjets = _nbObjetsParSalle * _nbSalles.x * _nbSalles.y; // Nombre d'objets à instancier au total

        if (_lesPosDisponibles.Count == 0)
        {
            Debug.LogWarning("Aucun espace libre");
            return;
        }
        //instancier les objets selon le nb d'objets voulu
        for (int i = 0; i < nbObjets; i++)
        {
            bool estPlace = false;
            //Place un joyaux si il y en a un
            if (Random.Range(0, 101) <= _probabiliteJoyaux)
            {
                estPlace = PlacerObjetAleatoirement(_tJoyauxModeles, conteneur);
            }

            if (_lesPosDisponibles.Count == 0)
            {
                Debug.LogWarning("Aucun espace libre");
                break; // Sort de la boucle
            }
            //Si aucun joyaux n'est place, on essaie de placer un bonus
            if (!estPlace && Random.Range(0, 101) <= _probabiliteBonus)
            {
                PlacerObjetAleatoirement(_tBonusModeles, conteneur);
            }

            if (_lesPosDisponibles.Count == 0)
            {
                Debug.LogWarning("Aucun espace libre");
                break; // Sort de la boucle
            }
        }
    }

    //--------------------------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------------------------
    // Instancier l'activateur
    //--------------------------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// Cette fonction est responsable d'instancier l'activateur dans le niveau.//tp3 Patrick
    /// Modifier au synthèse Patrick
    /// </summary>
    /// <param name="posSalleClef">La position de la salle de la clé </param>
    /// <param name="posSallePorte">La position de la salle de la porte</param>
    private void InstancierActivateur(int posSalleClef, int posSallePorte)
    {
        // Nombre d'activateurs a instancier en fonction du niveau, si c'est le niveau 5, on instancie deux, sinon si c'est le niveau 10, on instancie trois.
        int nbActivateurs = 1;
        if (_donneesPerso.niveau == 5) nbActivateurs = 2;
        else if (_donneesPerso.niveau == 10) nbActivateurs = 3;

        //Pour chaque activateur a instancier
        for (int i = 0; i < nbActivateurs; i++)
        {
            bool activateurInstancie = false;
            //Tant qu'il n'y a pas d'activateur instancie
            while (!activateurInstancie)
            {
                // Sélectionne une salle aléatoire parmi les salles disponibles:
                int posSalleActivateur = Random.Range(0, _lesPosSalle.Count);

                // Vérifie que la salle sélectionnée n'est pas celle de la clé ou de la porte:
                if (posSalleActivateur != posSalleClef && posSalleActivateur != posSallePorte)
                {
                    // Obtient les limites de la salle sélectionnée:
                    Salle salle = _tSallesModeles[1];
                    Vector2Int minBounds = salle.MinBounds;
                    Vector2Int maxBounds = salle.MaxBounds;

                    // Génère des coordonnées x et y aléatoires dans les limites de la salle:
                    int x = Random.Range(minBounds.x, maxBounds.x);
                    int y = Random.Range(minBounds.y, maxBounds.y);

                    // Convertit les coordonnées en position du monde:
                    Vector3Int tilePosition = new Vector3Int(x, y, 0);

                    // Vérifie si la tuile à ces coordonnées est vide:
                    TileBase tile = _tilemap.GetTile(tilePosition);
                    if (tile == null)
                    {
                        // Instancie l'activateur au-dessus de la tuile vide:
                        Vector3 activateurPosition = _tilemap.GetCellCenterWorld(tilePosition) * _tilemap.cellSize.y;
                        Vector3 salleActivateur = (Vector3)_lesPosSalle[posSalleActivateur];
                        if (VerifierSiPosValide(Vector3Int.FloorToInt(activateurPosition + salleActivateur)))
                        {
                            Instantiate(_activateur, activateurPosition - Vector3.up + salleActivateur, Quaternion.identity);
                            _lesPosDisponibles.Remove(Vector2Int.FloorToInt(activateurPosition));
                            activateurInstancie = true;
                        }
                    }
                }
            }
        }
    }

    //--------------------------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------------------------
    // Instancier les ennemis
    //--------------------------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// Cette fonction instancie les ennemis
    /// S'assure que les cases gauche et droite 
    /// ainsi que la case au dessus soient vides
    /// Tandis que la case en dessous est pleine
    /// #synthese Elyzabelle
    /// </summary>
    void InstancierEnnemis()
    {
        // Crée un conteneur qui contiendra tous les ennemis:
        Transform conteneurEnnemis = new GameObject("Ennemis").transform;
        conteneurEnnemis.parent = transform; // Positionne le conteneur dans Niveau

        for (int i = 0; i < _nbEnnemis; i++) // Tente d'instancier le nombre d'ennemis requis
        {
            int random = Random.Range(0, _ennemis.Length);
            bool posValide = false; // Indicateur pour savoir si l'ennemi a été instancié
                                    // Continue jusqu'à trouver une position appropriée ou épuiser les positions disponibles:
            while (!posValide && _lesPosDisponibles.Count > 0)
            {
                Vector2Int pos = ObtenirUnePosLibre(); // Choisit une position libre
                Vector3Int positionActuelle = (Vector3Int)pos; //Position d'instanciation
                                                               //Si la position actuelle est appropiée pour l'ennemi, instancie l'ennemi:
                if (VerifierSiPosValide(positionActuelle))
                {
                    // Instancie l'ennemi:
                    Instantiate(_ennemis[random], (Vector3)(Vector3Int)pos, Quaternion.identity, conteneurEnnemis);
                    posValide = true; // Marque l'ennemi comme instancié
                }
            }

            if (!posValide)
            {
                // Si aucune position appropriée n'a été trouvée après avoir épuisé les positions disponibles:
                Debug.LogWarning("Aucune position appropriée trouvée pour un ennemi.");
                break; // Sort de la boucle
            }
        }
    }

    //--------------------------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------------------------
    // Vérification
    //--------------------------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// Vérifie si la case est appropiée
    /// pour l'instanciation
    /// #synthese Elyzabelle
    /// </summary>
    /// <param name="pos"> La position de la case à vérifier </param>
    /// <returns> Retourne true si la case est appropiée</returns>
    bool VerifierSiPosValide(Vector3Int pos)
    {
        // Définit les positions adjacentes:
        Vector3Int gauche = new Vector3Int(pos.x - 1, pos.y, pos.z);
        Vector3Int droite = new Vector3Int(pos.x + 1, pos.y, pos.z);
        Vector3Int auDessus = new Vector3Int(pos.x, pos.y + 1, pos.z);
        Vector3Int enDessous = new Vector3Int(pos.x, pos.y - 1, pos.z);
        Vector3Int enDessousGauche = new Vector3Int(pos.x - 1, pos.y - 1, pos.z);
        Vector3Int enDessousDroite = new Vector3Int(pos.x + 1, pos.y - 1, pos.z);
        Vector3Int enDessous2 = new Vector3Int(pos.x, pos.y - 2, pos.z);
        Vector3Int enDessousGauche2 = new Vector3Int(pos.x - 1, pos.y - 2, pos.z);
        Vector3Int enDessousDroite2 = new Vector3Int(pos.x + 1, pos.y - 2, pos.z);

        // Vérifie si les cases adjacentes sont vides et si la case en dessous est pleine:
        if (ObtenirPosVide(gauche) && ObtenirPosVide(droite) && ObtenirPosVide(auDessus)
        && ObtenirPosVide(enDessous) && ObtenirPosVide(enDessousGauche) && ObtenirPosVide(enDessousDroite)
        && !ObtenirPosVide(enDessous2) && !ObtenirPosVide(enDessousGauche2) && !ObtenirPosVide(enDessousDroite2))
        {
            return true;
        }
        else return false;
    }

    //--------------------------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------------------------
    // Obtenir une position libre
    //--------------------------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// Cette fonction verifie si la case est vide
    /// #synthese Elyzabelle
    /// </summary>
    /// <param name="position"> La position de la case a vérifier </param>
    /// <returns> Retourne true si la case est vide </returns>
    bool ObtenirPosVide(Vector3Int position)
    {
        TileBase tile = _tilemap.GetTile(position); //Vérifie si la case est vide
        return tile == null; //Retourne true si la case est vide
    }

    /// <summary>
    /// Choisi une position libre
    /// Retire la position choisie
    /// #tp3 Elyzabelle
    /// </summary>
    /// <returns>position où l'objet va apparaître</returns>
    Vector2Int ObtenirUnePosLibre()
    {
        int indexPosLibre = Random.Range(0, _lesPosDisponibles.Count); //Choisi une position disponible aléatoire
        Vector2Int pos = _lesPosDisponibles[indexPosLibre];
        _lesPosDisponibles.RemoveAt(indexPosLibre); //Retire la position choisie
        return pos; //Retoune la position à laquelle l'objet va apparaître
    }

    //--------------------------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------------------------
    // Instanciation des objets
    //--------------------------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// Fonction qui place des objets aleatoirement
    /// #tp3 Elyzabelle
    /// </summary>
    /// <param name="modeles">Modele des objets</param>
    /// <param name="parent"> Objet parent </param>
    /// <returns></returns>
    bool PlacerObjetAleatoirement(GameObject[] modeles, Transform parent)
    {
        if (_lesPosDisponibles.Count == 0) return false; //Si aucune position disponible on quitte la fonction

        int index = Random.Range(0, modeles.Length); // Choisi un objet au hasard
        GameObject objetModele = modeles[index]; // Va chercher l'objet dans le tableau

        Vector2Int pos = ObtenirUnePosLibre();
        // Position de l'apparition des objets dans les salles:
        Vector3 pos3 = (Vector3)(Vector2)pos + _tilemap.transform.position + _tilemap.tileAnchor;
        Instantiate(objetModele, pos3, Quaternion.identity, parent); // Fait apparaître les objets
        return true;
    }
}