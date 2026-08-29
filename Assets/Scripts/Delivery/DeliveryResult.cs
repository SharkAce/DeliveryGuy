public class DeliveryResult
{
    public int DeliveryNumber { get; private set; }
    public float DeliveryTime { get; private set; }
    public float TargetTime { get; private set; }
    public float FoodQuality { get; private set; }
    public float DrivingScore { get; private set; }
    public DrivingRating DrivingRating { get; private set; }
    public float Tip { get; private set; }
    public int DeliveryScore { get; private set; }
    public int TotalScore { get; private set; }

    public DeliveryResult(
        int deliveryNumber,
        float deliveryTime,
        float targetTime,
        float foodQuality,
        float drivingScore,
        DrivingRating drivingRating,
        float tip,
        int deliveryScore,
        int totalScore
    )
    {
        DeliveryNumber = deliveryNumber;
        DeliveryTime = deliveryTime;
        TargetTime = targetTime;
        FoodQuality = foodQuality;
        DrivingScore = drivingScore;
        DrivingRating = drivingRating;
        Tip = tip;
        DeliveryScore = deliveryScore;
        TotalScore = totalScore;
    }
}