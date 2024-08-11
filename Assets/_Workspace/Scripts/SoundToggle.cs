public class SoundToggle : ToggleButton
{
    protected override bool _activeState
    {
        get => AudioManager.SoundActiveState;
        set => AudioManager.SoundActiveState = value;
    }
    public override void Toggle()
    {
        base.Toggle();
        ServiceLocator.GetService<AudioManager>().SetSoundMute();
    }
}