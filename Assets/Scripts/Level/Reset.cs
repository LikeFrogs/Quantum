using UnityEngine;
using UnityEngine.SceneManagement;

public class Reset : MonoBehaviour {
    //resets the current scene
	void Update () {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
        }
	}
}
