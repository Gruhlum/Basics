using UnityEngine;

namespace HexTecGames.Basics
{
    public class Shake : MonoBehaviour
    {
        [SerializeField] private Transform target = default;
        [Header("Settings")]
        [SerializeField] private int power = 2;
        [SerializeField] private float decaySpeed = 1f;
        [SerializeField] private float frequency = 1;
        [SerializeField] private float intensity = 1;
        [SerializeField] private float maxAngle = 15f;

        public float Trauma
        {
            get
            {
                return trauma;
            }
            private set
            {
                trauma = value;
            }
        }
        private float trauma;



        private void Reset()
        {
            target = GetComponent<Transform>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                AddTrauma(1);
            }
            if (Input.GetKeyDown(KeyCode.V))
            {
                AddTrauma(0.5f);
            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                AddTrauma(0.2f);
            }

            if (Trauma > 0)
            {
                ApplyTrauma();
                DecayTrauma();
            }
        }

        private void ApplyTrauma()
        {
            float currentIntensity = Mathf.Pow(Trauma, power) * intensity;
            float noiseX = Mathf.PerlinNoise1D(Time.time * frequency) - 0.5f;
            float noiseY = Mathf.PerlinNoise1D((Time.time + 3.36f) * frequency) - 0.5f;
            float noiseRotation = Mathf.PerlinNoise1D((Time.time + 5.31f) * frequency) - 0.5f;
            Vector3 targetVector = new Vector3(noiseX * currentIntensity, noiseY * currentIntensity, target.transform.position.z);
            Quaternion targetRotation = Quaternion.Euler(0, 0, noiseRotation * currentIntensity * maxAngle);
            Debug.Log(currentIntensity + " - " + targetVector);
            target.transform.position = Vector3.Lerp(target.transform.position, targetVector, Time.deltaTime * 50);
            target.rotation = Quaternion.Lerp(target.transform.rotation, targetRotation, Time.deltaTime * 50);
        }

        private void DecayTrauma()
        {
            Trauma = Mathf.MoveTowards(Trauma, 0, Time.deltaTime * decaySpeed);
        }

        public void AddTrauma(float value)
        {
            Trauma = Mathf.Min(Trauma + value, 1);
        }
    }
}