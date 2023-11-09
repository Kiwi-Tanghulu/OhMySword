using System;

namespace Server
{
    public class DEFINE
    {
        public static readonly Vector3[] PlayerSpawnTable = { new Vector3(-6, -15, -50) };
        public static readonly Vector3[] ScoreBoxSpawnTable = { new Vector3(0, 0, 0) };

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
