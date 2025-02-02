using Character;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace InteractableItems
{
    public class Portal : MonoBehaviour, ITriggerItem
    {
        [HideInInspector] public string sceneName;

        #if UNITY_EDITOR
        public SceneAsset scene;
        private void OnValidate()
        {
            sceneName = "";
            if (scene != null)
            {
                sceneName = scene.name;
            }
        }
        #endif

        public void TriggerAction(Player player)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}