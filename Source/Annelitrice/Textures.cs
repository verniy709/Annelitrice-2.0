using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace Annelitrice
{
    [StaticConstructorOnStartup]
    public static class Textures
    {
        public static Dictionary<BodyPartDef, Dictionary<HediffDef, Texture2D>> pairedAppendanceTextures = new Dictionary<BodyPartDef, Dictionary<HediffDef, Texture2D>>();
        public static readonly Texture2D BackgroundMenu = ContentFinder<Texture2D>.Get("Anneli_UI/UI_Background");
        static Textures()
        {

        }
        //public static readonly Texture2D Arm_Left/BoneBlade_UI = ContentFinder<Texture2D>.Get("Anneli_UI/Appendages_UI/Arm_Left/BoneBlade_UI");
        //public static readonly Texture2D Arm_Left/BoneSpike_UI = ContentFinder<Texture2D>.Get("Anneli_UI/Appendages_UI/Arm_Left/BoneSpike_UI");
        //public static readonly Texture2D Arm_Left/CrushingArm_UI = ContentFinder<Texture2D>.Get("Anneli_UI/Appendages_UI/Arm_Left/CrushingArm_UI");
        //public static readonly Texture2D Arm_Left/Tentacle_UI = ContentFinder<Texture2D>.Get("Anneli_UI/Appendages_UI/Arm_Left/Tentacle_UI");
        //public static readonly Texture2D Arm_Right/BoneBlade_UI = ContentFinder<Texture2D>.Get("Anneli_UI/Appendages_UI/Arm_Right/BoneBlade_UI");
        //public static readonly Texture2D Arm_Right/BoneSpike_UI = ContentFinder<Texture2D>.Get("Anneli_UI/Appendages_UI/Arm_Right/BoneSpike_UI");
        //public static readonly Texture2D Arm_Right/CrushingArm_UI = ContentFinder<Texture2D>.Get("Anneli_UI/Appendages_UI/Arm_Right/CrushingArm_UI");
        //public static readonly Texture2D Arm_Right/Tentacle_UI = ContentFinder<Texture2D>.Get("Anneli_UI/Appendages_UI/Arm_Right/Tentacle_UI");
        //public static readonly Texture2D Back/BoneWing_UI = ContentFinder<Texture2D>.Get("Anneli_UI/Appendages_UI/Back/BoneWing_UI");
        //public static readonly Texture2D Back/Lump_UI = ContentFinder<Texture2D>.Get("Anneli_UI/Appendages_UI/Back/Lump_UI");
        //public static readonly Texture2D Back/PatagiumWing_UI = ContentFinder<Texture2D>.Get("Anneli_UI/Appendages_UI/Back/PatagiumWing_UI");
        //public static readonly Texture2D Back/Tentacle_UI = ContentFinder<Texture2D>.Get("Anneli_UI/Appendages_UI/Back/Tentacle_UI");
        //public static readonly Texture2D Head/Kuchisake_UI = ContentFinder<Texture2D>.Get("Anneli_UI/Appendages_UI/Head/Kuchisake_UI");
        //public static readonly Texture2D Head/Piercing_UI = ContentFinder<Texture2D>.Get("Anneli_UI/Appendages_UI/Head/Piercing_UI");
        //public static readonly Texture2D Head/ProliferatingBrain_UI = ContentFinder<Texture2D>.Get("Anneli_UI/Appendages_UI/Head/ProliferatingBrain_UI");
        //public static readonly Texture2D Head/ThirdEye_UI = ContentFinder<Texture2D>.Get("Anneli_UI/Appendages_UI/Head/ThirdEye_UI");
        //public static readonly Texture2D Leg_Left/Arachno_UI = ContentFinder<Texture2D>.Get("Anneli_UI/Appendages_UI/Leg_Left/Arachno_UI");
        //public static readonly Texture2D Leg_Left/DollJoint_UI = ContentFinder<Texture2D>.Get("Anneli_UI/Appendages_UI/Leg_Left/DollJoint_UI");
        //public static readonly Texture2D Leg_Left/Lumps_UI = ContentFinder<Texture2D>.Get("Anneli_UI/Appendages_UI/Leg_Left/Lumps_UI");
        //public static readonly Texture2D Leg_Left/VenomGland_UI = ContentFinder<Texture2D>.Get("Anneli_UI/Appendages_UI/Leg_Left/VenomGland_UI");
        //public static readonly Texture2D Leg_Right/Arachno_UI = ContentFinder<Texture2D>.Get("Anneli_UI/Appendages_UI/Leg_Right/Arachno_UI");
        //public static readonly Texture2D Leg_Right/DollJoint_UI = ContentFinder<Texture2D>.Get("Anneli_UI/Appendages_UI/Leg_Right/DollJoint_UI");
        //public static readonly Texture2D Leg_Right/Lumps_UI = ContentFinder<Texture2D>.Get("Anneli_UI/Appendages_UI/Leg_Right/Lumps_UI");
        //public static readonly Texture2D Leg_Right/VenomGland_UI = ContentFinder<Texture2D>.Get("Anneli_UI/Appendages_UI/Leg_Right/VenomGland_UI");
        //public static readonly Texture2D Peritoneum/Banelings_UI = ContentFinder<Texture2D>.Get("Anneli_UI/Appendages_UI/Peritoneum/Banelings_UI");
        //public static readonly Texture2D Peritoneum/Boomalope_UI = ContentFinder<Texture2D>.Get("Anneli_UI/Appendages_UI/Peritoneum/Boomalope_UI");
        //public static readonly Texture2D Peritoneum/GastrovascularCavity_UI = ContentFinder<Texture2D>.Get("Anneli_UI/Appendages_UI/Peritoneum/GastrovascularCavity_UI");
        //public static readonly Texture2D Peritoneum/Piercing_UI = ContentFinder<Texture2D>.Get("Anneli_UI/Appendages_UI/Peritoneum/Piercing_UI");
        public static readonly Texture2D Blue_Minus = ContentFinder<Texture2D>.Get("Anneli_UI/Blue_Minus");
        public static readonly Texture2D Blue_Plus = ContentFinder<Texture2D>.Get("Anneli_UI/Blue_Plus");
        public static readonly Texture2D Empty_Slot = ContentFinder<Texture2D>.Get("Anneli_UI/Empty_Slot");
        public static readonly Texture2D Green_Minus = ContentFinder<Texture2D>.Get("Anneli_UI/Green_Minus");
        public static readonly Texture2D Green_Plus = ContentFinder<Texture2D>.Get("Anneli_UI/Green_Plus");
        public static readonly Texture2D Heal_Button = ContentFinder<Texture2D>.Get("Anneli_UI/Heal_Button");
        public static readonly Texture2D Red_Minus = ContentFinder<Texture2D>.Get("Anneli_UI/Red_Minus");
        public static readonly Texture2D Red_Plus = ContentFinder<Texture2D>.Get("Anneli_UI/Red_Plus");
        public static readonly Texture2D UI_Evolution_Points = ContentFinder<Texture2D>.Get("Anneli_UI/UI_Evolution_Points");
    }
}
