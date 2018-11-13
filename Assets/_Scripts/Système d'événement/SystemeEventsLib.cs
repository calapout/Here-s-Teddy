namespace UnityEngine
{
    //Namespace personaliser pour la classe d'information du système d'évènement
    namespace SystemeEventsLib
    {
        //Liste des différents évènements
        public enum NomEvent { mortEnnemiEvent, spawnEvent, sauvegardeEvent, chargerEvent, pauseEvent, chargementEvent, quitterEvent, updateUiVieEvent, updateUiExpEvent}
        //Signature des fonctions accepté dans le dictionaire d'évènements
        public delegate void Action<in InfoEvent>(InfoEvent infoEvent);

        //Classe d'informations des évènements
        public class InfoEvent {
            public GameObject Cible { get; set; } //Cible qui doit être affectée
            public int Ennemi { get; set; } //L'index de l'ennemi à instantier
            public Vector3 Position { get; set; } //La position de l'ennemi
            public int Experience { get; set; }
            public int ExpMax { get; set; }
            public int ExpTotal { get; set; }
            public int ExpNextNiveau { get; set; }
            public bool EnPause { get; set; }
            public int HP { get; set; }
            public int HPMax { get; set; }
            public int Niveau { get; set; }
            //Contructeur par défaut de la classe
            public InfoEvent(GameObject cible = null)
            {
                this.Cible = cible;
            }
        }
    }
}