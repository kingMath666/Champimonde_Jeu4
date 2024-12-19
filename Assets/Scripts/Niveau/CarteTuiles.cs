using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Classe qui contrôle les tuiles des niveaux prédéfinis avec leur pourcentage d'apparition.
/// Auteurs du code : Elyzabelle Rollin et Patrick Watt-Charron
/// Auteur des commentaires : Patrick Watt-Charron
/// </summary>
public class CarteTuiles : MonoBehaviour
{
    [Header("Paramètres")]
    [SerializeField][Range(0, 100)] int _probabilite; // Les probabilités que les tuiles apparaissent

    [Header("Composants")]
    [SerializeField] Tilemap _tilemap; //La tilemap de carteTuile

    void Awake()
    {
        AfficherTuiles();
    }

    //--------------------------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------------------------
    // Affiche les tilemaps choisies dans celle du Niveau
    //--------------------------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------------------------

    // / <summary>
    // / Fait un tirage des tilemaps
    // / Affiche les tilemaps choisies dans la tilemap du Niveau
    // / #tp3 Elyzabelle
    // / </summary>
    void AfficherTuiles()
    {
        Niveau niveau = GetComponentInParent<Niveau>(); // Cherche le composant Niveau dans le parent du GameObject actuel
        BoundsInt bounds = _tilemap.cellBounds; // Obtient les limites du Tilemap en termes de cellules
        int tirage = Random.Range(0, 101); // Génère un nombre aléatoire entre 0 et 100 inclus et le stocke dans la variable "tirage"

        if (tirage <= _probabilite) // Vérifie si le nombre tiré est inférieur ou égal à "_probabilite"
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++) // Parcourt chaque cellule dans les limites du Tilemap selon l'axe Y
            {
                for (int x = bounds.xMin; x < bounds.xMax; x++) // Parcourt chaque cellule dans les limites du Tilemap selon l'axe X
                {
                    Vector3Int posTuile = new Vector3Int(x, y, 0); //position de la tuile
                    TileBase tuile = _tilemap.GetTile(posTuile); // Récupération de la tuile de la tilemap
                    Vector3 posTilemap = _tilemap.transform.position; // Obtient les coordonnées du GameObject et les arrondit à la position entière la plus proche
                    niveau.TransfererTuile(posTuile, tuile, posTilemap);
                }
            }
        }
        gameObject.SetActive(false); // Désactive le GameObject actuel
    }

    //--------------------------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------------------------
    // Opacité des tilemaps selon probabilité
    //--------------------------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// Fonction qui gère le changement de l'opacité de notre tuile.
    /// Quand le jeu n'est pas actif, nous pouvons changer la valeur et voir le résultat.
    /// Quand le jeu est actif, l'opacité sera au maximum pour assurer une uniformité pendant le jeu.
    /// </summary>
    void GererOpacite()
    {
        Color couleurActuelle = _tilemap.color; // Récupère la couleur et l'opacité de notre tilemap

        if (!Application.isPlaying) // Si le jeu n'est pas en cours
        {
            couleurActuelle.a = (float)_probabilite / 100; // Configure l'opacité avec le pourcentage de probabilité
        }
        _tilemap.color = couleurActuelle; // Applique l'opacité configurée ci-dessus à la couleur de la tilemap
    }

    /// <summary>
    /// Cette méthode est automatiquement appelée par Unity chaque fois qu'un changement est apporté 
    /// à l'inspecteur d'un GameObject dans l'éditeur Unity, avant que la modification ne soit appliquée. 
    /// Cela permet de valider ou de modifier des propriétés avant qu'elles ne soient réellement utilisées dans le jeu.
    /// </summary>
    void OnValidate()
    {
        if (!Application.isPlaying) GererOpacite(); // Si le jeu n'est pas en cours, appelle la fonction GererOpacite().
    }
}