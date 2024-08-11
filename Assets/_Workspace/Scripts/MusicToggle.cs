public class MusicToggle : ToggleButton
{
    protected override bool _activeState
    {
        get => AudioManager.MusicActiveState;
        set => AudioManager.MusicActiveState = value;
    }
    public override void Toggle()
    {
        base.Toggle();
        ServiceLocator.GetService<AudioManager>().SetMusicMute();
    }
}
