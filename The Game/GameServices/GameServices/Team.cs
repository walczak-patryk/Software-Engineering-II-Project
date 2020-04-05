namespace GameMaster
{
    public class Team
    {
        public TeamColor color;
        public TeamRole role;
        public int size;

        public Team()
        {
            size = 0;
        }

        public TeamColor getColor()
        {
            return color;
        }

        public void SetColor(TeamColor Color)
        {
            this.color = Color;
        }

        public TeamRole getRole()
        {
            return role;
        }

        public void SetRole(TeamRole Role)
        {
            this.role = Role;
        }

        public Player GetLeader()
        {
            return new Player(1,new Team(),false);
        }   
    }

    public enum TeamRole
    {
        Leader,
        Member
    }

    public enum TeamColor
    {
        Red,
        Blue
    }
}
