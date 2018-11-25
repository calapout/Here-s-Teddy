namespace UnityEngine
{
    namespace EventCentralization
    {
        public enum EventName { OnDeath, OnAttack, OnDamage, OnScoreChange}
        public delegate void Action<in EventInfo>(EventInfo eventInfo);
        
        public class EventInfo
        {
            public int Id { get; set; }
            public GameObject Target { get; set; }
            public int Damage { get; set; }
            public int Health { get; set; }
            public GameObject HealthUI { get; set; }

            public EventInfo (int id = 10000, GameObject target = null)
            {
                this.Id = id;
                this.Target = target;
            }
        }
    }
}