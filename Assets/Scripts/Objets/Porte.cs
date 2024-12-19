using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Cette classe gère la transition vers la scène suivante lorsque le personnage entre en collision avec la porte.//tp3 Patrick
/// Elle vérifie d'abord si le GameObject en collision a le tag du personnage. Si c'est le cas et que le personnage possède la clef nécessaire,
/// elle déclenche la transition vers la scène suivante à l'aide des données de navigation fournies.
/// #tp3 Patrick
/// </summary>
public class Porte : MonoBehaviour
{
    [Header("Références")]
    [SerializeField] GameObject _tagPerso; // Référence vers le GameObject du personnage.//tp3 Patrick
    [SerializeField] SONavigation _navigation;// Référence vers les données de la navigation.//tp3 Patrick
    [SerializeField] GameObject _canvasBonus; //Canvas des points bonus //tp4 Elyzabelle
    [SerializeField] SOPerso _donneesPerso; // Référence vers les données du personnage.//tp3 Patrick

    [Header("Sons")]
    [SerializeField] AudioClip _clip_clef; //Son de la porte qui ouvre.//#tp4 Elyzabelle
    [SerializeField] AudioClip _clip_pasClef; //Son de la porte barrée.//#tp4 Elyzabelle

    [Header("Instances")]
    static Porte _instance; //Contient l'info de la porte
    public static Porte instance => _instance; //Crée une instance accessible

    [Header("UnityEvents")]
    UnityEvent _finDuNiveau = new UnityEvent(); //Contient l'info de la fin du niveau //#tp4 Elyzabelle
    public UnityEvent finDuNiveau => _finDuNiveau; //Crée une instance accessible //#tp4 Elyzabelle

    void Awake()
    {
        if (_instance == null) _instance = this; //Singleton
        else
        {
            Debug.LogWarning("Plus d'une instance de Porte dans la scène.");
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == _tagPerso.tag) //Si l'objet est en collision avec le perso//tp3 Patrick
        {
            if (_donneesPerso.possedeClef)
            {
                GestAudio.instance.JouerEffetSonore(_clip_clef); //Joue le son de la porte qui ouvre.//#tp4 Elyzabelle
                finDuNiveau.Invoke();
                Instantiate(_canvasBonus, new Vector3(0, 0, 0), Quaternion.identity);

            }
            else GestAudio.instance.JouerEffetSonore(_clip_pasClef); //Joue le son de la porte barrée.//#tp4 Elyzabelle
        }
    }
}