using Headtrip.Objects.Abstract.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Objects.Instance
{
    public sealed class MDungeonInstance : ADatabaseObject
    {
        public Guid DungeonInstanceId { get; set; }
        public string? DungeonName { get; set; } = null!;
        public int DifficultyModifier { get; set; } // The difficulty modifier selected at the start of the run
        public int CurrentDepth { get; set; } // How deep have the players gone into the dungeon
        public int Seed { get; set; } // RNG Seed used to procedurally generate the instance
        public string? LevelName { get; set; } = null!; // The name of the level that needs to be passed to the unreal engine instance
    }
}
