using Mastermind;



class Program
{
    static void Main(string[] args)
    {

        Gameplay gameplay = new Gameplay();
        if (gameplay.Setup(args))
            gameplay.Play();
    }
}
