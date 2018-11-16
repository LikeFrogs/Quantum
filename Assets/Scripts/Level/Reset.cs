using UnityEngine;
using UnityEngine.SceneManagement;

public class Reset : MonoBehaviour {
    //resets the current scene
	void Update () {
        if (Input.GetKeyDown(KeyCode.R)) {
            ResetScene();
        }
	}

    /// <summary>
    /// reset the scene
    /// </summary>
    public void ResetScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }
}
