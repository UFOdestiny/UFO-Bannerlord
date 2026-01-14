using TaleWorlds.CampaignSystem.GameComponents;
using UFO.Setting;
namespace UFO.Model;

public class ModifiedCharacterDevelopmentModel : DefaultCharacterDevelopmentModel
{
    public override int MaxAttribute => SettingsManager.MaxAttr.Value;
}


