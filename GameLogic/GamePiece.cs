namespace GameLogic
{
    public class GamePiece
    {
        private ePieceColor m_PieceColor;
        private ePieceType m_PieceType;
        private int m_Row;
        private int m_Column;

        public ePieceColor PieceColor
        {
            get { return m_PieceColor; }
            private set { m_PieceColor = value; }
        }

        public ePieceType PieceType
        {
            get { return m_PieceType; }
            set { m_PieceType = value; }
        }

        public int Row
        {
            get { return m_Row; }
            set { m_Row = value; }
        }

        public int Column
        {
            get { return m_Column; }
            set { m_Column = value; }
        }

        public GamePiece(ePieceColor i_PieceColor, int i_Row, int i_Column)
        {
            m_PieceColor = i_PieceColor;
            m_PieceType = ePieceType.Regular;
            m_Row = i_Row;
            m_Column = i_Column;
        }

        public void PromoteToKing()
        {
            m_PieceType = ePieceType.King;
        }

        public char GetSymbol()
        {
            return m_PieceType == ePieceType.King
                ? (m_PieceColor == ePieceColor.PlayerO ? '♔' : '♚')  
                : (m_PieceColor == ePieceColor.PlayerO ? '⚪' : '⚫');  
        }
    }
}
