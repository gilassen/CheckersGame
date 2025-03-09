namespace GameLogic
{
    public class Player
    {
        private string m_Name;
        private ePieceColor m_PlayerColor;
        private int m_RegularPiecesCaptured;
        private int m_KingsCaptured;
        private int m_TotalPoints;
        private int m_RoundPoints;

        public string Name
        {
            get { return m_Name; }
            private set { m_Name = value; }
        }

        public ePieceColor PlayerColor
        {
            get { return m_PlayerColor; }
            private set { m_PlayerColor = value; }
        }

        public int RegularPiecesCaptured
        {
            get { return m_RegularPiecesCaptured; }
            private set { m_RegularPiecesCaptured = value; }
        }

        public int KingsCaptured
        {
            get { return m_KingsCaptured; }
            private set { m_KingsCaptured = value; }
        }

        public int TotalPoints
        {
            get { return m_TotalPoints; }
            private set { m_TotalPoints = value; }
        }

        public int RoundPoints
        {
            get { return m_RoundPoints; }
            private set { m_RoundPoints = value; }
        }

        public Player(string i_Name, ePieceColor i_PlayerColor)
        {
            m_Name = i_Name;
            m_PlayerColor = i_PlayerColor;
            m_RegularPiecesCaptured = 0;
            m_KingsCaptured = 0;
            m_TotalPoints = 0;
            m_RoundPoints = 0;
        }

        public void AddPoints(int i_Points)
        {
            m_TotalPoints += i_Points;
            m_RoundPoints += i_Points;
        }
    }
}
