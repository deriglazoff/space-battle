﻿namespace SpaceBattle.Api.Commands;

public class MacroCommand : ICommand
{
    private readonly ICommand[] commands;

    public MacroCommand(params ICommand[] commands)
    {
        this.commands = commands;
    }

    public void Execute()
    {
        foreach (var command in commands)
        {
            try
            {
                command.Execute();
            }
            catch (Exception e)
            {

            }
        }
    }
}