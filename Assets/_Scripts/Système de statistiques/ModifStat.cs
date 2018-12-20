/**
 * ModifStat.cs
 * Classe de l'objet de modificateur de statistique
 * @author Yoann Paquette
 * @version Mercredi 19 Décembre 2018
 */
namespace StatsPersoSysteme //Espace de nom du système de statistiques
{
    public class ModifStat
    {
        public readonly float ValeurMod; //Valeur du modificateur
        public readonly object Source; //Source du modificateur

        /**
         * Constructeur par défaut de la classe ModifStat
         * @param float valeurMod [Valeur du modificateur]
         * @param object source [Source du modificateur]
         */
        public ModifStat(float valeurMod, object source)
        {
            ValeurMod = valeurMod;
            Source = source;
        }

        /**
         * Constructeur optionel de la classe ModifStat
         * @param float valeurMod [Valeur du modificateur]
         * @param object source (Optionel) [Source du modificateur. Si non spécifié, met la source comme null] 
         */
        public ModifStat(float valeurMod) : this(valeurMod, null) { }
    }
}