namespace UnityEngine
{
    //Namespace personaliser pour la classe d'information du système d'évènement
    namespace SystemeEventsLib
    {
        //Liste des différents évènements
        public enum NomEvent { MortEvent, AttaqueEvent, DegatEvent, ScoreUpdateEvent}
        //Signature des fonctions accepté dans le dictionaire d'évènements
        public delegate void Action<in InfoEvent>(InfoEvent infoEvent);

        //Classe d'informations des évènements
        public class InfoEvent
        {
            public int Id { get; set; } //Identification de l'objet
            public GameObject Cible { get; set; } //Cible qui doit être affectée

            //Contructeur par défaut de la classe
            public InfoEvent(int id = 10000, GameObject cible = null)
            {
                this.Id = id;
                this.Cible = cible;
            }
        }
    }
}