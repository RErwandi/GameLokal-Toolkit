using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace GameLokal.Toolkit.Pattern
{
    public class Factory : MonoBehaviour
    {
        public enum BlueprintSelectionMode
        {
            Random,
            Sequential,
            Shuffle
        }

        public enum SpawnLocation
        {
            Default,
            SameSceneAsTarget,
            ChildOfTarget,
            DontDestroyOnLoad
        }

        public GameObject[] factoryBlueprints;
        public BlueprintSelectionMode blueprintSelectionMode = BlueprintSelectionMode.Random;
        
        public GameObject spawnTarget;
        public SpawnLocation spawnLocation = SpawnLocation.SameSceneAsTarget;
        
        [Tooltip("Sacrifices oldest instance if necessary")]
        public bool sacrificeOldest;
        public bool respawnTarget = true;
        public float respawnDelay = 3.0f;
        public bool reapInstancesOnDestroy = true;

        [Min(1), SerializeField]
        private int maxInstances = 1;
        
        public UnityEvent onSpawn;
        public UnityEvent onRespawn;

        private List<GameObject> instances;

        private void OnEnable()
        {
            instances?.RemoveAll(item => item == null);
        }

        private void OnDestroy()
        {
            if(reapInstancesOnDestroy && instances != null)
            {
                foreach (var instance in instances.Where(instance => instance != null))
                {
                    Destroy(instance);
                }
            }
        }

        public void SetTarget(GameObject target)
        {
            if(target != null)
            {
                spawnTarget = target;
            }
        }

        public GameObject GetInstance(int index)
        {
            if (instances != null && instances.Count > index)
                return instances[index];
            else
                return null;
        }

        public void Spawn()
        {
            if(spawnTarget == null || factoryBlueprints == null  || factoryBlueprints.Length == 0)
            {
                Debug.LogWarning(
                    $"Factory '{gameObject.name}' : Cannot spawn as there are no spawn target or factory blueprints");
                return;
            }

            if (instances == null)
                instances = new List<GameObject>();

            if(instances.Count == maxInstances && sacrificeOldest)
            {
                var oldest = instances[0];
                instances.RemoveAt(0);
                Destroy(oldest);
            }

            if (instances.Count < maxInstances)
            {
                var newInstance = Spawn(SelectBlueprint(), spawnTarget);

                switch(spawnLocation)
                {
                    case SpawnLocation.Default:
                        break;
                    case SpawnLocation.SameSceneAsTarget:
                        UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(newInstance, spawnTarget.scene);
                        break;
                    case SpawnLocation.ChildOfTarget:
                        newInstance.transform.parent = spawnTarget.transform;
                        break;
                    case SpawnLocation.DontDestroyOnLoad:
                        DontDestroyOnLoad(newInstance);
                        break;
                }

                instances.Add(newInstance);
                onSpawn?.Invoke();
            }

        }

        private void LateUpdate()
        {
            if(instances != null)
            {
                List<int> todelete = new List<int>();
                for(int i = 0; i < instances.Count; i++)
                {
                    if(instances[i] == null)
                    {
                        todelete.Add(i);
                    }
                }

                foreach (var index in todelete)
                {
                    instances.RemoveAt(index);

                    if(respawnTarget)
                        AddRespawnCoroutine();
                }
            }
        }

        private List<Coroutine> respawnCoroutines; 

        private void AddRespawnCoroutine()
        {
            if (respawnCoroutines == null)
                respawnCoroutines = new List<Coroutine>();
            else
            {
                respawnCoroutines.RemoveAll(o => o == null);
            }

            respawnCoroutines.Add(StartCoroutine(Respawn(respawnDelay)));
        }

        private IEnumerator Respawn(float time)
        {
            yield return new WaitForSeconds(time);
            onRespawn?.Invoke();
            Spawn();
        }

        private GameObject Spawn(GameObject blueprint, GameObject target)
        {
            var go = Instantiate(blueprint, target.transform.position, target.transform.rotation);
            go.name = (blueprint.name);
            return go;
        }

        private int currentBlueprintIndex = -1;

        private GameObject SelectBlueprint()
        {
            if(factoryBlueprints == null || factoryBlueprints.Length == 0)
            {
                Debug.LogError($"Factory '{gameObject.name}' could not spawn anything as there are no blueprints set up");
                return null;
            }

            switch(blueprintSelectionMode)
            {
                default:
                    currentBlueprintIndex = Random.Range(0, factoryBlueprints.Length);
                    break;
                case BlueprintSelectionMode.Sequential:
                    currentBlueprintIndex++;
                    currentBlueprintIndex %= factoryBlueprints.Length;
                    break;
                case BlueprintSelectionMode.Shuffle:
                    currentBlueprintIndex = Shuffle(currentBlueprintIndex);
                    break;
            }
            return factoryBlueprints[currentBlueprintIndex];
        }

        private List<int> shuffleIndices;

        private int Shuffle(int i)
        {
            if(shuffleIndices == null || shuffleIndices.Count != factoryBlueprints.Length)
            {
                shuffleIndices = Enumerable.Range(0, factoryBlueprints.Length).OrderBy(x => Random.value).ToList();
            }
            return shuffleIndices[(shuffleIndices.IndexOf(i) + 1) % shuffleIndices.Count];
        }
    }
}