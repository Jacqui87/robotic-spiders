using RoboticSpiders.Models;

bool keepRunning;

do
{
    Console.Clear();

    // Wall
    Console.WriteLine("Please enter the wall size (e.g., '5 5'):");
    var wall = new Wall(Console.ReadLine());
    if (wall.Error is not null)
    {
        Console.WriteLine("Error: " + wall.Error);
        keepRunning = AskToRepeat();
        continue;
    }
    Console.WriteLine($"Designated wall size is: {wall.MaxX} x {wall.MaxY}");
    Console.WriteLine();

    // Spider
    Console.WriteLine("Please enter the spider's starting position and direction (e.g., '1 2 Up'):");
    var spider = new Spider(Console.ReadLine());
    if (spider.Error is not null)
    {
        Console.WriteLine("Error: " + spider.Error);
        keepRunning = AskToRepeat();
        continue;
    }

    // Add spider to wall
    if (!wall.AddSpider(spider))
    {
        Console.WriteLine("Error: " + spider.Error);
        keepRunning = AskToRepeat();
        continue;
    }

    Console.WriteLine($"The spider's starting position is ({spider.X},{spider.Y}) & is facing: {spider.Facing}");
    Console.WriteLine();

    // Instructions
    Console.WriteLine("Please enter the movement instructions (e.g., 'LFLFLFLFF'):");
    var instructions = Console.ReadLine();
    Console.WriteLine();

    // Execute movements
    spider.ExecuteInstructions(instructions, wall);

    // Output results
    if (spider.Error is not null)
        Console.WriteLine("Error: " + spider.Error);
    else
        Console.WriteLine("Final position & direction: " + spider);

    Console.WriteLine();

    keepRunning = AskToRepeat();

} while (keepRunning);

// Helper method to ask if the user wants to repeat
static bool AskToRepeat()
{
    Console.WriteLine("Do you want to run again? (y/n):");
    var answer = Console.ReadLine();
    return answer?.Trim().ToLower() == "y";
}
