using BepInEx;
using HarmonyLib;
using SpecialNefia.Config;
using SpecialNefia.Deploy;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Reflection;
using System;
using System.Dynamic;
using System.Linq;
using SpecialNefia.Patch;

namespace SpecialNefia
{
    internal static class ModInfo
    {
        internal const string GUID = "byakko.mods.SPN";
        internal const string Name = "SpecialNefia";
        internal const string Version = "0.23.237";
    }

    [BepInPlugin(ModInfo.GUID, ModInfo.Name, ModInfo.Version)]
    internal class SpecialNefia : BaseUnityPlugin
    {
        internal static SpecialNefia Instance { get; private set; }

        private void Awake()
        {
            Instance = this;

            var harmony = new Harmony(ModInfo.GUID);
            harmony.PatchAll();

            ModConfig.LoadConfig();
        }

        public void OnStartCore()
        {
            DeployTypeFallback.DeployTypeFallbackSetting();
        }

        internal static void Log(object payload)
        {
            Instance.Logger.LogInfo(payload);
        }

        internal static void LogILInstructions(IEnumerable<CodeInstruction> instructions)
        {
            foreach (var instruction in instructions)
            {
                OpCode opCode = instruction.opcode;
                object operand = instruction.operand;
                Console.WriteLine($"OpCode: {opCode} Operand: {operand}");
                switch (opCode.Name)
                {
                    case "ldfld":
                        if (operand is FieldInfo fieldInfo)
                        {
                            Console.WriteLine($"  Field: {fieldInfo.DeclaringType.Name}::{fieldInfo.Name}");
                        }
                        break;
                    case "call":
                        if (operand is MethodInfo methodInfo)
                        {
                            Console.WriteLine($"  Method: {methodInfo.DeclaringType.Name}::{methodInfo.Name}");
                        }
                        break;
                    default:
                        break;
                }
            }
        }
    }
}