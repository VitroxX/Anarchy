﻿using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;

namespace Discord
{
    public static class GuildExtensions
    {
        #region management
        public static Guild CreateGuild(this DiscordClient client, GuildCreationProperties properties)
        {
            var resp = client.HttpClient.Post("/guilds", JsonConvert.SerializeObject(properties));

            return resp.Content.Deserialize<Guild>().SetClient(client);
        }


        public static Guild ModifyGuild(this DiscordClient client, long guildId, GuildModProperties properties)
        {
            var resp = client.HttpClient.Patch($"/guilds/{guildId}", JsonConvert.SerializeObject(properties));

            if (resp.StatusCode == HttpStatusCode.NotFound)
                throw new GuildNotFoundException(client, guildId);

            return resp.Content.Deserialize<Guild>().SetClient(client);
        }


        public static bool DeleteGuild(this DiscordClient client, long guildId)
        {
            var resp = client.HttpClient.Post($"/guilds/{guildId}/delete");

            if (resp.StatusCode == HttpStatusCode.NotFound)
                throw new GuildNotFoundException(client, guildId);

            return resp.StatusCode == HttpStatusCode.NoContent;
        }


        public static bool KickGuildMember(this DiscordClient client, long guildId, long userId)
        {
            var resp = client.HttpClient.Delete($"/guilds/{guildId}/members/{userId}");

            if (resp.StatusCode == HttpStatusCode.NotFound)
                throw new UserNotFoundException(client, userId);

            return resp.StatusCode == HttpStatusCode.NoContent;
        }


        public static List<Ban> GetGuildBans(this DiscordClient client, long guildId)
        {
            var resp = client.HttpClient.Get($"/guilds/{guildId}/bans");

            if (resp.StatusCode == HttpStatusCode.NotFound)
                throw new GuildNotFoundException(client, guildId);

            return resp.Content.Deserialize<List<Ban>>();
        }


        public static Ban GetGuildBan(this DiscordClient client, long guildId, long userId)
        {
            var resp = client.HttpClient.Get($"/guilds/{guildId}/bans/{userId}");

            if (resp.StatusCode == HttpStatusCode.NotFound)
                throw new BanNotFoundException(client, guildId);

            return resp.Content.Deserialize<Ban>();
        }


        public static bool BanGuildMember(this DiscordClient client, long guildId, long userId, int deleteMessageDays, string reason)
        {
            var resp = client.HttpClient.Put($"guilds/{guildId}/bans/{userId}?delete-message-days={deleteMessageDays}&reason={reason}");

            if (resp.StatusCode == HttpStatusCode.NotFound)
                throw new UserNotFoundException(client, userId);

            return resp.StatusCode == HttpStatusCode.NoContent;
        }


        public static bool UnbanGuildMember(this DiscordClient client, long guildId, long userId)
        {
            var resp = client.HttpClient.Delete($"guilds/{guildId}/bans/{userId}");

            return resp.StatusCode == HttpStatusCode.NoContent;
        }
        #endregion


        public static List<PartialGuild> GetClientGuilds(this DiscordClient client, int limit = 100, long afterId = 0)
        {
            return client.HttpClient.Get($"/users/@me/guilds?limit={limit}&after={afterId}").Content.Deserialize<List<PartialGuild>>().SetClientsInList(client);
        }


        public static Guild GetGuild(this DiscordClient client, long guildId)
        {
            var resp = client.HttpClient.Get("/guilds/" + guildId);

            if (resp.StatusCode == HttpStatusCode.NotFound)
                throw new GuildNotFoundException(client, guildId);

            return resp.Content.Deserialize<Guild>().SetClient(client);
        }


        public static List<Channel> GetGuildChannels(this DiscordClient client, long guildId)
        {
            var resp = client.HttpClient.Get($"/guilds/{guildId}/channels");

            if (resp.StatusCode == HttpStatusCode.NotFound)
                throw new GuildNotFoundException(client, guildId);

            return resp.Content.Deserialize<List<Channel>>().SetClientsInList(client);
        }


        #region members
        public static List<GuildMember> GetGuildMembers(this DiscordClient client, long guildId, int limit, long afterId = 0)
        {
            var resp = client.HttpClient.Get($"/guilds/{guildId}/members?limit={limit}&after={afterId}");

            if (resp.StatusCode == HttpStatusCode.NotFound)
                throw new GuildNotFoundException(client, guildId);

            return resp.Content.Deserialize<List<GuildMember>>();
        }


        public static List<GuildMember> GetAllGuildMembers(this DiscordClient client, long guildId)
        {
            List<GuildMember> members = client.GetGuildMembers(guildId, 1000);

            while (true)
            {
                List<GuildMember> newMembers = client.GetGuildMembers(guildId, 1000, members[members.Count - 1].User.Id);
                if (newMembers.Count == 0) break;
                members.AddRange(newMembers);
            }

            return members;
        }
        #endregion


        public static Invite JoinGuild(this DiscordClient client, string invCode)
        {
            var resp = client.HttpClient.Post($"/invite/{invCode}");

            if (resp.StatusCode == HttpStatusCode.NotFound)
                throw new InvalidInviteException(client, invCode);

            return resp.Content.Deserialize<Invite>().SetClient(client);
        }


        public static bool LeaveGuild(this DiscordClient client, long guildId)
        {
            var resp = client.HttpClient.Delete($"/users/@me/guilds/{guildId}");

            if (resp.StatusCode == HttpStatusCode.NotFound)
                throw new GuildNotFoundException(client, guildId);

            return resp.StatusCode == HttpStatusCode.NoContent;
        }


        public static bool ChangeNickname(this DiscordClient client, long guildId, string nickname)
        {
            var resp = client.HttpClient.Patch($"/guilds/{guildId}/members/@me/nick", "{\"nick\":\"" + nickname + "\"}");

            if (resp.StatusCode == HttpStatusCode.NotFound)
                throw new GuildNotFoundException(client, guildId);

            return resp.StatusCode == HttpStatusCode.OK;
        }
    }
}