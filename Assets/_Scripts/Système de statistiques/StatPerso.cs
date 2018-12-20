using System;
using System.Collections.Generic;
using System.Collections.ObjectModel; //Collections C#

/**
 * StatPerso.cs
 * Classe d'objet d'une statistique de joueur
 * @author Yoann paquette
 * @version Mercredi 19 Décembre 2018
 */
namespace StatsPersoSysteme //Espace de nom du système de statistiques
{
    [Serializable]
    public class StatPerso
    {
        public float ValeurBase { get; set; } //Valeur de base de la statistique
        //Valeur de la statistique avec les modificateurs appliqués
        public float Stat
        {
            get
            {
                //Si la valeur à changée OU qu'elle est différente de la précédente...
                if (aChanger || ValeurBase != prevValeurBase)
                {
                    prevValeurBase = ValeurBase; //Applique la nouvelle valeur de base
                    _stat = CalculerStat(); //Calcule la statistique avec les modificateurs
                    aChanger = false;
                }
                return _stat;
            }
        }

        bool aChanger = true; //La statistique à tel changée?
        float _stat; //Valeur privée de la statistique
        float prevValeurBase = float.MinValue; //Valeur présédente de la statistique

        //readonly = Des éléments peuvent y être ajoutés ou retirés, mais un élément ne peut pas y être modifié directement
        readonly List<ModifStat> modifsStat; //Liste privée des modificateurs qui affecte la statistique
        public readonly ReadOnlyCollection<ModifStat> ModifsStat; //Collection des modificateurs qui affecte la statistique

        /**
         * Constructeur par défaut de la classe StstPerso
         * Initialisation de la Liste et de la Collection
         * @param void
         */
        public StatPerso()
        {
            modifsStat = new List<ModifStat>();
            ModifsStat = modifsStat.AsReadOnly();
        }

        /**
         * Constructeur optionel de la classe StstPerso
         * @param float valeurBase (Optionel) [Valeur de base de la statistique]
         */
        public StatPerso(float valeurBase) : this()
        {
            ValeurBase = valeurBase;
        }

        /**
         * Fonction qui ajoute un objet de la class ModifStat à la Liste de modificateurs
         * @param ModifStat mod
         * @return void
         */
        public void AjouterModif(ModifStat mod)
        {
            aChanger = true;
            modifsStat.Add(mod);
        }

        /**
         * Fonction qui retire un objet de la class ModifStat à la Liste de modificateurs et qui renvoit une confirmation
         * @param ModifStat mod
         * @return bool (false = pas retiré, true = retiré)
         */
        public bool RetirerModif(ModifStat mod)
        {
            //Si le modificateur a été retiré avec succès, return true
            //Sinon, return false
            if (modifsStat.Remove(mod))
            {
                aChanger = true;
                return true;
            }
            return false;
        }

        /**
         * Fonction qui retire tous les objets de la class ModifStat, selon la source, à la Liste de modificateurs et qui renvoit une confirmation
         * @param object source [Source des modificateurs]
         * @return bool (false = pas retiré, true = retiré)
         */
        public bool RetirerTousModifSource(object source)
        {
            bool aRetirer = false;

            //Boucle à travers tous les éléments de la Listes de modificateurs
            for (int i = modifsStat.Count - 1; i >= 0; i--)
            {
                //Si la source du modificateur est la même que celle spécifiée...
                if (modifsStat[i].Source == source)
                {
                    //Retire le modificateur de la Liste
                    aChanger = true;
                    aRetirer = true;
                    modifsStat.RemoveAt(i);
                }
            }
            return aRetirer;
        }

        /**
         * Fonction qui calcule et qui retourne la valeur de la statistique avec les modificateurs appliquées
         * @param void
         * @return float (Valeur de la statistique)
         */
        float CalculerStat()
        {
            float valeurStat = ValeurBase;

            //Pour chaque modificateur dans la Liste, ajoute la valeur à la statistique
            //et retourne un arrondissement (si pourcentage) de la valeur totale
            foreach (ModifStat mod in modifsStat)
            {
                valeurStat += mod.ValeurMod;
            }
            return (float)Math.Round(valeurStat, 4);
        }
    }
}