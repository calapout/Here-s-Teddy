namespace StatsPersoSysteme
{
    public class ModifStat
    {
        public readonly float ValeurMod;
        public readonly object Source;

        public ModifStat(float valeurMod, object source)
        {
            ValeurMod = valeurMod;
            Source = source;
        }

        public ModifStat(float valeurMod) : this(valeurMod, null) { }
    }
}