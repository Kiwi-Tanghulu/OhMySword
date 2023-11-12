using System;

namespace Server
{
    public class DEFINE
    {
        public static readonly Vector3[] PlayerSpawnTable = {
            new Vector3(27f, -25f, -16f),
            new Vector3(30f, -25f, -7f),
            new Vector3(17f, -24.7f, -5f),
            new Vector3(4f, -24.5f, -3f),
            new Vector3(10f, -19.5f, -26f),
            new Vector3(-1f, -19.5f, -14f),
            new Vector3(-4f, -15f, -45f),
            new Vector3(7f, -15.7f, -36f),
            new Vector3(-20f, -16f, -45f),
            new Vector3(-17f, -12.9f, -38f),
            new Vector3(-26f, -12.5f, -24f),
            new Vector3(-15f, -12.9f, -10f),
            new Vector3(-40f, -12.9f, -9f),
            new Vector3(-40f, -12.9f, -33f),
        };

        public static readonly Dictionary<ushort, Vector3[]> ScoreBoxSpawnTables = new Dictionary<ushort, Vector3[]>() {
            [(ushort)ObjectType.StoneScoreBox] = new Vector3[] {
                new Vector3(-2, -15, -41),
                new Vector3(-17, -12.8f, -34),
                new Vector3(-47, -12.6f, -13),
                new Vector3(5, -19.5f, -17),
                new Vector3(12, -24.7f, 1),
                new Vector3(33, -25, -12),
                new Vector3(-9, -16, -48),
                new Vector3(-28, -12.5f, -20),
                new Vector3(17, -24.5f, 9)
            },
            [(ushort)ObjectType.WoodenScoreBox] = new Vector3[] {
                new Vector3(-20.5f, -12.2f, -12),
                new Vector3(-43, -13, -30),
                new Vector3(-18, -16, -45),
                new Vector3(5.5f, -19.5f, -23.5f),
                new Vector3(17, -25, -18),
                new Vector3(38, -25, -23)
            },
            [(ushort)ObjectType.EggScoreBox] = new Vector3[] {
                new Vector3(-12, -14.5f, -59),
                new Vector3(-35.5f, -11.5f, -17),
                new Vector3(0, -23.5f, -2),
                new Vector3(-7.5f, -18, -19),
            },
        };

        public static readonly Dictionary<ushort, ScoreBoxDropTable> XPSpawnTable = new Dictionary<ushort, ScoreBoxDropTable>() {
            [(ushort)ObjectType.StoneScoreBox] = new ScoreBoxDropTable(138, new Vector3[] {
                new Vector3(0, 1, 0),
                new Vector3(-1, 1, 1),
                new Vector3(1, 1, 1),
                new Vector3(0, 1, -1),
                new Vector3(-2, 1, -2),
                new Vector3(0, 1, 2),
                new Vector3(2, 1, 2),
                new Vector3(2, 1, 0),
                new Vector3(2, 1, -2),
                new Vector3(0, 1, -2),
                new Vector3(-2, 1, -2),
                new Vector3(-2, 1, 0)
            }),
            [(ushort)ObjectType.WoodenScoreBox] = new ScoreBoxDropTable(441, new Vector3[] {
                new Vector3(2, 1, 0),
                new Vector3(0, 1, 2),
                new Vector3(- 2, 1, 0),
                new Vector3(0, 1, -2),
                new Vector3(1, 1, 1),
                new Vector3(-1, 1, 1),
                new Vector3(-1, 1, -1),
                new Vector3(1, 1, -1),
                new Vector3(0, 1, 0)
            }),
            [(ushort)ObjectType.EggScoreBox] = new ScoreBoxDropTable(1000, new Vector3[] {
                new Vector3(0, 1, 0)
            }),
        };

        public const float Rad2Deg = 180f / MathF.PI;
        public const float Deg2Rad = MathF.PI / 180f;
    }
}
