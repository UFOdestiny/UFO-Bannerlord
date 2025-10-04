using TaleWorlds.MountAndBlade;

public static class BKHacks
{
    public static AttackInformation SavedAttackInformation;

    public static void SyncCorruptedData(ref AttackInformation attackInformation)
    {
        if (attackInformation.AttackerAgentCharacter != SavedAttackInformation.AttackerAgentCharacter)
        {
            attackInformation = SavedAttackInformation;
        }
    }
}
