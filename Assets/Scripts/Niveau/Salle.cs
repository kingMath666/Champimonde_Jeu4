using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe qui contrôle les salles instanciées dans le niveau.
/// Auteurs du code : Elyzabelle Rollin et Patrick Watt-Charron
/// Auteur des commentaires : Patrick Watt-Charron
/// </summary>
public class Salle : MonoBehaviour
{
    [Header("Paramètres")]
    static Vector2Int _tailleAvecBordures = new Vector2Int(32, 18); // Vecteur2 pour les dimensions de la salle
    static public Vector2Int tailleAvecBordures => _tailleAvecBordures; // Accesseur permettant la lecture de la taille de la salle depuis un autre script
    [SerializeField] Vector2 _grandeurBoite; // Variable pour la taille de la salle
    [SerializeField] Vector2 pos1 = new Vector2(-0.5f, 0); // Position prédéfinie 1.//tp3 Patrick
    [SerializeField] Vector2 pos2 = new Vector2(-4.5f, -7); // Position prédéfinie 2.//tp3 Patrick
    [SerializeField] Vector2 pos3 = new Vector2(11, 7); // Position prédéfinie 3.//tp3 Patrick
    static List<Vector2> _lesPosObject = new List<Vector2>(); // Liste des positions des objets dans la salle.//tp3 Patrick
    static public List<Vector2> lesPosObject => _lesPosObject; // Propriété permettant d'accéder à la liste des positions des objets dans la salle.//tp3 Patrick

    void Awake()
    {
        _lesPosObject.Add(pos1);//Rajoute la postion 1 de la salle a la liste des position de salle//tp3 Patrick
        _lesPosObject.Add(pos2);//Rajoute la postion 2 de la salle a la liste des position de salle//tp3 Patrick
        _lesPosObject.Add(pos3);//Rajoute la postion 3 de la salle a la liste des position de salle//tp3 Patrick
    }

    /// <summary>
    /// Dessine des bordures à la boîte en blanc pour visualiser les dimensions.
    /// </summary>
    void OnDrawGizmos()
    {
        _grandeurBoite = new Vector2(_tailleAvecBordures.x, _tailleAvecBordures.y); // Calcule la taille de la boîte
        Gizmos.color = Color.white; // Configure la couleur du gizmos
        Vector2 pointDepart = (Vector2)transform.position; // Positionne le début de la boîte
        Gizmos.DrawWireCube(pointDepart, _grandeurBoite); // Dessine la boîte de collision
    }

    /// <summary>
    /// Renvoie la position minimale de la salle.
    /// #synthese Patrick
    /// </summary>
    public Vector2Int MinBounds
    {
        get
        {
            int minX = int.MaxValue; // Initialise minX à la valeur maximale possible pour un entier
            int minY = int.MaxValue; // Initialise minY à la valeur maximale possible pour un entier

            foreach (Vector2 pos in _lesPosObject) // Parcourt chaque position dans _lesPosObject
            {
                if (pos.x < minX) // Si la position x actuelle est inférieure à minX
                    minX = Mathf.FloorToInt(pos.x); // Met à jour minX avec la valeur entière inférieure la plus proche de pos.x
                if (pos.y < minY) // Si la position y actuelle est inférieure à minY
                    minY = Mathf.FloorToInt(pos.y); // Met à jour minY avec la valeur entière inférieure la plus proche de pos.y
            }

            return new Vector2Int(minX, minY); // Retourne un nouveau Vector2Int avec les valeurs minimales de x et y
        }
    }

    /// <summary>
    /// Renvoie la position maximale de la salle.
    /// #synthese Patrick
    /// </summary>
    public Vector2Int MaxBounds
    {
        get
        {
            int maxX = int.MinValue; // Initialise maxX à la valeur minimale possible pour un entier
            int maxY = int.MinValue; // Initialise maxY à la valeur minimale possible pour un entier

            foreach (Vector2 pos in _lesPosObject) // Parcourt chaque position dans _lesPosObject
            {
                if (pos.x > maxX) // Si la position x actuelle est supérieure à maxX
                    maxX = Mathf.CeilToInt(pos.x); // Met à jour maxX avec la valeur entière supérieure la plus proche de pos.x
                if (pos.y > maxY) // Si la position y actuelle est supérieure à maxY
                    maxY = Mathf.CeilToInt(pos.y); // Met à jour maxY avec la valeur entière supérieure la plus proche de pos.y
            }

            return new Vector2Int(maxX, maxY); // Retourne un nouveau Vector2Int avec les valeurs maximales de x et y
        }
    }

}