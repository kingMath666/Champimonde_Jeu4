using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.InteropServices;

/// <summary>
/// Lecture et sauvegarde des pointages dans le fichier texte sauvegarde.tim
/// #Tp4Patrick
/// </summary>
[CreateAssetMenu(fileName = "Sauvegarde", menuName = "Sauvegarde")]
public class SOSauvegarde : ScriptableObject
{
    [SerializeField] string _fichierPointages = "sauvegarde.tim"; // Déclaration d'une variable pour stocker le nom du fichier de sauvegarde //#tp4Patrick

    // Déclaration d'une fonction native en C# qui sera utilisée pour la synchronisation WebGL //#tp4Patrick
    [DllImport("__Internal")]
    static extern void SynchroniserWebGL();

    // Propriété permettant d'accéder à la liste des meilleurs pointages //#tp4Patrick
    [SerializeField] private List<ChargerPointage> _lesMeilleursPointages;
    public List<ChargerPointage> lesMeilleursPointages => _lesMeilleursPointages;

    /// <summary>
    /// Fonction permettant d'ajouter un nouveau pointage
    /// #tp4Patrick
    /// </summary>
    /// <param name="nom">variable recueillant le nom du joueur</param>
    /// <param name="estActif">variable recueillant l'état actif du joueur</param>
    public void AjouterPointages(string nom, bool estActif)
    {
        // Recherche de l'index du joueur actuel dans la liste des meilleurs pointages //#tp4Patrick
        int indexJoueurActuel = lesMeilleursPointages.FindIndex(s => s.nom == "Votre Nom...");
        // Si le joueur actuel est trouvé dans la liste //#tp4Patrick
        if (indexJoueurActuel != -1)
        {
            // Mise à jour du nom et de l'état actif du joueur actuel //#tp4Patrick
            lesMeilleursPointages[indexJoueurActuel].nom = nom;
            lesMeilleursPointages[indexJoueurActuel].estActif = estActif;
        }
        // Tri de la liste des meilleurs pointages en ordre décroissant de pointage //#tp4Patrick
        lesMeilleursPointages.Sort((a, b) => b.pointage.CompareTo(a.pointage));
        // Si le nombre de pointages dépasse 5, retire le dernier pointage //#tp4Patrick
        if (lesMeilleursPointages.Count > 5)
        {
            lesMeilleursPointages.RemoveAt(5);
        }
        SauvegarderPointages(); // Appelle la méthode pour sauvegarder les pointages //#tp4Patrick
    }

    /// <summary>
    /// Fonction pour sauvegarder les Pointage
    /// #tp4Patrick
    /// </summary>
    void SauvegarderPointages()
    {
        // Chemin complet du fichier de sauvegarde dans le dossier persistant de l'application //#tp4Patrick
        string cheminFichier = Application.persistentDataPath + "/" + _fichierPointages;
        // Convertit les données de sauvegarde en format JSON //#tp4Patrick
        string contenu = JsonUtility.ToJson(this);
        // Écrit le contenu JSON dans le fichier //#tp4Patrick
        File.WriteAllText(cheminFichier, contenu);
        // Si l'application tourne sur WebGL, synchronise avec WebGL //#tp4Patrick
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            SynchroniserWebGL();
            Debug.Log("Synchronisation WebGL");
        }
    }

    /// <summary>
    /// Fonction pour charger les pointages
    /// #tp4Patrick
    /// </summary>
    public void ChargerPointages()
    {
        _lesMeilleursPointages.Clear();
        // Chemin complet du fichier de sauvegarde dans le dossier persistant de l'application //#tp4Patrick
        string cheminFichier = Application.persistentDataPath + "/" + _fichierPointages;
        // Si le fichier existe //#tp4Patrick
        if (File.Exists(cheminFichier))
        {
            // Lit le contenu du fichier //#tp4Patrick
            string contenu = File.ReadAllText(cheminFichier);
            // Convertit le contenu JSON en données de sauvegarde //#tp4Patrick
            JsonUtility.FromJsonOverwrite(contenu, this);
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
            UnityEditor.AssetDatabase.SaveAssets();
#endif
        }
        else // Si le fichier n'existe pas //#tp4Patrick
        {
            // Crée un nouveau fichier avec des données par défaut //#tp4Patrick
            // File.Create(cheminFichier).Close();
            // SauvegardeDonnees donneesParDefaut = new SauvegardeDonnees();
            string contenu = JsonUtility.ToJson(this);
            File.WriteAllText(cheminFichier, contenu);
        }
    }
}

// Classe représentant un pointage chargé //#tp4Patrick
[System.Serializable]
public class ChargerPointage
{
    public int pointage; // Pointage du joueur //#tp4Patrick
    public string nom; // Nom du joueur //#tp4Patrick
    public bool estActif; // Indique si le joueur est actif //#tp4Patrick

    // Constructeur de la classe ChargerPointage //#tp4Patrick
    public ChargerPointage(int pointage, string nom, bool estActif)
    {
        this.pointage = pointage; // Affecte la valeur du pointage passé en paramètre à la variable pointage de l'instance de la classe ChargerPointage //#tp4Patrick
        this.nom = nom; // Affecte la valeur du nom passé en paramètre à la variable nom de l'instance de la classe ChargerPointage //#tp4Patrick
        this.estActif = estActif; // Affecte la valeur de estActif passée en paramètre à la variable estActif de l'instance de la classe ChargerPointage //#tp4Patrick
    }
}