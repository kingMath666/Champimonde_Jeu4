using UnityEngine;

/// <summary>
/// Cette classe gère la détection de proximité de la clef par rapport au personnage.
/// Elle déclenche des événements audio lorsque le personnage entre ou sort de la zone de proximité de la clef.
/// #tp4 Elyzabelle
/// </summary>
public class ProximiteClef : MonoBehaviour
{
    [Header("Références")]
    [SerializeField] GameObject _tagPerso; // Référence vers le GameObject du personnage.

    /// <summary>
    /// Méthode appelée lorsqu'un objet entre dans le déclencheur de collision 2D attaché à cet objet.
    /// Elle vérifie si l'objet entrant a le même tag que celui du personnage.
    /// Si c'est le cas, elle déclenche le début de la musique de l'événement A.
    /// </summary>
    /// <param name="other">Le collider de l'objet entrant en collision.</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == _tagPerso.tag) //Si le perso entre dans la zone de la clef
        {
            GestAudio.instance.ChangerEtatLecturePiste(TypePiste.MusiqueEvenA, true); //Joue la musique de l'événement A
        }
    }

    /// <summary>
    /// Méthode appelée lorsqu'un objet sort du déclencheur de collision 2D attaché à cet objet.
    /// Elle vérifie si l'objet sortant a le même tag que celui du personnage.
    /// Si c'est le cas, elle déclenche la fin de la musique de l'événement A.
    /// </summary>
    /// <param name="other">Le collider de l'objet sortant de collision.</param>
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == _tagPerso.tag) //Si le perso sort de la zone de la clef
        {
            GestAudio.instance.ChangerEtatLecturePiste(TypePiste.MusiqueEvenA, false); //Joue la musique de base
        }
    }
}
