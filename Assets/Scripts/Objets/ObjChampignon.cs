using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe représentant un champignon dans le jeu.
/// Hérite de la classe Objet.
/// synthèse Patrick
/// </summary>
public class ObjChampignon : Objet
{
    [Header("Composants")]
    Animator _anim;
    SpriteRenderer _sr;

    void Awake()
    {
        _anim = GetComponent<Animator>();
        _sr = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// Méthode appelée lorsqu'un autre objet entre en collision avec cet objet (déclencheur).
    /// </summary>
    /// <param name="other">Le collider de l'autre objet en collision.</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        // Vérifie si l'objet en collision a le même tag que le personnage
        if (other.tag == _nomPerso.tag)
        {
            // Appelle la méthode ChangerGrandeur du personnage singleton pour modifier sa taille
            Perso.instance.ChangerGrandeur(0.5f, 3f);
            // Détruit l'objet champignon du jeu
            StartCoroutine(CoroutineDelaisDeReapparition());
        }
    }

    /// <summary>
    /// Méthode permettant de faire disparaitre l'objet
    /// et de le reapparaitre au bout de 10s
    /// #synthèse Elyzabelle
    /// </summary>
    /// <returns></returns>
    IEnumerator CoroutineDelaisDeReapparition()
    {
        _anim.SetTrigger("Disparaitre");
        yield return new WaitForSeconds(10f);
        _anim.SetTrigger("Reapparaitre");
    }
}
