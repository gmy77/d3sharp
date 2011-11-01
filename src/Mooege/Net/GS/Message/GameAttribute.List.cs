
namespace Mooege.Net.GS.Message
{
    public partial class GameAttribute
    {
        // TODO: move into categories? will probably just end up as properties on actor objects

        public static readonly GameAttributeI Axe_Bad_Data = new GameAttributeI(0, 0, -1, 0, 0, "", "", "Axe_Bad_Data", GameAttributeEncoding.Int, 0, 0, 0, 32);
        public static readonly GameAttributeI Attribute_Timer = new GameAttributeI(1, 0, 5, 0, 1, "", "", "Attribute_Timer", GameAttributeEncoding.Int, 31, 0, 0, 32);
        public static readonly GameAttributeI Attribute_Pool = new GameAttributeI(2, 0, -1, 0, 1, "", "", "Attribute_Pool", GameAttributeEncoding.Int, 1, 0, 0, 32);
        public static readonly GameAttributeI Death_Count = new GameAttributeI(3, 0, -1, 0, 1, "", "", "Death_Count", GameAttributeEncoding.Int, 1, 0, 0, 32);
        public static readonly GameAttributeI DualWield_Hand = new GameAttributeI(4, 0, -1, 0, 1, "", "", "DualWield_Hand", GameAttributeEncoding.IntMinMax, 1, 0, 2, 2);
        public static readonly GameAttributeI DualWield_Hand_Next = new GameAttributeI(5, 0, -1, 0, 1, "", "", "DualWield_Hand_Next", GameAttributeEncoding.IntMinMax, 1, 0, 2, 2);
        public static readonly GameAttributeI Respawn_Game_Time = new GameAttributeI(6, 0, -1, 0, 1, "", "", "Respawn_Game_Time", GameAttributeEncoding.Int, 1, 0, 0, 32);
        public static readonly GameAttributeI Backpack_Slots = new GameAttributeI(7, 0, -1, 0, 1, "", "", "Backpack_Slots", GameAttributeEncoding.IntMinMax, 1, 0, 128, 8);
        public static readonly GameAttributeI Shared_Stash_Slots = new GameAttributeI(8, 0, -1, 0, 1, "", "", "Shared_Stash_Slots", GameAttributeEncoding.IntMinMax, 1, 0, 350, 9);
        public static readonly GameAttributeF Attack = new GameAttributeF(9, 0f, -1, 0, 0, "", "((Attack.Agg + Stats_All_Bonus + Attack_Bonus) * (1 + Attack_Bonus_Percent)) * (1 - Attack_Reduction_Percent)", "Attack", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeF Precision = new GameAttributeF(10, 0f, -1, 0, 0, "", "((Precision.Agg + Stats_All_Bonus + Precision_Bonus) * (1 + Precision_Bonus_Percent)) * (1 - Precision_Reduction_Percent)", "Precision", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeF Vitality = new GameAttributeF(11, 0f, -1, 0, 0, "", "((Vitality.Agg + Stats_All_Bonus + Vitality_Bonus) * (1 + Vitality_Bonus_Percent)) * (1 - Vitality_Reduction_Percent)", "Vitality", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeF Defense = new GameAttributeF(12, 0f, -1, 0, 0, "", "((Defense.Agg + Stats_All_Bonus + Defense_Bonus) * (1 + Defense_Bonus_Percent)) * (1 - Defense_Reduction_Percent)", "Defense", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeF Attack_Bonus = new GameAttributeF(13, 0f, -1, 0, 0, "", "", "Attack_Bonus", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeF Precision_Bonus = new GameAttributeF(14, 0f, -1, 0, 0, "", "", "Precision_Bonus", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeF Vitality_Bonus = new GameAttributeF(15, 0f, -1, 0, 0, "", "", "Vitality_Bonus", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeF Defense_Bonus = new GameAttributeF(16, 0f, -1, 0, 0, "", "", "Defense_Bonus", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeF Attack_Bonus_Percent = new GameAttributeF(17, 0f, -1, 0, 0, "", "", "Attack_Bonus_Percent", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeF Precision_Bonus_Percent = new GameAttributeF(18, 0f, -1, 0, 0, "", "", "Precision_Bonus_Percent", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeF Vitality_Bonus_Percent = new GameAttributeF(19, 0f, -1, 0, 0, "", "", "Vitality_Bonus_Percent", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeF Defense_Bonus_Percent = new GameAttributeF(20, 0f, -1, 0, 0, "", "", "Defense_Bonus_Percent", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeF Attack_Reduction_Percent = new GameAttributeF(21, 0f, -1, 1, 0, "", "", "Attack_Reduction_Percent", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeF Precision_Reduction_Percent = new GameAttributeF(22, 0f, -1, 1, 0, "", "", "Precision_Reduction_Percent", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeF Vitality_Reduction_Percent = new GameAttributeF(23, 0f, -1, 1, 0, "", "", "Vitality_Reduction_Percent", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeF Defense_Reduction_Percent = new GameAttributeF(24, 0f, -1, 1, 0, "", "", "Defense_Reduction_Percent", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeF Armor = new GameAttributeF(25, 0, -1, 0, 0, "", "", "Armor", GameAttributeEncoding.Float32, 9, 0, 0, 32);
        public static readonly GameAttributeF Armor_Bonus_Percent = new GameAttributeF(26, 0, -1, 0, 0, "", "", "Armor_Bonus_Percent", GameAttributeEncoding.Float32, 9, 0, 0, 32);
        public static readonly GameAttributeF Armor_Item = new GameAttributeF(27, 0, -1, 0, 0, "", "0", "Armor_Item", GameAttributeEncoding.Float32, 9, 0, 0, 32);
        public static readonly GameAttributeF Armor_Bonus_Item = new GameAttributeF(28, 0, -1, 0, 0, "", "", "Armor_Bonus_Item", GameAttributeEncoding.Float32, 9, 0, 0, 32);
        public static readonly GameAttributeF Armor_Item_Percent = new GameAttributeF(29, 0, -1, 0, 0, "", "0", "Armor_Item_Percent", GameAttributeEncoding.Float32, 9, 0, 0, 32);
        public static readonly GameAttributeF Armor_Item_SubTotal = new GameAttributeF(30, 0, -1, 0, 0, "FLOOR((Armor_Item + Armor_Bonus_Item) * (Armor_Item_Percent + 1))", "", "Armor_Item_SubTotal", GameAttributeEncoding.Float32, 9, 0, 0, 32);
        public static readonly GameAttributeF Armor_Item_Total = new GameAttributeF(31, 0, -1, 0, 0, "(Armor_Item > 0)?(Max(Armor_Item_SubTotal, 1)):Armor_Item_SubTotal", "", "Armor_Item_Total", GameAttributeEncoding.Float32, 9, 0, 0, 32);
        public static readonly GameAttributeF Armor_Total = new GameAttributeF(32, 0, -1, 0, 0, "", "FLOOR((Armor + Armor_Item_Total) * (Armor_Bonus_Percent + 1))", "Armor_Total", GameAttributeEncoding.Float32, 9, 0, 0, 32);
        public static readonly GameAttributeI Experience_Granted = new GameAttributeI(33, 0, -1, 0, 1, "", "", "Experience_Granted", GameAttributeEncoding.Int, 1, 0, 0, 32);
        public static readonly GameAttributeI Experience_Next = new GameAttributeI(34, 0, -1, 0, 1, "", "", "Experience_Next", GameAttributeEncoding.Int, 1, 0, 0, 32);
        public static readonly GameAttributeI Gold_Granted = new GameAttributeI(35, 0, -1, 0, 1, "", "", "Gold_Granted", GameAttributeEncoding.Int, 9, 0, 0, 32);
        public static readonly GameAttributeI Gold = new GameAttributeI(36, 0, -1, 0, 1, "", "", "Gold", GameAttributeEncoding.Int, 9, 0, 0, 32);
        public static readonly GameAttributeF Gold_Find = new GameAttributeF(37, 0f, -1, 0, 0, "", "", "Gold_Find", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeI Level = new GameAttributeI(38, 0, -1, 0, 1, "", "", "Level", GameAttributeEncoding.IntMinMax, 31, -1, 127, 8);
        public static readonly GameAttributeI Level_Cap = new GameAttributeI(39, 0, -1, 0, 1, "", "", "Level_Cap", GameAttributeEncoding.IntMinMax, 1, -1, 127, 8);
        public static readonly GameAttributeF Magic_Find = new GameAttributeF(40, 0f, -1, 0, 0, "", "", "Magic_Find", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeF Treasure_Find = new GameAttributeF(41, 0f, 14, 0, 0, "", "", "Treasure_Find", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeI Resource_Cost_Reduction_Amount = new GameAttributeI(42, 0, 10, 0, 1, "", "", "Resource_Cost_Reduction_Amount", GameAttributeEncoding.IntMinMax, 9, -4095, 16383, 15);
        public static readonly GameAttributeF Resource_Cost_Reduction_Total = new GameAttributeF(43, 0f, 10, 0, 0, "", "Resource_Cost_Reduction_Amount", "Resource_Cost_Reduction_Total", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeF Resource_Set_Point_Bonus = new GameAttributeF(44, 0f, 10, 0, 0, "", "", "Resource_Set_Point_Bonus", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeF Faster_Healing_Percent = new GameAttributeF(45, 0f, -1, 0, 0, "", "", "Faster_Healing_Percent", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeF Spending_Resource_Heals_Percent = new GameAttributeF(46, 0f, 10, 0, 0, "", "", "Spending_Resource_Heals_Percent", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeF Bonus_Healing_Received_Percent = new GameAttributeF(47, 0f, -1, 0, 0, "", "", "Bonus_Healing_Received_Percent", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeF Reduced_Healing_Received_Percent = new GameAttributeF(48, 0f, -1, 0, 0, "", "", "Reduced_Healing_Received_Percent", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeF Experience_Bonus = new GameAttributeF(49, 0f, -1, 0, 0, "", "", "Experience_Bonus", GameAttributeEncoding.Float16Or32, 9, 0f, 0f, 0);
        public static readonly GameAttributeF Experience_Bonus_Percent = new GameAttributeF(50, 0f, -1, 0, 0, "", "", "Experience_Bonus_Percent", GameAttributeEncoding.Float16Or32, 9, 0f, 0f, 0);
        public static readonly GameAttributeF Health_Globe_Bonus_Chance = new GameAttributeF(51, 0f, -1, 0, 0, "", "", "Health_Globe_Bonus_Chance", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeF Health_Globe_Bonus_Mult_Chance = new GameAttributeF(52, 0f, -1, 0, 0, "", "", "Health_Globe_Bonus_Mult_Chance", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeF Health_Globe_Bonus_Health = new GameAttributeF(53, 0f, -1, 0, 0, "", "", "Health_Globe_Bonus_Health", GameAttributeEncoding.Float16Or32, 9, 0f, 0f, 0);
        public static readonly GameAttributeF Increased_Health_From_Globes_Percent = new GameAttributeF(54, 0f, -1, 0, 0, "", "", "Increased_Health_From_Globes_Percent", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeF Increased_Health_From_Globes_Percent_Total = new GameAttributeF(55, 0f, -1, 0, 0, "", "Increased_Health_From_Globes_Percent", "Increased_Health_From_Globes_Percent_Total", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeF Bonus_Health_Percent_Per_Second_From_Globes = new GameAttributeF(56, 0f, -1, 0, 0, "", "", "Bonus_Health_Percent_Per_Second_From_Globes", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeF Bonus_Health_Percent_Per_Second_From_Globes_Total = new GameAttributeF(57, 0f, -1, 0, 0, "", "Bonus_Health_Percent_Per_Second_From_Globes", "Bonus_Health_Percent_Per_Second_From_Globes_Total", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeF Mana_Gained_From_Globes_Percent = new GameAttributeF(58, 0f, -1, 0, 0, "", "", "Mana_Gained_From_Globes_Percent", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeF Mana_Gained_From_Globes = new GameAttributeF(59, 0f, -1, 0, 0, "", "", "Mana_Gained_From_Globes", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeF Resistance = new GameAttributeF(60, 0f, 0, 0, 0, "", "", "Resistance", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeF Resistance_Percent = new GameAttributeF(61, 0f, 0, 0, 0, "", "", "Resistance_Percent", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeF Resistance_Total = new GameAttributeF(62, 0f, 0, 0, 0, "", "(Resistance + Resistance_All#NONE) * ((Resistance_Percent_All#NONE + Resistance_Percent + 1))", "Resistance_Total", GameAttributeEncoding.Float16Or32, 9, 0f, 0f, 0);
        public static readonly GameAttributeF Resistance_All = new GameAttributeF(63, 0f, -1, 0, 0, "", "", "Resistance_All", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeF Resistance_Percent_All = new GameAttributeF(64, 0f, -1, 0, 0, "", "", "Resistance_Percent_All", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeI Skill = new GameAttributeI(65, 0, 4, 0, 1, "", "", "Skill", GameAttributeEncoding.IntMinMax, 9, 0, 4095, 12);
        public static readonly GameAttributeI Skill_Total = new GameAttributeI(66, 0, 4, 0, 1, "", "Skill", "Skill_Total", GameAttributeEncoding.IntMinMax, 9, 0, 4095, 12);
        public static readonly GameAttributeI TeamID = new GameAttributeI(67, -1, -1, 1, 1, "", "", "TeamID", GameAttributeEncoding.IntMinMax, 31, -1, 23, 5);
        public static readonly GameAttributeI Team_Override = new GameAttributeI(68, -1, -1, 1, 1, "", "", "Team_Override", GameAttributeEncoding.IntMinMax, 31, -1, 23, 5);
        public static readonly GameAttributeB Invulnerable = new GameAttributeB(69, 0, -1, 1, 1, "", "", "Invulnerable", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeB Loading = new GameAttributeB(70, 0, -1, 1, 1, "", "", "Loading", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeI Loading_Player_ACD = new GameAttributeI(71, -1, -1, 3, 1, "", "", "Loading_Player_ACD", GameAttributeEncoding.Int, 0, 0, 0, 32);
        public static readonly GameAttributeI Loading_Power_SNO = new GameAttributeI(72, -1, -1, 3, 1, "", "", "Loading_Power_SNO", GameAttributeEncoding.Int, 0, 0, 0, 32);
        public static readonly GameAttributeI Loading_Anim_Tag = new GameAttributeI(73, -1, -1, 3, 1, "", "", "Loading_Anim_Tag", GameAttributeEncoding.Int, 0, 0, 0, 32);
        public static readonly GameAttributeB No_Damage = new GameAttributeB(74, 0, -1, 1, 1, "", "", "No_Damage", GameAttributeEncoding.IntMinMax, 0, 0, 1, 1);
        public static readonly GameAttributeB No_AutoPickup = new GameAttributeB(75, 0, -1, 1, 1, "", "", "No_AutoPickup", GameAttributeEncoding.IntMinMax, 0, 0, 1, 1);
        public static readonly GameAttributeF Light_Radius_Percent_Bonus = new GameAttributeF(76, 0f, 0, 0, 0, "", "", "Light_Radius_Percent_Bonus", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeF Hitpoints_Cur = new GameAttributeF(77, 0f, -1, 0, 0, "", "Min(Hitpoints_Cur.Agg, Hitpoints_Max_Total)", "Hitpoints_Cur", GameAttributeEncoding.Float16Or32, 31, 0f, 0f, 0);
        public static readonly GameAttributeF Hitpoints_Factor_Level = new GameAttributeF(78, 0f, -1, 0, 0, "", "", "Hitpoints_Factor_Level", GameAttributeEncoding.Float16Or32, 31, 0f, 0f, 0);
        public static readonly GameAttributeF Hitpoints_Factor_Vitality = new GameAttributeF(79, 0f, -1, 0, 0, "", "", "Hitpoints_Factor_Vitality", GameAttributeEncoding.Float16Or32, 31, 0f, 0f, 0);
        public static readonly GameAttributeF Hitpoints_Total_From_Vitality = new GameAttributeF(80, 0f, -1, 0, 0, "", "Vitality * Hitpoints_Factor_Vitality", "Hitpoints_Total_From_Vitality", GameAttributeEncoding.Float16Or32, 31, 0f, 0f, 0);
        public static readonly GameAttributeF Hitpoints_Total_From_Level = new GameAttributeF(81, 0f, -1, 0, 0, "", "(Level - 1) * Hitpoints_Factor_Level", "Hitpoints_Total_From_Level", GameAttributeEncoding.Float16Or32, 31, 0f, 0f, 0);
        public static readonly GameAttributeF Hitpoints_Granted = new GameAttributeF(82, 0f, -1, 0, 0, "", "", "Hitpoints_Granted", GameAttributeEncoding.Float16Or32, 31, 0f, 0f, 0);
        public static readonly GameAttributeI Hitpoints_Granted_Duration = new GameAttributeI(83, 0, -1, 0, 1, "", "", "Hitpoints_Granted_Duration", GameAttributeEncoding.IntMinMax, 31, 0, 16777215, 24);
        public static readonly GameAttributeF Hitpoints_Max = new GameAttributeF(84, 0f, -1, 0, 0, "", "", "Hitpoints_Max", GameAttributeEncoding.Float16Or32, 31, 0f, 0f, 0);
        public static readonly GameAttributeF Hitpoints_Max_Bonus = new GameAttributeF(85, 0f, -1, 0, 0, "", "", "Hitpoints_Max_Bonus", GameAttributeEncoding.Float16Or32, 31, 0f, 0f, 0);
        public static readonly GameAttributeF Hitpoints_Max_Total = new GameAttributeF(86, 0f, -1, 0, 0, "", "Max((Hitpoints_Max + Hitpoints_Total_From_Level + Hitpoints_Total_From_Vitality + Hitpoints_Max_Bonus) * (Hitpoints_Max_Percent_Bonus + Hitpoints_Max_Percent_Bonus_Item + 1), 1)", "Hitpoints_Max_Total", GameAttributeEncoding.Float16Or32, 31, 0f, 0f, 0);
        public static readonly GameAttributeF Hitpoints_Percent = new GameAttributeF(87, 0f, -1, 0, 0, "", "", "Hitpoints_Percent", GameAttributeEncoding.Float16, 31, 0f, 0f, 16);
        public static readonly GameAttributeF Hitpoints_Regen_Per_Second = new GameAttributeF(88, 0f, -1, 0, 0, "", "", "Hitpoints_Regen_Per_Second", GameAttributeEncoding.Float16Or32, 31, 0f, 0f, 0);
        public static readonly GameAttributeF Hitpoints_Max_Percent_Bonus = new GameAttributeF(89, 0f, -1, 0, 0, "", "", "Hitpoints_Max_Percent_Bonus", GameAttributeEncoding.Float16, 31, 0f, 0f, 16);
        public static readonly GameAttributeF Hitpoints_Max_Percent_Bonus_Item = new GameAttributeF(90, 0f, -1, 0, 0, "", "", "Hitpoints_Max_Percent_Bonus_Item", GameAttributeEncoding.Float16, 31, 0f, 0f, 16);
        public static readonly GameAttributeF Hitpoints_Healed_Target = new GameAttributeF(91, 0f, -1, 0, 0, "", "", "Hitpoints_Healed_Target", GameAttributeEncoding.Float16Or32, 1, 0f, 0f, 0);
        public static readonly GameAttributeI Resource_Type_Primary = new GameAttributeI(92, -1, -1, 3, 1, "", "", "Resource_Type_Primary", GameAttributeEncoding.IntMinMax, 1, -1, 7, 4);
        public static readonly GameAttributeI Resource_Type_Secondary = new GameAttributeI(93, -1, -1, 3, 1, "", "", "Resource_Type_Secondary", GameAttributeEncoding.IntMinMax, 1, -1, 7, 4);
        public static readonly GameAttributeF Resource_Cur = new GameAttributeF(94, 0f, 10, 0, 0, "", "Max(Resource_Cur.Agg, 0)", "Resource_Cur", GameAttributeEncoding.Int, 1, 0f, 0f, 32);
        public static readonly GameAttributeF Resource_Max = new GameAttributeF(95, 0f, 10, 0, 0, "", "", "Resource_Max", GameAttributeEncoding.Float16Or32, 1, 0f, 0f, 0);
        public static readonly GameAttributeF Resource_Max_Bonus = new GameAttributeF(96, 0f, 10, 0, 0, "", "", "Resource_Max_Bonus", GameAttributeEncoding.Float16Or32, 9, 0f, 0f, 0);
        public static readonly GameAttributeF Resource_Max_Total = new GameAttributeF(97, 0f, 10, 0, 0, "", "Max((Resource_Max + ((Level#NONE - 1) * Resource_Factor_Level) + Resource_Max_Bonus) * (Resource_Max_Percent_Bonus + 1), 0)", "Resource_Max_Total", GameAttributeEncoding.Int, 9, 0f, 0f, 32);
        public static readonly GameAttributeF Resource_Factor_Level = new GameAttributeF(98, 0f, 10, 0, 0, "", "", "Resource_Factor_Level", GameAttributeEncoding.Float16Or32, 1, 0f, 0f, 0);
        public static readonly GameAttributeF Resource_Granted = new GameAttributeF(99, 0f, 10, 0, 0, "", "", "Resource_Granted", GameAttributeEncoding.Float16Or32, 9, 0f, 0f, 0);
        public static readonly GameAttributeI Resource_Granted_Duration = new GameAttributeI(100, 0, 10, 0, 1, "", "", "Resource_Granted_Duration", GameAttributeEncoding.IntMinMax, 9, 0, 16777215, 24);
        public static readonly GameAttributeF Resource_Percent = new GameAttributeF(101, 0f, 10, 0, 0, "", "", "Resource_Percent", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeF Resource_Regen_Per_Second = new GameAttributeF(102, 0f, 10, 0, 0, "", "", "Resource_Regen_Per_Second", GameAttributeEncoding.Float16Or32, 9, 0f, 0f, 0);
        public static readonly GameAttributeF Resource_Regen_Bonus_Percent = new GameAttributeF(103, 0f, 10, 0, 0, "", "", "Resource_Regen_Bonus_Percent", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeF Resource_Regen_Total = new GameAttributeF(104, 0f, 10, 0, 0, "", "Resource_Regen_Per_Second * (1 + Resource_Regen_Bonus_Percent) + (Resource_Regen_Percent_Per_Second * Resource_Max_Total)", "Resource_Regen_Total", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeF Resource_Max_Percent_Bonus = new GameAttributeF(105, 0f, 10, 0, 0, "", "Resource_Percent", "Resource_Max_Percent_Bonus", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeF Resource_Capacity_Used = new GameAttributeF(106, 0f, 10, 0, 0, "", "", "Resource_Capacity_Used", GameAttributeEncoding.Float16Or32, 9, 0f, 0f, 0);
        public static readonly GameAttributeF Resource_Effective_Max = new GameAttributeF(107, 0f, 10, 0, 0, "", "Resource_Max_Total - Resource_Capacity_Used", "Resource_Effective_Max", GameAttributeEncoding.Float16Or32, 9, 0f, 0f, 0);
        public static readonly GameAttributeF Resource_Regen_Percent_Per_Second = new GameAttributeF(108, 0f, 10, 0, 0, "", "", "Resource_Regen_Percent_Per_Second", GameAttributeEncoding.Float16Or32, 9, 0f, 0f, 0);
        public static readonly GameAttributeF Resource_Degeneration_Stop_Point = new GameAttributeF(109, 0f, 10, 0, 0, "", "", "Resource_Degeneration_Stop_Point", GameAttributeEncoding.Float16Or32, 9, 0f, 0f, 0);
        public static readonly GameAttributeF Movement_Scalar = new GameAttributeF(110, 0f, -1, 0, 0, "", "", "Movement_Scalar", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeF Walking_Rate = new GameAttributeF(111, 0f, -1, 0, 0, "", "", "Walking_Rate", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeF Running_Rate = new GameAttributeF(112, 0f, -1, 0, 0, "", "", "Running_Rate", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeF Sprinting_Rate = new GameAttributeF(113, 0f, -1, 0, 0, "", "", "Sprinting_Rate", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeF Strafing_Rate = new GameAttributeF(114, 0f, -1, 0, 0, "", "", "Strafing_Rate", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeF Walking_Rate_Total = new GameAttributeF(115, 0f, -1, 0, 0, "", "Walking_Rate * Movement_Scalar_Total", "Walking_Rate_Total", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeF Running_Rate_Total = new GameAttributeF(116, 0f, -1, 0, 0, "", "Running_Rate * Movement_Scalar_Total", "Running_Rate_Total", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeF Sprinting_Rate_Total = new GameAttributeF(117, 0f, -1, 0, 0, "", "Sprinting_Rate * Movement_Scalar_Total", "Sprinting_Rate_Total", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeF Strafing_Rate_Total = new GameAttributeF(118, 0f, -1, 0, 0, "", "Strafing_Rate * Movement_Scalar_Total", "Strafing_Rate_Total", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeF Movement_Bonus_Total = new GameAttributeF(119, 0f, -1, -1, 0, "0", "Movement_Bonus_Run_Speed", "Movement_Bonus_Total", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeF Movement_Scalar_Subtotal = new GameAttributeF(120, 0f, -1, -1, 0, "0", "Max(0.1, Movement_Scalar) * (1 + Movement_Bonus_Total) * (1 - Movement_Scalar_Reduction_Percent * (1 - Min(1, Movement_Scalar_Reduction_Resistance)))", "Movement_Scalar_Subtotal", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeF Movement_Scalar_Capped_Total = new GameAttributeF(121, 0f, -1, -1, 0, "0", "Min(1.25, Movement_Scalar_Subtotal)", "Movement_Scalar_Capped_Total", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeF Movement_Scalar_Uncapped_Bonus = new GameAttributeF(122, 0f, -1, 0, 0, "", "", "Movement_Scalar_Uncapped_Bonus", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeF Movement_Scalar_Total = new GameAttributeF(123, 0f, -1, -1, 0, "0", "Movement_Scalar_Capped_Total + Movement_Scalar_Uncapped_Bonus", "Movement_Scalar_Total", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeF Movement_Bonus_Run_Speed = new GameAttributeF(124, 0f, -1, 1, 0, "", "", "Movement_Bonus_Run_Speed", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeF Casting_Speed = new GameAttributeF(125, 0f, -1, 0, 0, "", "", "Casting_Speed", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeF Casting_Speed_Bonus = new GameAttributeF(126, 0f, -1, 0, 0, "", "", "Casting_Speed_Bonus", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeF Casting_Speed_Total = new GameAttributeF(127, 0f, -1, 0, 0, "", "(Casting_Speed + Casting_Speed_Bonus) * Max(0.1, 1 + Casting_Speed_Percent)", "Casting_Speed_Total", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeB Always_Hits = new GameAttributeB(128, 0, -1, 1, 1, "", "", "Always_Hits", GameAttributeEncoding.IntMinMax, 3, 0, 1, 1);
        public static readonly GameAttributeF Hit_Chance = new GameAttributeF(129, 0f, -1, 0, 0, "", "", "Hit_Chance", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeF Attacks_Per_Second_Item = new GameAttributeF(130, 0f, -1, 0, 0, "", "0", "Attacks_Per_Second_Item", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeF Attacks_Per_Second_Item_Percent = new GameAttributeF(131, 0f, -1, 0, 0, "", "0", "Attacks_Per_Second_Item_Percent", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeF Attacks_Per_Second_Item_Subtotal = new GameAttributeF(132, 0f, -1, 0, 0, "Attacks_Per_Second_Item * (1 + Attacks_Per_Second_Item_Percent)", "0", "Attacks_Per_Second_Item_Subtotal", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeF Attacks_Per_Second_Item_Bonus = new GameAttributeF(133, 0f, -1, 0, 0, "", "", "Attacks_Per_Second_Item_Bonus", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeF Attacks_Per_Second_Item_Total = new GameAttributeF(134, 0f, -1, 0, 0, "(Attacks_Per_Second_Item_Subtotal + Attacks_Per_Second_Item_Bonus)", "", "Attacks_Per_Second_Item_Total", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeF Attacks_Per_Second = new GameAttributeF(135, 0f, -1, 0, 0, "0", "", "Attacks_Per_Second", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeF Attacks_Per_Second_Bonus = new GameAttributeF(136, 0f, -1, 0, 0, "0", "", "Attacks_Per_Second_Bonus", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeF Attacks_Per_Second_Total = new GameAttributeF(137, 0f, -1, 0, 0, "0", "(((Attacks_Per_Second_Item_CurrentHand > 0.0) ? Attacks_Per_Second_Item_CurrentHand : Attacks_Per_Second) + Attacks_Per_Second_Bonus + Attacks_Per_Second_Item_Bonus) * Max(0.1, (1 + Attacks_Per_Second_Percent))", "Attacks_Per_Second_Total", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeF Attacks_Per_Second_Percent = new GameAttributeF(138, 0f, -1, 0, 0, "", "", "Attacks_Per_Second_Percent", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeF AI_Cooldown_Reduction_Percent = new GameAttributeF(139, 0f, -1, 0, 0, "0", "", "AI_Cooldown_Reduction_Percent", GameAttributeEncoding.Float16, 0, 0f, 0f, 16);
        public static readonly GameAttributeF Power_Cooldown_Reduction_Percent = new GameAttributeF(140, 0f, -1, 0, 0, "0", "", "Power_Cooldown_Reduction_Percent", GameAttributeEncoding.Float16, 0, 0f, 0f, 16);
        public static readonly GameAttributeF Damage_Delta = new GameAttributeF(141, 0f, 0, 0, 0, "", "", "Damage_Delta", GameAttributeEncoding.Float16Or32, 9, 0f, 0f, 0);
        public static readonly GameAttributeF Damage_Delta_Total = new GameAttributeF(142, 0f, 0, 0, 0, "", "Max(Damage_Delta - Damage_Bonus_Min + Damage_Weapon_Delta_Total_CurrentHand, 0)", "Damage_Delta_Total", GameAttributeEncoding.Float16Or32, 9, 0f, 0f, 0);
        public static readonly GameAttributeF Damage_Min = new GameAttributeF(143, 0f, 0, 0, 0, "", "", "Damage_Min", GameAttributeEncoding.Float16Or32, 9, 0f, 0f, 0);
        public static readonly GameAttributeF Damage_Bonus_Min = new GameAttributeF(144, 0f, 0, 0, 0, "", "", "Damage_Bonus_Min", GameAttributeEncoding.Float16Or32, 9, 0f, 0f, 0);
        public static readonly GameAttributeF Damage_Min_Total = new GameAttributeF(145, 0f, 0, 0, 0, "", "Damage_Min_Subtotal + Damage_Type_Percent_Bonus * Damage_Min_Subtotal#Physical", "Damage_Min_Total", GameAttributeEncoding.Float16Or32, 9, 0f, 0f, 0);
        public static readonly GameAttributeF Damage_Min_Subtotal = new GameAttributeF(146, 0f, 0, 0, 0, "", "Damage_Min + Damage_Bonus_Min + Damage_Weapon_Min_Total_CurrentHand", "Damage_Min_Subtotal", GameAttributeEncoding.Float16Or32, 9, 0f, 0f, 0);
        public static readonly GameAttributeF Damage_Percent_All_From_Skills = new GameAttributeF(147, 0f, -1, 0, 0, "", "", "Damage_Percent_All_From_Skills", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeF Damage_Weapon_Delta = new GameAttributeF(148, 0f, 0, 0, 0, "", "", "Damage_Weapon_Delta", GameAttributeEncoding.Float16Or32, 9, 0f, 0f, 0);
        public static readonly GameAttributeF Damage_Weapon_Delta_SubTotal = new GameAttributeF(149, 0f, 0, 0, 0, "(Damage_Weapon_Delta > 0.0) ? (Max(1, Damage_Weapon_Delta - Damage_Weapon_Bonus_Min)) : Damage_Weapon_Delta", "", "Damage_Weapon_Delta_SubTotal", GameAttributeEncoding.Float16Or32, 9, 0f, 0f, 0);
        public static readonly GameAttributeF Damage_Weapon_Max = new GameAttributeF(150, 0f, 0, 0, 0, "(Damage_Weapon_Min + Damage_Weapon_Delta)", "", "Damage_Weapon_Max", GameAttributeEncoding.Float16Or32, 9, 0f, 0f, 0);
        public static readonly GameAttributeF Damage_Weapon_Max_Total = new GameAttributeF(151, 0f, 0, 0, 0, "(Damage_Weapon_Min_Total + Damage_Weapon_Delta_Total)", "", "Damage_Weapon_Max_Total", GameAttributeEncoding.Float16Or32, 9, 0f, 0f, 0);
        public static readonly GameAttributeF Damage_Weapon_Delta_Total = new GameAttributeF(152, 0f, 0, 0, 0, "Max((Damage_Weapon_Delta_SubTotal + Damage_Weapon_Bonus_Delta) * (1 + Damage_Weapon_Percent_Total), 0)", "", "Damage_Weapon_Delta_Total", GameAttributeEncoding.Float16Or32, 9, 0f, 0f, 0);
        public static readonly GameAttributeF Damage_Weapon_Delta_Total_All = new GameAttributeF(153, 0f, -1, 0, 0, "(Damage_Weapon_Delta_Total#Physical + Damage_Weapon_Delta_Total#Fire + Damage_Weapon_Delta_Total#Cold + Damage_Weapon_Delta_Total#Lightning + Damage_Weapon_Delta_Total#Poison + Damage_Weapon_Delta_Total#Arcane + Damage_Weapon_Delta_Total#Holy)", "", "Damage_Weapon_Delta_Total_All", GameAttributeEncoding.Float16Or32, 9, 0f, 0f, 0);
        public static readonly GameAttributeF Damage_Weapon_Bonus_Delta = new GameAttributeF(154, 0f, 0, 0, 0, "", "", "Damage_Weapon_Bonus_Delta", GameAttributeEncoding.Float16Or32, 9, 0f, 0f, 0);
        public static readonly GameAttributeF Damage_Weapon_Min = new GameAttributeF(155, 0f, 0, 0, 0, "", "", "Damage_Weapon_Min", GameAttributeEncoding.Float16Or32, 9, 0f, 0f, 0);
        public static readonly GameAttributeF Damage_Weapon_Min_Total = new GameAttributeF(156, 0f, 0, 0, 0, "(Damage_Weapon_Min + Damage_Weapon_Bonus_Min) * (1 + Damage_Weapon_Percent_Total)", "", "Damage_Weapon_Min_Total", GameAttributeEncoding.Float16Or32, 9, 0f, 0f, 0);
        public static readonly GameAttributeF Damage_Weapon_Min_Total_All = new GameAttributeF(157, 0f, -1, 0, 0, "(Damage_Weapon_Min_Total#Physical + Damage_Weapon_Min_Total#Fire + Damage_Weapon_Min_Total#Cold + Damage_Weapon_Min_Total#Lightning + Damage_Weapon_Min_Total#Poison + Damage_Weapon_Min_Total#Arcane + Damage_Weapon_Min_Total#Holy)", "", "Damage_Weapon_Min_Total_All", GameAttributeEncoding.Float16Or32, 9, 0f, 0f, 0);
        public static readonly GameAttributeF Damage_Weapon_Bonus_Min = new GameAttributeF(158, 0f, 0, 0, 0, "", "", "Damage_Weapon_Bonus_Min", GameAttributeEncoding.Float16Or32, 9, 0f, 0f, 0);
        public static readonly GameAttributeF Damage_Weapon_Percent_Bonus = new GameAttributeF(159, 0f, 0, 0, 0, "", "", "Damage_Weapon_Percent_Bonus", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeF Damage_Weapon_Percent_All = new GameAttributeF(160, 0f, -1, 0, 0, "", "", "Damage_Weapon_Percent_All", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeF Damage_Weapon_Percent_Total = new GameAttributeF(161, 0f, 0, 0, 0, "Damage_Weapon_Percent_Bonus + Damage_Weapon_Percent_All#NONE", "", "Damage_Weapon_Percent_Total", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeF Damage_Type_Percent_Bonus = new GameAttributeF(162, 0f, 0, 0, 0, "", "", "Damage_Type_Percent_Bonus", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeF Damage_Percent_Bonus_Witchdoctor = new GameAttributeF(163, 0f, -1, 0, 0, "", "", "Damage_Percent_Bonus_Witchdoctor", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeF Damage_Percent_Bonus_Wizard = new GameAttributeF(164, 0f, -1, 0, 0, "", "", "Damage_Percent_Bonus_Wizard", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeI Crit_Percent_Base = new GameAttributeI(165, 0, -1, 0, 0, "", "", "Crit_Percent_Base", GameAttributeEncoding.Int, 9, 0, 0, 32);
        public static readonly GameAttributeI Crit_Percent_Bonus_Capped = new GameAttributeI(166, 0, -1, 0, 0, "", "", "Crit_Percent_Bonus_Capped", GameAttributeEncoding.Int, 9, 0, 0, 32);
        public static readonly GameAttributeI Crit_Percent_Bonus_Uncapped = new GameAttributeI(167, 0, -1, 0, 0, "", "", "Crit_Percent_Bonus_Uncapped", GameAttributeEncoding.Int, 9, 0, 0, 32);
        public static readonly GameAttributeI Crit_Percent_Cap = new GameAttributeI(168, 0, -1, 0, 0, "", "", "Crit_Percent_Cap", GameAttributeEncoding.Int, 9, 0, 0, 32);
        public static readonly GameAttributeF Crit_Damage_Percent = new GameAttributeF(169, 0f, 0, 0, 0, "", "", "Crit_Damage_Percent", GameAttributeEncoding.Float32, 9, 0f, 0f, 32);
        public static readonly GameAttributeI Crit_Effect_Time = new GameAttributeI(170, 0, -1, 3, 1, "", "", "Crit_Effect_Time", GameAttributeEncoding.Int, 9, 0, 0, 32);
        public static readonly GameAttributeF Pierce_Chance = new GameAttributeF(171, 0f, -1, 0, 0, "", "", "Pierce_Chance", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeF Damage_Absorb_Percent = new GameAttributeF(172, 0f, -1, 0, 0, "", "", "Damage_Absorb_Percent", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeF Damage_Reduction_Total = new GameAttributeF(173, 0f, 0, 0, 0, "", "", "Damage_Reduction_Total", GameAttributeEncoding.Float16Or32, 9, 0f, 0f, 0);
        public static readonly GameAttributeF Damage_Reduction_Current = new GameAttributeF(174, 0f, 0, 0, 0, "", "", "Damage_Reduction_Current", GameAttributeEncoding.Float16Or32, 9, 0f, 0f, 0);
        public static readonly GameAttributeI Damage_Reduction_Last_Tick = new GameAttributeI(175, 0, 0, 3, 1, "", "", "Damage_Reduction_Last_Tick", GameAttributeEncoding.Int, 9, 0, 0, 32);
        public static readonly GameAttributeF Block_Chance = new GameAttributeF(176, 0f, -1, 0, 0, "", "", "Block_Chance", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeF Block_Chance_Total = new GameAttributeF(177, 0f, -1, 0, 0, "", "Block_Chance + Block_Chance_Item_Total", "Block_Chance_Total", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeF Block_Chance_Bonus_Item = new GameAttributeF(178, 0f, -1, 0, 0, "", "", "Block_Chance_Bonus_Item", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeF Block_Chance_Item = new GameAttributeF(179, 0f, -1, 0, 0, "", "0", "Block_Chance_Item", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeF Block_Chance_Item_Total = new GameAttributeF(180, 0f, -1, 0, 0, "Block_Chance_Item + Block_Chance_Bonus_Item", "", "Block_Chance_Item_Total", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeF Block_Amount = new GameAttributeF(181, 0f, -1, 0, 0, "", "", "Block_Amount", GameAttributeEncoding.Float16Or32, 9, 0f, 0f, 0);
        public static readonly GameAttributeF Block_Amount_Bonus_Percent = new GameAttributeF(182, 0f, -1, 0, 0, "", "", "Block_Amount_Bonus_Percent", GameAttributeEncoding.Float16Or32, 9, 0f, 0f, 0);
        public static readonly GameAttributeF Block_Amount_Total_Min = new GameAttributeF(183, 0f, -1, 0, 0, "", "(Block_Amount + Block_Amount_Item_Min + Block_Amount_Item_Bonus) * (1 + Block_Amount_Bonus_Percent)", "Block_Amount_Total_Min", GameAttributeEncoding.Float16Or32, 9, 0f, 0f, 0);
        public static readonly GameAttributeF Block_Amount_Total_Max = new GameAttributeF(184, 0f, -1, 0, 0, "", "(Block_Amount + Block_Amount_Item_Min + Block_Amount_Item_Delta + Block_Amount_Item_Bonus) * (1 + Block_Amount_Bonus_Percent)", "Block_Amount_Total_Max", GameAttributeEncoding.Float16Or32, 9, 0f, 0f, 0);
        public static readonly GameAttributeF Block_Amount_Item_Min = new GameAttributeF(185, 0f, -1, 0, 0, "", "", "Block_Amount_Item_Min", GameAttributeEncoding.Float16Or32, 9, 0f, 0f, 0);
        public static readonly GameAttributeF Block_Amount_Item_Delta = new GameAttributeF(186, 0f, -1, 0, 0, "", "", "Block_Amount_Item_Delta", GameAttributeEncoding.Float16Or32, 9, 0f, 0f, 0);
        public static readonly GameAttributeF Block_Amount_Item_Bonus = new GameAttributeF(187, 0f, -1, 0, 0, "", "", "Block_Amount_Item_Bonus", GameAttributeEncoding.Float16Or32, 9, 0f, 0f, 0);
        public static readonly GameAttributeF Dodge_Rating_Base = new GameAttributeF(188, 0f, -1, 0, 0, "", "", "Dodge_Rating_Base", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeF Dodge_Rating = new GameAttributeF(189, 0f, -1, 0, 0, "", "", "Dodge_Rating", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeF Dodge_Rating_Total = new GameAttributeF(190, 0f, -1, 0, 0, "", "Dodge_Rating_Base + Dodge_Rating", "Dodge_Rating_Total", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeF Dodge_Chance_Bonus = new GameAttributeF(191, 0f, -1, 7, 0, "", "", "Dodge_Chance_Bonus", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeF Dodge_Chance_Bonus_Melee = new GameAttributeF(192, 0f, -1, 7, 0, "", "", "Dodge_Chance_Bonus_Melee", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeF Dodge_Chance_Bonus_Ranged = new GameAttributeF(193, 0f, -1, 7, 0, "", "", "Dodge_Chance_Bonus_Ranged", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeF Get_Hit_Current = new GameAttributeF(194, 0f, -1, 0, 0, "", "", "Get_Hit_Current", GameAttributeEncoding.Float16Or32, 1, 0f, 0f, 0);
        public static readonly GameAttributeF Get_Hit_Max_Base = new GameAttributeF(195, 0f, -1, 0, 0, "", "", "Get_Hit_Max_Base", GameAttributeEncoding.Float16Or32, 1, 0f, 0f, 0);
        public static readonly GameAttributeF Get_Hit_Max_Per_Level = new GameAttributeF(196, 0f, -1, 0, 0, "", "", "Get_Hit_Max_Per_Level", GameAttributeEncoding.Float16Or32, 1, 0f, 0f, 0);
        public static readonly GameAttributeF Get_Hit_Max = new GameAttributeF(197, 0f, -1, 0, 0, "", "Get_Hit_Max_Base + (Get_Hit_Max_Per_Level * Level)", "Get_Hit_Max", GameAttributeEncoding.Float16Or32, 1, 0f, 0f, 0);
        public static readonly GameAttributeF Get_Hit_Recovery_Base = new GameAttributeF(198, 0f, -1, 0, 0, "", "", "Get_Hit_Recovery_Base", GameAttributeEncoding.Float16Or32, 1, 0f, 0f, 0);
        public static readonly GameAttributeF Get_Hit_Recovery_Per_Level = new GameAttributeF(199, 0f, -1, 0, 0, "", "", "Get_Hit_Recovery_Per_Level", GameAttributeEncoding.Float16Or32, 1, 0f, 0f, 0);
        public static readonly GameAttributeF Get_Hit_Recovery = new GameAttributeF(200, 0f, -1, 0, 0, "", "Get_Hit_Recovery_Base + (Get_Hit_Recovery_Per_Level * Level)", "Get_Hit_Recovery", GameAttributeEncoding.Float16Or32, 9, 0f, 0f, 0);
        public static readonly GameAttributeF Get_Hit_Damage = new GameAttributeF(201, 0f, -1, 0, 0, "", "", "Get_Hit_Damage", GameAttributeEncoding.Float16Or32, 9, 0f, 0f, 0);
        public static readonly GameAttributeF Get_Hit_Damage_Scalar = new GameAttributeF(202, 0f, -1, 0, 0, "", "", "Get_Hit_Damage_Scalar", GameAttributeEncoding.Float16Or32, 1, 0f, 0f, 0);
        public static readonly GameAttributeF Proc_On_Death = new GameAttributeF(203, 0f, 8, 0, 0, "", "", "Proc_On_Death", GameAttributeEncoding.Float16, 0, 0f, 0f, 16);
        public static readonly GameAttributeF Proc_On_Attack = new GameAttributeF(204, 0f, 8, 0, 0, "", "", "Proc_On_Attack", GameAttributeEncoding.Float16, 0, 0f, 0f, 16);
        public static readonly GameAttributeF Proc_On_Hit = new GameAttributeF(205, 0f, 8, 0, 0, "", "", "Proc_On_Hit", GameAttributeEncoding.Float16, 0, 0f, 0f, 16);
        public static readonly GameAttributeF Proc_On_Critical = new GameAttributeF(206, 0f, 8, 0, 0, "", "", "Proc_On_Critical", GameAttributeEncoding.Float16, 0, 0f, 0f, 16);
        public static readonly GameAttributeF Proc_On_Block = new GameAttributeF(207, 0f, 8, 0, 0, "", "", "Proc_On_Block", GameAttributeEncoding.Float16, 0, 0f, 0f, 16);
        public static readonly GameAttributeF Proc_On_Slay = new GameAttributeF(208, 0f, 8, 0, 0, "", "", "Proc_On_Slay", GameAttributeEncoding.Float16, 0, 0f, 0f, 16);
        public static readonly GameAttributeF Proc_On_Spawn = new GameAttributeF(209, 0f, 8, 0, 0, "", "", "Proc_On_Spawn", GameAttributeEncoding.Float16, 0, 0f, 0f, 16);
        public static readonly GameAttributeI Last_Damage_MainActor = new GameAttributeI(210, -1, -1, 3, 1, "", "", "Last_Damage_MainActor", GameAttributeEncoding.Int, 1, 0, 0, 32);
        public static readonly GameAttributeI Last_ACD_Attacked = new GameAttributeI(211, -1, -1, 3, 1, "", "", "Last_ACD_Attacked", GameAttributeEncoding.Int, 1, 0, 0, 32);
        public static readonly GameAttributeB Ignores_Critical_Hits = new GameAttributeB(212, 0, -1, 1, 1, "", "", "Ignores_Critical_Hits", GameAttributeEncoding.IntMinMax, 0, 0, 1, 1);
        public static readonly GameAttributeB Immunity = new GameAttributeB(213, 0, -1, 1, 1, "", "", "Immunity", GameAttributeEncoding.IntMinMax, 0, 0, 1, 1);
        public static readonly GameAttributeB Untargetable = new GameAttributeB(214, 0, -1, 1, 1, "", "", "Untargetable", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeB Immobolize = new GameAttributeB(215, 0, -1, 1, 1, "", "", "Immobolize", GameAttributeEncoding.IntMinMax, 1, 0, 1, 1);
        public static readonly GameAttributeB Immune_To_Knockback = new GameAttributeB(216, 0, -1, 1, 1, "", "", "Immune_To_Knockback", GameAttributeEncoding.IntMinMax, 0, 0, 1, 1);
        public static readonly GameAttributeB Power_Immobilize = new GameAttributeB(217, 0, -1, 1, 1, "", "", "Power_Immobilize", GameAttributeEncoding.IntMinMax, 0, 0, 1, 1);
        public static readonly GameAttributeF Stun_Chance = new GameAttributeF(218, 0f, -1, 0, 0, "", "", "Stun_Chance", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeF Stun_Length = new GameAttributeF(219, 0f, -1, 0, 0, "", "", "Stun_Length", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeF Stun_Recovery = new GameAttributeF(220, 0f, -1, 0, 0, "", "", "Stun_Recovery", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeF Stun_Recovery_Speed = new GameAttributeF(221, 0f, -1, 0, 0, "", "", "Stun_Recovery_Speed", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeB Stunned = new GameAttributeB(222, 0, -1, 1, 1, "", "", "Stunned", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeB Stun_Immune = new GameAttributeB(223, 0, -1, 1, 1, "", "", "Stun_Immune", GameAttributeEncoding.IntMinMax, 0, 0, 1, 1);
        public static readonly GameAttributeF Poison_Length_Reduction = new GameAttributeF(224, 0f, -1, 0, 0, "", "", "Poison_Length_Reduction", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeB Poisoned = new GameAttributeB(225, 0, -1, 1, 1, "", "", "Poisoned", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeB Bleeding = new GameAttributeB(226, 0, -1, 1, 1, "", "", "Bleeding", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeF Bleed_Duration = new GameAttributeF(227, 0f, -1, 0, 0, "", "", "Bleed_Duration", GameAttributeEncoding.Float16, 1, 0f, 0f, 16);
        public static readonly GameAttributeB Chilled = new GameAttributeB(228, 0, -1, 1, 1, "", "", "Chilled", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeF Freeze_Length_Reduction = new GameAttributeF(229, 0f, -1, 0, 0, "", "", "Freeze_Length_Reduction", GameAttributeEncoding.Float16, 1, 0f, 0f, 16);
        public static readonly GameAttributeB Freeze_Immune = new GameAttributeB(230, 0, -1, 1, 1, "", "", "Freeze_Immune", GameAttributeEncoding.IntMinMax, 0, 0, 1, 1);
        public static readonly GameAttributeB Webbed = new GameAttributeB(231, 0, -1, 1, 1, "", "", "Webbed", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeB Slow = new GameAttributeB(232, 0, -1, 1, 1, "", "", "Slow", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeB FireAura = new GameAttributeB(233, 0, -1, 1, 1, "", "", "FireAura", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeB LightningAura = new GameAttributeB(234, 0, -1, 1, 1, "", "", "LightningAura", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeB ColdAura = new GameAttributeB(235, 0, -1, 1, 1, "", "", "ColdAura", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeB PoisonAura = new GameAttributeB(236, 0, -1, 1, 1, "", "", "PoisonAura", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeB Blind = new GameAttributeB(237, 0, -1, 1, 1, "", "", "Blind", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeB Enraged = new GameAttributeB(238, 0, -1, 1, 1, "", "", "Enraged", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeB Slowdown_Immune = new GameAttributeB(239, 0, -1, 1, 1, "", "", "Slowdown_Immune", GameAttributeEncoding.IntMinMax, 0, 0, 1, 1);
        public static readonly GameAttributeB Gethit_Immune = new GameAttributeB(240, 0, -1, 1, 1, "", "", "Gethit_Immune", GameAttributeEncoding.IntMinMax, 0, 0, 1, 1);
        public static readonly GameAttributeF Suffocation_Per_Second = new GameAttributeF(241, 0f, -1, 0, 0, "", "", "Suffocation_Per_Second", GameAttributeEncoding.Float16Or32, 1, 0f, 0f, 0);
        public static readonly GameAttributeF Suffocation_Unit_Value = new GameAttributeF(242, 0f, -1, 1, 0, "", "", "Suffocation_Unit_Value", GameAttributeEncoding.Float16Or32, 1, 0f, 0f, 0);
        public static readonly GameAttributeF Thorns_Percent = new GameAttributeF(243, 0f, 0, 0, 0, "", "", "Thorns_Percent", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeF Thorns_Percent_All = new GameAttributeF(244, 0f, -1, 0, 0, "", "", "Thorns_Percent_All", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeF Thorns_Percent_Total = new GameAttributeF(245, 0f, 0, 0, 0, "", "Thorns_Percent + Thorns_Percent_All#NONE", "Thorns_Percent_Total", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeF Thorns_Fixed = new GameAttributeF(246, 0f, 0, 0, 0, "", "", "Thorns_Fixed", GameAttributeEncoding.Float16Or32, 9, 0f, 0f, 0);
        public static readonly GameAttributeF Steal_Health_Percent = new GameAttributeF(247, 0f, -1, 0, 0, "", "", "Steal_Health_Percent", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeF Steal_Mana_Percent = new GameAttributeF(248, 0f, -1, 0, 0, "", "", "Steal_Mana_Percent", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeF Resource_On_Hit = new GameAttributeF(249, 0f, 10, 0, 0, "", "", "Resource_On_Hit", GameAttributeEncoding.Float16Or32, 9, 0f, 0f, 0);
        public static readonly GameAttributeF Resource_On_Kill = new GameAttributeF(250, 0f, 10, 0, 0, "", "", "Resource_On_Kill", GameAttributeEncoding.Float16Or32, 9, 0f, 0f, 0);
        public static readonly GameAttributeF Resource_On_Crit = new GameAttributeF(251, 0f, 10, 0, 0, "", "", "Resource_On_Crit", GameAttributeEncoding.Float16Or32, 9, 0f, 0f, 0);
        public static readonly GameAttributeF Hitpoints_On_Hit = new GameAttributeF(252, 0f, -1, 0, 0, "", "", "Hitpoints_On_Hit", GameAttributeEncoding.Float16Or32, 9, 0f, 0f, 0);
        public static readonly GameAttributeF Hitpoints_On_Kill = new GameAttributeF(253, 0f, -1, 0, 0, "", "", "Hitpoints_On_Kill", GameAttributeEncoding.Float16Or32, 9, 0f, 0f, 0);
        public static readonly GameAttributeF Damage_To_Mana = new GameAttributeF(254, 0f, -1, 0, 0, "", "", "Damage_To_Mana", GameAttributeEncoding.Float16Or32, 9, 0f, 0f, 0);
        public static readonly GameAttributeI Last_Proc_Time = new GameAttributeI(255, 0, -1, 3, 1, "", "", "Last_Proc_Time", GameAttributeEncoding.Int, 9, 0, 0, 32);
        public static readonly GameAttributeF Damage_Power_Delta = new GameAttributeF(256, 0f, 0, 0, 0, "", "", "Damage_Power_Delta", GameAttributeEncoding.Float16Or32, 9, 0f, 0f, 0);
        public static readonly GameAttributeF Damage_Power_Min = new GameAttributeF(257, 0f, 0, 0, 0, "", "", "Damage_Power_Min", GameAttributeEncoding.Float16Or32, 9, 0f, 0f, 0);
        public static readonly GameAttributeI Rope_Overlay = new GameAttributeI(258, -1, -1, 3, 1, "", "", "Rope_Overlay", GameAttributeEncoding.Int, 31, 0, 0, 32);
        public static readonly GameAttributeI General_Cooldown = new GameAttributeI(259, -1, -1, 3, 1, "", "", "General_Cooldown", GameAttributeEncoding.Int, 1, 0, 0, 32);
        public static readonly GameAttributeI Power_Cooldown = new GameAttributeI(260, -1, 4, 1, 1, "", "", "Power_Cooldown", GameAttributeEncoding.Int, 1, 0, 0, 32);
        public static readonly GameAttributeI Power_Cooldown_Start = new GameAttributeI(261, -1, 4, 1, 1, "", "", "Power_Cooldown_Start", GameAttributeEncoding.Int, 1, 0, 0, 32);
        public static readonly GameAttributeI Proc_Cooldown = new GameAttributeI(262, 0, 8, 1, 1, "", "", "Proc_Cooldown", GameAttributeEncoding.Int, 1, 0, 0, 32);
        public static readonly GameAttributeI Emote_Cooldown = new GameAttributeI(263, 0, -1, 1, 1, "", "", "Emote_Cooldown", GameAttributeEncoding.Int, 1, 0, 0, 32);
        public static readonly GameAttributeF Projectile_Speed = new GameAttributeF(264, 0f, -1, 0, 0, "", "", "Projectile_Speed", GameAttributeEncoding.Float16, 31, 0f, 0f, 16);
        public static readonly GameAttributeF Projectile_Speed_Increase_Percent = new GameAttributeF(265, 0f, -1, 0, 0, "", "", "Projectile_Speed_Increase_Percent", GameAttributeEncoding.Float16, 31, 0f, 0f, 16);
        public static readonly GameAttributeB Destroy_When_Path_Blocked = new GameAttributeB(266, 0, -1, 1, 1, "", "", "Destroy When Path Blocked", GameAttributeEncoding.IntMinMax, 0, 0, 1, 1);
        public static readonly GameAttributeB Skill_Toggled_State = new GameAttributeB(267, 0, 4, 1, 1, "", "", "Skill_Toggled_State", GameAttributeEncoding.IntMinMax, 1, 0, 1, 1);
        public static readonly GameAttributeI Act = new GameAttributeI(268, -1, -1, 3, 1, "", "", "Act", GameAttributeEncoding.Int, 9, 0, 0, 32);
        public static readonly GameAttributeI Difficulty = new GameAttributeI(269, -1, -1, 3, 1, "", "", "Difficulty", GameAttributeEncoding.IntMinMax, 9, -1, 4, 3);
        public static readonly GameAttributeF Last_Damage_Amount = new GameAttributeF(270, 0f /* NAN? -1 set instead of -1f i think */, -1, 3, 0, "", "", "Last_Damage_Amount", GameAttributeEncoding.Float16Or32, 0, 0f, 0f, 0);
        public static readonly GameAttributeB In_Knockback = new GameAttributeB(271, 0, -1, 1, 1, "", "", "In_Knockback", GameAttributeEncoding.IntMinMax, 0, 0, 1, 1);
        public static readonly GameAttributeF Amplify_Damage_Type_Percent = new GameAttributeF(272, 0f, 0, 0, 0, "", "", "Amplify_Damage_Type_Percent", GameAttributeEncoding.Float16, 0, 0f, 0f, 16);
        public static readonly GameAttributeF Amplify_Damage_Percent = new GameAttributeF(273, 0f, -1, 0, 0, "", "", "Amplify_Damage_Percent", GameAttributeEncoding.Float16, 0, 0f, 0f, 16);
        public static readonly GameAttributeI Durability_Cur = new GameAttributeI(274, 0, -1, 0, 1, "", "", "Durability_Cur", GameAttributeEncoding.Int, 8, 0, 0, 32);
        public static readonly GameAttributeI Durability_Max = new GameAttributeI(275, 0, -1, 0, 1, "", "", "Durability_Max", GameAttributeEncoding.Int, 8, 0, 0, 32);
        public static readonly GameAttributeI Durability_Last_Damage = new GameAttributeI(276, 0, -1, 0, 1, "", "", "Durability_Last_Damage", GameAttributeEncoding.Int, 0, 0, 0, 32);
        public static readonly GameAttributeI Item_Quality_Level = new GameAttributeI(277, -1, -1, 1, 1, "", "", "Item_Quality_Level", GameAttributeEncoding.IntMinMax, 8, -1, 11, 4);
        public static readonly GameAttributeF Item_Cost_Percent_Bonus = new GameAttributeF(278, 0f, -1, 0, 0, "", "", "Item_Cost_Percent_Bonus", GameAttributeEncoding.Float16, 8, 0f, 0f, 16);
        public static readonly GameAttributeB Item_Equipped = new GameAttributeB(279, 0, -1, 1, 1, "", "", "Item_Equipped", GameAttributeEncoding.IntMinMax, 8, 0, 1, 1);
        public static readonly GameAttributeF Requirement = new GameAttributeF(280, 0f, 1, 1, 0, "FLOOR(Requirement.Agg * (1 + Requirements_Ease_Percent#NONE))", "", "Requirement", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeF Requirements_Ease_Percent = new GameAttributeF(281, 0f, -1, 0, 0, "", "0", "Requirements_Ease_Percent", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeF Requirement_When_Equipped = new GameAttributeF(282, 0f, 1, 1, 0, "", "", "Requirement_When_Equipped", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeI Sockets = new GameAttributeI(283, 0, -1, 0, 1, "", "0", "Sockets", GameAttributeEncoding.IntMinMax, 8, 0, 3, 2);
        public static readonly GameAttributeI Sockets_Filled = new GameAttributeI(284, 0, -1, 0, 1, "", "0", "Sockets_Filled", GameAttributeEncoding.IntMinMax, 8, 0, 3, 2);
        public static readonly GameAttributeF Stats_All_Bonus = new GameAttributeF(285, 0f, -1, 0, 0, "", "", "Stats_All_Bonus", GameAttributeEncoding.Float16, 1, 0f, 0f, 16);
        public static readonly GameAttributeI Item_Bound_To_ACD = new GameAttributeI(286, -1, -1, 3, 1, "", "0", "Item_Bound_To_ACD", GameAttributeEncoding.Int, 8, 0, 0, 32);
        public static readonly GameAttributeI Item_Binding_Level_Override = new GameAttributeI(287, 0, -1, 1, 1, "", "0", "Item_Binding_Level_Override", GameAttributeEncoding.IntMinMax, 8, 0, 5, 3);
        public static readonly GameAttributeI ItemStackQuantityHi = new GameAttributeI(288, 0, -1, 4, 1, "", "", "ItemStackQuantityHi", GameAttributeEncoding.Int, 8, 0, 0, 32);
        public static readonly GameAttributeI ItemStackQuantityLo = new GameAttributeI(289, 0, -1, 4, 1, "", "", "ItemStackQuantityLo", GameAttributeEncoding.Int, 8, 0, 0, 32);
        public static readonly GameAttributeF Run_Speed_Granted = new GameAttributeF(290, 0f, -1, 0, 0, "", "", "Run_Speed_Granted", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeI Run_Speed_Duration = new GameAttributeI(291, 0, -1, 0, 1, "", "", "Run_Speed_Duration", GameAttributeEncoding.IntMinMax, 9, 0, 16777215, 24);
        public static readonly GameAttributeI IdentifyCost = new GameAttributeI(292, 0, -1, 0, 1, "", "", "IdentifyCost", GameAttributeEncoding.Int, 8, 0, 0, 32);
        public static readonly GameAttributeI Seed = new GameAttributeI(293, 0, -1, 4, 1, "", "0", "Seed", GameAttributeEncoding.Int, 8, 0, 0, 32);
        public static readonly GameAttributeB IsCrafted = new GameAttributeB(294, 0, -1, 1, 1, "", "0", "IsCrafted", GameAttributeEncoding.IntMinMax, 8, 0, 1, 1);
        public static readonly GameAttributeI DyeType = new GameAttributeI(295, 0, -1, 1, 1, "", "0", "DyeType", GameAttributeEncoding.IntMinMax, 8, -1, 30, 5);
        public static readonly GameAttributeI SocketAffix = new GameAttributeI(296, -1, -1, 3, 1, "", "0", "SocketAffix", GameAttributeEncoding.Int, 8, 0, 0, 32);
        public static readonly GameAttributeI EnchantAffix = new GameAttributeI(297, -1, -1, 3, 1, "", "0", "EnchantAffix", GameAttributeEncoding.Int, 8, 0, 0, 32);
        public static readonly GameAttributeI HighlySalvageable = new GameAttributeI(298, 0, -1, 1, 1, "", "0", "HighlySalvageable", GameAttributeEncoding.Int, 8, 0, 0, 32);
        public static readonly GameAttributeB Always_Plays_GetHit = new GameAttributeB(299, 0, -1, 1, 1, "", "", "Always_Plays_GetHit", GameAttributeEncoding.IntMinMax, 0, 0, 1, 1);
        public static readonly GameAttributeB Hidden = new GameAttributeB(300, 0, -1, 1, 1, "", "", "Hidden", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeI RActor_Fade_Group = new GameAttributeI(301, -1, -1, 1, 1, "", "", "RActor_Fade_Group", GameAttributeEncoding.Int, 31, 0, 0, 32);
        public static readonly GameAttributeI Quest_Range = new GameAttributeI(302, -1, -1, 3, 1, "", "", "Quest Range", GameAttributeEncoding.Int, 31, 0, 0, 32);
        public static readonly GameAttributeI Attack_Cooldown_Min = new GameAttributeI(303, 0, -1, 0, 1, "", "", "Attack_Cooldown_Min", GameAttributeEncoding.IntMinMax, 0, 0, 16777215, 24);
        public static readonly GameAttributeI Attack_Cooldown_Delta = new GameAttributeI(304, 0, -1, 0, 1, "", "", "Attack_Cooldown_Delta", GameAttributeEncoding.IntMinMax, 0, 0, 16777215, 24);
        public static readonly GameAttributeI InitialCooldownMinTotal = new GameAttributeI(305, 0, -1, 0, 1, "", "InitialCooldownMin / Attacks_Per_Second_Total", "InitialCooldownMinTotal", GameAttributeEncoding.IntMinMax, 0, 0, 16777215, 24);
        public static readonly GameAttributeI InitialCooldownDeltaTotal = new GameAttributeI(306, 0, -1, 0, 1, "", "InitialCooldownDelta / Attacks_Per_Second_Total", "InitialCooldownDeltaTotal", GameAttributeEncoding.IntMinMax, 0, 0, 16777215, 24);
        public static readonly GameAttributeI Attack_Cooldown_Min_Total = new GameAttributeI(307, 0, -1, 0, 1, "", "Attack_Cooldown_Min / Attacks_Per_Second_Total", "Attack_Cooldown_Min_Total", GameAttributeEncoding.IntMinMax, 0, 0, 16777215, 24);
        public static readonly GameAttributeI Attack_Cooldown_Delta_Total = new GameAttributeI(308, 0, -1, 0, 1, "", "Attack_Cooldown_Delta / Attacks_Per_Second_Total", "Attack_Cooldown_Delta_Total", GameAttributeEncoding.IntMinMax, 0, 0, 16777215, 24);
        public static readonly GameAttributeI Closing_Cooldown_Min_Total = new GameAttributeI(309, 0, -1, 0, 1, "", "", "Closing_Cooldown_Min_Total", GameAttributeEncoding.IntMinMax, 0, 0, 16777215, 24);
        public static readonly GameAttributeI Closing_Cooldown_Delta_Total = new GameAttributeI(310, 0, -1, 0, 1, "", "", "Closing_Cooldown_Delta_Total", GameAttributeEncoding.IntMinMax, 0, 0, 16777215, 24);
        public static readonly GameAttributeB Quest_Monster = new GameAttributeB(311, 0, -1, 0, 1, "", "", "Quest_Monster", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeI Quest_Monster_Effect = new GameAttributeI(312, -1, -1, 3, 1, "", "", "Quest_Monster Effect", GameAttributeEncoding.Int, 31, 0, 0, 32);
        public static readonly GameAttributeI Treasure_Class = new GameAttributeI(313, -1, -1, 3, 1, "", "", "Treasure_Class", GameAttributeEncoding.Int, 0, 0, 0, 32);
        public static readonly GameAttributeB Removes_Body_On_Death = new GameAttributeB(314, 0, -1, 1, 1, "", "", "Removes_Body_On_Death", GameAttributeEncoding.IntMinMax, 0, 0, 1, 1);
        public static readonly GameAttributeI InitialCooldownMin = new GameAttributeI(315, 0, -1, 0, 1, "", "", "InitialCooldownMin", GameAttributeEncoding.IntMinMax, 0, 0, 16777215, 24);
        public static readonly GameAttributeI InitialCooldownDelta = new GameAttributeI(316, 0, -1, 0, 1, "", "", "InitialCooldownDelta", GameAttributeEncoding.IntMinMax, 0, 0, 16777215, 24);
        public static readonly GameAttributeF Knockback_Weight = new GameAttributeF(317, 0f, -1, 0, 0, "", "", "Knockback_Weight", GameAttributeEncoding.Float16, 0, 0f, 0f, 16);
        public static readonly GameAttributeB UntargetableByPets = new GameAttributeB(318, 0, -1, 1, 1, "", "", "UntargetableByPets", GameAttributeEncoding.IntMinMax, 0, 0, 1, 1);
        public static readonly GameAttributeI Damage_State_Current = new GameAttributeI(319, 0, -1, 0, 1, "", "", "Damage_State_Current", GameAttributeEncoding.IntMinMax, 31, 0, 15, 4);
        public static readonly GameAttributeI Damage_State_Max = new GameAttributeI(320, 0, -1, 0, 1, "", "", "Damage_State_Max", GameAttributeEncoding.IntMinMax, 31, 0, 15, 4);
        public static readonly GameAttributeB Is_Player_Decoy = new GameAttributeB(321, 0, -1, 1, 1, "", "", "Is_Player_Decoy", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeF Custom_Target_Weight = new GameAttributeF(322, 0f, 3, 0, 0, "", "", "Custom_Target_Weight", GameAttributeEncoding.Float16, 0, 0f, 0f, 16);
        public static readonly GameAttributeI Gizmo_State = new GameAttributeI(323, -1, -1, 3, 1, "", "", "Gizmo_State", GameAttributeEncoding.IntMinMax, 4, -1, 30, 5);
        public static readonly GameAttributeI Gizmo_Charges = new GameAttributeI(324, 0, -1, 1, 1, "", "", "Gizmo_Charges", GameAttributeEncoding.IntMinMax, 4, -1, 30, 5);
        public static readonly GameAttributeB Chest_Open = new GameAttributeB(325, 0, 3, 0, 1, "", "", "Chest_Open", GameAttributeEncoding.IntMinMax, 4, 0, 1, 1);
        public static readonly GameAttributeB Door_Locked = new GameAttributeB(326, 0, -1, 1, 1, "", "", "Door_Locked", GameAttributeEncoding.IntMinMax, 4, 0, 1, 1);
        public static readonly GameAttributeI Door_Timer = new GameAttributeI(327, -1, -1, 3, 1, "", "", "Door_Timer", GameAttributeEncoding.IntMinMax, 4, -1, 16777214, 24);
        public static readonly GameAttributeB Gizmo_Disabled_By_Script = new GameAttributeB(328, 0, -1, 1, 1, "", "", "Gizmo_Disabled_By_Script", GameAttributeEncoding.IntMinMax, 4, 0, 1, 1);
        public static readonly GameAttributeI Gizmo_Operator_ACDID = new GameAttributeI(329, -1, -1, 3, 1, "", "", "Gizmo_Operator_ACDID", GameAttributeEncoding.Int, 4, 0, 0, 32);
        public static readonly GameAttributeI Triggering_Count = new GameAttributeI(330, 0, -1, 0, 1, "", "", "Triggering_Count", GameAttributeEncoding.Int, 4, 0, 0, 32);
        public static readonly GameAttributeF Gate_Position = new GameAttributeF(331, 0f, -1, 0, 0, "", "", "Gate_Position", GameAttributeEncoding.Float16, 4, 0f, 0f, 16);
        public static readonly GameAttributeF Gate_Velocity = new GameAttributeF(332, 0f, -1, 0, 0, "", "", "Gate_Velocity", GameAttributeEncoding.Float16, 4, 0f, 0f, 16);
        public static readonly GameAttributeB Gizmo_Has_Been_Operated = new GameAttributeB(333, 0, -1, 1, 1, "", "", "Gizmo_Has_Been_Operated", GameAttributeEncoding.IntMinMax, 4, 0, 1, 1);
        public static readonly GameAttributeI Pet_Owner = new GameAttributeI(334, -1, -1, 3, 1, "", "", "Pet_Owner", GameAttributeEncoding.IntMinMax, 31, -1, 8, 4);
        public static readonly GameAttributeI Pet_Creator = new GameAttributeI(335, -1, -1, 3, 1, "", "", "Pet_Creator", GameAttributeEncoding.IntMinMax, 31, -1, 8, 4);
        public static readonly GameAttributeI Pet_Type = new GameAttributeI(336, -1, -1, 3, 1, "", "", "Pet_Type", GameAttributeEncoding.IntMinMax, 31, -1, 24, 5);
        public static readonly GameAttributeB DropsNoLoot = new GameAttributeB(337, 0, -1, 1, 1, "", "", "DropsNoLoot", GameAttributeEncoding.IntMinMax, 0, 0, 1, 1);
        public static readonly GameAttributeB GrantsNoXP = new GameAttributeB(338, 0, -1, 1, 1, "", "", "GrantsNoXP", GameAttributeEncoding.IntMinMax, 0, 0, 1, 1);
        public static readonly GameAttributeI Hireling_Class = new GameAttributeI(339, 0, -1, 1, 1, "", "", "Hireling_Class", GameAttributeEncoding.IntMinMax, 31, 0, 4, 3);
        public static readonly GameAttributeI Summoned_By_SNO = new GameAttributeI(340, -1, -1, 3, 1, "", "", "Summoned_By_SNO", GameAttributeEncoding.Int, 0, 0, 0, 32);
        public static readonly GameAttributeB Is_NPC = new GameAttributeB(341, 0, -1, 1, 1, "", "", "Is_NPC", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeB NPC_Is_Operatable = new GameAttributeB(342, 0, -1, 1, 1, "", "", "NPC_Is_Operatable", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeB NPC_Is_Escorting = new GameAttributeB(343, 0, -1, 1, 1, "", "", "NPC_Is_Escorting", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeB NPC_Has_Interact_Options = new GameAttributeB(344, 0, 12, 1, 1, "", "", "NPC_Has_Interact_Options", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeI Conversation_Icon = new GameAttributeI(345, -1, 12, 3, 1, "", "", "Conversation_Icon", GameAttributeEncoding.IntMinMax, 31, -1, 6, 3);
        public static readonly GameAttributeI Callout_Cooldown = new GameAttributeI(346, -1, 16, 1, 1, "", "", "Callout_Cooldown", GameAttributeEncoding.Int, 31, 0, 0, 32);
        public static readonly GameAttributeI Banter_Cooldown = new GameAttributeI(347, -1, 16, 1, 1, "", "", "Banter_Cooldown", GameAttributeEncoding.Int, 31, 0, 0, 32);
        public static readonly GameAttributeI Conversation_Heard_Count = new GameAttributeI(348, 0, 16, 1, 1, "", "", "Conversation_Heard_Count", GameAttributeEncoding.Int, 0, 0, 0, 32);
        public static readonly GameAttributeI Last_Tick_Shop_Entered = new GameAttributeI(349, -1, -1, 3, 1, "", "", "Last_Tick_Shop_Entered", GameAttributeEncoding.Int, 0, 0, 0, 32);
        public static readonly GameAttributeB Is_Helper = new GameAttributeB(350, 0, -1, 1, 1, "", "", "Is_Helper", GameAttributeEncoding.IntMinMax, 0, 0, 1, 1);
        public static readonly GameAttributeI Axe = new GameAttributeI(351, 0, -1, 0, 0, "", "", "Axe", GameAttributeEncoding.Int, 9, 0, 0, 32);
        public static readonly GameAttributeI Axe2H = new GameAttributeI(352, 0, -1, 0, 0, "", "", "Axe2H", GameAttributeEncoding.Int, 9, 0, 0, 32);
        public static readonly GameAttributeI ThrowingAxe = new GameAttributeI(353, 0, -1, 0, 0, "", "", "ThrowingAxe", GameAttributeEncoding.Int, 9, 0, 0, 32);
        public static readonly GameAttributeI AxeAny = new GameAttributeI(354, 0, -1, 0, 0, "Pin(Axe + Axe2H + ThrowingAxe, 0, 1)", "Pin(Axe + Axe2H + ThrowingAxe, 0, 1)", "AxeAny", GameAttributeEncoding.Int, 9, 0, 0, 32);
        public static readonly GameAttributeI Bow = new GameAttributeI(355, 0, -1, 0, 0, "", "", "Bow", GameAttributeEncoding.Int, 9, 0, 0, 32);
        public static readonly GameAttributeI Crossbow = new GameAttributeI(356, 0, -1, 0, 0, "", "", "Crossbow", GameAttributeEncoding.Int, 9, 0, 0, 32);
        public static readonly GameAttributeI BowAny = new GameAttributeI(357, 0, -1, 0, 0, "Pin(Bow + Crossbow, 0, 1)", "Pin(Bow + Crossbow, 0, 1)", "BowAny", GameAttributeEncoding.Int, 9, 0, 0, 32);
        public static readonly GameAttributeI Club = new GameAttributeI(358, 0, -1, 0, 0, "", "", "Club", GameAttributeEncoding.Int, 9, 0, 0, 32);
        public static readonly GameAttributeI Club2H = new GameAttributeI(359, 0, -1, 0, 0, "", "", "Club2H", GameAttributeEncoding.Int, 9, 0, 0, 32);
        public static readonly GameAttributeI ClubAny = new GameAttributeI(360, 0, -1, 0, 0, "Pin(Club + Club2H, 0, 1)", "Pin(Club + Club2H, 0, 1)", "ClubAny", GameAttributeEncoding.Int, 9, 0, 0, 32);
        public static readonly GameAttributeI Dagger = new GameAttributeI(361, 0, -1, 0, 0, "", "", "Dagger", GameAttributeEncoding.Int, 9, 0, 0, 32);
        public static readonly GameAttributeI Mace = new GameAttributeI(362, 0, -1, 0, 0, "", "", "Mace", GameAttributeEncoding.Int, 9, 0, 0, 32);
        public static readonly GameAttributeI Mace2H = new GameAttributeI(363, 0, -1, 0, 0, "", "", "Mace2H", GameAttributeEncoding.Int, 9, 0, 0, 32);
        public static readonly GameAttributeI MaceAny = new GameAttributeI(364, 0, -1, 0, 0, "Pin(Mace + Mace2H, 0, 1)", "Pin(Mace + Mace2H, 0, 1)", "MaceAny", GameAttributeEncoding.Int, 9, 0, 0, 32);
        public static readonly GameAttributeI Sword = new GameAttributeI(365, 0, -1, 0, 0, "", "", "Sword", GameAttributeEncoding.Int, 9, 0, 0, 32);
        public static readonly GameAttributeI Sword2H = new GameAttributeI(366, 0, -1, 0, 0, "", "", "Sword2H", GameAttributeEncoding.Int, 9, 0, 0, 32);
        public static readonly GameAttributeI SwordAny = new GameAttributeI(367, 0, -1, 0, 0, "Pin(Sword + Sword2H, 0, 1)", "Pin(Sword + Sword2H, 0, 1)", "SwordAny", GameAttributeEncoding.Int, 9, 0, 0, 32);
        public static readonly GameAttributeI Polearm = new GameAttributeI(368, 0, -1, 0, 0, "", "", "Polearm", GameAttributeEncoding.Int, 9, 0, 0, 32);
        public static readonly GameAttributeI Spear = new GameAttributeI(369, 0, -1, 0, 0, "", "", "Spear", GameAttributeEncoding.Int, 9, 0, 0, 32);
        public static readonly GameAttributeI Wand = new GameAttributeI(370, 0, -1, 0, 0, "", "", "Wand", GameAttributeEncoding.Int, 9, 0, 0, 32);
        public static readonly GameAttributeI ColdStaff = new GameAttributeI(371, 0, -1, 0, 0, "", "", "ColdStaff", GameAttributeEncoding.Int, 9, 0, 0, 32);
        public static readonly GameAttributeI FireStaff = new GameAttributeI(372, 0, -1, 0, 0, "", "", "FireStaff", GameAttributeEncoding.Int, 9, 0, 0, 32);
        public static readonly GameAttributeI LightningStaff = new GameAttributeI(373, 0, -1, 0, 0, "", "", "LightningStaff", GameAttributeEncoding.Int, 9, 0, 0, 32);
        public static readonly GameAttributeI PoisonStaff = new GameAttributeI(374, 0, -1, 0, 0, "", "", "PoisonStaff", GameAttributeEncoding.Int, 9, 0, 0, 32);
        public static readonly GameAttributeI StaffAny = new GameAttributeI(375, 0, -1, 0, 0, "Pin(ColdStaff + FireStaff + LightningStaff + PoisonStaff, 0, 1)", "Pin(ColdStaff + FireStaff + LightningStaff + PoisonStaff, 0, 1)", "StaffAny", GameAttributeEncoding.Int, 9, 0, 0, 32);
        public static readonly GameAttributeI Weapon1H = new GameAttributeI(376, 0, -1, 0, 0, "Pin(Axe + Club + Dagger + Mace + Sword + Wand, 0, 1)", "Pin(Axe + Club + Dagger + Mace + Sword + Wand, 0, 1)", "Weapon1H", GameAttributeEncoding.Int, 9, 0, 0, 32);
        public static readonly GameAttributeI Weapon2H = new GameAttributeI(377, 0, -1, 0, 0, "Pin(Axe2H + BowAny + Club2H + Mace2H + Sword2H + Polearm + Spear + StaffAny, 0, 1)", "Pin(Axe2H + BowAny + Club2H + Mace2H + Sword2H + Polearm + Spear + StaffAny, 0, 1)", "Weapon2H", GameAttributeEncoding.Int, 9, 0, 0, 32);
        public static readonly GameAttributeI WeaponMelee = new GameAttributeI(378, 0, -1, 0, 0, "Pin(Axe + Axe2H + ClubAny + Dagger + MaceAny + SwordAny + Polearm + Spear + Wand + StaffAny, 0, 1)", "Pin(Axe + Axe2H + ClubAny + Dagger + MaceAny + SwordAny + Polearm + Spear + Wand + StaffAny, 0, 1)", "WeaponMelee", GameAttributeEncoding.Int, 9, 0, 0, 32);
        public static readonly GameAttributeI WeaponRanged = new GameAttributeI(379, 0, -1, 0, 0, "Pin(ThrowingAxe + BowAny, 0, 1)", "Pin(ThrowingAxe + BowAny, 0, 1)", "WeaponRanged", GameAttributeEncoding.Int, 9, 0, 0, 32);
        public static readonly GameAttributeI Quiver = new GameAttributeI(380, 0, -1, 0, 0, "", "", "Quiver", GameAttributeEncoding.Int, 9, 0, 0, 32);
        public static readonly GameAttributeI Reincarnation_Buff = new GameAttributeI(381, -1, -1, 3, 1, "", "", "Reincarnation_Buff", GameAttributeEncoding.Int, 31, 0, 0, 32);
        public static readonly GameAttributeI Dead_Body_AnimTag = new GameAttributeI(382, -1, -1, 3, 1, "", "", "Dead_Body_AnimTag", GameAttributeEncoding.Int, 0, 0, 0, 32);
        public static readonly GameAttributeI Spawned_by_ACDID = new GameAttributeI(383, -1, -1, 3, 1, "", "", "Spawned_by_ACDID", GameAttributeEncoding.Int, 0, 0, 0, 32);
        public static readonly GameAttributeI Summoned_By_ACDID = new GameAttributeI(384, -1, -1, 3, 1, "", "", "Summoned_By_ACDID", GameAttributeEncoding.Int, 31, 0, 0, 32);
        public static readonly GameAttributeI Summoner_ID = new GameAttributeI(385, -1, -1, 3, 1, "", "", "Summoner_ID", GameAttributeEncoding.Int, 31, 0, 0, 32);
        public static readonly GameAttributeI Banner_ACDID = new GameAttributeI(386, -1, -1, 3, 1, "", "", "Banner_ACDID", GameAttributeEncoding.Int, 0, 0, 0, 32);
        public static readonly GameAttributeF Breakable_Shield_HP = new GameAttributeF(387, 0f, -1, 0, 0, "", "", "Breakable_Shield_HP", GameAttributeEncoding.Float16Or32, 0, 0f, 0f, 0);
        public static readonly GameAttributeI Current_WeaponClass = new GameAttributeI(388, -1, -1, 3, 1, "", "", "Current_WeaponClass", GameAttributeEncoding.IntMinMax, 31, -1, 19, 5);
        public static readonly GameAttributeB Weapons_Sheathed = new GameAttributeB(389, 0, -1, 1, 1, "", "", "Weapons_Sheathed", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeB Held_In_OffHand = new GameAttributeB(390, 0, -1, 1, 1, "", "0", "Held_In_OffHand", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeF Attacks_Per_Second_Item_MainHand = new GameAttributeF(391, 0f, -1, 0, 0, "(Held_In_OffHand ? 0 : Attacks_Per_Second_Item_Subtotal )", "", "Attacks_Per_Second_Item_MainHand", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeF Attacks_Per_Second_Item_OffHand = new GameAttributeF(392, 0f, -1, 0, 0, "(Held_In_OffHand ? Attacks_Per_Second_Item_Subtotal : 0)", "", "Attacks_Per_Second_Item_OffHand", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeF Attacks_Per_Second_Item_Total_MainHand = new GameAttributeF(393, 0f, -1, 0, 0, "Attacks_Per_Second_Item_MainHand + Attacks_Per_Second_Item_Bonus", "", "Attacks_Per_Second_Item_Total_MainHand", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeF Attacks_Per_Second_Item_Total_OffHand = new GameAttributeF(394, 0f, -1, 0, 0, "Attacks_Per_Second_Item_OffHand + Attacks_Per_Second_Item_Bonus", "", "Attacks_Per_Second_Item_Total_OffHand", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeF Damage_Weapon_Min_Total_MainHand = new GameAttributeF(395, 0f, 0, 0, 0, "(Held_In_OffHand#NONE ? 0 : Damage_Weapon_Min_Total )", "", "Damage_Weapon_Min_Total_MainHand", GameAttributeEncoding.Float16Or32, 9, 0f, 0f, 0);
        public static readonly GameAttributeF Damage_Weapon_Min_Total_OffHand = new GameAttributeF(396, 0f, 0, 0, 0, "(Held_In_OffHand#NONE ? Damage_Weapon_Min_Total : 0)", "", "Damage_Weapon_Min_Total_OffHand", GameAttributeEncoding.Float16Or32, 9, 0f, 0f, 0);
        public static readonly GameAttributeF Damage_Weapon_Delta_Total_MainHand = new GameAttributeF(397, 0f, 0, 0, 0, "(Held_In_OffHand#NONE ? 0 : Damage_Weapon_Delta_Total )", "", "Damage_Weapon_Delta_Total_MainHand", GameAttributeEncoding.Float16Or32, 9, 0f, 0f, 0);
        public static readonly GameAttributeF Damage_Weapon_Delta_Total_OffHand = new GameAttributeF(398, 0f, 0, 0, 0, "(Held_In_OffHand#NONE ? Damage_Weapon_Delta_Total : 0)", "", "Damage_Weapon_Delta_Total_OffHand", GameAttributeEncoding.Float16Or32, 9, 0f, 0f, 0);
        public static readonly GameAttributeF Attacks_Per_Second_Item_CurrentHand = new GameAttributeF(399, 0f, -1, 0, 0, "", "(DualWield_Hand ? Attacks_Per_Second_Item_OffHand : Attacks_Per_Second_Item_MainHand)", "Attacks_Per_Second_Item_CurrentHand", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeF Damage_Weapon_Min_Total_CurrentHand = new GameAttributeF(400, 0f, 0, 0, 0, "", "(DualWield_Hand#NONE ? Damage_Weapon_Min_Total_OffHand : Damage_Weapon_Min_Total_MainHand)", "Damage_Weapon_Min_Total_CurrentHand", GameAttributeEncoding.Float16Or32, 9, 0f, 0f, 0);
        public static readonly GameAttributeF Damage_Weapon_Delta_Total_CurrentHand = new GameAttributeF(401, 0f, 0, 0, 0, "", "(DualWield_Hand#NONE ? Damage_Weapon_Delta_Total_OffHand : Damage_Weapon_Delta_Total_MainHand)", "Damage_Weapon_Delta_Total_CurrentHand", GameAttributeEncoding.Float16Or32, 9, 0f, 0f, 0);
        public static readonly GameAttributeI Has_Special_Death_AnimTag = new GameAttributeI(402, -1, -1, 1, 1, "", "", "Has_Special_Death_AnimTag", GameAttributeEncoding.Int, 0, 0, 0, 32);
        public static readonly GameAttributeI Death_Type_Override = new GameAttributeI(403, -1, -1, 3, 1, "", "", "Death_Type_Override", GameAttributeEncoding.Int, 0, 0, 0, 32);
        public static readonly GameAttributeB In_Combat = new GameAttributeB(404, 0, -1, 1, 1, "", "", "In_Combat", GameAttributeEncoding.IntMinMax, 1, 0, 1, 1);
        public static readonly GameAttributeB In_Conversation = new GameAttributeB(405, 0, -1, 3, 1, "", "", "In_Conversation", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeI Last_Tick_Potion_Used = new GameAttributeI(406, -1, -1, 3, 1, "", "", "Last_Tick_Potion_Used", GameAttributeEncoding.Int, 0, 0, 0, 32);
        public static readonly GameAttributeF Potion_Dilution_Percent = new GameAttributeF(407, 0f, -1, 0, 0, "", "", "Potion_Dilution_Percent", GameAttributeEncoding.Float16, 0, 0f, 0f, 16);
        public static readonly GameAttributeF Out_Of_Combat_Health_Regen_Percent = new GameAttributeF(408, 0f, -1, 0, 0, "", "", "Out_Of_Combat_Health_Regen_Percent", GameAttributeEncoding.Float16, 0, 0f, 0f, 16);
        public static readonly GameAttributeF Out_Of_Combat_Mana_Regen_Percent = new GameAttributeF(409, 0f, -1, 0, 0, "", "", "Out_Of_Combat_Mana_Regen_Percent", GameAttributeEncoding.Float16, 0, 0f, 0f, 16);
        public static readonly GameAttributeI Potion_Dilution_Duration = new GameAttributeI(410, -1, -1, 3, 1, "", "", "Potion_Dilution_Duration", GameAttributeEncoding.IntMinMax, 0, -1, 16777214, 24);
        public static readonly GameAttributeF Potion_Dilution_Scalar = new GameAttributeF(411, 0f, -1, 0, 0, "", "", "Potion_Dilution_Scalar", GameAttributeEncoding.Float16, 0, 0f, 0f, 16);
        public static readonly GameAttributeB Feared = new GameAttributeB(412, 0, -1, 1, 1, "", "", "Feared", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeB Fear_Immune = new GameAttributeB(413, 0, -1, 1, 1, "", "", "Fear_Immune", GameAttributeEncoding.IntMinMax, 0, 0, 1, 1);
        public static readonly GameAttributeI Last_Damage_ACD = new GameAttributeI(414, -1, -1, 3, 1, "", "", "Last_Damage_ACD", GameAttributeEncoding.Int, 31, 0, 0, 32);
        public static readonly GameAttributeI Attached_To_ACD = new GameAttributeI(415, -1, -1, 3, 1, "", "", "Attached_To_ACD", GameAttributeEncoding.Int, 31, 0, 0, 32);
        public static readonly GameAttributeI Attachment_ACD = new GameAttributeI(416, -1, -1, 3, 1, "", "", "Attachment_ACD", GameAttributeEncoding.Int, 31, 0, 0, 32);
        public static readonly GameAttributeI Normal_Attack_Replacement_Power_SNO = new GameAttributeI(417, -1, -1, 3, 1, "", "", "Normal_Attack_Replacement_Power_SNO", GameAttributeEncoding.Int, 1, 0, 0, 32);
        public static readonly GameAttributeF Damage_Type_Override = new GameAttributeF(418, 0f, 0, 0, 0, "", "", "Damage_Type_Override", GameAttributeEncoding.Float16Or32, 0, 0f, 0f, 0);
        public static readonly GameAttributeF Minion_Count_Bonus_Percent = new GameAttributeF(419, 0f, -1, 0, 0, "", "", "Minion_Count_Bonus_Percent", GameAttributeEncoding.Float16, 0, 0f, 0f, 16);
        public static readonly GameAttributeI Champion_Teleport_Next_Tick = new GameAttributeI(420, 0, -1, 0, 1, "", "", "Champion_Teleport_Next_Tick", GameAttributeEncoding.Int, 0, 0, 0, 32);
        public static readonly GameAttributeF Champion_Teleport_Time_Min_In_Seconds = new GameAttributeF(421, 0f, -1, 0, 0, "", "", "Champion_Teleport_Time_Min_In_Seconds", GameAttributeEncoding.Float16, 0, 0f, 0f, 16);
        public static readonly GameAttributeF Champion_Teleport_Time_Delta_In_Seconds = new GameAttributeF(422, 0f, -1, 0, 0, "", "", "Champion_Teleport_Time_Delta_In_Seconds", GameAttributeEncoding.Float16, 0, 0f, 0f, 16);
        public static readonly GameAttributeI Champion_Clone_Next_Tick = new GameAttributeI(423, 0, -1, 0, 1, "", "", "Champion_Clone_Next_Tick", GameAttributeEncoding.Int, 0, 0, 0, 32);
        public static readonly GameAttributeF Champion_Clone_Time_Min_In_Seconds = new GameAttributeF(424, 0f, -1, 0, 0, "", "", "Champion_Clone_Time_Min_In_Seconds", GameAttributeEncoding.Float16, 0, 0f, 0f, 16);
        public static readonly GameAttributeF Champion_Clone_Time_Delta_In_Seconds = new GameAttributeF(425, 0f, -1, 0, 0, "", "", "Champion_Clone_Time_Delta_In_Seconds", GameAttributeEncoding.Float16, 0, 0f, 0f, 16);
        public static readonly GameAttributeF Champion_Clone_Hitpoint_Bonus_Percent = new GameAttributeF(426, 0f, -1, 0, 0, "", "", "Champion_Clone_Hitpoint_Bonus_Percent", GameAttributeEncoding.Float16, 0, 0f, 0f, 16);
        public static readonly GameAttributeF Champion_Clone_Damage_Bonus_Percent = new GameAttributeF(427, 0f, -1, 0, 0, "", "", "Champion_Clone_Damage_Bonus_Percent", GameAttributeEncoding.Float16, 0, 0f, 0f, 16);
        public static readonly GameAttributeI Champion_Ghostly_Next_Tick = new GameAttributeI(428, 0, -1, 0, 1, "", "", "Champion_Ghostly_Next_Tick", GameAttributeEncoding.Int, 0, 0, 0, 32);
        public static readonly GameAttributeF Champion_Ghostly_Inactive_Time_Min_In_Seconds = new GameAttributeF(429, 0f, -1, 0, 0, "", "", "Champion_Ghostly_Inactive_Time_Min_In_Seconds", GameAttributeEncoding.Float16, 0, 0f, 0f, 16);
        public static readonly GameAttributeF Champion_Ghostly_Inactive_Time_Delta_In_Seconds = new GameAttributeF(430, 0f, -1, 0, 0, "", "", "Champion_Ghostly_Inactive_Time_Delta_In_Seconds", GameAttributeEncoding.Float16, 0, 0f, 0f, 16);
        public static readonly GameAttributeF Champion_Ghostly_Active_Time_Min_In_Seconds = new GameAttributeF(431, 0f, -1, 0, 0, "", "", "Champion_Ghostly_Active_Time_Min_In_Seconds", GameAttributeEncoding.Float16, 0, 0f, 0f, 16);
        public static readonly GameAttributeF Champion_Ghostly_Active_Time_Delta_In_Seconds = new GameAttributeF(432, 0f, -1, 0, 0, "", "", "Champion_Ghostly_Active_Time_Delta_In_Seconds", GameAttributeEncoding.Float16, 0, 0f, 0f, 16);
        public static readonly GameAttributeF Champion_Ghostly_Saved_Dodge_Chance = new GameAttributeF(433, 0f, -1, 0, 0, "", "", "Champion_Ghostly_Saved_Dodge_Chance", GameAttributeEncoding.Float16, 0, 0f, 0f, 16);
        public static readonly GameAttributeB Champion_Ghostly = new GameAttributeB(434, 0, -1, 1, 1, "", "", "Champion_Ghostly", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeI Base_Element = new GameAttributeI(435, -1, -1, 1, 1, "", "", "Base_Element", GameAttributeEncoding.IntMinMax, 0, -1, 7, 4);
        public static readonly GameAttributeF Projectile_Amount_Bonus_Percent = new GameAttributeF(436, 0f, -1, 0, 0, "", "", "Projectile_Amount_Bonus_Percent", GameAttributeEncoding.Float16, 0, 0f, 0f, 16);
        public static readonly GameAttributeF Projectile_Reflect_Chance = new GameAttributeF(437, 0f, 0, 0, 0, "", "", "Projectile_Reflect_Chance", GameAttributeEncoding.Float16, 0, 0f, 0f, 16);
        public static readonly GameAttributeF Attack_Fear_Chance = new GameAttributeF(438, 0f, -1, 0, 0, "", "", "Attack_Fear_Chance", GameAttributeEncoding.Float16, 0, 0f, 0f, 16);
        public static readonly GameAttributeF Attack_Fear_Time_Min = new GameAttributeF(439, 0f, -1, 0, 0, "", "", "Attack_Fear_Time_Min", GameAttributeEncoding.Float16, 0, 0f, 0f, 16);
        public static readonly GameAttributeF Attack_Fear_Time_Delta = new GameAttributeF(440, 0f, -1, 0, 0, "", "", "Attack_Fear_Time_Delta", GameAttributeEncoding.Float16, 0, 0f, 0f, 16);
        public static readonly GameAttributeB Buff_Visual_Effect = new GameAttributeB(441, 0, 9, 1, 1, "", "", "Buff_Visual_Effect", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeI Buff_Icon_Start_Tick0 = new GameAttributeI(442, 0, 4, 1, 1, "", "", "Buff_Icon_Start_Tick0", GameAttributeEncoding.Int, 31, 0, 0, 32);
        public static readonly GameAttributeI Buff_Icon_Start_Tick1 = new GameAttributeI(443, 0, 4, 1, 1, "", "", "Buff_Icon_Start_Tick1", GameAttributeEncoding.Int, 31, 0, 0, 32);
        public static readonly GameAttributeI Buff_Icon_Start_Tick2 = new GameAttributeI(444, 0, 4, 1, 1, "", "", "Buff_Icon_Start_Tick2", GameAttributeEncoding.Int, 31, 0, 0, 32);
        public static readonly GameAttributeI Buff_Icon_Start_Tick3 = new GameAttributeI(445, 0, 4, 1, 1, "", "", "Buff_Icon_Start_Tick3", GameAttributeEncoding.Int, 31, 0, 0, 32);
        public static readonly GameAttributeI Buff_Icon_End_Tick0 = new GameAttributeI(446, 0, 4, 1, 1, "", "", "Buff_Icon_End_Tick0", GameAttributeEncoding.Int, 31, 0, 0, 32);
        public static readonly GameAttributeI Buff_Icon_End_Tick1 = new GameAttributeI(447, 0, 4, 1, 1, "", "", "Buff_Icon_End_Tick1", GameAttributeEncoding.Int, 31, 0, 0, 32);
        public static readonly GameAttributeI Buff_Icon_End_Tick2 = new GameAttributeI(448, 0, 4, 1, 1, "", "", "Buff_Icon_End_Tick2", GameAttributeEncoding.Int, 31, 0, 0, 32);
        public static readonly GameAttributeI Buff_Icon_End_Tick3 = new GameAttributeI(449, 0, 4, 1, 1, "", "", "Buff_Icon_End_Tick3", GameAttributeEncoding.Int, 31, 0, 0, 32);
        public static readonly GameAttributeB Could_Have_Ragdolled = new GameAttributeB(450, 0, -1, 1, 1, "", "", "Could_Have_Ragdolled", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeI Ambient_Damage_Effect_Last_Time = new GameAttributeI(451, 0, -1, 1, 1, "", "", "Ambient_Damage_Effect_Last_Time", GameAttributeEncoding.Int, 0, 0, 0, 32);
        public static readonly GameAttributeF Scale_Bonus = new GameAttributeF(452, 0f, -1, 0, 0, "", "", "Scale_Bonus", GameAttributeEncoding.Float16, 31, 0f, 0f, 16);
        public static readonly GameAttributeB Deleted_On_Server = new GameAttributeB(453, 0, -1, 1, 1, "", "", "Deleted_On_Server", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeB Does_No_Damage = new GameAttributeB(454, 0, -1, 1, 1, "", "", "Does_No_Damage", GameAttributeEncoding.IntMinMax, 0, 0, 1, 1);
        public static readonly GameAttributeB Does_Fake_Damage = new GameAttributeB(455, 0, -1, 1, 1, "", "", "Does_Fake_Damage", GameAttributeEncoding.IntMinMax, 0, 0, 1, 1);
        public static readonly GameAttributeF SlowTime_Debuff = new GameAttributeF(456, 0f, -1, 0, 0, "", "", "SlowTime_Debuff", GameAttributeEncoding.Float16, 31, 0f, 0f, 16);
        public static readonly GameAttributeB Blocks_Projectiles = new GameAttributeB(457, 0, -1, 1, 1, "", "", "Blocks_Projectiles", GameAttributeEncoding.IntMinMax, 0, 0, 1, 1);
        public static readonly GameAttributeB Frozen = new GameAttributeB(458, 0, -1, 1, 1, "", "", "Frozen", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeF Freeze_Damage_Percent_Bonus = new GameAttributeF(459, 0f, -1, 0, 0, "", "", "Freeze_Damage_Percent_Bonus", GameAttributeEncoding.Float16, 0, 0f, 0f, 16);
        public static readonly GameAttributeB Buff_Active = new GameAttributeB(460, 0, 4, 1, 1, "", "", "Buff_Active", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeF DualWield_BothAttack_Chance = new GameAttributeF(461, 0f, -1, 0, 0, "", "", "DualWield_BothAttack_Chance", GameAttributeEncoding.Float16, 0, 0f, 0f, 16);
        public static readonly GameAttributeI Summon_Expiration_Tick = new GameAttributeI(462, 0, -1, 0, 1, "", "", "Summon_Expiration_Tick", GameAttributeEncoding.Int, 0, 0, 0, 32);
        public static readonly GameAttributeI Summon_Count = new GameAttributeI(463, 0, -1, 0, 1, "", "", "Summon_Count", GameAttributeEncoding.Int, 0, 0, 0, 32);
        public static readonly GameAttributeB Uninterruptible = new GameAttributeB(464, 0, -1, 1, 1, "", "", "Uninterruptible", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeB Queue_Death = new GameAttributeB(465, 0, -1, 1, 1, "", "", "Queue Death", GameAttributeEncoding.IntMinMax, 0, 0, 1, 1);
        public static readonly GameAttributeB CantStartDisplayedPowers = new GameAttributeB(466, 0, -1, 1, 1, "", "", "CantStartDisplayedPowers", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeI Wizard_Slowtime_Proxy_ACD = new GameAttributeI(467, -1, -1, 3, 1, "", "", "Wizard_Slowtime_Proxy_ACD", GameAttributeEncoding.Int, 0, 0, 0, 32);
        public static readonly GameAttributeF DPS = new GameAttributeF(468, 0f, -1, 1, 0, "", "", "DPS", GameAttributeEncoding.Float16Or32, 31, 0f, 0f, 0);
        public static readonly GameAttributeI Resurrection_Power = new GameAttributeI(469, -1, -1, 3, 1, "", "", "Resurrection_Power", GameAttributeEncoding.Int, 0, 0, 0, 32);
        public static readonly GameAttributeF Freeze_Damage = new GameAttributeF(470, 0f, -1, 1, 0, "", "", "Freeze_Damage", GameAttributeEncoding.Float16Or32, 31, 0f, 0f, 0);
        public static readonly GameAttributeF Freeze_Capacity = new GameAttributeF(471, 0f, -1, 0, 0, "", "", "Freeze_Capacity", GameAttributeEncoding.Float16Or32, 0, 0f, 0f, 0);
        public static readonly GameAttributeF Thaw_Rate = new GameAttributeF(472, 0f, -1, 0, 0, "", "", "Thaw_Rate", GameAttributeEncoding.Float16Or32, 0, 0f, 0f, 0);
        public static readonly GameAttributeF Chilled_Dur_Bonus_Percent = new GameAttributeF(473, 0f, -1, 0, 0, "", "", "Chilled_Dur_Bonus_Percent", GameAttributeEncoding.Float16Or32, 0, 0f, 0f, 0);
        public static readonly GameAttributeF DOT_DPS = new GameAttributeF(474, 0f, -1, 0, 0, "", "", "DOT_DPS", GameAttributeEncoding.Float16Or32, 31, 0f, 0f, 0);
        public static readonly GameAttributeF DamageCap_Percent = new GameAttributeF(475, 0f, -1, 1, 0, "", "", "DamageCap_Percent", GameAttributeEncoding.Float16, 31, 0f, 0f, 16);
        public static readonly GameAttributeI Item_Time_Sold = new GameAttributeI(476, 0, -1, 1, 1, "", "", "Item_Time_Sold", GameAttributeEncoding.Int, 8, 0, 0, 32);
        public static readonly GameAttributeI Forced_Hireling_Power = new GameAttributeI(477, -1, -1, 3, 1, "", "", "Forced_Hireling_Power", GameAttributeEncoding.Int, 0, 0, 0, 32);
        public static readonly GameAttributeB IsRooted = new GameAttributeB(478, 0, -1, 1, 1, "", "", "IsRooted", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeI RootTargetACD = new GameAttributeI(479, -1, -1, 3, 1, "", "", "RootTargetACD", GameAttributeEncoding.Int, 31, 0, 0, 32);
        public static readonly GameAttributeF RootAutoDecayPerSecond = new GameAttributeF(480, 0f, -1, 1, 0, "", "", "RootAutoDecayPerSecond", GameAttributeEncoding.Float16, 31, 0f, 0f, 16);
        public static readonly GameAttributeF RootUnitValue = new GameAttributeF(481, 0f, -1, 1, 0, "", "", "RootUnitValue", GameAttributeEncoding.Float16, 31, 0f, 0f, 16);
        public static readonly GameAttributeI RootTotalTicks = new GameAttributeI(482, 0, -1, 1, 1, "", "", "RootTotalTicks", GameAttributeEncoding.Int, 0, 0, 0, 32);
        public static readonly GameAttributeB Hide_Affixes = new GameAttributeB(483, 0, -1, 1, 1, "", "", "Hide_Affixes", GameAttributeEncoding.IntMinMax, 8, 0, 1, 1);
        public static readonly GameAttributeI Skill_Socket_Bonus = new GameAttributeI(484, 0, -1, 1, 1, "", "", "Skill_Socket_Bonus", GameAttributeEncoding.IntMinMax, 8, 0, 31, 5);
        public static readonly GameAttributeI Rune_Rank = new GameAttributeI(485, 0, 11, 0, 1, "", "", "Rune_Rank", GameAttributeEncoding.IntMinMax, 8, 0, 255, 8);
        public static readonly GameAttributeI Rune_Attuned_Power = new GameAttributeI(486, -1, -1, 1, 1, "", "", "Rune_Attuned_Power", GameAttributeEncoding.Int, 8, 0, 0, 32);
        public static readonly GameAttributeI Rune_A = new GameAttributeI(487, 0, 11, 0, 1, "", "", "Rune_A", GameAttributeEncoding.IntMinMax, 31, 0, 255, 8);
        public static readonly GameAttributeI Rune_B = new GameAttributeI(488, 0, 11, 0, 1, "", "", "Rune_B", GameAttributeEncoding.IntMinMax, 31, 0, 255, 8);
        public static readonly GameAttributeI Rune_C = new GameAttributeI(489, 0, 11, 0, 1, "", "", "Rune_C", GameAttributeEncoding.IntMinMax, 31, 0, 255, 8);
        public static readonly GameAttributeI Rune_D = new GameAttributeI(490, 0, 11, 0, 1, "", "", "Rune_D", GameAttributeEncoding.IntMinMax, 31, 0, 255, 8);
        public static readonly GameAttributeI Rune_E = new GameAttributeI(491, 0, 11, 0, 1, "", "", "Rune_E", GameAttributeEncoding.IntMinMax, 31, 0, 255, 8);
        public static readonly GameAttributeF Resistance_Stun = new GameAttributeF(492, 0f, -1, 0, 0, "", "", "Resistance_Stun", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeF Resistance_Stun_Total = new GameAttributeF(493, 0f, -1, 0, 0, "", "Resistance_Stun + Resistance_StunRootFreeze", "Resistance_Stun_Total", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeF Resistance_Root = new GameAttributeF(494, 0f, -1, 0, 0, "", "", "Resistance_Root", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeF Resistance_Root_Total = new GameAttributeF(495, 0f, -1, 0, 0, "", "Resistance_Root + Resistance_StunRootFreeze", "Resistance_Root_Total", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeF Resistance_Freeze = new GameAttributeF(496, 0f, -1, 0, 0, "", "", "Resistance_Freeze", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeF Resistance_Freeze_Total = new GameAttributeF(497, 0f, -1, 0, 0, "", "Resistance_Freeze + Resistance_StunRootFreeze", "Resistance_Freeze_Total", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeF Resistance_StunRootFreeze = new GameAttributeF(498, 0f, -1, 0, 0, "", "", "Resistance_StunRootFreeze", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeF CrowdControl_Reduction = new GameAttributeF(499, 0f, -1, 0, 0, "", "", "CrowdControl_Reduction", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeF Fury_Generation_Bonus_Percent = new GameAttributeF(500, 0f, -1, 0, 0, "", "", "Fury_Generation_Bonus_Percent", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeF Spirit_Generation_Bonus_Percent = new GameAttributeF(501, 0f, -1, 0, 0, "", "", "Spirit_Generation_Bonus_Percent", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeB Displays_Team_Effect = new GameAttributeB(502, 0, -1, 1, 1, "", "", "Displays_Team_Effect", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeB Cannot_Be_Added_To_AI_Target_List = new GameAttributeB(503, 0, -1, 1, 1, "", "", "Cannot_Be_Added_To_AI_Target_List", GameAttributeEncoding.IntMinMax, 0, 0, 1, 1);
        public static readonly GameAttributeI SkillKit = new GameAttributeI(504, -1, -1, 3, 1, "", "", "SkillKit", GameAttributeEncoding.Int, 1, 0, 0, 32);
        public static readonly GameAttributeB Immune_To_Charm = new GameAttributeB(505, 0, -1, 1, 1, "", "", "Immune_To_Charm", GameAttributeEncoding.IntMinMax, 0, 0, 1, 1);
        public static readonly GameAttributeB Immune_To_Blind = new GameAttributeB(506, 0, -1, 1, 1, "", "", "Immune_To_Blind", GameAttributeEncoding.IntMinMax, 0, 0, 1, 1);
        public static readonly GameAttributeB Damage_Shield = new GameAttributeB(507, 0, -1, 1, 1, "", "", "Damage_Shield", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeB Silenced = new GameAttributeB(508, 0, -1, 1, 1, "", "", "Silenced", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeB Diseased = new GameAttributeB(509, 0, -1, 1, 1, "", "", "Diseased", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeI Guard_Object_ACDID = new GameAttributeI(510, -1, -1, 3, 1, "", "", "Guard_Object_ACDID", GameAttributeEncoding.Int, 0, 0, 0, 32);
        public static readonly GameAttributeI Follow_Target_ACDID = new GameAttributeI(511, -1, -1, 3, 1, "", "", "Follow_Target_ACDID", GameAttributeEncoding.Int, 0, 0, 0, 32);
        public static readonly GameAttributeI Follow_Target_Type = new GameAttributeI(512, 0, -1, 3, 1, "", "", "Follow_Target_Type", GameAttributeEncoding.IntMinMax, 0, 0, 2, 2);
        public static readonly GameAttributeI Forced_Enemy_ACDID = new GameAttributeI(513, -1, -1, 1, 1, "", "", "Forced_Enemy_ACDID", GameAttributeEncoding.Int, 0, 0, 0, 32);
        public static readonly GameAttributeI NPC_Talk_Target_ANN = new GameAttributeI(514, -1, -1, 3, 1, "", "", "NPC_Talk_Target_ANN", GameAttributeEncoding.Int, 0, 0, 0, 32);
        public static readonly GameAttributeI NPC_Conv_Target_ANN = new GameAttributeI(515, -1, -1, 3, 1, "", "", "NPC_Conv_Target_ANN", GameAttributeEncoding.Int, 0, 0, 0, 32);
        public static readonly GameAttributeI Script_Target_ACDID = new GameAttributeI(516, -1, 3, 3, 1, "", "", "Script_Target_ACDID", GameAttributeEncoding.Int, 0, 0, 0, 32);
        public static readonly GameAttributeI Look_Target_Server_ANN = new GameAttributeI(517, -1, -1, 1, 1, "", "", "Look_Target_Server_ANN", GameAttributeEncoding.Int, 31, 0, 0, 32);
        public static readonly GameAttributeF Look_Target_Broadcast_Intensity = new GameAttributeF(518, 0f, -1, 0, 0, "", "", "Look_Target_Broadcast_Intensity", GameAttributeEncoding.Float16, 31, 0f, 0f, 16);
        public static readonly GameAttributeF Look_Target_Broadcast_Radius = new GameAttributeF(519, 0f, -1, 0, 0, "", "", "Look_Target_Broadcast_Radius", GameAttributeEncoding.Float16, 31, 0f, 0f, 16);
        public static readonly GameAttributeB Stealthed = new GameAttributeB(520, 0, -1, 1, 1, "", "", "Stealthed", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeI GemQuality = new GameAttributeI(521, 0, -1, 4, 1, "", "", "GemQuality", GameAttributeEncoding.IntMinMax, 8, 0, 10, 4);
        public static readonly GameAttributeB SalvageUnlocked = new GameAttributeB(522, 0, -1, 1, 1, "", "", "SalvageUnlocked", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeI TalismanLevel = new GameAttributeI(523, 0, -1, 1, 1, "", "", "TalismanLevel", GameAttributeEncoding.IntMinMax, 31, 0, 4, 3);
        public static readonly GameAttributeI Talisman_Slots = new GameAttributeI(524, 0, -1, 0, 1, "", "", "Talisman_Slots", GameAttributeEncoding.IntMinMax, 31, 0, 9, 4);
        public static readonly GameAttributeI UpgradeLevelA = new GameAttributeI(525, 0, -1, 1, 1, "", "", "UpgradeLevelA", GameAttributeEncoding.IntMinMax, 31, 0, 255, 8);
        public static readonly GameAttributeI UpgradeLevelB = new GameAttributeI(526, 0, -1, 1, 1, "", "", "UpgradeLevelB", GameAttributeEncoding.IntMinMax, 31, 0, 255, 8);
        public static readonly GameAttributeI UpgradeLevelC = new GameAttributeI(527, 0, -1, 1, 1, "", "", "UpgradeLevelC", GameAttributeEncoding.IntMinMax, 31, 0, 255, 8);
        public static readonly GameAttributeI UpgradeLevelD = new GameAttributeI(528, 0, -1, 1, 1, "", "", "UpgradeLevelD", GameAttributeEncoding.IntMinMax, 31, 0, 255, 8);
        public static readonly GameAttributeF ElixirDuration = new GameAttributeF(529, 0f, -1, 1, 0, "", "0", "ElixirDuration", GameAttributeEncoding.Float16, 31, 0f, 0f, 16);
        public static readonly GameAttributeI ItemBuffIcon = new GameAttributeI(530, 0, 4, 1, 1, "", "0", "ItemBuffIcon", GameAttributeEncoding.Int, 31, 0, 0, 32);
        public static readonly GameAttributeF ScrollDuration = new GameAttributeF(531, 0f, -1, 1, 0, "", "0", "ScrollDuration", GameAttributeEncoding.Float16, 0, 0f, 0f, 16);
        public static readonly GameAttributeI Gizmo_Actor_SNO_To_Spawn = new GameAttributeI(532, -1, -1, 3, 1, "", "", "Gizmo_Actor_SNO_To_Spawn", GameAttributeEncoding.Int, 4, 0, 0, 32);
        public static readonly GameAttributeF Gizmo_Actor_To_Spawn_Scale = new GameAttributeF(533, 0f, -1, 1, 0, "", "", "Gizmo_Actor_To_Spawn_Scale", GameAttributeEncoding.Float16, 4, 0f, 0f, 16);
        public static readonly GameAttributeI Death_Replacement_Power_SNO = new GameAttributeI(534, -1, -1, 3, 1, "", "", "Death_Replacement_Power_SNO", GameAttributeEncoding.Int, 0, 0, 0, 32);
        public static readonly GameAttributeB Attachment_Handled_By_Client = new GameAttributeB(535, 0, -1, 1, 1, "", "", "Attachment_Handled_By_Client", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeB AI_In_Special_State = new GameAttributeB(536, 0, -1, 1, 1, "", "", "AI_In_Special_State", GameAttributeEncoding.IntMinMax, 0, 0, 1, 1);
        public static readonly GameAttributeB AI_Used_Scripted_Spawn_Anim = new GameAttributeB(537, 0, -1, 1, 1, "", "", "AI_Used_Scripted_Spawn_Anim", GameAttributeEncoding.IntMinMax, 0, 0, 1, 1);
        public static readonly GameAttributeB AI_Spawned_By_Inactive_Marker = new GameAttributeB(538, 0, -1, 1, 1, "", "", "AI_Spawned_By_Inactive_Marker", GameAttributeEncoding.IntMinMax, 0, 0, 1, 1);
        public static readonly GameAttributeI Headstone_Player_ANN = new GameAttributeI(539, -1, -1, 3, 1, "", "", "Headstone_Player_ANN", GameAttributeEncoding.Int, 31, 0, 0, 32);
        public static readonly GameAttributeF Resource_Cost_Reduction_Percent = new GameAttributeF(540, 0f, 10, 0, 0, "", "", "Resource_Cost_Reduction_Percent", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeF Resistance_Penetration = new GameAttributeF(541, 0f, 0, 0, 0, "", "", "Resistance_Penetration", GameAttributeEncoding.Float16, 0, 0f, 0f, 16);
        public static readonly GameAttributeF Resistance_Penetration_Total = new GameAttributeF(542, 0f, 0, 0, 0, "", "(Resistance_Penetration + Resistance_Penetration_All#NONE) * (Resistance_Penetration_Percent_All#NONE + 1)", "Resistance_Penetration_Total", GameAttributeEncoding.Float16, 0, 0f, 0f, 16);
        public static readonly GameAttributeF Resistance_Penetration_All = new GameAttributeF(543, 0f, -1, 0, 0, "", "", "Resistance_Penetration_All", GameAttributeEncoding.Float16, 0, 0f, 0f, 16);
        public static readonly GameAttributeF Resistance_Penetration_Percent_All = new GameAttributeF(544, 0f, -1, 0, 0, "", "", "Resistance_Penetration_Percent_All", GameAttributeEncoding.Float16, 0, 0f, 0f, 16);
        public static readonly GameAttributeI Fury_Effect_Level = new GameAttributeI(545, 0, -1, 0, 1, "", "", "Fury_Effect_Level", GameAttributeEncoding.Int, 31, 0, 0, 32);
        public static readonly GameAttributeF Health_Potion_Bonus_Heal_Percent = new GameAttributeF(546, 0f, -1, 0, 0, "", "", "Health_Potion_Bonus_Heal_Percent", GameAttributeEncoding.Float16, 0, 0f, 0f, 16);
        public static readonly GameAttributeI Free_Cast = new GameAttributeI(547, 0, 4, 1, 1, "", "", "Free_Cast", GameAttributeEncoding.Int, 9, 0, 0, 32);
        public static readonly GameAttributeB Free_Cast_All = new GameAttributeB(548, 0, -1, 1, 1, "", "", "Free_Cast_All", GameAttributeEncoding.IntMinMax, 9, 0, 1, 1);
        public static readonly GameAttributeF Movement_Scalar_Reduction_Percent = new GameAttributeF(549, 0f, -1, 1, 0, "", "", "Movement_Scalar_Reduction_Percent", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeF Movement_Scalar_Reduction_Resistance = new GameAttributeF(550, 0f, -1, 0, 0, "", "", "Movement_Scalar_Reduction_Resistance", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeF Damage_Absorb_Percent_All = new GameAttributeF(551, 0f, -1, 0, 0, "", "", "Damage_Absorb_Percent_All", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeI World_Seed = new GameAttributeI(552, 0, -1, 0, 1, "", "", "World_Seed", GameAttributeEncoding.Int, 1, 0, 0, 32);
        public static readonly GameAttributeI Kill_Count_Record = new GameAttributeI(553, 0, -1, 1, 1, "", "", "Kill_Count_Record", GameAttributeEncoding.IntMinMax, 0, 0, 16777215, 24);
        public static readonly GameAttributeI Object_Destruction_Record = new GameAttributeI(554, 0, -1, 1, 1, "", "", "Object_Destruction_Record", GameAttributeEncoding.IntMinMax, 0, 0, 16777215, 24);
        public static readonly GameAttributeI Single_Attack_Record = new GameAttributeI(555, 0, -1, 1, 1, "", "", "Single_Attack_Record", GameAttributeEncoding.IntMinMax, 0, 0, 16777215, 24);
        public static readonly GameAttributeI Environment_Attack_Record = new GameAttributeI(556, 0, -1, 1, 1, "", "", "Environment_Attack_Record", GameAttributeEncoding.IntMinMax, 0, 0, 16777215, 24);
        public static readonly GameAttributeB Root_Immune = new GameAttributeB(557, 0, -1, 1, 1, "", "", "Root_Immune", GameAttributeEncoding.IntMinMax, 0, 0, 1, 1);
        public static readonly GameAttributeF Monster_Play_Get_Hit_Bonus = new GameAttributeF(558, 0f, -1, 0, 0, "", "", "Monster_Play_Get_Hit_Bonus", GameAttributeEncoding.Float16Or32, 0, 0f, 0f, 0);
        public static readonly GameAttributeI Stored_Contact_Frame = new GameAttributeI(559, 0, -1, 1, 1, "", "", "Stored_Contact_Frame", GameAttributeEncoding.IntMinMax, 0, 0, 4, 3);
        public static readonly GameAttributeI Buff_Icon_Count0 = new GameAttributeI(560, 0, 4, 0, 1, "", "", "Buff_Icon_Count0", GameAttributeEncoding.IntMinMax, 31, 0, 16777215, 24);
        public static readonly GameAttributeI Buff_Icon_Count1 = new GameAttributeI(561, 0, 4, 0, 1, "", "", "Buff_Icon_Count1", GameAttributeEncoding.IntMinMax, 31, 0, 16777215, 24);
        public static readonly GameAttributeI Buff_Icon_Count2 = new GameAttributeI(562, 0, 4, 0, 1, "", "", "Buff_Icon_Count2", GameAttributeEncoding.IntMinMax, 31, 0, 16777215, 24);
        public static readonly GameAttributeI Buff_Icon_Count3 = new GameAttributeI(563, 0, 4, 0, 1, "", "", "Buff_Icon_Count3", GameAttributeEncoding.IntMinMax, 31, 0, 16777215, 24);
        public static readonly GameAttributeB Observer = new GameAttributeB(564, 0, -1, 1, 1, "", "", "Observer", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeB Resurrect_As_Observer = new GameAttributeB(565, 0, -1, 1, 1, "", "", "Resurrect_As_Observer", GameAttributeEncoding.IntMinMax, 0, 0, 1, 1);
        public static readonly GameAttributeI Combo_Level = new GameAttributeI(566, 0, -1, 1, 1, "", "", "Combo_Level", GameAttributeEncoding.IntMinMax, 31, 0, 3, 2);
        public static readonly GameAttributeI Combo_Time_Last_Move = new GameAttributeI(567, 0, -1, 1, 1, "", "", "Combo_Time_Last_Move", GameAttributeEncoding.Int, 31, 0, 0, 32);
        public static readonly GameAttributeB Burrowed = new GameAttributeB(568, 0, -1, 1, 1, "", "", "Burrowed", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeI Death_Replacement_Effect_Group_SNO = new GameAttributeI(569, -1, -1, 3, 1, "", "", "Death_Replacement_Effect_Group_SNO", GameAttributeEncoding.Int, 0, 0, 0, 32);
        public static readonly GameAttributeI Checkpoint_Resurrection_Allowed_Game_Time = new GameAttributeI(570, 0, -1, 0, 1, "", "", "Checkpoint_Resurrection_Allowed_Game_Time", GameAttributeEncoding.Int, 31, 0, 0, 32);
        public static readonly GameAttributeI Checkpoint_Resurrection_Forced_Game_Time = new GameAttributeI(571, 0, -1, 0, 1, "", "", "Checkpoint_Resurrection_Forced_Game_Time", GameAttributeEncoding.Int, 0, 0, 0, 32);
        public static readonly GameAttributeI Controlling_TimedEvent_SNO = new GameAttributeI(572, -1, -1, 3, 1, "", "", "Controlling_TimedEvent_SNO", GameAttributeEncoding.Int, 0, 0, 0, 32);
        public static readonly GameAttributeF Casting_Speed_Percent = new GameAttributeF(573, 0f, -1, 0, 0, "", "", "Casting_Speed_Percent", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeB Using_Bossbar = new GameAttributeB(574, 0, -1, 1, 1, "", "", "Using_Bossbar", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeB Power_Buff_0_Visual_Effect = new GameAttributeB(575, 0, 4, 1, 1, "", "", "Power_Buff_0_Visual_Effect", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeB Power_Buff_1_Visual_Effect = new GameAttributeB(576, 0, 4, 1, 1, "", "", "Power_Buff_1_Visual_Effect", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeB Power_Buff_2_Visual_Effect = new GameAttributeB(577, 0, 4, 1, 1, "", "", "Power_Buff_2_Visual_Effect", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeB Power_Buff_3_Visual_Effect = new GameAttributeB(578, 0, 4, 1, 1, "", "", "Power_Buff_3_Visual_Effect", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeI Store_SNO = new GameAttributeI(579, 0, 0, 3, 1, "", "", "Store SNO", GameAttributeEncoding.Int, 31, 0, 0, 32);
        public static readonly GameAttributeB Busy = new GameAttributeB(580, 0, -1, 1, 1, "", "", "Busy", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeB Afk = new GameAttributeB(581, 0, -1, 1, 1, "", "", "Afk", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeI Last_Action_Timestamp = new GameAttributeI(582, 0, -1, 1, 1, "", "", "Last Action Timestamp", GameAttributeEncoding.Int, 0, 0, 0, 32);
        public static readonly GameAttributeF Repair_Discount_Percent = new GameAttributeF(583, 0f, -1, 0, 0, "", "", "Repair_Discount_Percent", GameAttributeEncoding.Float16, 31, 0f, 0f, 16);
        public static readonly GameAttributeB Resource_Degeneration_Prevented = new GameAttributeB(584, 0, -1, 1, 1, "", "", "Resource_Degeneration_Prevented", GameAttributeEncoding.IntMinMax, 0, 0, 1, 1);
        public static readonly GameAttributeB Operatable = new GameAttributeB(585, 0, -1, 4, 1, "", "", "Operatable", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeI Look_Override = new GameAttributeI(586, 0, -1, 0, 1, "", "", "Look_Override", GameAttributeEncoding.Int, 31, 0, 0, 32);
        public static readonly GameAttributeI Spawner_Concurrent_Count_ID = new GameAttributeI(587, -1, -1, 3, 1, "", "", "Spawner_Concurrent_Count_ID", GameAttributeEncoding.Int, 0, 0, 0, 32);
        public static readonly GameAttributeB Disabled = new GameAttributeB(588, 0, -1, 1, 1, "", "", "Disabled", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeI Skill_Override = new GameAttributeI(589, -1, 3, 1, 1, "", "", "Skill_Override", GameAttributeEncoding.Int, 31, 0, 0, 32);
        public static readonly GameAttributeB Skill_Override_Active = new GameAttributeB(590, 0, -1, 1, 1, "", "", "Skill_Override_Active", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeB Is_Power_Proxy = new GameAttributeB(591, 0, -1, 1, 1, "", "", "Is_Power_Proxy", GameAttributeEncoding.IntMinMax, 0, 0, 1, 1);
        public static readonly GameAttributeB Force_No_Death_Animation = new GameAttributeB(592, 0, -1, 1, 1, "", "", "Force_No_Death_Animation", GameAttributeEncoding.IntMinMax, 0, 0, 1, 1);
        public static readonly GameAttributeI Player_WeaponClass_Anim_Override = new GameAttributeI(593, -1, -1, 1, 1, "", "", "Player_WeaponClass_Anim_Override", GameAttributeEncoding.IntMinMax, 31, -1, 19, 5);
        public static readonly GameAttributeB Operatable_Story_Gizmo = new GameAttributeB(594, 0, -1, 1, 1, "", "", "Operatable_Story_Gizmo", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeB Power_Buff_0_Visual_Effect_None = new GameAttributeB(595, 0, 4, 1, 1, "", "", "Power_Buff_0_Visual_Effect_None", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeB Power_Buff_0_Visual_Effect_A = new GameAttributeB(596, 0, 4, 1, 1, "", "", "Power_Buff_0_Visual_Effect_A", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeB Power_Buff_0_Visual_Effect_B = new GameAttributeB(597, 0, 4, 1, 1, "", "", "Power_Buff_0_Visual_Effect_B", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeB Power_Buff_0_Visual_Effect_C = new GameAttributeB(598, 0, 4, 1, 1, "", "", "Power_Buff_0_Visual_Effect_C", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeB Power_Buff_0_Visual_Effect_D = new GameAttributeB(599, 0, 4, 1, 1, "", "", "Power_Buff_0_Visual_Effect_D", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeB Power_Buff_0_Visual_Effect_E = new GameAttributeB(600, 0, 4, 1, 1, "", "", "Power_Buff_0_Visual_Effect_E", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeB Power_Buff_1_Visual_Effect_None = new GameAttributeB(601, 0, 4, 1, 1, "", "", "Power_Buff_1_Visual_Effect_None", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeB Power_Buff_1_Visual_Effect_A = new GameAttributeB(602, 0, 4, 1, 1, "", "", "Power_Buff_1_Visual_Effect_A", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeB Power_Buff_1_Visual_Effect_B = new GameAttributeB(603, 0, 4, 1, 1, "", "", "Power_Buff_1_Visual_Effect_B", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeB Power_Buff_1_Visual_Effect_C = new GameAttributeB(604, 0, 4, 1, 1, "", "", "Power_Buff_1_Visual_Effect_C", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeB Power_Buff_1_Visual_Effect_D = new GameAttributeB(605, 0, 4, 1, 1, "", "", "Power_Buff_1_Visual_Effect_D", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeB Power_Buff_1_Visual_Effect_E = new GameAttributeB(606, 0, 4, 1, 1, "", "", "Power_Buff_1_Visual_Effect_E", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeB Power_Buff_2_Visual_Effect_None = new GameAttributeB(607, 0, 4, 1, 1, "", "", "Power_Buff_2_Visual_Effect_None", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeB Power_Buff_2_Visual_Effect_A = new GameAttributeB(608, 0, 4, 1, 1, "", "", "Power_Buff_2_Visual_Effect_A", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeB Power_Buff_2_Visual_Effect_B = new GameAttributeB(609, 0, 4, 1, 1, "", "", "Power_Buff_2_Visual_Effect_B", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeB Power_Buff_2_Visual_Effect_C = new GameAttributeB(610, 0, 4, 1, 1, "", "", "Power_Buff_2_Visual_Effect_C", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeB Power_Buff_2_Visual_Effect_D = new GameAttributeB(611, 0, 4, 1, 1, "", "", "Power_Buff_2_Visual_Effect_D", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeB Power_Buff_2_Visual_Effect_E = new GameAttributeB(612, 0, 4, 1, 1, "", "", "Power_Buff_2_Visual_Effect_E", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeB Power_Buff_3_Visual_Effect_None = new GameAttributeB(613, 0, 4, 1, 1, "", "", "Power_Buff_3_Visual_Effect_None", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeB Power_Buff_3_Visual_Effect_A = new GameAttributeB(614, 0, 4, 1, 1, "", "", "Power_Buff_3_Visual_Effect_A", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeB Power_Buff_3_Visual_Effect_B = new GameAttributeB(615, 0, 4, 1, 1, "", "", "Power_Buff_3_Visual_Effect_B", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeB Power_Buff_3_Visual_Effect_C = new GameAttributeB(616, 0, 4, 1, 1, "", "", "Power_Buff_3_Visual_Effect_C", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeB Power_Buff_3_Visual_Effect_D = new GameAttributeB(617, 0, 4, 1, 1, "", "", "Power_Buff_3_Visual_Effect_D", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeB Power_Buff_3_Visual_Effect_E = new GameAttributeB(618, 0, 4, 1, 1, "", "", "Power_Buff_3_Visual_Effect_E", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeI Walk_Passability_Power_SNO = new GameAttributeI(619, -1, -1, 1, 1, "", "", "Walk_Passability_Power_SNO", GameAttributeEncoding.Int, 31, 0, 0, 32);
        public static readonly GameAttributeI Passability_Power_SNO = new GameAttributeI(620, -1, -1, 1, 1, "", "", "Passability_Power_SNO", GameAttributeEncoding.Int, 31, 0, 0, 32);
        public static readonly GameAttributeI Flippy_ID = new GameAttributeI(621, -1, -1, 3, 1, "", "", "Flippy_ID", GameAttributeEncoding.Int, 31, 0, 0, 32);
        public static readonly GameAttributeI Summoning_Machine_Num_Casters = new GameAttributeI(622, 0, -1, 0, 1, "", "", "Summoning_Machine_Num_Casters", GameAttributeEncoding.IntMinMax, 0, 0, 255, 8);
        public static readonly GameAttributeI Summoning_Machine_Spawn_Count = new GameAttributeI(623, 0, 15, 0, 1, "", "", "Summoning_Machine_Spawn_Count", GameAttributeEncoding.IntMinMax, 0, 0, 4095, 12);
        public static readonly GameAttributeI Summoning_Machine_Next_Spawn_Ticks = new GameAttributeI(624, 0, -1, 0, 1, "", "", "Summoning_Machine_Next_Spawn_Ticks", GameAttributeEncoding.Int, 0, 0, 0, 32);
        public static readonly GameAttributeI Summoning_Machine_Spawn_Team = new GameAttributeI(625, -1, -1, 1, 1, "", "", "Summoning_Machine_Spawn_Team", GameAttributeEncoding.IntMinMax, 0, -1, 23, 5);
        public static readonly GameAttributeF Screen_Attack_Radius_Constant = new GameAttributeF(626, 60f, -1, 3, 0, "", "", "Screen_Attack_Radius_Constant", GameAttributeEncoding.Float16, 0, 0f, 0f, 16);
        public static readonly GameAttributeF Damage_Done_Reduction_Percent = new GameAttributeF(627, 0f, -1, 1, 0, "", "", "Damage_Done_Reduction_Percent", GameAttributeEncoding.Float16, 0, 0f, 0f, 16);
        public static readonly GameAttributeI Set_Item_Count = new GameAttributeI(628, 0, 17, 0, 1, "", "", "Set_Item_Count", GameAttributeEncoding.Int, 31, 0, 0, 32);
        public static readonly GameAttributeF Spawner_Countdown_Percent = new GameAttributeF(629, 0f, -1, 1, 0, "", "", "Spawner_Countdown_Percent", GameAttributeEncoding.Float16, 31, 0f, 0f, 16);
        public static readonly GameAttributeB Attack_Slow = new GameAttributeB(630, 0, -1, 1, 1, "", "", "Attack_Slow", GameAttributeEncoding.IntMinMax, 0, 0, 1, 1);
        public static readonly GameAttributeB Power_Disabled = new GameAttributeB(631, 0, 4, 1, 1, "", "", "Power_Disabled", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeI Weapon_Effect_Override = new GameAttributeI(632, 0, -1, 1, 1, "", "", "Weapon_Effect_Override", GameAttributeEncoding.IntMinMax, 31, 0, 14, 4);
        public static readonly GameAttributeF Debuff_Duration_Reduction_Percent = new GameAttributeF(633, 0f, -1, 0, 0, "", "", "Debuff_Duration_Reduction_Percent", GameAttributeEncoding.Float16, 0, 0f, 0f, 16);
        public static readonly GameAttributeB Uses_PvP_Power_Tags = new GameAttributeB(634, 0, -1, 1, 1, "", "", "Uses_PvP_Power_Tags", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeI Trait = new GameAttributeI(635, 0, 4, 1, 1, "", "", "Trait", GameAttributeEncoding.IntMinMax, 31, -1, 30, 5);
        public static readonly GameAttributeI Last_ACD_Attacked_By = new GameAttributeI(636, -1, -1, 3, 1, "", "", "Last_ACD_Attacked_By", GameAttributeEncoding.Int, 0, 0, 0, 32);
        public static readonly GameAttributeB ItemMeltUnlocked = new GameAttributeB(637, 0, -1, 1, 1, "", "", "ItemMeltUnlocked", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeF Gold_PickUp_Radius = new GameAttributeF(638, 0f, -1, 0, 0, "", "", "Gold_PickUp_Radius", GameAttributeEncoding.Float16, 31, 0f, 0f, 16);
        public static readonly GameAttributeI Client_Only_Effect = new GameAttributeI(639, 0, 12, 1, 1, "", "", "Client Only Effect", GameAttributeEncoding.Int, 31, 0, 0, 32);
        public static readonly GameAttributeB Has_Doppelganger_Cloned = new GameAttributeB(640, 0, -1, 1, 1, "", "", "Has_Doppelganger_Cloned", GameAttributeEncoding.IntMinMax, 0, 0, 1, 1);
        public static readonly GameAttributeI Buff_Icon_Start_Tick4 = new GameAttributeI(641, 0, 4, 1, 1, "", "", "Buff_Icon_Start_Tick4", GameAttributeEncoding.Int, 31, 0, 0, 32);
        public static readonly GameAttributeI Buff_Icon_Start_Tick5 = new GameAttributeI(642, 0, 4, 1, 1, "", "", "Buff_Icon_Start_Tick5", GameAttributeEncoding.Int, 31, 0, 0, 32);
        public static readonly GameAttributeI Buff_Icon_Start_Tick6 = new GameAttributeI(643, 0, 4, 1, 1, "", "", "Buff_Icon_Start_Tick6", GameAttributeEncoding.Int, 31, 0, 0, 32);
        public static readonly GameAttributeI Buff_Icon_Start_Tick7 = new GameAttributeI(644, 0, 4, 1, 1, "", "", "Buff_Icon_Start_Tick7", GameAttributeEncoding.Int, 31, 0, 0, 32);
        public static readonly GameAttributeI Buff_Icon_End_Tick4 = new GameAttributeI(645, 0, 4, 1, 1, "", "", "Buff_Icon_End_Tick4", GameAttributeEncoding.Int, 31, 0, 0, 32);
        public static readonly GameAttributeI Buff_Icon_End_Tick5 = new GameAttributeI(646, 0, 4, 1, 1, "", "", "Buff_Icon_End_Tick5", GameAttributeEncoding.Int, 31, 0, 0, 32);
        public static readonly GameAttributeI Buff_Icon_End_Tick6 = new GameAttributeI(647, 0, 4, 1, 1, "", "", "Buff_Icon_End_Tick6", GameAttributeEncoding.Int, 31, 0, 0, 32);
        public static readonly GameAttributeI Buff_Icon_End_Tick7 = new GameAttributeI(648, 0, 4, 1, 1, "", "", "Buff_Icon_End_Tick7", GameAttributeEncoding.Int, 31, 0, 0, 32);
        public static readonly GameAttributeI Buff_Icon_Count4 = new GameAttributeI(649, 0, 4, 0, 1, "", "", "Buff_Icon_Count4", GameAttributeEncoding.IntMinMax, 31, 0, 16777215, 24);
        public static readonly GameAttributeI Buff_Icon_Count5 = new GameAttributeI(650, 0, 4, 0, 1, "", "", "Buff_Icon_Count5", GameAttributeEncoding.IntMinMax, 31, 0, 16777215, 24);
        public static readonly GameAttributeI Buff_Icon_Count6 = new GameAttributeI(651, 0, 4, 0, 1, "", "", "Buff_Icon_Count6", GameAttributeEncoding.IntMinMax, 31, 0, 16777215, 24);
        public static readonly GameAttributeI Buff_Icon_Count7 = new GameAttributeI(652, 0, 4, 0, 1, "", "", "Buff_Icon_Count7", GameAttributeEncoding.IntMinMax, 31, 0, 16777215, 24);
        public static readonly GameAttributeB Power_Buff_4_Visual_Effect = new GameAttributeB(653, 0, 4, 1, 1, "", "", "Power_Buff_4_Visual_Effect", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeB Power_Buff_5_Visual_Effect = new GameAttributeB(654, 0, 4, 1, 1, "", "", "Power_Buff_5_Visual_Effect", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeB Power_Buff_6_Visual_Effect = new GameAttributeB(655, 0, 4, 1, 1, "", "", "Power_Buff_6_Visual_Effect", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeB Power_Buff_7_Visual_Effect = new GameAttributeB(656, 0, 4, 1, 1, "", "", "Power_Buff_7_Visual_Effect", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeB Power_Buff_4_Visual_Effect_None = new GameAttributeB(657, 0, 4, 1, 1, "", "", "Power_Buff_4_Visual_Effect_None", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeB Power_Buff_4_Visual_Effect_A = new GameAttributeB(658, 0, 4, 1, 1, "", "", "Power_Buff_4_Visual_Effect_A", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeB Power_Buff_4_Visual_Effect_B = new GameAttributeB(659, 0, 4, 1, 1, "", "", "Power_Buff_4_Visual_Effect_B", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeB Power_Buff_4_Visual_Effect_C = new GameAttributeB(660, 0, 4, 1, 1, "", "", "Power_Buff_4_Visual_Effect_C", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeB Power_Buff_4_Visual_Effect_D = new GameAttributeB(661, 0, 4, 1, 1, "", "", "Power_Buff_4_Visual_Effect_D", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeB Power_Buff_4_Visual_Effect_E = new GameAttributeB(662, 0, 4, 1, 1, "", "", "Power_Buff_4_Visual_Effect_E", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeB Power_Buff_5_Visual_Effect_None = new GameAttributeB(663, 0, 4, 1, 1, "", "", "Power_Buff_5_Visual_Effect_None", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeB Power_Buff_5_Visual_Effect_A = new GameAttributeB(664, 0, 4, 1, 1, "", "", "Power_Buff_5_Visual_Effect_A", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeB Power_Buff_5_Visual_Effect_B = new GameAttributeB(665, 0, 4, 1, 1, "", "", "Power_Buff_5_Visual_Effect_B", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeB Power_Buff_5_Visual_Effect_C = new GameAttributeB(666, 0, 4, 1, 1, "", "", "Power_Buff_5_Visual_Effect_C", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeB Power_Buff_5_Visual_Effect_D = new GameAttributeB(667, 0, 4, 1, 1, "", "", "Power_Buff_5_Visual_Effect_D", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeB Power_Buff_5_Visual_Effect_E = new GameAttributeB(668, 0, 4, 1, 1, "", "", "Power_Buff_5_Visual_Effect_E", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeB Power_Buff_6_Visual_Effect_None = new GameAttributeB(669, 0, 4, 1, 1, "", "", "Power_Buff_6_Visual_Effect_None", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeB Power_Buff_6_Visual_Effect_A = new GameAttributeB(670, 0, 4, 1, 1, "", "", "Power_Buff_6_Visual_Effect_A", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeB Power_Buff_6_Visual_Effect_B = new GameAttributeB(671, 0, 4, 1, 1, "", "", "Power_Buff_6_Visual_Effect_B", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeB Power_Buff_6_Visual_Effect_C = new GameAttributeB(672, 0, 4, 1, 1, "", "", "Power_Buff_6_Visual_Effect_C", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeB Power_Buff_6_Visual_Effect_D = new GameAttributeB(673, 0, 4, 1, 1, "", "", "Power_Buff_6_Visual_Effect_D", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeB Power_Buff_6_Visual_Effect_E = new GameAttributeB(674, 0, 4, 1, 1, "", "", "Power_Buff_6_Visual_Effect_E", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeB Power_Buff_7_Visual_Effect_None = new GameAttributeB(675, 0, 4, 1, 1, "", "", "Power_Buff_7_Visual_Effect_None", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeB Power_Buff_7_Visual_Effect_A = new GameAttributeB(676, 0, 4, 1, 1, "", "", "Power_Buff_7_Visual_Effect_A", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeB Power_Buff_7_Visual_Effect_B = new GameAttributeB(677, 0, 4, 1, 1, "", "", "Power_Buff_7_Visual_Effect_B", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeB Power_Buff_7_Visual_Effect_C = new GameAttributeB(678, 0, 4, 1, 1, "", "", "Power_Buff_7_Visual_Effect_C", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeB Power_Buff_7_Visual_Effect_D = new GameAttributeB(679, 0, 4, 1, 1, "", "", "Power_Buff_7_Visual_Effect_D", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeB Power_Buff_7_Visual_Effect_E = new GameAttributeB(680, 0, 4, 1, 1, "", "", "Power_Buff_7_Visual_Effect_E", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeF Resource_Gain_Bonus_Percent = new GameAttributeF(681, 0f, 10, 0, 0, "", "", "Resource_Gain_Bonus_Percent", GameAttributeEncoding.Float16, 9, 0f, 0f, 16);
        public static readonly GameAttributeI Looping_Animation_Start_Time = new GameAttributeI(682, 0, -1, 1, 1, "", "", "Looping_Animation_Start_Time", GameAttributeEncoding.Int, 31, 0, 0, 32);
        public static readonly GameAttributeI Looping_Animation_End_Time = new GameAttributeI(683, 0, -1, 1, 1, "", "", "Looping_Animation_End_Time", GameAttributeEncoding.Int, 31, 0, 0, 32);
        public static readonly GameAttributeI Heal_Effect_Last_Played_Tick = new GameAttributeI(684, -1, -1, 3, 1, "", "", "Heal_Effect_Last_Played_Tick", GameAttributeEncoding.Int, 0, 0, 0, 32);
        public static readonly GameAttributeI PVP_Kills = new GameAttributeI(685, 0, -1, 0, 1, "", "", "PVP_Kills", GameAttributeEncoding.Int, 1, 0, 0, 32);
        public static readonly GameAttributeI PVP_Deaths = new GameAttributeI(686, 0, -1, 0, 1, "", "", "PVP_Deaths", GameAttributeEncoding.Int, 1, 0, 0, 32);
        public static readonly GameAttributeI PVP_Assists = new GameAttributeI(687, 0, -1, 0, 1, "", "", "PVP_Assists", GameAttributeEncoding.Int, 1, 0, 0, 32);
        public static readonly GameAttributeI PVP_Progression_Points_Gained = new GameAttributeI(688, 0, -1, 0, 1, "", "", "PVP_Progression_Points_Gained", GameAttributeEncoding.Int, 1, 0, 0, 32);
        public static readonly GameAttributeI PVP_Current_Kill_Streak = new GameAttributeI(689, 0, -1, 0, 1, "", "", "PVP_Current_Kill_Streak", GameAttributeEncoding.Int, 1, 0, 0, 32);
        public static readonly GameAttributeI PVP_Current_Death_Streak = new GameAttributeI(690, 0, -1, 0, 1, "", "", "PVP_Current_Death_Streak", GameAttributeEncoding.Int, 1, 0, 0, 32);
        public static readonly GameAttributeI PVP_Longest_Kill_Streak = new GameAttributeI(691, 0, -1, 0, 1, "", "", "PVP_Longest_Kill_Streak", GameAttributeEncoding.Int, 1, 0, 0, 32);
        public static readonly GameAttributeI PVP_Longest_Death_Streak = new GameAttributeI(692, 0, -1, 0, 1, "", "", "PVP_Longest_Death_Streak", GameAttributeEncoding.Int, 1, 0, 0, 32);
        public static readonly GameAttributeF Turn_Rate_Scalar = new GameAttributeF(693, 1f, -1, 5, 0, "", "", "Turn_Rate_Scalar", GameAttributeEncoding.Float16, 31, 0f, 0f, 16);
        public static readonly GameAttributeF Turn_Accel_Scalar = new GameAttributeF(694, 1f, -1, 5, 0, "", "", "Turn_Accel_Scalar", GameAttributeEncoding.Float16, 31, 0f, 0f, 16);
        public static readonly GameAttributeF Turn_Deccel_Scalar = new GameAttributeF(695, 1f, -1, 5, 0, "", "", "Turn_Deccel_Scalar", GameAttributeEncoding.Float16, 31, 0f, 0f, 16);
        public static readonly GameAttributeB No_Health_Drop = new GameAttributeB(696, 0, -1, 1, 1, "", "", "No_Health_Drop", GameAttributeEncoding.IntMinMax, 0, 0, 1, 1);
        public static readonly GameAttributeB Leader = new GameAttributeB(697, 0, -1, 1, 1, "", "", "Leader", GameAttributeEncoding.IntMinMax, 0, 0, 1, 1);
        public static readonly GameAttributeB IsTrialActor = new GameAttributeB(698, 0, -1, 1, 1, "", "", "IsTrialActor", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeB InBossEncounter = new GameAttributeB(699, 0, -1, 1, 1, "", "", "InBossEncounter", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeB MinimapActive = new GameAttributeB(700, 0, -1, 1, 1, "", "", "MinimapActive", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeI MinimapIconOverride = new GameAttributeI(701, -1, -1, 0, 1, "", "", "MinimapIconOverride", GameAttributeEncoding.Int, 31, 0, 0, 32);
        public static readonly GameAttributeI Last_Blocked_ACD = new GameAttributeI(702, -1, -1, 3, 1, "", "", "Last_Blocked_ACD", GameAttributeEncoding.Int, 0, 0, 0, 32);
        public static readonly GameAttributeI Last_Blocked_Time = new GameAttributeI(703, 0, -1, 1, 1, "", "", "Last_Blocked_Time", GameAttributeEncoding.Int, 0, 0, 0, 32);
        public static readonly GameAttributeB Deactivate_Lure = new GameAttributeB(704, 0, -1, 1, 1, "", "", "Deactivate Lure", GameAttributeEncoding.IntMinMax, 0, 0, 1, 1);
        public static readonly GameAttributeB Weapons_Hidden = new GameAttributeB(705, 0, -1, 1, 1, "", "", "Weapons_Hidden", GameAttributeEncoding.IntMinMax, 31, 0, 1, 1);
        public static readonly GameAttributeB Actor_Updates_Attributes_From_Owner = new GameAttributeB(706, 0, -1, 1, 1, "", "", "Actor_Updates_Attributes_From_Owner", GameAttributeEncoding.IntMinMax, 0, 0, 1, 1);
        public static readonly GameAttributeI Taunt_Target_ACD = new GameAttributeI(707, -1, -1, 1, 1, "", "", "Taunt_Target_ACD", GameAttributeEncoding.Int, 0, 0, 0, 32);
        public static readonly GameAttributeF UI_Only_Percent_Damage_Increase = new GameAttributeF(708, 0f, -1, 0, 0, "", "", "UI_Only_Percent_Damage_Increase", GameAttributeEncoding.Float16, 1, 0f, 0f, 16);

        public static readonly GameAttribute[] Attributes = new GameAttribute[] {
            Axe_Bad_Data,
            Attribute_Timer,
            Attribute_Pool,
            Death_Count,
            DualWield_Hand,
            DualWield_Hand_Next,
            Respawn_Game_Time,
            Backpack_Slots,
            Shared_Stash_Slots,
            Attack,
            Precision,
            Vitality,
            Defense,
            Attack_Bonus,
            Precision_Bonus,
            Vitality_Bonus,
            Defense_Bonus,
            Attack_Bonus_Percent,
            Precision_Bonus_Percent,
            Vitality_Bonus_Percent,
            Defense_Bonus_Percent,
            Attack_Reduction_Percent,
            Precision_Reduction_Percent,
            Vitality_Reduction_Percent,
            Defense_Reduction_Percent,
            Armor,
            Armor_Bonus_Percent,
            Armor_Item,
            Armor_Bonus_Item,
            Armor_Item_Percent,
            Armor_Item_SubTotal,
            Armor_Item_Total,
            Armor_Total,
            Experience_Granted,
            Experience_Next,
            Gold_Granted,
            Gold,
            Gold_Find,
            Level,
            Level_Cap,
            Magic_Find,
            Treasure_Find,
            Resource_Cost_Reduction_Amount,
            Resource_Cost_Reduction_Total,
            Resource_Set_Point_Bonus,
            Faster_Healing_Percent,
            Spending_Resource_Heals_Percent,
            Bonus_Healing_Received_Percent,
            Reduced_Healing_Received_Percent,
            Experience_Bonus,
            Experience_Bonus_Percent,
            Health_Globe_Bonus_Chance,
            Health_Globe_Bonus_Mult_Chance,
            Health_Globe_Bonus_Health,
            Increased_Health_From_Globes_Percent,
            Increased_Health_From_Globes_Percent_Total,
            Bonus_Health_Percent_Per_Second_From_Globes,
            Bonus_Health_Percent_Per_Second_From_Globes_Total,
            Mana_Gained_From_Globes_Percent,
            Mana_Gained_From_Globes,
            Resistance,
            Resistance_Percent,
            Resistance_Total,
            Resistance_All,
            Resistance_Percent_All,
            Skill,
            Skill_Total,
            TeamID,
            Team_Override,
            Invulnerable,
            Loading,
            Loading_Player_ACD,
            Loading_Power_SNO,
            Loading_Anim_Tag,
            No_Damage,
            No_AutoPickup,
            Light_Radius_Percent_Bonus,
            Hitpoints_Cur,
            Hitpoints_Factor_Level,
            Hitpoints_Factor_Vitality,
            Hitpoints_Total_From_Vitality,
            Hitpoints_Total_From_Level,
            Hitpoints_Granted,
            Hitpoints_Granted_Duration,
            Hitpoints_Max,
            Hitpoints_Max_Bonus,
            Hitpoints_Max_Total,
            Hitpoints_Percent,
            Hitpoints_Regen_Per_Second,
            Hitpoints_Max_Percent_Bonus,
            Hitpoints_Max_Percent_Bonus_Item,
            Hitpoints_Healed_Target,
            Resource_Type_Primary,
            Resource_Type_Secondary,
            Resource_Cur,
            Resource_Max,
            Resource_Max_Bonus,
            Resource_Max_Total,
            Resource_Factor_Level,
            Resource_Granted,
            Resource_Granted_Duration,
            Resource_Percent,
            Resource_Regen_Per_Second,
            Resource_Regen_Bonus_Percent,
            Resource_Regen_Total,
            Resource_Max_Percent_Bonus,
            Resource_Capacity_Used,
            Resource_Effective_Max,
            Resource_Regen_Percent_Per_Second,
            Resource_Degeneration_Stop_Point,
            Movement_Scalar,
            Walking_Rate,
            Running_Rate,
            Sprinting_Rate,
            Strafing_Rate,
            Walking_Rate_Total,
            Running_Rate_Total,
            Sprinting_Rate_Total,
            Strafing_Rate_Total,
            Movement_Bonus_Total,
            Movement_Scalar_Subtotal,
            Movement_Scalar_Capped_Total,
            Movement_Scalar_Uncapped_Bonus,
            Movement_Scalar_Total,
            Movement_Bonus_Run_Speed,
            Casting_Speed,
            Casting_Speed_Bonus,
            Casting_Speed_Total,
            Always_Hits,
            Hit_Chance,
            Attacks_Per_Second_Item,
            Attacks_Per_Second_Item_Percent,
            Attacks_Per_Second_Item_Subtotal,
            Attacks_Per_Second_Item_Bonus,
            Attacks_Per_Second_Item_Total,
            Attacks_Per_Second,
            Attacks_Per_Second_Bonus,
            Attacks_Per_Second_Total,
            Attacks_Per_Second_Percent,
            AI_Cooldown_Reduction_Percent,
            Power_Cooldown_Reduction_Percent,
            Damage_Delta,
            Damage_Delta_Total,
            Damage_Min,
            Damage_Bonus_Min,
            Damage_Min_Total,
            Damage_Min_Subtotal,
            Damage_Percent_All_From_Skills,
            Damage_Weapon_Delta,
            Damage_Weapon_Delta_SubTotal,
            Damage_Weapon_Max,
            Damage_Weapon_Max_Total,
            Damage_Weapon_Delta_Total,
            Damage_Weapon_Delta_Total_All,
            Damage_Weapon_Bonus_Delta,
            Damage_Weapon_Min,
            Damage_Weapon_Min_Total,
            Damage_Weapon_Min_Total_All,
            Damage_Weapon_Bonus_Min,
            Damage_Weapon_Percent_Bonus,
            Damage_Weapon_Percent_All,
            Damage_Weapon_Percent_Total,
            Damage_Type_Percent_Bonus,
            Damage_Percent_Bonus_Witchdoctor,
            Damage_Percent_Bonus_Wizard,
            Crit_Percent_Base,
            Crit_Percent_Bonus_Capped,
            Crit_Percent_Bonus_Uncapped,
            Crit_Percent_Cap,
            Crit_Damage_Percent,
            Crit_Effect_Time,
            Pierce_Chance,
            Damage_Absorb_Percent,
            Damage_Reduction_Total,
            Damage_Reduction_Current,
            Damage_Reduction_Last_Tick,
            Block_Chance,
            Block_Chance_Total,
            Block_Chance_Bonus_Item,
            Block_Chance_Item,
            Block_Chance_Item_Total,
            Block_Amount,
            Block_Amount_Bonus_Percent,
            Block_Amount_Total_Min,
            Block_Amount_Total_Max,
            Block_Amount_Item_Min,
            Block_Amount_Item_Delta,
            Block_Amount_Item_Bonus,
            Dodge_Rating_Base,
            Dodge_Rating,
            Dodge_Rating_Total,
            Dodge_Chance_Bonus,
            Dodge_Chance_Bonus_Melee,
            Dodge_Chance_Bonus_Ranged,
            Get_Hit_Current,
            Get_Hit_Max_Base,
            Get_Hit_Max_Per_Level,
            Get_Hit_Max,
            Get_Hit_Recovery_Base,
            Get_Hit_Recovery_Per_Level,
            Get_Hit_Recovery,
            Get_Hit_Damage,
            Get_Hit_Damage_Scalar,
            Proc_On_Death,
            Proc_On_Attack,
            Proc_On_Hit,
            Proc_On_Critical,
            Proc_On_Block,
            Proc_On_Slay,
            Proc_On_Spawn,
            Last_Damage_MainActor,
            Last_ACD_Attacked,
            Ignores_Critical_Hits,
            Immunity,
            Untargetable,
            Immobolize,
            Immune_To_Knockback,
            Power_Immobilize,
            Stun_Chance,
            Stun_Length,
            Stun_Recovery,
            Stun_Recovery_Speed,
            Stunned,
            Stun_Immune,
            Poison_Length_Reduction,
            Poisoned,
            Bleeding,
            Bleed_Duration,
            Chilled,
            Freeze_Length_Reduction,
            Freeze_Immune,
            Webbed,
            Slow,
            FireAura,
            LightningAura,
            ColdAura,
            PoisonAura,
            Blind,
            Enraged,
            Slowdown_Immune,
            Gethit_Immune,
            Suffocation_Per_Second,
            Suffocation_Unit_Value,
            Thorns_Percent,
            Thorns_Percent_All,
            Thorns_Percent_Total,
            Thorns_Fixed,
            Steal_Health_Percent,
            Steal_Mana_Percent,
            Resource_On_Hit,
            Resource_On_Kill,
            Resource_On_Crit,
            Hitpoints_On_Hit,
            Hitpoints_On_Kill,
            Damage_To_Mana,
            Last_Proc_Time,
            Damage_Power_Delta,
            Damage_Power_Min,
            Rope_Overlay,
            General_Cooldown,
            Power_Cooldown,
            Power_Cooldown_Start,
            Proc_Cooldown,
            Emote_Cooldown,
            Projectile_Speed,
            Projectile_Speed_Increase_Percent,
            Destroy_When_Path_Blocked,
            Skill_Toggled_State,
            Act,
            Difficulty,
            Last_Damage_Amount,
            In_Knockback,
            Amplify_Damage_Type_Percent,
            Amplify_Damage_Percent,
            Durability_Cur,
            Durability_Max,
            Durability_Last_Damage,
            Item_Quality_Level,
            Item_Cost_Percent_Bonus,
            Item_Equipped,
            Requirement,
            Requirements_Ease_Percent,
            Requirement_When_Equipped,
            Sockets,
            Sockets_Filled,
            Stats_All_Bonus,
            Item_Bound_To_ACD,
            Item_Binding_Level_Override,
            ItemStackQuantityHi,
            ItemStackQuantityLo,
            Run_Speed_Granted,
            Run_Speed_Duration,
            IdentifyCost,
            Seed,
            IsCrafted,
            DyeType,
            SocketAffix,
            EnchantAffix,
            HighlySalvageable,
            Always_Plays_GetHit,
            Hidden,
            RActor_Fade_Group,
            Quest_Range,
            Attack_Cooldown_Min,
            Attack_Cooldown_Delta,
            InitialCooldownMinTotal,
            InitialCooldownDeltaTotal,
            Attack_Cooldown_Min_Total,
            Attack_Cooldown_Delta_Total,
            Closing_Cooldown_Min_Total,
            Closing_Cooldown_Delta_Total,
            Quest_Monster,
            Quest_Monster_Effect,
            Treasure_Class,
            Removes_Body_On_Death,
            InitialCooldownMin,
            InitialCooldownDelta,
            Knockback_Weight,
            UntargetableByPets,
            Damage_State_Current,
            Damage_State_Max,
            Is_Player_Decoy,
            Custom_Target_Weight,
            Gizmo_State,
            Gizmo_Charges,
            Chest_Open,
            Door_Locked,
            Door_Timer,
            Gizmo_Disabled_By_Script,
            Gizmo_Operator_ACDID,
            Triggering_Count,
            Gate_Position,
            Gate_Velocity,
            Gizmo_Has_Been_Operated,
            Pet_Owner,
            Pet_Creator,
            Pet_Type,
            DropsNoLoot,
            GrantsNoXP,
            Hireling_Class,
            Summoned_By_SNO,
            Is_NPC,
            NPC_Is_Operatable,
            NPC_Is_Escorting,
            NPC_Has_Interact_Options,
            Conversation_Icon,
            Callout_Cooldown,
            Banter_Cooldown,
            Conversation_Heard_Count,
            Last_Tick_Shop_Entered,
            Is_Helper,
            Axe,
            Axe2H,
            ThrowingAxe,
            AxeAny,
            Bow,
            Crossbow,
            BowAny,
            Club,
            Club2H,
            ClubAny,
            Dagger,
            Mace,
            Mace2H,
            MaceAny,
            Sword,
            Sword2H,
            SwordAny,
            Polearm,
            Spear,
            Wand,
            ColdStaff,
            FireStaff,
            LightningStaff,
            PoisonStaff,
            StaffAny,
            Weapon1H,
            Weapon2H,
            WeaponMelee,
            WeaponRanged,
            Quiver,
            Reincarnation_Buff,
            Dead_Body_AnimTag,
            Spawned_by_ACDID,
            Summoned_By_ACDID,
            Summoner_ID,
            Banner_ACDID,
            Breakable_Shield_HP,
            Current_WeaponClass,
            Weapons_Sheathed,
            Held_In_OffHand,
            Attacks_Per_Second_Item_MainHand,
            Attacks_Per_Second_Item_OffHand,
            Attacks_Per_Second_Item_Total_MainHand,
            Attacks_Per_Second_Item_Total_OffHand,
            Damage_Weapon_Min_Total_MainHand,
            Damage_Weapon_Min_Total_OffHand,
            Damage_Weapon_Delta_Total_MainHand,
            Damage_Weapon_Delta_Total_OffHand,
            Attacks_Per_Second_Item_CurrentHand,
            Damage_Weapon_Min_Total_CurrentHand,
            Damage_Weapon_Delta_Total_CurrentHand,
            Has_Special_Death_AnimTag,
            Death_Type_Override,
            In_Combat,
            In_Conversation,
            Last_Tick_Potion_Used,
            Potion_Dilution_Percent,
            Out_Of_Combat_Health_Regen_Percent,
            Out_Of_Combat_Mana_Regen_Percent,
            Potion_Dilution_Duration,
            Potion_Dilution_Scalar,
            Feared,
            Fear_Immune,
            Last_Damage_ACD,
            Attached_To_ACD,
            Attachment_ACD,
            Normal_Attack_Replacement_Power_SNO,
            Damage_Type_Override,
            Minion_Count_Bonus_Percent,
            Champion_Teleport_Next_Tick,
            Champion_Teleport_Time_Min_In_Seconds,
            Champion_Teleport_Time_Delta_In_Seconds,
            Champion_Clone_Next_Tick,
            Champion_Clone_Time_Min_In_Seconds,
            Champion_Clone_Time_Delta_In_Seconds,
            Champion_Clone_Hitpoint_Bonus_Percent,
            Champion_Clone_Damage_Bonus_Percent,
            Champion_Ghostly_Next_Tick,
            Champion_Ghostly_Inactive_Time_Min_In_Seconds,
            Champion_Ghostly_Inactive_Time_Delta_In_Seconds,
            Champion_Ghostly_Active_Time_Min_In_Seconds,
            Champion_Ghostly_Active_Time_Delta_In_Seconds,
            Champion_Ghostly_Saved_Dodge_Chance,
            Champion_Ghostly,
            Base_Element,
            Projectile_Amount_Bonus_Percent,
            Projectile_Reflect_Chance,
            Attack_Fear_Chance,
            Attack_Fear_Time_Min,
            Attack_Fear_Time_Delta,
            Buff_Visual_Effect,
            Buff_Icon_Start_Tick0,
            Buff_Icon_Start_Tick1,
            Buff_Icon_Start_Tick2,
            Buff_Icon_Start_Tick3,
            Buff_Icon_End_Tick0,
            Buff_Icon_End_Tick1,
            Buff_Icon_End_Tick2,
            Buff_Icon_End_Tick3,
            Could_Have_Ragdolled,
            Ambient_Damage_Effect_Last_Time,
            Scale_Bonus,
            Deleted_On_Server,
            Does_No_Damage,
            Does_Fake_Damage,
            SlowTime_Debuff,
            Blocks_Projectiles,
            Frozen,
            Freeze_Damage_Percent_Bonus,
            Buff_Active,
            DualWield_BothAttack_Chance,
            Summon_Expiration_Tick,
            Summon_Count,
            Uninterruptible,
            Queue_Death,
            CantStartDisplayedPowers,
            Wizard_Slowtime_Proxy_ACD,
            DPS,
            Resurrection_Power,
            Freeze_Damage,
            Freeze_Capacity,
            Thaw_Rate,
            Chilled_Dur_Bonus_Percent,
            DOT_DPS,
            DamageCap_Percent,
            Item_Time_Sold,
            Forced_Hireling_Power,
            IsRooted,
            RootTargetACD,
            RootAutoDecayPerSecond,
            RootUnitValue,
            RootTotalTicks,
            Hide_Affixes,
            Skill_Socket_Bonus,
            Rune_Rank,
            Rune_Attuned_Power,
            Rune_A,
            Rune_B,
            Rune_C,
            Rune_D,
            Rune_E,
            Resistance_Stun,
            Resistance_Stun_Total,
            Resistance_Root,
            Resistance_Root_Total,
            Resistance_Freeze,
            Resistance_Freeze_Total,
            Resistance_StunRootFreeze,
            CrowdControl_Reduction,
            Fury_Generation_Bonus_Percent,
            Spirit_Generation_Bonus_Percent,
            Displays_Team_Effect,
            Cannot_Be_Added_To_AI_Target_List,
            SkillKit,
            Immune_To_Charm,
            Immune_To_Blind,
            Damage_Shield,
            Silenced,
            Diseased,
            Guard_Object_ACDID,
            Follow_Target_ACDID,
            Follow_Target_Type,
            Forced_Enemy_ACDID,
            NPC_Talk_Target_ANN,
            NPC_Conv_Target_ANN,
            Script_Target_ACDID,
            Look_Target_Server_ANN,
            Look_Target_Broadcast_Intensity,
            Look_Target_Broadcast_Radius,
            Stealthed,
            GemQuality,
            SalvageUnlocked,
            TalismanLevel,
            Talisman_Slots,
            UpgradeLevelA,
            UpgradeLevelB,
            UpgradeLevelC,
            UpgradeLevelD,
            ElixirDuration,
            ItemBuffIcon,
            ScrollDuration,
            Gizmo_Actor_SNO_To_Spawn,
            Gizmo_Actor_To_Spawn_Scale,
            Death_Replacement_Power_SNO,
            Attachment_Handled_By_Client,
            AI_In_Special_State,
            AI_Used_Scripted_Spawn_Anim,
            AI_Spawned_By_Inactive_Marker,
            Headstone_Player_ANN,
            Resource_Cost_Reduction_Percent,
            Resistance_Penetration,
            Resistance_Penetration_Total,
            Resistance_Penetration_All,
            Resistance_Penetration_Percent_All,
            Fury_Effect_Level,
            Health_Potion_Bonus_Heal_Percent,
            Free_Cast,
            Free_Cast_All,
            Movement_Scalar_Reduction_Percent,
            Movement_Scalar_Reduction_Resistance,
            Damage_Absorb_Percent_All,
            World_Seed,
            Kill_Count_Record,
            Object_Destruction_Record,
            Single_Attack_Record,
            Environment_Attack_Record,
            Root_Immune,
            Monster_Play_Get_Hit_Bonus,
            Stored_Contact_Frame,
            Buff_Icon_Count0,
            Buff_Icon_Count1,
            Buff_Icon_Count2,
            Buff_Icon_Count3,
            Observer,
            Resurrect_As_Observer,
            Combo_Level,
            Combo_Time_Last_Move,
            Burrowed,
            Death_Replacement_Effect_Group_SNO,
            Checkpoint_Resurrection_Allowed_Game_Time,
            Checkpoint_Resurrection_Forced_Game_Time,
            Controlling_TimedEvent_SNO,
            Casting_Speed_Percent,
            Using_Bossbar,
            Power_Buff_0_Visual_Effect,
            Power_Buff_1_Visual_Effect,
            Power_Buff_2_Visual_Effect,
            Power_Buff_3_Visual_Effect,
            Store_SNO,
            Busy,
            Afk,
            Last_Action_Timestamp,
            Repair_Discount_Percent,
            Resource_Degeneration_Prevented,
            Operatable,
            Look_Override,
            Spawner_Concurrent_Count_ID,
            Disabled,
            Skill_Override,
            Skill_Override_Active,
            Is_Power_Proxy,
            Force_No_Death_Animation,
            Player_WeaponClass_Anim_Override,
            Operatable_Story_Gizmo,
            Power_Buff_0_Visual_Effect_None,
            Power_Buff_0_Visual_Effect_A,
            Power_Buff_0_Visual_Effect_B,
            Power_Buff_0_Visual_Effect_C,
            Power_Buff_0_Visual_Effect_D,
            Power_Buff_0_Visual_Effect_E,
            Power_Buff_1_Visual_Effect_None,
            Power_Buff_1_Visual_Effect_A,
            Power_Buff_1_Visual_Effect_B,
            Power_Buff_1_Visual_Effect_C,
            Power_Buff_1_Visual_Effect_D,
            Power_Buff_1_Visual_Effect_E,
            Power_Buff_2_Visual_Effect_None,
            Power_Buff_2_Visual_Effect_A,
            Power_Buff_2_Visual_Effect_B,
            Power_Buff_2_Visual_Effect_C,
            Power_Buff_2_Visual_Effect_D,
            Power_Buff_2_Visual_Effect_E,
            Power_Buff_3_Visual_Effect_None,
            Power_Buff_3_Visual_Effect_A,
            Power_Buff_3_Visual_Effect_B,
            Power_Buff_3_Visual_Effect_C,
            Power_Buff_3_Visual_Effect_D,
            Power_Buff_3_Visual_Effect_E,
            Walk_Passability_Power_SNO,
            Passability_Power_SNO,
            Flippy_ID,
            Summoning_Machine_Num_Casters,
            Summoning_Machine_Spawn_Count,
            Summoning_Machine_Next_Spawn_Ticks,
            Summoning_Machine_Spawn_Team,
            Screen_Attack_Radius_Constant,
            Damage_Done_Reduction_Percent,
            Set_Item_Count,
            Spawner_Countdown_Percent,
            Attack_Slow,
            Power_Disabled,
            Weapon_Effect_Override,
            Debuff_Duration_Reduction_Percent,
            Uses_PvP_Power_Tags,
            Trait,
            Last_ACD_Attacked_By,
            ItemMeltUnlocked,
            Gold_PickUp_Radius,
            Client_Only_Effect,
            Has_Doppelganger_Cloned,
            Buff_Icon_Start_Tick4,
            Buff_Icon_Start_Tick5,
            Buff_Icon_Start_Tick6,
            Buff_Icon_Start_Tick7,
            Buff_Icon_End_Tick4,
            Buff_Icon_End_Tick5,
            Buff_Icon_End_Tick6,
            Buff_Icon_End_Tick7,
            Buff_Icon_Count4,
            Buff_Icon_Count5,
            Buff_Icon_Count6,
            Buff_Icon_Count7,
            Power_Buff_4_Visual_Effect,
            Power_Buff_5_Visual_Effect,
            Power_Buff_6_Visual_Effect,
            Power_Buff_7_Visual_Effect,
            Power_Buff_4_Visual_Effect_None,
            Power_Buff_4_Visual_Effect_A,
            Power_Buff_4_Visual_Effect_B,
            Power_Buff_4_Visual_Effect_C,
            Power_Buff_4_Visual_Effect_D,
            Power_Buff_4_Visual_Effect_E,
            Power_Buff_5_Visual_Effect_None,
            Power_Buff_5_Visual_Effect_A,
            Power_Buff_5_Visual_Effect_B,
            Power_Buff_5_Visual_Effect_C,
            Power_Buff_5_Visual_Effect_D,
            Power_Buff_5_Visual_Effect_E,
            Power_Buff_6_Visual_Effect_None,
            Power_Buff_6_Visual_Effect_A,
            Power_Buff_6_Visual_Effect_B,
            Power_Buff_6_Visual_Effect_C,
            Power_Buff_6_Visual_Effect_D,
            Power_Buff_6_Visual_Effect_E,
            Power_Buff_7_Visual_Effect_None,
            Power_Buff_7_Visual_Effect_A,
            Power_Buff_7_Visual_Effect_B,
            Power_Buff_7_Visual_Effect_C,
            Power_Buff_7_Visual_Effect_D,
            Power_Buff_7_Visual_Effect_E,
            Resource_Gain_Bonus_Percent,
            Looping_Animation_Start_Time,
            Looping_Animation_End_Time,
            Heal_Effect_Last_Played_Tick,
            PVP_Kills,
            PVP_Deaths,
            PVP_Assists,
            PVP_Progression_Points_Gained,
            PVP_Current_Kill_Streak,
            PVP_Current_Death_Streak,
            PVP_Longest_Kill_Streak,
            PVP_Longest_Death_Streak,
            Turn_Rate_Scalar,
            Turn_Accel_Scalar,
            Turn_Deccel_Scalar,
            No_Health_Drop,
            Leader,
            IsTrialActor,
            InBossEncounter,
            MinimapActive,
            MinimapIconOverride,
            Last_Blocked_ACD,
            Last_Blocked_Time,
            Deactivate_Lure,
            Weapons_Hidden,
            Actor_Updates_Attributes_From_Owner,
            Taunt_Target_ACD,
            UI_Only_Percent_Damage_Increase,
        };
    }
}
