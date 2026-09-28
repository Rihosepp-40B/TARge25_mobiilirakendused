namespace TicTacToe.Game
{
    public class GameStatistics
    {
        public int Player1Wins { get; set; }
        public int Player2Wins { get; set; }
        public int Ties { get; set; }

        public int TotalGames
        {
            get
            {
                return Player1Wins + Player2Wins + Ties;
            }
        }

        public void Player1Won()
        {
            Player1Wins++;
        }

        public void Player2Won()
        {
            Player2Wins++;
        }

        public void GameTied()
        {
            Ties++;
        }
    }
}