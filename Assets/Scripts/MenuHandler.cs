using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MenuHandler : MonoBehaviour
{
   public void StartGame()
   {
      LevelLoader.Instance.StartCoroutine(LevelLoader.Instance.LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
   }

   public void QuitGame()
   {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif   
   }
}
