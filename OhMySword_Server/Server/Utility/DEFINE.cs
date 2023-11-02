using System;

namespace Server
{
    public class DEFINE
    {
        public static readonly Vector3[] PlayerSpawnTable = { new Vector3(0, 0, 0) };
        public static readonly Vector3[] ScoreBoxSpawnTable = { new Vector3(0, 0, 0) };

        // 점수 구조물마다 달라야 함?
        public static readonly Dictionary<ushort, Vector3[][]> XPSpawnTable = new Dictionary<ushort, Vector3[][]>() {
            [(ushort)ObjectType.WoodenScoreBox] = new Vector3[][] {
                new Vector3[] { new Vector3(0, 0, 0), new Vector3(1, 1, 1) }, // 100 경험치
                new Vector3[] { new Vector3(2, 2, 2), new Vector3(3, 3, 3) }, // 10 경험치
                new Vector3[] { new Vector3(4, 4, 4), new Vector3(5, 5, 5) }, // 1 경험치
            },
            [(ushort)ObjectType.StoneScoreBox] = new Vector3[][] {
                new Vector3[] { new Vector3(0, 0, 0), new Vector3(1, 1, 1) }, // 100 경험치
                new Vector3[] { new Vector3(2, 2, 2), new Vector3(3, 3, 3) }, // 10 경험치
                new Vector3[] { new Vector3(4, 4, 4), new Vector3(5, 5, 5) }, // 1 경험치
            },
        };
    }
}
