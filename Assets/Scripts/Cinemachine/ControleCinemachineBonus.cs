using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

/// <summary>
/// Classe qui permet de changer lae cible de suivi (Follow) et la cible du regard (LookAt) de la caméra virtuelle Cinemachine
/// Permet aussi de changer la priorité de la caméra virtuelle
/// #tp4 Patrick
/// </summary>
public class ControleCinemachineBonus : MonoBehaviour
{
    [Header("Paramètres")]
    [SerializeField] float _tempsBonus = 5f;//temps du bonus//#tp4 Patrick

    CinemachineVirtualCamera _cinemachineBonus;// Caméra virtuelle Cinemachine//#tp4 Patrick
    private static ControleCinemachineBonus _instance;//création d'une instance de ControleCinemachineBonus//#tp4 Patrick

    public static ControleCinemachineBonus instance//creation d'une instance de ControleCinemachineBonus//#tp4 Patrick
    {
        get { return _instance; }//retourne l'instance//#tp4 Patrick
    }

    void Awake()
    {
        if (_instance != null)//si l'instance n'est pas null//#tp4 Patrick
        {
            Destroy(gameObject);//destruction du gameObject//#tp4 Patrick
        }
        else
        {
            _instance = this;//initialisation de l'instance//#tp4 Patrick
        }
        _cinemachineBonus = GetComponent<CinemachineVirtualCamera>();//recupere le composant camera virtuelle//#tp4 Patrick
    }

    /// <summary>
    /// Fonction pour changer la cible de suivi (Follow) et la cible du regard (LookAt) de la caméra virtuelle Cinemachine
    /// #tp4 Patrick
    /// </summary>
    /// <param name="nouvelleCible">Nouvelle cible</param>
    public void ChangerCible(Transform nouvelleCible)
    {
        if (_cinemachineBonus != null)//si la camera virtuelle n'est pas null//#tp4 Patrick
        {
            _cinemachineBonus.Follow = nouvelleCible;// Change la cible de suivi (Follow) vers le nouveau transform//#tp4 Patrick
            _cinemachineBonus.LookAt = nouvelleCible;// Change la cible du regard (LookAt) vers le nouveau transform//#tp4 Patrick
        }
        else
        {
            Debug.Log("Aucune caméra virtuelle Cinemachine n'est assignée au script.");
        }
    }

    /// <summary>
    /// Fonction pour changer la camera virtuelle
    /// #tp4 Patrick
    /// </summary>
    public void ChangerCamera()
    {
        _cinemachineBonus.Priority = 15;//change la priorité de la camera virtuelle//#tp4 Patrick
        StartCoroutine(CoroutineChangerCamera());//coroutine pour changer la camera virtuelle//#tp4 Patrick
    }

    /// <summary>
    /// Coroutine pour changer la camera virtuelle
    /// #tp4 Patrick
    /// </summary>
    IEnumerator CoroutineChangerCamera()
    {
        yield return new WaitForSeconds(_tempsBonus);//attends 3 secondes//#tp4 Patrick
        _cinemachineBonus.Priority = 5;//change la priorité de la camera virtuelle//#tp4 Patrick
    }
}