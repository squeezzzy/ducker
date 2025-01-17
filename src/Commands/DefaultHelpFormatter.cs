﻿using System.Text;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Converters;
using DSharpPlus.CommandsNext.Entities;
using DSharpPlus.Entities;
using ducker.Config;

namespace ducker.Commands
{
    /// <summary>
    /// Default help command formatter (need true in commands config)
    /// </summary>
    public class DefaultHelpFormatter : BaseHelpFormatter
    {
        public DiscordEmbedBuilder EmbedBuilder { get; }
        private Command Command { get; set; }

        public DefaultHelpFormatter(CommandContext ctx) : base(ctx)
        {
            EmbedBuilder = new DiscordEmbedBuilder()
                .WithTitle("Help")
                .WithColor(Bot.MainEmbedColor);
        }

        public override BaseHelpFormatter WithCommand(Command command)
        {
            Command = command;
            EmbedBuilder.WithDescription($"{Formatter.InlineCode(command.Name)} - {command.Description ?? "No description provided."}");

            if (command is CommandGroup cgroup && cgroup.IsExecutableWithoutSubcommands)
            {
                EmbedBuilder.WithDescription($"{this.EmbedBuilder.Description}\n\nThis group can be executed as a standalone command.");
            }
            if (command.Aliases?.Any() == true)
            {
                EmbedBuilder.AddField("Aliases", string.Join(", ", command.Aliases.Select(Formatter.InlineCode)), false);
            }
            if (command.Overloads?.Any() == true)
            {
                var sb = new StringBuilder();

                foreach (var ovl in command.Overloads.OrderByDescending(x => x.Priority))
                {
                    sb.Append('`').Append(command.QualifiedName);

                    foreach (var arg in ovl.Arguments)
                        sb.Append(arg.IsOptional || arg.IsCatchAll ? " [" : " <").Append(arg.Name).Append(arg.IsCatchAll ? "..." : "").Append(arg.IsOptional || arg.IsCatchAll ? ']' : '>');

                    sb.Append("`\n");

                    foreach (var arg in ovl.Arguments)
                        sb.Append('`').Append(arg.Name).Append(" (").Append(this.CommandsNext.GetUserFriendlyTypeName(arg.Type)).Append(")`: ").Append(arg.Description ?? "No description provided.").Append('\n');

                    sb.Append('\n');
                }

                this.EmbedBuilder.AddField("Arguments", sb.ToString().Trim(), false);
            }
            return this;
        }

        public override BaseHelpFormatter WithSubcommands(IEnumerable<Command> subcommands)
        {
            EmbedBuilder.AddField(Command != null ? "Subcommands" : "Commands", string.Join(", ", subcommands.Select(x => Formatter.InlineCode(x.Name))), false);
            return this;
        }

        public override CommandHelpMessage Build()
        {
            if (this.Command == null)
            {
                this.EmbedBuilder.WithDescription($"List of all server commands." +
                                                  $"\nPrefix for this server: {ConfigJson.GetConfigField().Prefix}, but you can use slash commands(just type `/`)" +
                                                  $"\nUse `{ConfigJson.GetConfigField().Prefix}help <command>` to see certain command description");
            }
            return new CommandHelpMessage(embed: EmbedBuilder.Build());
        }
    }
}