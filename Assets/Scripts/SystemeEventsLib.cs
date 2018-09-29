namespace UnityEngine
{
    namespace SystemeEventsLib
    {
        public enum NomEvent { MortEvent, AttaqueEvent, DegatEvent, ScoreUpdateEvent}
        public delegate void Action<in InfoEvent>(InfoEvent infoEvent);

        public class InfoEvent
        {
            public int Id { get; set; }
            public GameObject Cible { get; set; }
            public int Degat { get; set; }
            public int Vie { get; set; }
            public GameObject BarreVie { get; set; }

            public InfoEvent(int id = 10000, GameObject cible = null)
            {
                this.Id = id;
                this.Cible = cible;
            }
        }
    }
}