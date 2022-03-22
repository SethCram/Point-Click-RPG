using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //used to reload scene

public class PlayerManager : MonoBehaviour
{
    #region Singleton
    //singleton pattern:
    public static PlayerManager instance; //static vars can be accessed by any other class and are dynamically updated

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of Player Manager found!");

            return; //so dont set instance
        }

        instance = this;
    }
    #endregion

    public GameObject player;

    //reload the scene:
    public void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); //reloads current scene using its 'buildIndex'
    }

    private void Update()
    {

    }
}
