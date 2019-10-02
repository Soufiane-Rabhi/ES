namespace ES.Domain.Models
{
    public class Player
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ShortName { get; set; }
        public string Sex { get; set; }     
        public string Picture { get; set; }
        public Data Data { get; set; }
        public Country Country { get; set; }
    }

    public class PlayersList
    {
        public Player[] Players { get; set; }
    }
}

   


