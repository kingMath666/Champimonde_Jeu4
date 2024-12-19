using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Sert à détruire la rétroaction
/// une fois l'animation terminée
/// #synthese Elyzabelle
/// </summary>
public class RetroBouclier : MonoBehaviour
{
    /// <summary>
    /// Sert à détruire la rétroaction
    /// une fois l'animation terminée
    /// </summary>
    void Detruire()
    {
        Destroy(gameObject);
    }
}
