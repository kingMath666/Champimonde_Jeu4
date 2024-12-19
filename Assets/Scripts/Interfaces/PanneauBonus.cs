using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

/// <summary>
/// Gère l'affichage des bonus dans un panneau à l'écran.
/// #tp4 Elyzabelle
/// </summary>
public class PanneauBonus : MonoBehaviour
{
    [Header("Instances")]
    static public PanneauBonus _instance; // Instance du panneau
    public static PanneauBonus instance => _instance; // Getter pour l'instance

    [Header("Paramètes")]
    int _totalBonus; // Total des bonus

    [Header("Références")]
    [SerializeField] SOPerso _donneesPerso; // Données du personnage


    [Header("Champs textes du panneau")]
    [SerializeField] TextMeshProUGUI _champDetailTempsRestant; // Texte qui affiche le temps restant
    // Texte qui affiche le total des points du bonus de temps restant:
    [SerializeField] TextMeshProUGUI _champTotalPointsTempsRestant;
    // Texte qui affiche le total des points bonus de l'activateur:
    [SerializeField] TextMeshProUGUI _champTotalPointsActivateur;
    // Texte qui affiche le total des points bonus des marguerites:
    [SerializeField] TextMeshProUGUI _champTotalPointsMarguerite;
    [SerializeField] TextMeshProUGUI _champTotalPointsBonus; // Texte qui affiche le total des points bonus

    void Awake()
    {
        _champDetailTempsRestant.text = "";
        _champTotalPointsTempsRestant.text = "";
        _champTotalPointsActivateur.text = "";
        if (_instance == null) _instance = this; // Singleton
        else
        {
            Debug.LogWarning("Plus d'une instance de PanneauBonus dans la scène.");
            Destroy(gameObject);
        }
    }

    void Start()
    {
        AfficherLesBonus(); // Affiche les bonus du joueur
    }

    //--------------------------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------------------------
    // Affichage des bonus
    //--------------------------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// Affiche le bonus de temps dans le panneau.
    /// </summary>
    /// <param name="tempsRestant">Le temps restant du bonus</param>
    /// <param name="bonusTemps">Le nombre de points bonus par unité de temps</param>
    public void AfficherBonusTemps(int tempsRestant, int bonusTemps)
    {
        _totalBonus += tempsRestant * bonusTemps;
        _champDetailTempsRestant.text = $"Temps restant : {tempsRestant}s * {bonusTemps}";
        StartCoroutine(CoroutineAfficherTextes(tempsRestant * bonusTemps, _champTotalPointsTempsRestant));
    }

    /// <summary>
    /// Affiche le bonus de marguerites dans le panneau.
    /// </summary>
    /// <param name="bonusMarguerite">Le nombre de points bonus des marguerites</param>
    public void AfficherBonusFleurs(int bonusMarguerite)
    {
        _totalBonus += bonusMarguerite;
        StartCoroutine(CoroutineAfficherTextes(bonusMarguerite, _champTotalPointsMarguerite));
    }

    /// <summary>
    /// Affiche le bonus de l'activateur dans le panneau.
    /// </summary>
    /// <param name="bonusActivateur">Le nombre de points bonus de l'activateur</param>
    public void AfficherBonusActivateur(int bonusActivateur)
    {
        _totalBonus += bonusActivateur;
        StartCoroutine(CoroutineAfficherTextes(bonusActivateur, _champTotalPointsActivateur));
    }

    /// <summary>
    /// Affiche le total des points bonus dans le panneau.
    /// </summary>
    public void AfficherTotalBonus()
    {
        StartCoroutine(CoroutineAfficherTextes(_totalBonus, _champTotalPointsBonus));
    }

    /// <summary>
    /// Fonction qui permet d'afficher tous les bonus
    /// #tp4 Elyzabelle
    /// </summary>
    void AfficherLesBonus()
    {
        AfficherBonusTemps(_donneesPerso.tempsRestant, 10);

        if (_donneesPerso.possedeBonusFleurs) AfficherBonusFleurs(_donneesPerso.valeurBonusMarguerite);
        else AfficherBonusFleurs(0);

        if (_donneesPerso.possedeBonusActivateur) AfficherBonusActivateur(_donneesPerso.valeurBonusActivateur);
        else AfficherBonusActivateur(0);

        AfficherTotalBonus();
    }

    /// <summary>
    /// Affiche le texte graduellement et arrete l'execution de
    ///  la coroutine une fois la valeur du bonus atteinte
    /// </summary>
    /// <param name="bonus">valeur du bonus</param>
    /// <param name="champTexte">champ de texte à modifier</param>
    /// <returns></returns>
    IEnumerator CoroutineAfficherTextes(int bonus, TextMeshProUGUI champTexte)
    {
        int valeur = 0;
        if (bonus > valeur) // Si la valeur du bonus est plus grande que 0
        {
            while (valeur < bonus) // Tant que la valeur du bonus n'est pas atteinte
            {
                valeur++; // Augmenter la valeur
                champTexte.text = $"{valeur}"; // Changer le texte
                yield return new WaitForSeconds(0.0000000000001f);
            }
            // Sortir de la coroutine une fois que la valeur affichée atteint celle souhaitée
            yield break;
        }
        else champTexte.text = $"{valeur}"; //si la valeur du bonus est inférieure à 0
    }
}
