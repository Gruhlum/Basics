namespace HexTecGames.Basics.Profiles
{
    [System.Serializable]
    public class Profile
    {
        public string Name
        {
            get
            {
                return name;
            }
            private set
            {
                name = value;
            }
        }
        private string name;


        public Profile(string name)
        {
            this.Name = name;
        }

        public void Rename(string name)
        {
            this.Name = name;
        }
    }
}