using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// Classe gérant l'enregistrement des pointages
/// #Tp4Patrick
/// </summary>
public class Donnees : MonoBehaviour
{
    [Header("Champs de Saisie")]
    [SerializeField] TMP_InputField[] _tInputField; // Tableau des champs de saisie des noms des joueurs.//#Tp4Patrick

    [Header("Scriptable Object")]
    [SerializeField] SOSauvegarde _sauvegarde; // Référence vers le scriptable object gérant la sauvegarde des pointages.//#Tp4Patrick

    [Header("Boutons")]
    [SerializeField] Button _btnEnregistrer; // Bouton pour enregistrer les modifications.//#Tp4Patrick
    [SerializeField] Button _btnMenu; // Bouton pour retourner au menu.//#Tp4Patrick

    /// <summary>
    /// Méthode pour enregistrer les pointages des joueurs
    ///     #Tp4Patrick
    /// </summary>
    public void EnregistrerPointages()
    {
        string nomJoueur;// Variable pour stocker le nom du joueur en cours.//#Tp4Patrick
        for (int i = 0; i < _tInputField.Length; i++) // Boucle parcourant les champs de saisie des noms.//#Tp4Patrick
        {
            if (_tInputField[i].interactable) // Vérifie si le champ de saisie est activé.//#Tp4Patrick
            {
                if (_tInputField[i].text != "Votre Nom...")//Vérifie si le champ de saisie est différent de la valeur par défaut.//#Tp4Patrick
                {
                    nomJoueur = _tInputField[i].text; // Récupère le nom du joueur depuis le champ de saisie.//#Tp4Patrick
                    bool actif = false; // Initialise l'état actif du joueur à false.//#Tp4Patrick
                    _sauvegarde.AjouterPointages(nomJoueur, actif); // Appelle la méthode pour ajouter les pointages dans la sauvegarde.//#Tp4Patrick
                    _tInputField[i].interactable = false; // Désactive le champ de saisie après l'enregistrement.//#Tp4Patrick
                    _btnEnregistrer.interactable = false; // Désactive le bouton d'enregistrement après l'opération.//#Tp4Patrick
                    _btnMenu.interactable = true; // Active le bouton du menu après l'opération.//#Tp4Patrick
                    break; // Sort de la boucle après avoir enregistré le premier joueur trouvé.//#Tp4Patrick
                }

            }
        }

    }
}
