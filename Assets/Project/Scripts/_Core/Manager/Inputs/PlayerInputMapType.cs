namespace Inputs
{
    public enum PlayerInputMapType
    {
        None = 0,
        Raid,
        UI,
    }

    public static class PlayerInputExtension
    {
        public static string Key(this PlayerInputMapType mapType) =>
            mapType switch
            {
                PlayerInputMapType.Raid => "Raid",
                PlayerInputMapType.UI   => "UI",
                _                       => "_"
            };
    }
}