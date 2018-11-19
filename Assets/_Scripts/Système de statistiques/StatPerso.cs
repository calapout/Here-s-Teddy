using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace StatsPersoSysteme
{
    [Serializable]
    public class StatPerso
    {
        public float ValeurBase { get; set; }
        public float Stat
        {
            get
            {
                if (aChanger || ValeurBase != prevValeurBase)
                {
                    prevValeurBase = ValeurBase;
                    _stat = CalculerStat();
                    aChanger = false;
                }
                return _stat;
            }
        }

        bool aChanger = true;
        float _stat;
        float prevValeurBase = float.MinValue;

        readonly List<ModifStat> modifsStat;
        public readonly ReadOnlyCollection<ModifStat> ModifsStat;

        public StatPerso()
        {
            modifsStat = new List<ModifStat>();
            ModifsStat = modifsStat.AsReadOnly();
        }

        public StatPerso(float valeurBase) : this()
        {
            ValeurBase = valeurBase;
        }

        public void AjouterModif(ModifStat mod)
        {
            aChanger = true;
            modifsStat.Add(mod);
        }

        public bool RetirerModif(ModifStat mod)
        {
            if (modifsStat.Remove(mod))
            {
                aChanger = true;
                return true;
            }
            return false;
        }

        public bool RetirerTousModifSource(object source)
        {
            bool aRetirer = false;

            for (int i = modifsStat.Count - 1; i >= 0; i--)
            {
                if (modifsStat[i].Source == source)
                {
                    aChanger = true;
                    aRetirer = true;
                    modifsStat.RemoveAt(i);
                }
            }
            return aRetirer;
        }

        float CalculerStat()
        {
            float valeurStat = ValeurBase;

            foreach (ModifStat mod in modifsStat)
            {
                valeurStat += mod.ValeurMod;
            }
            return (float)Math.Round(valeurStat, 4);
        }
    }
}