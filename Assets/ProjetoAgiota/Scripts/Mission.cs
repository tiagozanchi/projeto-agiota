public enum CarColors
{
    Black,
    Blue,
    Green,
    Red,
    Yellow,
    White
}

public class Mission
{
    public int NumberOfCars;
    public int TrackLength;
    public CarColors TrackedCarColor;
    public int TrackedCarPosition;

    public Mission(int numberOfCars, int trackLength, CarColors trackerCarColor, int trackedCarPosition)
    {
        NumberOfCars = numberOfCars;
        TrackLength = trackLength;
        TrackedCarColor = trackerCarColor;
        TrackedCarPosition = trackedCarPosition;
    }
}
