﻿using Newtonsoft.Json;

namespace Discord
{
    /// <summary>
    /// Represents a <see cref="Channel"/> specific to guild voice channels
    /// </summary>
    public class VoiceChannel : GuildChannel
    {
        [JsonProperty("bitrate")]
        public uint Bitrate { get; private set; }


        [JsonProperty("user_limit")]
        public uint UserLimit { get; private set; }


        /// <summary>
        /// Updates the channel's info
        /// </summary>
        public override void Update()
        {
            VoiceChannel channel = Client.GetVoiceChannel(Id);
            Name = channel.Name;
            Bitrate = channel.Bitrate;
            UserLimit = channel.UserLimit;
            Position = channel.Position;
            ParentId = channel.ParentId;
            PermissionOverwrites = channel.PermissionOverwrites;
        }


        /// <summary>
        /// Modifies the channel
        /// </summary>
        /// <param name="properties">Options for modifying the channel</param>
        public void Modify(VoiceChannelProperties properties)
        {
            if (!properties.NameProperty.Set)
                properties.Name = Name;
            if (!properties.PositionProperty.Set)
                properties.Position = Position;
            if (!properties.ParentProperty.Set)
                properties.ParentId = ParentId;
            if (!properties.BitrateProperty.Set)
                properties.Bitrate = Bitrate;
            if (!properties.UserLimitProperty.Set)
                properties.UserLimit = UserLimit;

            VoiceChannel channel = Client.ModifyVoiceChannel(Id, properties);
            Name = channel.Name;
            Position = channel.Position;
            ParentId = channel.ParentId;
            PermissionOverwrites = channel.PermissionOverwrites;
            Bitrate = channel.Bitrate;
            UserLimit = channel.UserLimit;
        }


        /// <summary>
        /// Creates an invite
        /// </summary>
        /// <param name="properties">Options for creating the invite</param>
        /// <returns></returns>
        public PartialInvite CreateInvite(InviteProperties properties = null)
        {
            return Client.CreateInvite(Id, properties);
        }
    }
}