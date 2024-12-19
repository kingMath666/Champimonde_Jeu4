using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

/// <summary>
/// Classe qui permet de changer la cible de suivi (Follow) et la cible du regard (LookAt) de la caméra virtuelle Cinemachine
/// #tp4 Patrick
/// </summary>
public class ControleCinemachinePrincipale : MonoBehaviour
{
    CinemachineVirtualCamera _cinemachinePrincipale;// Caméra virtuelle Cinemachine//#tp4 Patrick
    private static ControleCinemachinePrincipale _instance;//création d'une instance de ControleCinemachinePrincipale//#tp4 Patrick

    public static ControleCinemachinePrincipale instance//creation d'une instance de ControleCinemachinePrincipale//#tp4 Patrick
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
        _cinemachinePrincipale = this.GetComponent<CinemachineVirtualCamera>();//recupere le composant camera virtuelle//#tp4 Patrick
    }

    /// <summary>
    /// Fonction pour changer la cible de suivi (Follow) et la cible du regard (LookAt) de la caméra virtuelle Cinemachine
    /// #tp4 Patrick 
    /// </summary>
    /// <param name="nouvelleCible">Nouvelle cible</param>
    public void ChangerCible(Transform nouvelleCible)
    {
        if (_cinemachinePrincipale != null)//si la camera virtuelle n'est pas null//#tp4 Patrick
        {
            _cinemachinePrincipale.Follow = nouvelleCible;// Change la cible de suivi (Follow) vers le nouveau transform//#tp4 Patrick
            _cinemachinePrincipale.LookAt = nouvelleCible;// Change la cible du regard (LookAt) vers le nouveau transform//#tp4 Patrick
        }
        else
        {
            Debug.Log("Aucune caméra virtuelle Cinemachine n'est assignée au script.");
        }
    }


}