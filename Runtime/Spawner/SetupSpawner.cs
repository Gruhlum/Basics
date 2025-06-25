using System.Collections;
using System.Collections.Generic;
using HexTecGames.Basics;
using UnityEngine;

namespace HexTecGames.Basics
{
    [System.Serializable]
    public class SetupSpawner<T, D> : Spawner<T> where T : Component, ISetup<D>
    {
        
        public T SpawnAndSetup(D data, bool activate = true)
        {
            T t = Spawn(activate);
            t.Setup(data);
            return t;
        }

        public List<T> SpawnAndSetup(IList<D> datas, bool activate = true)
        {
            var results = new List<T>();
            for (int i = 0; i < datas.Count; i++)
            {
                results.Add(SpawnAndSetup(datas[i], activate));
            }
            return results;
        }

        public List<T> DeactivateAllAndSpawnAndSetup(IList<D> datas, bool activate = true)
        {
            var results = DeactivateAllAndSpawn(datas.Count, activate);
            for (int i = 0; i < results.Count; i++)
            {
                results[i].Setup(datas[i]);
            }
            return results;
        }
    }
}