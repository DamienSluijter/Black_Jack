namespace Black_Jack.Classes
{
    public class Card
    {
        public string Name { get; }
        public int Value { get; }
        public bool IsAce => Value == 11;

        public Card(string name, int value)
        {
            Name = name;
            Value = value;
        }

        public string GetImagePath()
        {
            return $"Cards/{Name}.png";
        }
    }
}