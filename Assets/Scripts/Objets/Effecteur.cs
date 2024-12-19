using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Permet à l'effector d'effectuer de la
/// rétroaction lorsque le joueur l'active
/// #synthese Elyzabelle
/// </summary>
public class Effecteur : MonoBehaviour
{
    [Header("Références")]
    [SerializeField] GameObject _nomPerso; //nom du perso
    [SerializeField] ParticleSystem _particule; //systeme de particule

    [Header("Sons")]
    [SerializeField] AudioClip _boing; //Son de l'effector

    [Header("Composants")]
    [SerializeField] Animator _anim; //Anime l'effector

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == _nomPerso.tag) //Si le joueur entre en collision avec une feuille
        {
            _anim.SetTrigger("Saut"); //dé
            GestAudio.instance.JouerEffetSonore(_boing); //Joue le son de l'objet// #synthese Elyzabelle
            Vector3 pos = gameObject.transform.position; //Position de l'effector #tp3 Elyzabelle
            Instantiate(_particule, pos, Quaternion.identity); //Instancie les particules// #tp3 Elyzabelle
        }
        else return;
    }
}
