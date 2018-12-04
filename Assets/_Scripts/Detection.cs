using UnityEngine;
using UnityEngine.AI;

public class Detection : MonoBehaviour {
    /*
            Gestion de détection du joueur autour des objets et des ennemis pour leur destruction
     */

    [Header("       OverlapSphere")]
    public bool cast; //Utilise l'overlap sphere (Comme un SphereCollider en trigger,mias avec plus de contrôle)
    public Vector3 offset; //Offset de la position de création de la sphère
    public float rayon; //Rayon de la sphère
    public LayerMask layerMask; //LayerMask à ne pas ignorer
    public string nomAChercher; //Nom du gameObject à chercher
    public string tagAChercher; //Tag du gameObject à chercher

    [HideInInspector]
    public Collider[] colliderHits; //Tableau de tous les colliders qui répondes aux critères
    [HideInInspector]
    public bool trouvee; //Indique si le joueur a été trouvé

    Ennemi ennemi; //Référence script ennemi

    //Vérification dès termes de recherche entrées
    void Start() {
        if (nomAChercher == null) {
            nomAChercher = "";
        }

        if (tagAChercher == null) {
            tagAChercher = "";
        }

        if (gameObject.tag == "Ennemi") {
            ennemi = GetComponent<Ennemi>();
        }
    }

    //Destruction de l'item si le joueur n'est pas trouvé et destruction de l'ennemi s'il est mort et que le joueur n'est pas trouvé
    void Update() {
        if (cast) {
            if (nomAChercher != "" || tagAChercher != "") {
                Chercher();
            }
        }
    }

    //Cast la sphère et désactive le NavMeshAgent des ennemis vivant qui sont hors de portée du joueur
    void Chercher() {
        colliderHits = Physics.OverlapSphere(transform.position + offset, rayon, layerMask, QueryTriggerInteraction.Ignore);

        if (colliderHits.Length > 0) {
            foreach (Collider col in colliderHits) {
                if (col.tag == tagAChercher || col.name == nomAChercher) {
                    trouvee = true;
                    break;
                }
                else {
                    trouvee = false;
                }
            }
        }
        else {
            trouvee = false;
        }
    }

    /*//Affichage d'une sphère de debuggage
    void OnDrawGizmosSelected() {
        //Zone de détection en vert
        Gizmos.color = new Color(0f, 0.75f, 0f, 1f);
        Gizmos.DrawWireSphere(transform.position + offset, rayon);
    }*/

}
