using UnityEngine;

public class InstanciationExplosion : MonoBehaviour {

    AudioSource audioSource;//variable de l'audiosource
    Vector3 position;
    
    public void Boom()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.enabled = true;
        position = transform.position;
        Instantiate(Resources.Load("Explosion_Res") as GameObject, position, new Quaternion());
    }
}