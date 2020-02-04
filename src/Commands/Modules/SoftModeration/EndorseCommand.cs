using System.Threading.Tasks;

using Discord;
using Discord.Commands;

using BrackeysBot.Services;
using Discord.WebSocket;

namespace BrackeysBot.Commands
{
    public partial class SoftModerationModule : BrackeysBotModule
    {
        [Command("endorse"), Alias("star")]
        [Summary("Endorse a user and give them a star.")]
        [Remarks("endorse <user>")]
        [RequireGuru]
        public async Task EndorseUserAsync(
            [Summary("The user to endorse.")] SocketGuildUser guildUser) 
        {
            UserData user = Data.UserData.GetOrCreate(guildUser.Id);
            user.Stars++;

            await new EmbedBuilder()
                .WithAuthor(guildUser)
                .WithColor(Color.Gold)
                .WithDescription($"Gave a :star: to {guildUser.Mention}! They now have {user.Stars} stars!")
                .Build()
                .SendToChannel(Context.Channel);

            Data.SaveUserData();    
        }

        [Command("deleteendorse"), Alias("deletestar", "delstar", "delendorse")]
        [Summary("Remove a star from a user.")]
        [Remarks("deleteendorse <user>")]
        [RequireModerator]
        public async Task DeleteEndorseUserAsync(
            [Summary("The user to remove an endorsement.")] SocketGuildUser guildUser) 
        {
            UserData user = Data.UserData.GetOrCreate(guildUser.Id);

            EmbedBuilder builder = new EmbedBuilder();

            if (user.Stars == 0) {
                builder.WithColor(Color.Red).WithDescription("Can't remove a star, they have none!");
            } else {
                user.Stars--;
                builder.WithAuthor(guildUser).WithColor(Color.Green)
                    .WithDescription($"Took a :star: from {guildUser.Mention}! They now have {user.Stars} stars!");
            }

            await builder.Build()
                .SendToChannel(Context.Channel);

            Data.SaveUserData();    
        }

        [Command("clearendorse"), Alias("clearstar", "clearstars")]
        [Summary("Remove all stars from a user.")]
        [Remarks("clearendorse <user>")]
        [RequireModerator]
        public async Task WipeEndorseUserAsync(
            [Summary("The user to remove all endorsement.")] SocketGuildUser guildUser) 
        {
            UserData user = Data.UserData.GetOrCreate(guildUser.Id);

            user.Stars = 0;

            await new EmbedBuilder()
                .WithAuthor(guildUser)
                .WithColor(Color.Green)
                .WithDescription($"Removed all endorsements from {guildUser.Mention}!")
                .Build()
                .SendToChannel(Context.Channel);
            
            Data.SaveUserData();    
        }

        [Command("setendorse"), Alias("setrstar", "setstars")]
        [Summary("Set the stars of a user.")]
        [Remarks("setendorse <user> <amount>")]
        [RequireModerator]
        public async Task SetEndorseUserAsync(
            [Summary("The user to set the endorsement.")] SocketGuildUser guildUser,
            [Summary("The amount of endorsement to set.")] int amount) 
        {
            UserData user = Data.UserData.GetOrCreate(guildUser.Id);

            user.Stars = amount;

            await new EmbedBuilder()
                .WithAuthor(guildUser)
                .WithColor(Color.Green)
                .WithDescription($"Set the endorsements of {guildUser.Mention} to {amount}:star:!")
                .Build()
                .SendToChannel(Context.Channel);
            
            Data.SaveUserData();    
        }
    }
}