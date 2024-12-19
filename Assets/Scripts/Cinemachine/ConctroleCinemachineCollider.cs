using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe qui permet de changer la taille du collider de la caméra virtuelle
/// #tp4 Patrick
/// </summary>
public class ConctroleCinemachineCollider : MonoBehaviour
{
    GameObject _cmCollider;// Le collider de la caméra virtuelle//#tp4 Patrick

    void Start()
    {
        _cmCollider = this.gameObject;//Va chercher le collider de la caméra virtuelle//#tp4 Patrick
        AdapterTailleCollider();//Appel de la fonction pour adapter la taille du collider à la taille du niveau//#tp4 Patrick
    }

    /// <summary>
    /// Fonction pour adapter la taille du collider à la taille du niveau
    /// </summary>
    private void AdapterTailleCollider()
    {
        Vector2Int positionNiveau = Niveau.instance.CalculerPositionNiveau();//Appel de la fonction pour calculer la position du niveau//#tp4 Patrick
        _cmCollider.transform.localScale = (Vector3Int)positionNiveau;//Appliquer la taille du collider à la taille du niveau//#tp4 Patrick
        _cmCollider.transform.position = new Vector3Int(-Salle.tailleAvecBordures.x / 2, -Salle.tailleAvecBordures.y / 2, 0);//Appliquer la position du collider à la taille du niveau//#tp4 Patrick
    }
}
