using TaleWorlds.CampaignSystem.AgentOrigins;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace UFO.Extension
{
    public static class AgentExtensions
    {
        public static bool TryGetHuman(this Agent agent, out Agent character)
        {
            if (agent == null)
            {
                character = null;
                return false;
            }
            if (agent.IsHuman)
            {
                character = agent;
                return true;
            }
            if (agent.IsMount)
            {
                character = agent.RiderAgent;
                return character != null;
            }
            character = null;
            return false;
        }

        public static bool TryGetParty(this IAgentOriginBase origin, out PartyBase party)
        {
            if (!(origin is PartyAgentOrigin partyAgentOrigin))
            {
                if (!(origin is PartyGroupAgentOrigin partyGroupAgentOrigin))
                {
                    if (origin is SimpleAgentOrigin simpleAgentOrigin)
                    {
                        party = simpleAgentOrigin.Party;
                        return party != null;
                    }
                    party = null;
                    return false;
                }
                party = partyGroupAgentOrigin.Party;
                return party != null;
            }
            party = partyAgentOrigin.Party;
            return party != null;
        }

        public static bool IsOnPlayerEnemySide(this IAgentOriginBase origin)
        {
            PartyBase party;
            return origin.TryGetParty(out party) && party.MapEventSide?.OtherSide?.IsMainPartyAmongParties() == true;
        }

        public static bool IsHero(this Agent agent)
        {
            return agent?.IsHero ?? false;
        }

        public static bool IsPlayer(this Agent agent)
        {
            return agent?.Character?.IsPlayer() == true;
        }

        public static bool IsPlayerCompanion(this Agent agent)
        {
            PartyBase party;
            return !agent.IsPlayer() && agent.IsHero() && agent.Origin.TryGetParty(out party) && party.IsPlayerParty();
        }

        public static bool IsPlayerEnemy(this Agent agent)
        {
            return Mission.Current?.PlayerTeam != null && agent?.Team != null && Mission.Current.PlayerTeam.Side != agent.Team.Side;
        }

        public static bool IsPlayerAlly(this Agent agent)
        {
            return Mission.Current?.PlayerTeam != null && agent?.Team != null && Mission.Current.PlayerTeam.Side == agent.Team.Side;
        }
    }
}