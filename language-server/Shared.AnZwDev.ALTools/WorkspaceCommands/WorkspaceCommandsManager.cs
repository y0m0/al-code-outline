﻿using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.Workspace;
using AnZwDev.ALTools.Extensions;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using Microsoft.Dynamics.Nav.CodeAnalysis.Text;
using Microsoft.Dynamics.Nav.CodeAnalysis;

namespace AnZwDev.ALTools.WorkspaceCommands
{
    public class WorkspaceCommandsManager
    {

        public ALDevToolsServer ALDevToolsServer { get; }

        private Dictionary<string, WorkspaceCommand> _commands;

        public WorkspaceCommandsManager(ALDevToolsServer alDevToolsServer)
        {
            this.ALDevToolsServer = alDevToolsServer;
            _commands = new Dictionary<string, WorkspaceCommand>();
            RegisterCommands();
        }

        protected WorkspaceCommand RegisterCommand(WorkspaceCommand command)
        {
            _commands.Add(command.Name, command);
            return command;
        }

        protected virtual void RegisterCommands()
        {
            SyntaxTreeWorkspaceCommandsGroup groupCommand = new SyntaxTreeWorkspaceCommandsGroup(this.ALDevToolsServer, "runMultiple");

            this.RegisterCommand(new AddAppAreasWorkspaceCommand(this.ALDevToolsServer));
            this.RegisterCommand(new AddToolTipsWorkspaceCommand(this.ALDevToolsServer));
            this.RegisterCommand(new RefreshToolTipsWorkspaceCommand(this.ALDevToolsServer));
            this.RegisterCommand(new SetPageFieldToolTipWorkspaceCommand(this.ALDevToolsServer));
            this.RegisterCommand(new AddPageControlCaptionWorkspaceCommand(this.ALDevToolsServer));
            this.RegisterCommand(new AddDataClassificationWorkspaceCommand(this.ALDevToolsServer));
            this.RegisterCommand(new AddFieldCaptionsWorkspaceCommand(this.ALDevToolsServer));
            this.RegisterCommand(new LockRemovedFieldsCaptionsWorkspaceCommand(this.ALDevToolsServer));
            this.RegisterCommand(new LockFieldsCaptionsWorkspaceCommand(this.ALDevToolsServer));
            this.RegisterCommand(new AddObjectCaptionsWorkspaceCommand(this.ALDevToolsServer));
            this.RegisterCommand(new MakeFlowFieldsReadOnlyWorkspaceCommand(this.ALDevToolsServer));
            this.RegisterCommand(new AddTableDataCaptionFieldsWorkspaceCommand(this.ALDevToolsServer));
            this.RegisterCommand(new AddDropDownFieldGroupsWorkspaceCommand(this.ALDevToolsServer));
            this.RegisterCommand(new AddDotToToolTipsWorkspaceCommand(this.ALDevToolsServer));

            this.RegisterCommand(new FixKeywordsCaseWorkspaceCommand(this.ALDevToolsServer));

            this.RegisterCommand(new AddObjectsPermissionsWorkspaceCommand(this.ALDevToolsServer));
            this.RegisterCommand(new TrimTrailingWhitespaceWorkspaceCommand(this.ALDevToolsServer));
            this.RegisterCommand(new RemoveEmptyLinesWorkspaceCommand(this.ALDevToolsServer));
            this.RegisterCommand(new RemoveEmptySectionsWorkspaceCommand(this.ALDevToolsServer));
            this.RegisterCommand(new RemoveEmptyTriggersWorkspaceCommand(this.ALDevToolsServer));

#if BC            
            this.RegisterCommand(new RemoveWithWorkspaceCommand(this.ALDevToolsServer));
            this.RegisterCommand(new FixIdentifiersCaseWorkspaceCommand(this.ALDevToolsServer));
            this.RegisterCommand(new RemoveUnusedVariablesWorkspaceCommand(this.ALDevToolsServer));
            this.RegisterCommand(new AddParenthesesWorkspaceCommand(this.ALDevToolsServer));
            this.RegisterCommand(new RemoveStrSubstNoFromErrorWorkspaceCommand(this.ALDevToolsServer));
            this.RegisterCommand(new RemoveEventPublishersWorkspaceCommand(this.ALDevToolsServer));
            this.RegisterCommand(new RemoveRedundantAppAreasWorkspaceCommand(this.ALDevToolsServer));
            this.RegisterCommand(new AddEnumCaptionsWorkspaceCommand(this.ALDevToolsServer));
            this.RegisterCommand(new AddReferencedTablesPermissionsWorkspaceCommand(this.ALDevToolsServer));
            this.RegisterCommand(new GenerateCSVXmlPortHeadersWorkspaceCommand(this.ALDevToolsServer));
            this.RegisterCommand(new AddMissingCaseLinesWorkspaceCommand(this.ALDevToolsServer));
            this.RegisterCommand(new AddUsingRegionWorkspaceCommand(this.ALDevToolsServer));
            this.RegisterCommand(new UpdateUsingsListWorkspaceCommand(this.ALDevToolsServer));
            this.RegisterCommand(new AddProjectNamespacesWorkspaceCommand(this.ALDevToolsServer));
#endif
            this.RegisterCommand(new RemoveRedundantDataClassificationWorkspaceCommand(this.ALDevToolsServer));

            this.RegisterCommand(new RemoveVariableWorkspaceCommand(this.ALDevToolsServer));
            this.RegisterCommand(new ConvertObjectIdsToNamesWorkspaceCommand(this.ALDevToolsServer));
            this.RegisterCommand(new RemoveBeginEndWorkspaceCommandd(this.ALDevToolsServer));
            this.RegisterCommand(new RemoveProceduresSemicolonWorkspaceCommand(this.ALDevToolsServer));

            this.RegisterCommand(new CollapseEmptyBracketsWorkspaceCommand(this.ALDevToolsServer));

            this.RegisterCommand(groupCommand.AddCommand(new OneStatementPerLineWorkspaceCommand(this.ALDevToolsServer)));
            this.RegisterCommand(groupCommand.AddCommand(new SortProceduresWorkspaceCommand(this.ALDevToolsServer)));
            this.RegisterCommand(groupCommand.AddCommand(new SortTriggersWorkspaceCommand(this.ALDevToolsServer)));
            this.RegisterCommand(groupCommand.AddCommand(new SortVariablesWorkspaceCommand(this.ALDevToolsServer)));
            this.RegisterCommand(groupCommand.AddCommand(new SortPropertiesWorkspaceCommand(this.ALDevToolsServer)));
            this.RegisterCommand(groupCommand.AddCommand(new SortReportColumnsWorkspaceCommand(this.ALDevToolsServer)));
            this.RegisterCommand(groupCommand.AddCommand(new SortTableFieldsWorkspaceCommand(this.ALDevToolsServer)));
            this.RegisterCommand(groupCommand.AddCommand(new SortPermissionsWorkspaceCommand(this.ALDevToolsServer)));
            this.RegisterCommand(groupCommand.AddCommand(new SortPermissionSetListWorkspaceCommand(this.ALDevToolsServer)));
            this.RegisterCommand(groupCommand.AddCommand(new FormatDocumentWorkspaceCommand(this.ALDevToolsServer)));
            this.RegisterCommand(groupCommand.AddCommand(new SortCustomizationsWorkspaceCommand(this.ALDevToolsServer)));
#if BC
            this.RegisterCommand(groupCommand.AddCommand(new SortUsingsWorkspaceCommand(this.ALDevToolsServer)));
#endif

            this.RegisterCommand(groupCommand);
        }

        public WorkspaceCommandResult RunCommand(string commandName, string sourceCode, string projectPath, string filePath, TextRange range, Dictionary<string, string> parameters, List<string> excludeFiles, List<string> includeFiles)
        {
            if (_commands.ContainsKey(commandName))
            {
                ALProject project = ALDevToolsServer.Workspace.FindProject(projectPath);
                if (project == null)
                    project = ALDevToolsServer.Workspace.FindProject(filePath);

                WorkspaceCommand command = _commands[commandName];
                (WorkspaceCommandResult errorResult, bool canRun) = command.CanRun(sourceCode, project, filePath, range, parameters, excludeFiles, includeFiles);
                if (!canRun)
                    return errorResult;
                return _commands[commandName].Run(sourceCode, project, filePath, range, parameters, excludeFiles, includeFiles);
            }
            else
                throw new Exception($"Workspace command {commandName} not found.");
        }

        public List<WorkspaceCommandCodeAction> GetCodeActions(string sourceCode, string projectPath, string filePath, TextRange range)
        {
            var codeActions = new List<WorkspaceCommandCodeAction>();
            var project = ALDevToolsServer.Workspace.FindProject(projectPath);
            if (project == null)
                project = ALDevToolsServer.Workspace.FindProject(filePath);

            if (project != null)
            {
                SourceText sourceText = SourceText.From(sourceCode);
#if BC
                SyntaxTree syntaxTree = SyntaxTree.ParseObjectText(sourceText, null, project.GetSyntaxTreeParseOptions());
#else
                SyntaxTree syntaxTree = SyntaxTree.ParseObjectText(sourceText, null);
#endif
                var span = sourceText.Lines.GetTextSpan(new LinePositionSpan(new LinePosition(range.start.line, range.start.character), new LinePosition(range.end.line, range.end.character)));
                var syntaxNode = syntaxTree.FindNodeByPositionInFullSpan(span.Start);

                foreach (var command in _commands.Values)
                {
                    command.CollectCodeActions(syntaxTree, syntaxNode, range, codeActions);
                }
            }

            return codeActions;
        }

    }
}
