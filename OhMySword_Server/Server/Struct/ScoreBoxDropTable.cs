namespace Server
{
    public struct ScoreBoxDropTable
    {
        public ushort score;
        public Vector3[] positions;

        public ScoreBoxDropTable(ushort score, Vector3[] positions)
        {
            this.score = score;
            this.positions = positions;
        }
    }
}
