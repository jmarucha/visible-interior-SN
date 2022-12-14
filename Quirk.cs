using UnityEngine;
using System.Collections.Generic;


namespace VisibleLockerInterior {
    internal class PrefabId {
        public const string AirBladder = "d98d673a-8992-4486-a49c-81aa058e51dc";
        public const string Battery = "d4bfebc0-a5e6-47d3-b4a7-d5e47f614ed6";
        public const string builder = "c6f3c2fd-5b80-4aaf-81c3-f056651b868c";
        public const string Constructor = "dd0298c1-49c2-44a0-8b32-da98e12228fb";
        public const string CoralChunk = "3cf87a6b-261c-47b9-aae7-e011eaf78c30";
        public const string coral_reef_blood_mushrooms_01_04 =
            "a6dac068-6f8d-4e32-b5e7-2e34a9f97d11";
        public const string Coral_reef_purple_mushrooms_01_04 =
            "99cdec62-302b-4999-ba49-f50c73575a4d";
        public const string Flashlight = "12c95e66-fb54-47b3-87f1-8e318394b839";
        public const string Gravsphere = "d11dfcc3-bce7-4870-a112-65a5dab5141b";
        public const string JeweledDiskPiece = "b447ac10-bf6b-4005-8ffc-8d9a4e84450b";
        public const string kyanite = "6e7f3d62-7e76-4415-af64-5dcd88fc3fe4";
        public const string LaserCutter = "d4aa649b-7508-44e4-89fb-29334f12a64e";
        public const string Pipe = "08078333-1a00-42f8-8492-e2640c17a961";
        public const string PlasteelTank = "7835815a-da3b-474a-8585-8716c637bae6";
        public const string PrecursorIonBattery = "811c128d-a85f-4b0a-b9c4-4071db4fb7aa";
        public const string RadiationGloves = "fa9b3999-d201-47fc-a8c5-e7c4030b6b60";
        public const string RadiationHelmet = "22001838-381d-492d-9f02-aa1233f5a55d";
        public const string RadiationSuit = "e862224b-3da8-41bd-809e-6d26ae557ea5";
        public const string rebreather = "1ed1fcc0-b5db-4518-bfde-317e672bb339";
        public const string ReinforcedDiveSuit = "23b590fa-f4ac-4eba-a2b7-e10f8c439d07";
        public const string ReinforcedGloves = "6cdcb6ff-94ac-4b98-820f-54047f5482b3";
        public const string salt = "f654e870-3101-4ff3-8bb4-528dceef43a5";
        public const string SeaGlide = "422b14d3-69c6-43c9-8ceb-84d29f5c3a8b";
        public const string SmallStorage = "9d9ed0b0-df64-45ee-9b90-34386a98b233";
        public const string StalkerTooth = "3f9ee5c6-ac92-43ec-9ae0-b1e6b263e7e2";
        public const string stillSuit = "b0c57af4-d3fb-459b-b67c-2f537f781f1e";
        public const string Tank = "25522394-6c49-470d-8f48-62f09478aa82";
        public const string Welder = "9ef36033-b60c-4f8b-8c3a-b15035de3116";
    }

    internal class Quirk {
        public static Quaternion GetIdealRotationByTechType(TechType techType) {
            switch (techType) {
                case TechType.PipeSurfaceFloater:
                case TechType.LEDLight:
                    return new Quaternion(1, 0, 0, 0);
                case TechType.WiringKit:
                case TechType.AdvancedWiringKit:
                case TechType.Seaglide:
                case TechType.Cap1:
                case TechType.Cap2:
                case TechType.WhirlpoolTorpedo:
                case TechType.GasTorpedo:
                    return new Quaternion(0, 1, 0, 0);
                case TechType.PowerUpgradeModule:
                case TechType.CyclopsHullModule1:
                case TechType.CyclopsHullModule2:
                case TechType.CyclopsShieldModule:
                case TechType.CyclopsSonarModule:
                case TechType.CyclopsSeamothRepairModule:
                case TechType.CyclopsDecoyModule:
                case TechType.CyclopsFireSuppressionModule:
                case TechType.CyclopsThermalReactorModule:
                case TechType.CyclopsHullModule3:
                case TechType.VehiclePowerUpgradeModule:
                case TechType.SeamothSolarCharge:
                case TechType.VehicleStorageModule:
                case TechType.SeamothElectricalDefense:
                case TechType.VehicleArmorPlating:
                case TechType.SeamothTorpedoModule:
                case TechType.SeamothSonarModule:
                case TechType.VehicleHullModule1:
                case TechType.VehicleHullModule2:
                case TechType.VehicleHullModule3:
                case TechType.ExosuitJetUpgradeModule:
                case TechType.ExosuitDrillArmModule:
                case TechType.ExosuitThermalReactorModule:
                case TechType.ExosuitPropulsionArmModule:
                case TechType.ExosuitGrapplingArmModule:
                case TechType.ExosuitTorpedoArmModule:
                case TechType.ExoHullModule1:
                case TechType.ExoHullModule2:
                case TechType.FilteredWater:
                case TechType.DisinfectedWater:
                case TechType.StillsuitWater:
                case TechType.BigFilteredWater:
                    return new Quaternion(1, 0, 0, 1);
                case TechType.Knife:
                case TechType.Beacon:
                case TechType.CyclopsDecoy:
                case TechType.Flare:
                case TechType.HeatBlade:
                case TechType.JellyPlant:
                case TechType.AcidMushroom:
                case TechType.Poster:
                case TechType.PosterExoSuit1:
                case TechType.PosterExoSuit2:
                case TechType.PosterKitty:
                    return new Quaternion(-1, 0, 0, 1);
                case TechType.FiberMesh:
                case TechType.Copper:
                case TechType.Silicone:
                case TechType.AramidFibers:
                case TechType.Compass:
                case TechType.LuggageBag:
                case TechType.FirstAidKit:
                case TechType.Coffee:
                case TechType.LabEquipment1:
                case TechType.LabEquipment2:
                case TechType.LabEquipment3:
                    return new Quaternion(0, 1, 0, 1);
                case TechType.SmallStorage:
                    return new Quaternion(0, -1, 0, 1);
                case TechType.StasisRifle:
                case TechType.Welder:
                case TechType.LaserCutter:
                    return new Quaternion(1, 1, 0, 0);
                case TechType.HighCapacityTank:
                    return new Quaternion(0, 1, 1, 0);
                case TechType.Scanner:
                    return new Quaternion(0, 1, -1, 0);
                case TechType.DiveReel:
                    return new Quaternion(1, -1, 0, 0);
                case TechType.FireExtinguisher:
                    return new Quaternion(1, -1, -1, 1);
                case TechType.PosterAurora:
                    return new Quaternion(-1, 1, 1, 1);
                default:
                    return Quaternion.identity;
            }
        }

        public static bool MustHaveAnimator(TechType techType) {
            switch (techType) {
                case TechType.Flashlight:
                case TechType.Beacon:
                case TechType.Scanner:
                case TechType.AirBladder:
                case TechType.Welder:
                case TechType.HoleFish:
                case TechType.Peeper:
                case TechType.Oculus:
                case TechType.GarryFish:
                case TechType.Boomerang:
                case TechType.Eyeye:
                case TechType.Bladderfish:
                case TechType.Hoverfish:
                case TechType.Reginald:
                case TechType.Spadefish:
                case TechType.Hoopfish:
                case TechType.Spinefish:
                case TechType.LavaBoomerang:
                case TechType.LavaEyeye:
                case TechType.Constructor:
                case TechType.StasisRifle:
                case TechType.Seaglide:
                case TechType.Floater:
                    return true;
                default:
                    return false;
            }
        }

        public static readonly Dictionary<string, Quaternion> overrideRotation =
            new Dictionary<string, Quaternion> {
                { PrefabId.AirBladder, new Quaternion(0f, 0f, 0.03926f, 0.999229f) },
                { PrefabId.builder, new Quaternion(-0.343891f, 0.61785f, -0.343891f, 0.61785f) },
                { PrefabId.CoralChunk, new Quaternion(-0.530816f, -0.467155f, -0.530816f, 0.467155f) },
                { PrefabId.Flashlight, new Quaternion(-0.431926f, 0.559857f, -0.431926f, 0.559857f) },
                { PrefabId.JeweledDiskPiece, new Quaternion(0.0185099f, 0.7068644f, -0.0185099f, 0.7068645f) },
                { PrefabId.kyanite, new Quaternion(0.728072f, -0.000635f, -0.000598f, 0.685501f) },
                { PrefabId.PlasteelTank, new Quaternion(0.4531f, 0.542863f, 0.542863f, 0.4531f) },
                { PrefabId.Pipe, new Quaternion(0.037516f, 0f, 0f, 0.999296f) },
                { PrefabId.RadiationGloves, new Quaternion(0.006979f, -0.000183f, -0.026176f, 0.999633f) },
                { PrefabId.RadiationHelmet, new Quaternion(-0.498951f, 0.614661f, 0.369584f, 0.486465f) },
                { PrefabId.RadiationSuit, new Quaternion(0.007418f, 0f, 0f, 0.999972f) },
                { PrefabId.rebreather, new Quaternion(-0.014398f, 0f, 0f, 0.999896f) },
                { PrefabId.ReinforcedDiveSuit, new Quaternion(0.030974f, -0.000243f, -0.00785f, 0.999489f) },
                { PrefabId.ReinforcedGloves, new Quaternion(0f, 0f, 0.007854f, 0.999969f) },
                { PrefabId.salt, new Quaternion(-0.013088f, -0.000228f, 0.017451f, 0.999762f) },
                { PrefabId.StalkerTooth, new Quaternion(0.207908f, 0.632674f, -0.05149f, 0.744208f) },
                { PrefabId.stillSuit, new Quaternion(0.021813f, -0.000267f, 0.012214f, 0.999687f) },
                { PrefabId.Tank, new Quaternion(0.4531f, 0.542863f, 0.542863f, 0.4531f) },
            };

        public static readonly Dictionary<string, Bounds> overrideBounds =
            new Dictionary<string, Bounds> {
                {
                    PrefabId.AirBladder,
                    new Bounds(
                        new Vector3(0.0619f, 0.007068498f, -0.0002430007f),
                        new Vector3(0.178336f, 0.087859f, 0.339964f)
                    )
                },
                {
                    PrefabId.builder,
                    new Bounds(
                        new Vector3(0.01269999f, -0.004678575f, -0.0003100187f),
                        new Vector3(0.08853771f, 0.1318165f, 0.2608819f)
                    )
                },
                {
                    PrefabId.Constructor,
                    new Bounds(
                        new Vector3(-0.03260001f, 0.0916f, 0f),
                        new Vector3(0.531f, 0.8034f, 1.7214f)
                    )
                },
                {
                    PrefabId.CoralChunk,
                    new Bounds(
                        new Vector3(-0.003804892f, 0.2374298f, 0.01184294f),
                        new Vector3(0.7985589f, 0.4784564f, 1.354631f)
                    )
                },
                {
                    PrefabId.Flashlight,
                    new Bounds(
                        new Vector3(0.01348841f, 0.04119494f, -0.08201248f),
                        new Vector3(0.2903295f, 0.1559973f, 0.3067509f)
                    )
                },
                {
                    PrefabId.Gravsphere,
                    new Bounds(
                        new Vector3(-0.00007439f, 0.00094625f, 0.00028172f),
                        new Vector3(0.60034540f, 0.59925530f, 0.59941430f)
                    )
                },
                {
                    PrefabId.JeweledDiskPiece,
                    new Bounds(
                        new Vector3(0.3593766f, -0.004222117f, 0f),
                        new Vector3(0.8154497f, 0.2189854f, 1.031052f)
                    )
                },
                {
                    PrefabId.kyanite,
                    new Bounds(
                        new Vector3(0.0002700239f, -0.008214779f, 0.4988671f),
                        new Vector3(0.3918906f, 0.3716984f, 1.098186f)
                    )
                },
                {
                    PrefabId.LaserCutter,
                    new Bounds(
                        new Vector3(0.0007827729f, -0.002409886f, -0.1530155f),
                        new Vector3(0.3066762f, 0.06486701f, 0.687931f)
                    )
                },
                {
                    PrefabId.Pipe,
                    new Bounds(
                        new Vector3(2.980232e-08f, -7.878616e-05f, 0.4543497f),
                        new Vector3(0.2072515f, 0.2068251f, 1.088706f)
                    )
                },
                {
                    PrefabId.PlasteelTank,
                    new Bounds(
                        new Vector3(0.02211406f, 0.01890655f, 0.3162712f),
                        new Vector3(0.2704018f, 0.2633288f, 0.8359069f)
                    )
                },
                {
                    PrefabId.RadiationGloves,
                    new Bounds(
                        new Vector3(-0.002794042f, 0.03097222f, 0.005473562f),
                        new Vector3(0.2843161f, 0.06340077f, 0.2059305f)
                    )
                },
                {
                    PrefabId.RadiationHelmet,
                    new Bounds(
                        new Vector3(-0.1100249f, 0.03522123f, -0.02401032f),
                        new Vector3(0.2162f, 0.3143559f, 0.2782074f)
                    )
                },
                {
                    PrefabId.RadiationSuit,
                    new Bounds(
                        new Vector3(0.002410352f, 0.02520297f, -0.01776272f),
                        new Vector3(0.3983711f, 0.1280354f, 0.6504773f)
                    )
                },
                {
                    PrefabId.rebreather,
                    new Bounds(
                        new Vector3(0f, 0.08395185f, 0.06031561f),
                        new Vector3(0.1802571f, 0.1532138f, 0.3076813f)
                    )
                },           
                {
                    PrefabId.ReinforcedDiveSuit,
                    new Bounds(
                        new Vector3(0.0006409958f, 0.02599409f, 0.0004706383f),
                        new Vector3(0.4221275f, 0.1450065f, 0.6044404f)
                    )
                },
                {
                    PrefabId.ReinforcedGloves,
                    new Bounds(
                        new Vector3(-0.0004340932f, 0.02319119f, 0f),
                        new Vector3(0.2488169f, 0.05719143f, 0.2061948f)
                    )
                },
                {
                    PrefabId.salt,
                    new Bounds(
                        new Vector3(-0.0462963f, 0.00818868f, -0.1180089f),
                        new Vector3(0.8311617f, 0.6303338f, 1.02827f)
                    )
                },
                {
                    PrefabId.SeaGlide,
                    new Bounds(
                        new Vector3(0f, 0.04667834f, -0.1517114f),
                        new Vector3(0.729893f, 0.5464605f, 1.042482f)
                    )
                },
                {
                    PrefabId.SmallStorage,
                    new Bounds(
                        new Vector3(0.04013546f, -0.07484745f, 0.02369413f),
                        new Vector3(0.39070990f, 0.23180780f, 0.76429770f)
                    )
                },
                {
                    PrefabId.StalkerTooth,
                    new Bounds(
                        new Vector3(-0.08459536f, 0.2833199f, 0.003859565f),
                        new Vector3(0.1979135f, 0.1693203f, 0.8754285f)
                    )
                },
                {
                    PrefabId.stillSuit,
                    new Bounds(
                        new Vector3(0.0002868474f, 0.04766153f, -0.01925156f),
                        new Vector3(0.4055041f, 0.1386842f, 0.6469215f)
                    )
                },
                {
                    PrefabId.Tank,
                    new Bounds(
                        new Vector3(0.02211406f, 0.01890655f, 0.3162712f),
                        new Vector3(0.2704018f, 0.2633288f, 0.8359069f)
                    )
                },
                {
                    PrefabId.Welder,
                    new Bounds(
                        new Vector3(0.01866118f, -0.00241248f, -0.03316336f),
                        new Vector3(0.3017175f, 0.06471497f, 0.4511945f)
                    )
                },
            };

        public static readonly Dictionary<string, float> overrideDeltaScale =
            new Dictionary<string, float> {
                { PrefabId.Battery, 0.5f },
                { PrefabId.builder, 0.6f },
                { PrefabId.Coral_reef_purple_mushrooms_01_04, 0.6f },
                { PrefabId.coral_reef_blood_mushrooms_01_04, 0.6f },
                { PrefabId.PrecursorIonBattery, 0.5f },
            };
    }
}
