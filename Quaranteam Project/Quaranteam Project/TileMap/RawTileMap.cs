using System.Collections.Generic;

namespace QuaranteamProject {
    public class RawTileMap {
        public int MapWidth { get; set; }
        public int MapHeight { get; set; }
        public bool IsOutside { get; set; }
        public RawAvatar Avatar { get; set; }
        public RawObstacles Obstacles { get; set; }
        public RawEnemies Enemies { get; set; }
        public RawPowerUps PowerUps { get; set; }
        public RawItems Items { get; set; }
    }

    public class RawPosition {
        public float X { get; set; }
        public float Y { get; set; }
    }

    public class RawAvatar {
        public float X { get; set; }
        public float Y { get; set; }
    }

    public class RawObstacles {
        public IList<RawQuestionBlock> QuestionBlocks { get; set; }
        public IList<RawUsedBlock> UsedBlocks { get; set; }
        public IList<RawBrickBlock> BrickBlocks { get; set; }
        public IList<RawFloorBlock> FloorBlocks { get; set; }
        public IList<RawCloud> Clouds { get; set; }
        public IList<RawStairBlock> StairBlocks { get; set; }
        public IList<RawHiddenBlock> HiddenBlocks { get; set; }
        public IList<RawCastle> Castle { get; set; }
        public IList<RawFlagpole> Flagpole { get; set; }
        public RawPeach Peach { get; set; }
        public RawGate Gate { get; set; }
        public IList<RawWarpPipes> WarpPipes { get; set; }
    }

    public class RawEnemies {
        public IList<RawGoomba> Goombas { get; set; }
        public IList<RawPiranah> Piranahs { get; set; }
        public IList<RawKoopa> Koopas { get; set; }
        public IList<RawMiniCovid> MiniCovids { get; set; }
        public IList<RawCovidBoss> CovidBosses { get; set; }
    }

    public class RawPowerUps {
        public IList<RawMushroom> SuperMushrooms { get; set; }
        public IList<Raw1Up> LifeMushrooms { get; set; }
        public IList<RawFireFlower> FireFlowers { get; set; }
        public IList<RawStarman> Starmans { get; set; }
        public IList<RawSanitizer> Sanitizers { get; set; }
        public IList<RawMask> Masks { get; set; }
        public IList<RawVaccine> Vaccines { get; set; }
    }

    public class RawItems {
        public IList<RawCoin> Coins { get; set; }
    }

    public class RawQuestionBlock {
        public float X { get; set; }
        public float Y { get; set; }
        public RawHiddenItems HiddenItems { get; set; }
    }

    public class RawUsedBlock {
        public float X { get; set; }
        public float Y { get; set; }

    }

    public class RawBrickBlock {
        public float X { get; set; }
        public float Y { get; set; }
        public string Color { get; set; }
        public RawHiddenItems HiddenItems { get; set; }
    }

    public class RawFloorBlock {
        public float X { get; set; }
        public float Y { get; set; }
        public string Color { get; set; }
    }

    public class RawCloud {
        public float X { get; set; }
        public float Y { get; set; }
        public string Type { get; set; }
        public float XSpeed { get; set; }
        public float YSpeed { get; set; }
    }

    public class RawStairBlock {
        public float X { get; set; }
        public float Y { get; set; }
        public bool IsDoor { get; set; }
    }

    public class RawHiddenBlock {
        public float X { get; set; }
        public float Y { get; set; }
        public string Color { get; set; }
        public RawHiddenItems HiddenItems { get; set; }
    }

    public class RawCastle {
        public float X { get; set; }
        public float Y { get; set; }
    }

    public class RawFlagpole {
        public float X { get; set; }
        public float Y { get; set; }
    }

    public class RawPeach {
        public float X { get; set; }
        public float Y { get; set; }
    }

    public class RawGate {
        public float X { get; set; }
        public float Y { get; set; }
    }

    public class RawWarpPipes {
        public string Part { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public RawWarp WarpTo { get; set; }
    }

    public class RawWarp {
        public string Level { get; set; }
        public float? X { get; set; }
        public float? Y { get; set; }
    }

    public class RawGoomba {
        public float X { get; set; }
        public float Y { get; set; }
    }

    public class RawPiranah {
        public float X { get; set; }
        public float Y { get; set; }
    }

    public class RawKoopa {
        public string Color { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
    }

    public class RawMiniCovid {
        public float X { get; set; }
        public float Y { get; set; }
    }

    public class RawCovidBoss {
        public float X { get; set; }
        public float Y { get; set; }
    }

    public class RawHiddenItems {
        public float Coins { get; set; }
        public float SuperMushrooms { get; set; }
        public float LifeMushrooms { get; set; }
        public float FireFlowers { get; set; }
        public float Starmans { get; set; }
        public float Sanitizers { get; set; }
        public float Masks { get; set; }
        public float Vaccines { get; set; }
    }

    public class RawMushroom {
        public float X { get; set; }
        public float Y { get; set; }
    }

    public class Raw1Up {
        public float X { get; set; }
        public float Y { get; set; }
    }

    public class RawFireFlower {
        public float X { get; set; }
        public float Y { get; set; }
    }

    public class RawStarman {
        public float X { get; set; }
        public float Y { get; set; }
    }

    public class RawCoin {
        public float X { get; set; }
        public float Y { get; set; }
    }

    public class RawSanitizer {
        public float X { get; set; }
        public float Y { get; set; }
    }

    public class RawMask {
        public float X { get; set; }
        public float Y { get; set; }
    }

    public class RawVaccine {
        public float X { get; set; }
        public float Y { get; set; }
    }
}
