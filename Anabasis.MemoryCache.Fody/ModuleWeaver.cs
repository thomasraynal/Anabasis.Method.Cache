﻿using Fody;
using System;
using System.Collections.Generic;

namespace Anabasis.MemoryCache.Fody
{
    public partial class ModuleWeaver : BaseModuleWeaver
    {
        public override void Execute()
        {
            var references = Fody.References.Init(this);

            var weavingCandidates = ModuleDefinition.GetWeavingCandidates(references);
    
            foreach(var weavingCandidate in weavingCandidates)
            {
                DebugWriteLine.WeaveMethod(ModuleDefinition, weavingCandidate.MethodDefinition, references);
            }

        }

        public override IEnumerable<string> GetAssembliesForScanning()
        {
            yield return "netstandard";
            yield return "mscorlib";
            yield return "Microsoft.Extensions.Caching.Abstractions";
            yield return "Anabasis.MemoryCache";
        }
    }
}
