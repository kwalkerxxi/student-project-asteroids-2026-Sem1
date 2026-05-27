using UnityEngine;
using LuverneHerlin;

namespace LuverneHerlin
{
    /// <summary>
    /// This script makes a game object act as a boss enemy in the 
    /// Asteroids remake that follows another game object with a 
    /// player tag around the screen and shoots missile objects for 
    /// a certain duration once the count down reaches zero.
    /// </summary>
    public class BossScript : MonoBehaviour
    {
        public string targetTag = "Player";
        public GameObject missileObject;
        public float durationTillSpawn, spawnCountdown;
        private Transform targetTransform;

        void Start()
        {
            spawnCountdown = durationTillSpawn;

            GameObject targetObject = GameObject.FindGameObjectWithTag(targetTag);
            if (targetObject != null)
            {
                targetTransform = targetObject.transform;
            }
        }

        void Update()
        {

            if (GameObject.FindGameObjectWithTag(targetTag) == null)
            {
                return;
            }

            targetTransform =  GameObject.FindGameObjectWithTag(targetTag).transform; 

            if(targetTransform == null)
            {
                return;
            }


            transform.LookAt(targetTransform.transform);
            transform.position = Vector3.MoveTowards(transform.position, targetTransform.position, 3f * Time.deltaTime);
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);

            spawnCountdown -= Time.deltaTime;
            if (spawnCountdown <= 0)
            {
                spawnCountdown = durationTillSpawn;
                Vector3 spawnPos = transform.position + transform.forward * 1f;
                Instantiate(missileObject, spawnPos, transform.rotation);
            }
        }
    }

}
