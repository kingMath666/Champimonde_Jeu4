using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Cette classe gère le comportement de la clef lorsqu'elle entre en collision avec le personnage.//tp3 Patrick
/// </summary>
public class Clef : MonoBehaviour
{
    [Header("Références")]
    [SerializeField] GameObject _tagPerso; // Référence vers le GameObject du personnage.//tp3 Patrick
    [SerializeField] SOPerso _donneesPerso; // Référence vers les données du personnage.//tp3 Patrick
    [SerializeField] Retroaction _retroModele; //Modèle de l'objet de rétroaction //#synthese Elyzabelle

    [Header("Sons")]
    [SerializeField] AudioClip _sonCle; //Son de la clef.//tp4 Elyzabelle


    /// <summary>
    /// Méthode appelée lorsqu'un objet entre en collision avec la clef.//tp3 Patrick
    /// </summary>
    /// <param name="other">Le collider de l'objet entrant en collision.</param>//tp3 Patrick
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == _tagPerso.tag) // Si l'objet est en collision avec le personnage.//tp3 Patrick
        {
            //Instanciation de la rétroaction:
            Retroaction retro = Instantiate(_retroModele, transform.position, Quaternion.identity, transform.parent);
            retro.ChangerTexte("+1 clé"); //Change le texte de rétroaction
            GestAudio.instance.JouerEffetSonore(_sonCle); //Joue le son de la clef//tp4 Elyzabelle
            _donneesPerso.PrendreClef(); // Appelle la méthode pour prendre la clef du personnage.//tp3 Patrick
            Destroy(gameObject); // Détruit la clef après qu'elle a été prise.//tp3 Patrick
        }
    }
}