using UnityEngine;

public class DestructionExplosion : MonoBehaviour {

    ParticleSystem ps;

    void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        if (ps.isStopped)
        {
            Destroy(gameObject);
        }
    }
}