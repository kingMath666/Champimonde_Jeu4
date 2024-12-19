using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// Classe qui permet d'afficher le tableau de pointage
/// #Tp4Patrick
/// </summary>
public class TableauAffichage : MonoBehaviour
{
    [Header("Textes")]
    [SerializeField] TMP_InputField[] _tZonesDeSaisie; // Déclaration d'un tableau d'objets TMP_InputField pour la saisie des noms.//#Tp4Patrick
    [SerializeField] TMP_Text[] _tTextesPointages; // Déclaration d'un tableau d'objets TMP_Text pour afficher les pointages.//#Tp4Patrick

    [Header("Scriptable Object")]
    [SerializeField] SOSauvegarde _sauvegarde; // Référence vers le scriptable object gérant la sauvegarde des pointages.//#Tp4Patrick
    [SerializeField] SOPerso _pointage; // Référence vers le scriptable object contenant les données du personnage.//#Tp4Patrick

    [Header("Boutons")]
    [SerializeField] Button _btnEnregistrer; // Référence vers le bouton pour enregistrer les modifications.//#Tp4Patrick
    [SerializeField] Button _btnMenu; // Référence vers le bouton pour retourner au menu.//#Tp4Patrick

    void Start()
    {
        _btnEnregistrer.interactable = false; // Désactivation du bouton d'enregistrement au démarrage.//#Tp4Patrick
        AfficherScores(); // Appel de la fonction pour afficher les scores.//#Tp4Patrick
    }

    /// <summary>
    /// Fonction qui permet d'afficher les meilleurs scores
    /// #Tp4Patrick
    /// </summary>
    void AfficherScores()
    {
        _sauvegarde.ChargerPointages(); // Chargement des pointages depuis la sauvegarde.//#Tp4Patrick
        // var meilleursScores = _sauvegarde.RenvoyerMeilleursScores(); // Récupération des meilleurs scores.//#Tp4Patrick
        _sauvegarde.lesMeilleursPointages.Add(new ChargerPointage(_pointage.pointage, "Votre Nom...", true)); // Ajout du pointage actuel dans la liste des meilleurs scores.//#Tp4Patrick
        _sauvegarde.lesMeilleursPointages.Sort((a, b) => b.pointage.CompareTo(a.pointage)); // Tri des meilleurs scores par ordre décroissant.//#Tp4Patrick

        for (int i = 0; i < _tTextesPointages.Length; i++) // Boucle parcourant les composants d'affichage des scores.//#Tp4Patrick
        {
            if (i < _sauvegarde.lesMeilleursPointages.Count) // Vérification si l'indice est inférieur au nombre de scores disponibles.//#Tp4Patrick
            {
                _tZonesDeSaisie[i].text = _sauvegarde.lesMeilleursPointages[i].nom; // Affichage du nom du joueur.//#Tp4Patrick
                _tTextesPointages[i].text = _sauvegarde.lesMeilleursPointages[i].pointage.ToString(); // Affichage du pointage du joueur.//#Tp4Patrick
                if (_sauvegarde.lesMeilleursPointages[i].nom == "Votre Nom..." && i­ != _tZonesDeSaisie.Length - 1) // Vérification si le joueur actuel est affiché et si ce n'est pas le dernier élément.//#Tp4Patrick
                {
                    _tZonesDeSaisie[i].interactable = true; // Activation de la saisie pour le joueur actuel.//#Tp4Patrick
                    _btnEnregistrer.interactable = true; // Activation du bouton d'enregistrement.//#Tp4Patrick
                    _btnMenu.interactable = false; // Désactivation du bouton du menu.//#Tp4Patrick
                }
                else // Si le joueur n'est pas le joueur actuel.//#Tp4Patrick
                {
                    _tZonesDeSaisie[i].interactable = false; // Désactivation de la saisie pour les autres joueurs.//#Tp4Patrick
                }
            }
            else // Si l'indice dépasse le nombre de scores disponibles.//#Tp4Patrick
            {
                _tZonesDeSaisie[i].text = "..."; // Affichage de points de suspension si aucun score n'est disponible.//#Tp4Patrick
                _tTextesPointages[i].text = "..."; // Affichage de points de suspension si aucun score n'est disponible.//#Tp4Patrick
                _tZonesDeSaisie[i].interactable = false; // Désactivation de la saisie.//#Tp4Patrick
            }

        }
    }
}