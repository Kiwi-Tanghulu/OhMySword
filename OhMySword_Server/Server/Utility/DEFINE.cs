using System;

namespace Server
{
    public class DEFINE
    {
        public static readonly Vector3[] PlayerSpawnTable = { new Vector3(-6, -15, -50) };
        public static readonly Vector3[] ScoreBoxSpawnTable = { new Vector3(0, 0, 0) };

        public static readonly Dictionary<ushort, Vector3[]> ScoreBoxSpawnTables = new Dictionary<ushort, Vector3[]>()
        {
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
            [(ushort)ObjectType.WoodenScoreBox] = new ScoreBoxDropTable(138, new Vector3[] {
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
        };

        public const float Rad2Deg = 180f / MathF.PI;
        public const float Deg2Rad = MathF.PI / 180f;
    }
}
