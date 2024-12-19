using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Sert à afficher un texte de rétroaction
/// au joueur
/// #tp3 Elyzabelle
/// </summary>
public class Retroaction : MonoBehaviour
{
    [Header("Champ texte")]
    [SerializeField] TextMeshProUGUI _champTexte; //Champ pour la rétroaction

    /// <summary>
    /// Modifie le champ texte pour le 
    /// texte voulu
    /// #tp3 Elyzabelle
    /// </summary>
    /// <param name="texte"></param>
    public void ChangerTexte(string texte)
    {
        _champTexte.text = texte;
    }

    /// <summary>
    /// Sert à détruire le champ texte à
    /// la fin de son animation
    /// #tp3 Elyzabelle
    /// </summary>
    public void Detruire()
    {
        Destroy(gameObject);
    }
}
