using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Classe qui permet d'afficher le niveau et l'argent du joueur dans la boutique.//tp3 Patrick
/// Permet d'initialiser les valeurs originales du SOPerso en quittant le mode lecture.//tp3 Patrick
/// </summary>
public class Boutique : MonoBehaviour
{
    [Header("Composants")]
    [SerializeField] SOPerso _donneesPerso; // Référence vers les données du personnage.//tp3 Patrick
    [SerializeField] TextMeshProUGUI _champNiveau; // Champ de texte pour afficher le niveau du joueur.//tp3 Patrick
    [SerializeField] TextMeshProUGUI _champArgent; // Champ de texte pour afficher l'argent du joueur.//tp3 Patrick

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        _donneesPerso.RestaurerPossession(); // Restaure les achats effectués par le joueur.//tp3 Patrick
    }
    void Update()
    {
        _champNiveau.text = "Niveau " + _donneesPerso.niveau; // Affiche le niveau du joueur dans le champ de texte.//tp3 Patrick
        _champArgent.text = _donneesPerso.argent + " $"; // Affiche l'argent du joueur dans le champ de texte.//tp3 Patrick
    }

    /// <summary>
    /// Méthode appelée sur tous les objets de jeu avant la fermeture de l'application.
    /// tp3 Patrick
    /// </summary>
    void OnApplicationQuit()
    {
        _donneesPerso.Initialiser(); // Initialise les données du personnage avant de quitter l'application.//tp3 Patrick
    }
}