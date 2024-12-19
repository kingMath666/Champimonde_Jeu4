using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Le ScriptableObject est utilisé pour stocker les données d'un objet de la boutique.//tp3 Patrick
[CreateAssetMenu(fileName = "Objet", menuName = "Objet boutique")]

/// <summary>
/// Classe qui définit les attributs d'un objet de la boutique.//tp3 Patrick
/// </summary>
public class SOObjet : ScriptableObject
{
    [Header("LES DONNÉES")]
    [SerializeField] string _nom; // Nom de l'objet.//tp3 Patrick
    [SerializeField][Tooltip("Icône de l'objet pour la boutique")] Sprite _sprite; // Icône de l'objet.//tp3 Patrick
    [SerializeField][Range(0, 200)] int _prixDeBase = 30; // Prix de base de l'objet.//tp3 Patrick
    [SerializeField, TextArea] string _description; // Description de l'objet.//tp3 Patrick

    // Propriété pour accéder et modifier le nom de l'objet.//tp3 Patrick
    public string nom { get => _nom; set => _nom = value; }
    // Propriété pour accéder et modifier l'icône de l'objet.//tp3 Patrick
    public Sprite sprite { get => _sprite; set => _sprite = value; }
    // Propriété pour accéder et modifier le prix de base de l'objet.//tp3 Patrick
    public int prixDeBase { get => _prixDeBase; set => _prixDeBase = Mathf.Clamp(value, 0, int.MaxValue); }
    // Propriété pour accéder et modifier la description de l'objet.//tp3 Patrick
    public string description { get => _description; set => _description = value; }
}
