using System;

namespace Server
{
    public class DEFINE
    {
        public static readonly Vector3[] PlayerSpawnTable = {
              new Vector3(27, -25, -16),
              new Vector3(30, -25, -7),
              new Vector3(17, -24.7f, -5),
              new Vector3(4, -24.5f, -3),
              new Vector3(10, -19.5f, -26),
              new Vector3(-1, -19.5f, -14),
              new Vector3(-4, -15, -45),
              new Vector3(7, -15.7f, -36),
              new Vector3(-20, -16, -45),
              new Vector3(-17, -12.9f, -38),
              new Vector3(-26, -12.5f, -24),
              new Vector3(-15, -12.9f, -10),
              new Vector3(-40, -12.9f, -9),
              new Vector3(-40, -12.9f, -33),
              new Vector3(0.37f, -12.9f, 28.94f),
              new Vector3(-39.98f, -12.9f, 43.64f),
              new Vector3(-65.45f, -12.9f, 76.12f),
              new Vector3(-27.22f, -9.63f, 54.43f),
              new Vector3(4.16f, -7.79f, 49.54f),
              new Vector3(3.87f, -9.22f, 73.56f),
              new Vector3(58.2f, -29.6f, 59.73f),
              new Vector3(52.35f, -29.6f, 29.14f),
              new Vector3(68.07f, -29.6f, 31.6f),
              new Vector3(64, -29.6f, 13.78f),
              new Vector3(82.12f, -29.6f, 51.91f),
              new Vector3(109, -29.6f, 26.6f),
              new Vector3(-37.06f, -5.89f, 64.24f),
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
                new Vector3(17, -24.5f, -9)
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
            [(ushort)ObjectType.GemScoreBox] = new Vector3[] {
                new Vector3(80, -34.5f, 0),
                new Vector3(118, -34.5f, 35),
                new Vector3(57, -34.5f, 52),
                new Vector3(9, -16.5f, 33),
                new Vector3(-12, -16.5f, 51),
                new Vector3(-43, -16.5f, 45),
                new Vector3(-57, -16.5f, 67),
                new Vector3(-75, -16.5f, 88),
                new Vector3(-6, -16.5f, 84),
            },
            [(ushort)ObjectType.DesertTreeScoreBox] = new Vector3[] {
                new Vector3(-7, -16.4f, 30),
                new Vector3(-44, -16.4f, 30),
                new Vector3(-60, -16.4f, 79),
                new Vector3(13, -16.4f, 63),
                new Vector3(39, -34.5f, 45),
                new Vector3(58, -34.5f, 28),
                new Vector3(90, -34.5f, 5),
                new Vector3(80, -34.5f, 47),
            },
            [(ushort)ObjectType.CoreScoreBox] = new Vector3[] {
                new Vector3(-27, -18, 26),
                new Vector3(-30, -12, 66),
                new Vector3(44, -26, 98),
                new Vector3(59, -36, 14),
                new Vector3(107, -36, 28),
            },
        };

        public static readonly Dictionary<ushort, ScoreBoxDropTable> XPSpawnTable = new Dictionary<ushort, ScoreBoxDropTable>() {
            [(ushort)ObjectType.StoneScoreBox] = new ScoreBoxDropTable(138, new Vector3[] {
                new Vector3(0, 0.5f, 0),
                new Vector3(-2, 0.5f, 2),
                new Vector3(2, 0.5f, 2),
                new Vector3(0, 0.5f, -2),
                new Vector3(-4, 0.5f, -4),
                new Vector3(0, 0.5f, 4),
                new Vector3(4, 0.5f, 4),
                new Vector3(4, 0.5f, 0),
                new Vector3(4, 0.5f, -4),
                new Vector3(0, 0.5f, -4),
                new Vector3(-4, 0.5f, -4),
                new Vector3(-4, 0.5f, 0)
            }),
            [(ushort)ObjectType.WoodenScoreBox] = new ScoreBoxDropTable(441, new Vector3[] {
                new Vector3(4, 0.5f, 0),
                new Vector3(0, 0.5f, 4),
                new Vector3(-4, 0.5f, 0),
                new Vector3(0, 0.5f, -4),
                new Vector3(2, 0.5f, 2),
                new Vector3(-2, 0.5f, 2),
                new Vector3(-2, 0.5f, -2),
                new Vector3(2, 0.5f, -2),
                new Vector3(0, 0.5f, 0)
            }),
            [(ushort)ObjectType.EggScoreBox] = new ScoreBoxDropTable(1000, new Vector3[] {
                new Vector3(0, 0, 0)
            }),
            [(ushort)ObjectType.GemScoreBox] = new ScoreBoxDropTable(224, new Vector3[] {
                new Vector3(-2, 0.5f, 0),
                new Vector3(2, 0.5f, 0),
                new Vector3(0, 0.5f, 2),
                new Vector3(0, 0.5f, -2),
                new Vector3(2, 0.5f, 2),
                new Vector3(-2, 0.5f, 2),
                new Vector3(-2, 0.5f, -2),
                new Vector3(2, 0.5f, -2),
            }),
            [(ushort)ObjectType.DesertTreeScoreBox] = new ScoreBoxDropTable(522, new Vector3[] {
                new Vector3(2, 0.5f, 2),
                new Vector3(-2, 0.5f, 2),
                new Vector3(-2, 0.5f, -2),
                new Vector3(2, 0.5f, -2),
                new Vector3(0, 0.5f, 0),
                new Vector3(0, 0.5f, 2),
                new Vector3(0, 0.5f, -2),
                new Vector3(2, 0.5f, 0),
                new Vector3(-2, 0.5f, 0),
            }),
            [(ushort)ObjectType.CoreScoreBox] = new ScoreBoxDropTable(1500, new Vector3[] {
                new Vector3(0, 0, 0),
                new Vector3(0, 0, 4),
                new Vector3(-3, 0, 3),
                new Vector3(3, 0, 3),
                new Vector3(-2, 0, -2),
                new Vector3(2, 0, -2),
            }),
        };

        public const float Rad2Deg = 180f / MathF.PI;
        public const float Deg2Rad = MathF.PI / 180f;
    }
}
