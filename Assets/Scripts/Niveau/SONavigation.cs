using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Le ScriptableObject est utilisé pour stocker des données de navigation. //tp3 Patrick
[CreateAssetMenu(fileName = "Ma navigation", menuName = "Navigation")]

/// <summary>
/// Classe qui permet d'effectuer la transition entre les scènes.//tp3 Patrick
/// </summary>
public class SONavigation : ScriptableObject
{
    [Header("Références")]
    [SerializeField] SOPerso _donneesPerso; // Référence vers les données du personnage.//tp3 Patrick

    [Header("Sons")]
    [SerializeField] AudioClip _clipBouton; // Son des boutons.//#synthese Elyzabelle


    //--------------------------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------------------------
    // Changement de scène
    //--------------------------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------------------------

    // Méthode pour démarrer le jeu.//tp3 Patrick
    public void Jouer()
    {
        _donneesPerso.Initialiser(); // Initialise les données du personnage.//tp3 Patrick
        AllerSceneSuivante(); // Charge la scène suivante.//tp3 Patrick
        JouerSonBouton();
    }

    // Méthode pour quitter la boutique et passer au niveau suivant.//tp3 Patrick
    public void SortirBoutique()
    {
        AllerScenePrecedente(); // Charge la scène précédente.//tp3 Patrick
        _donneesPerso.niveau++; // Incrémente le niveau du personnage.//tp3 Patrick
        JouerSonBouton();
    }

    // Méthode pour charger la scène suivante.//tp3 Patrick
    public void AllerSceneSuivante()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // Charge la scène suivante dans l'ordre de la build.//tp3 Patrick
        JouerSonBouton();
    }

    // Méthode pour charger la scène précédente.
    public void AllerScenePrecedente()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1); // Charge la scène précédente dans l'ordre de la build.//tp3 Patrick
        JouerSonBouton();
    }

    // Méthode pour charger la scène Générique.//tp4 Patrick
    public void AllerSceneGenerique()
    {
        SceneManager.LoadScene("SceneFinaleGenerique"); // Charge la scène Générique //tp4 Patrick
        JouerSonBouton();
    }

    // Méthode pour charger la scène d'accueil.//tp4 Elyzabelle
    public void AllerSceneAccueil()
    {
        SceneManager.LoadScene(0); // Charge la scène Générique //tp4 Patrick
        JouerSonBouton();
    }
    public void AllerSceneInstruction()
    {
        SceneManager.LoadScene(5); // Charge la scène d'instruction //#synthese Elyzabelle
        JouerSonBouton();
    }

    // Méthode pour charger la scène de la boutique.//tp4 Elyzabelle
    public void AllerSceneBoutique()
    {
        GestAudio.instance.ChangerEtatLecturePiste(TypePiste.MusiqueEvenB, false); //Joue la musique de base 
        SceneManager.LoadScene(2); // Charge la scène Générique //tp4 Patrick
        JouerSonBouton();
    }

    public void AllerSceneTableauHonneur()
    {
        SceneManager.LoadScene(4);
        JouerSonBouton();
    }

    void JouerSonBouton()
    {
        GestAudio.instance.JouerEffetSonore(_clipBouton);
    }
}