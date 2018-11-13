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
            public int Experience { get; set; } //Expérience gagner par le joueur
            public int ExpMax { get; set; } //Expérience pour atteindre le prochain niveau
            public int ExpTotal { get; set; } //Expérience total gagner par le joueur
            public int ExpNextNiveau { get; set; } //Expérience pour atteindre le prochain niveau en comptent les niveaux précédents
            public bool EnPause { get; set; } //Vérification de si le jeu doit être en pause
            public int HP { get; set; } //Points de vie du joueur
            public int HPMax { get; set; } //Points de vie maximum du joueur
            public int Niveau { get; set; } //Niveau du joueur
            //Contructeur par défaut de la classe
            public InfoEvent(GameObject cible = null)
            {
                this.Cible = cible;
            }
        }
    }
}