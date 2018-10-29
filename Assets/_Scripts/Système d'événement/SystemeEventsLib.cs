namespace UnityEngine
{
    //Namespace personaliser pour la classe d'information du système d'évènement
    namespace SystemeEventsLib
    {
        //Liste des différents évènements
        public enum NomEvent { MortEvent, spawnEvent }
        //Signature des fonctions accepté dans le dictionaire d'évènements
        public delegate void Action<in InfoEvent>(InfoEvent infoEvent);

        //Classe d'informations des évènements
        public class InfoEvent {
            public int Id { get; set; } //Identification de l'objet
            public GameObject Cible { get; set; } //Cible qui doit être affectée
            public int Ennemi { get; set; } //L'index de l'ennemi à instantier
            public Vector3 Position { get; set; } //La position de l'ennemi

            //Contructeur par défaut de la classe
            public InfoEvent(int id = 10000, GameObject cible = null)
            {
                this.Id = id;
                this.Cible = cible;
            }
        }
    }
}